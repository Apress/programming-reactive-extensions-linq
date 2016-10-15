using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq.Mapping;
using System.ComponentModel;

namespace DataBase101
{

    [Table]
    public class Book : INotifyPropertyChanged, INotifyPropertyChanging
    {
        [Column( IsPrimaryKey = true )]
        public string BookID { get; set; }

        [Column]
        public string Title { get; set; }

        [Column]
        public string PublisherID { get; set; }

        [Column( IsVersion = true )]
        private Binary _version; 




        private EntityRef<Publisher> _publisher;

        [Association( 
            OtherKey = "PublisherID", 
            ThisKey = "PublisherID", 
            Storage = "_publisher" ) ]
        public Publisher BookPublisher
        {
            get
            {
                return _publisher.Entity;
            }
            set
            {
                NotifyPropertyChanging( "BookPublisher" );
                _publisher.Entity = value;
                PublisherID = value.PublisherID;
                NotifyPropertyChanged( "BookPublisher" );
            }
        }

        [Column]
        public DateTime PublicationDate { get; set; }




        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged( string propertyName )
        {
            if (PropertyChanged != null)
            {
                PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;
        private void NotifyPropertyChanging( string propertyName )
        {
            if (PropertyChanging != null)
            {
                PropertyChanging( this, new PropertyChangingEventArgs(propertyName));
            }
        }
    }
}
