using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Xml.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace RxUISample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Since we're using the MVVM pattern, we're going to bind to our 
        // ViewModel object in the code, and our View code-behind will be
        // concerned only with things that are solely view-based, like 
        // minimizing/maximizing a window
        public AppViewModel ViewModel { get; protected set; }

        public MainWindow()
        {
            ViewModel = new AppViewModel();
            InitializeComponent();
        }
    }

    // Create a simple model class to store our Flickr results - since we will 
    // never update the properties once we create the object, we don't have to
    // use ReactiveObject, just good-old auto-properties.
    public class FlickrPhoto 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }

    // AppViewModel is where we will describe the interaction of our application
    // (we can describe the entire application in one class since this is very 
    // small). 
    public class AppViewModel : ReactiveObject
    {
        // In ReactiveUI, this is the syntax to declare a read-write property
        // that will notify Observers (as well as WPF) that a property has 
        // changed. If we declared this as a normal property, we couldn't tell 
        // when it has changed!

        string _SearchTerm;
        public string SearchTerm {
            get { return _SearchTerm; }
            set { this.RaiseAndSetIfChanged(x => x.SearchTerm, value); }
        }

        // We will describe this later, but ReactiveAsyncCommand is a Command
        // (like "Open", "Copy", "Delete", etc), that manages a task running
        // in the background.

        public ReactiveAsyncCommand ExecuteSearch { get; protected set; }


        /* ObservableAsPropertyHelper
         * 
         * Here's the interesting part: In ReactiveUI, we can take IObservables
         * and "pipe" them to a Property - whenever the Observable yields a new
         * value, we will notify ReactiveObject that the property has changed.
         * 
         * To do this, we have a class called ObservableAsPropertyHelper - this
         * class subscribes to an Observable and stores a copy of the latest value.
         * It also runs an action whenever the property changes, usually calling
         * ReactiveObject's RaisePropertyChanged.
         */

        ObservableAsPropertyHelper<List<FlickrPhoto>> _SearchResults;
        public List<FlickrPhoto> SearchResults {
            get { return _SearchResults.Value; }
        }

        // Here, we want to create a property to represent when the application 
        // is performing a search (i.e. when to show the "spinner" control that 
        // lets the user know that the app is busy). We also declare this property
        // to be the result of an Observable (i.e. its value is derived from 
        // some other property)

        ObservableAsPropertyHelper<Visibility> _SpinnerVisibility;
        public Visibility SpinnerVisibility {
            get { return _SpinnerVisibility.Value; }
        }

        public AppViewModel(ReactiveAsyncCommand testExecuteSearchCommand = null, IObservable<List<FlickrPhoto>> testSearchResults = null)
        {
            ExecuteSearch = testExecuteSearchCommand ?? new ReactiveAsyncCommand();

            /* Creating our UI declaratively
             * 
             * The Properties in this ViewModel are related to each other in different 
             * ways - with other frameworks, it is difficult to describe each relation
             * succinctly; the code to implement "The UI spinner spins while the search 
             * is live" usually ends up spread out over several event handlers.
             *
             * However, with RxUI, we can describe how properties are related in a very 
             * organized clear way. Let's describe the workflow of what the user does in
             * this application, in the order they do it.
             */

            // We're going to take a Property and turn it into an Observable here - this
            // Observable will yield a value every time the Search term changes (which in
            // the XAML, is connected to the TextBox). 
            //
            // We're going to use the Throttle operator to ignore changes that 
            // happen too quickly, since we don't want to issue a search for each 
            // key pressed! We then pull the Value of the change, then filter 
            // out changes that are identical, as well as strings that are empty.
            //
            // Finally, we use RxUI's InvokeCommand operator, which takes the String 
            // and calls the Execute method on the ExecuteSearch Command, after 
            // making sure the Command can be executed via calling CanExecute.
            this.ObservableForProperty(x => x.SearchTerm)
                .Throttle(TimeSpan.FromMilliseconds(800), RxApp.DeferredScheduler)
                .Select(x => x.Value)
                .DistinctUntilChanged()
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .InvokeCommand(ExecuteSearch);


            // How would we describe when to show the spinner in English? We 
            // might say something like, "The spinner's visibility is whether
            // the search is running". RxUI lets us write these kinds of 
            // statements in code.
            //
            // ExecuteSearch has an IObservable<int> called ItemsInFlight that
            // fires every time a new item starts or stops. We Select() that into
            // a Visibility (0 = Collapsed, > 0 = Visible), then we will use RxUI's
            // ToProperty operator, which is a helper to create an 
            // ObservableAsPropertyHelper object.
            //
            // Essentially, we're saying here, "The value of SpinnerVisibility is
            // the in-flight items Selected into a Visibility"
            _SpinnerVisibility = ExecuteSearch.ItemsInflight
                .Select(x => x > 0 ? Visibility.Visible : Visibility.Collapsed)
                .ToProperty(this, x => x.SpinnerVisibility, Visibility.Hidden);

            // Here, we're going to actually describe what happens when the Command
            // gets invoked - we're going to run the GetSearchResultsFromFlickr every
            // time the Command is executed. 
            //
            // The important bit here is the return value - an Observable. We're going
            // to end up here with a Stream of FlickrPhoto Lists: every time someone 
            // calls Execute, we eventually end up with a new list.

            IObservable<List<FlickrPhoto>> results;
            if (testSearchResults != null) {
                results = testSearchResults;
            } else {
                results = ExecuteSearch.RegisterAsyncFunction(term => GetSearchResultsFromFlickr((string)term));
            }

            // ...which we then immediately put into the SearchResults Property.
            _SearchResults = results.ToProperty(this, x => x.SearchResults, new List<FlickrPhoto>());
        }

        public static List<FlickrPhoto> GetSearchResultsFromFlickr(string searchTerm)
        {
            var doc = XDocument.Load(String.Format(CultureInfo.InvariantCulture,
                "http://api.flickr.com/services/feeds/photos_public.gne?tags={0}&format=rss_200",
                HttpUtility.UrlEncode(searchTerm)));

            if (doc.Root == null)
                return null;

            var titles = doc.Root.Descendants("{http://search.yahoo.com/mrss/}title")
                .Select(x => x.Value);

            var tagRegex = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            var descriptions = doc.Root.Descendants("{http://search.yahoo.com/mrss/}description")
                .Select(x => tagRegex.Replace(HttpUtility.HtmlDecode(x.Value), ""));

            var items = titles.Zip(descriptions,
                (t, d) => new FlickrPhoto { Title = t, Description = d }).ToArray();

            var urls = doc.Root.Descendants("{http://search.yahoo.com/mrss/}thumbnail")
                .Select(x => x.Attributes("url").First().Value);

            var ret = items.Zip(urls, (item, url) => { item.Url = url; return item; }).ToList();
            return ret;
        }
    }
}
