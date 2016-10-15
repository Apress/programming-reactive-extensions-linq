using System;
using System.Windows;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq.Mapping;
using System.Windows.Controls;


namespace DataBase101
{
    public partial class MainPage : PhoneApplicationPage
    {

        public MainPage()
        {
            InitializeComponent();
            DataContext db = new BooksDataContext( "isostore:/bookDB.sdf" );

            if (db.DatabaseExists())
                db.DeleteDatabase();
            db.CreateDatabase();

            CreateBook.Click += new RoutedEventHandler( CreateBook_Click );
            ShowData.Click += new RoutedEventHandler( ShowData_Click );

        }

        void ShowData_Click( object sender, RoutedEventArgs e )
        {
            BooksDataContext db = new BooksDataContext( "isostore:/bookDB.sdf" );

            var q = from b in db.Books
                    orderby b.Title
                    select new 
                    { 
                        b.Title, 
                        b.PublicationDate, 
                        b.BookPublisher 
                    };

            BooksLB.ItemsSource = q;
            
        }


        void CreateBook_Click( object sender, RoutedEventArgs e )
        {

            BooksDataContext db = new BooksDataContext( "isostore:/bookDB.sdf" );
            Progress.Items.Add("Connected to bookDB.sdf...");

            Publisher pub = new Publisher()
            {
                PublisherID = "1",
                Name = "APress",
                City = "Acton",
                Url = "http://Apress.com"
            };
            db.Publishers.InsertOnSubmit( pub );
            Progress.Items.Add( "Pub (Apress) created..." );



            Publisher pub2 = new Publisher()
            {
                PublisherID = "2",
                Name = "O'Reilly",
                City = "Cambridge",
                Url = "http://Oreilly.com"
            };
            db.Publishers.InsertOnSubmit( pub );
            Progress.Items.Add( "Pub (O'Reilly) created..." );



            Book theBook = new Book()
            {
                BookID = "1",
                BookPublisher = pub,
                PublicationDate = DateTime.Now,
                Title = "Programming Reactive Extensions"
            };
            db.Books.InsertOnSubmit( theBook );
            Progress.Items.Add( "Book (Rx) created..." );



            theBook = new Book()
            {
                BookID = "2",
                BookPublisher = pub,
                PublicationDate = DateTime.Now,
                Title="Migrating to Windows Phone"
            };
            db.Books.InsertOnSubmit( theBook );
            Progress.Items.Add( "Book (Migrating) created..." );

            theBook = new Book()
            {
                BookID = "3",
                BookPublisher = pub2,
                PublicationDate = DateTime.Now,
                Title = "Programming C#"
            };
            db.Books.InsertOnSubmit( theBook );
            Progress.Items.Add( "Book (C#) created..." );



            db.SubmitChanges();
            Progress.Items.Add( "DB Updated." );
            Progress.SelectedIndex = Progress.Items.Count -1;

        }
    }
}

             


