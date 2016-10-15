using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq.Mapping;

namespace DataBase101
{
    public class BooksDataContext : DataContext
    {
        public Table<Book> Books;
        public Table<Publisher> Publishers;
        public BooksDataContext( string connection ) : base( connection ) { }
    }
}
