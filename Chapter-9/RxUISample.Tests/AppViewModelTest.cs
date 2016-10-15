using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using Microsoft.Reactive.Testing;
using ReactiveUI;
using ReactiveUI.Testing;
using RxUISample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveUI.Xaml;

namespace RxUISample.Tests
{
    [TestClass]
    public class AppViewModelTest
    {
        [TestMethod]
        public void SearchesShouldntRunOnEveryKeystroke()
        {
            /* What does this first line do?
             * 
             * Remember that in RxUI, there are two Schedulers that the framework
             * uses: the Deferred Scheduler (i.e. the 'UI thread'), and the Task Pool
             * Scheduler (i.e. 'run stuff on the background').
             * 
             * Here, we create a new Test Scheduler, and call the With method - 
             * this means that inside the block given, the "official" schedulers 
             * are replaced with our test scheduler.
             */

            (new TestScheduler()).With(sched => {
                // Simulate the user entering some stuff into the TextBox
                var keyboardInput = sched.CreateColdObservable(
                    sched.OnNextAt(10, "R"),
                    sched.OnNextAt(20, "Ro"),
                    sched.OnNextAt(30, "Robo"),
                    sched.OnNextAt(40, "Robot"),
                    sched.OnNextAt(2000, "Hat"));

                // Make sure that the command can always execute if asked, stub
                // out the actual search code
                var fixture = new AppViewModel(new ReactiveAsyncCommand(null, 1000), Observable.Never<List<FlickrPhoto>>());

                // Wire up the keyboard input
                keyboardInput.Subscribe(x => fixture.SearchTerm = x);

                // Keep count of how many times the command was invoked
                int numTimesCommandInvoked = 0;
                fixture.ExecuteSearch.Subscribe(x => numTimesCommandInvoked++);

                //
                // Now the fun part - we're going to travel through time and see
                // how many times the command gets invoked!
                //

                sched.RunToMilliseconds(25);
                Assert.AreEqual(0, numTimesCommandInvoked);

                sched.RunToMilliseconds(40);
                Assert.AreEqual(0, numTimesCommandInvoked);

                sched.RunToMilliseconds(1800);
                Assert.AreEqual(1, numTimesCommandInvoked);

                sched.RunToMilliseconds(2010);
                Assert.AreEqual(1, numTimesCommandInvoked);

                sched.RunToMilliseconds(5000);
                Assert.AreEqual(2, numTimesCommandInvoked);
            });
        }

        [TestMethod]
        public void SpinnerShouldSpinWhileAppIsSearching()
        {
            (new TestScheduler()).With(sched => {
                // Here, we're going to create a dummy Observable that mocks
                // a web service call - mocking things that happen over time is
                // often tricky, but not with Rx!
                var searchObservable = Observable.Return(createSampleResults())
                    .Delay(TimeSpan.FromMilliseconds(5000), RxApp.TaskpoolScheduler);

                var command = new ReactiveAsyncCommand();
                command.RegisterAsyncObservable(x => searchObservable);

                var fixture = new AppViewModel(command, searchObservable);

                // The spinner should be hidden on startup
                Assert.AreNotEqual(Visibility.Visible, fixture.SpinnerVisibility);

                // Invoke the command
                fixture.ExecuteSearch.Execute("Robot");

                // Once we run the command, we should be showing the spinner
                sched.RunToMilliseconds(100);
                Assert.AreEqual(Visibility.Visible, fixture.SpinnerVisibility);

                // Fast forward to 6sec, the spinner should now be gone
                sched.RunToMilliseconds(6 * 1000);
                Assert.AreNotEqual(Visibility.Visible, fixture.SpinnerVisibility);
            });
        }

        [TestMethod]
        public void ChangingSearchTermShouldResultInSearchResults()
        {
            (new TestScheduler()).With(sched => {
                // Create a dummy Observable representing the actual query 
                var searchObservable = Observable.Return(createSampleResults())
                    .Delay(TimeSpan.FromMilliseconds(5 * 1000), RxApp.TaskpoolScheduler);

                // Create a dummy command to pass to the ViewModel that returns
                // our Observable
                var command = new ReactiveAsyncCommand();
                command.RegisterAsyncObservable(x => searchObservable);

                var fixture = new AppViewModel(command, searchObservable);
                Assert.IsTrue(fixture.SearchResults.Count == 0);

                fixture.SearchTerm = "Foo";

                // At 2 seconds in, we shouldn't have results yet
                sched.RunToMilliseconds(2 * 1000);
                Assert.IsTrue(fixture.SearchResults.Count == 0);

                // At 10 seconds, we should have the sample results
                var sampleData = createSampleResults();
                sched.RunToMilliseconds(10 * 1000);
                Assert.AreEqual(sampleData.Count, fixture.SearchResults.Count);

                // Make sure the two sequences are identical
                foreach(var item in sampleData.Zip(fixture.SearchResults, (expected, actual) => new { expected, actual })) {
                    Assert.AreEqual(item.expected.Title, item.actual.Title);
                    Assert.AreEqual(item.expected.Description, item.actual.Description);
                    Assert.AreEqual(item.expected.Url, item.actual.Url);
                }
            });
        }

        List<FlickrPhoto> createSampleResults()
        {
            return new List<FlickrPhoto>() {
                new FlickrPhoto() {
                    Description = "A sample image description",
                    Title = "Sample Image",
                    Url = "http://www.example.com/image.gif",
                },
            };
        }
    }
}
