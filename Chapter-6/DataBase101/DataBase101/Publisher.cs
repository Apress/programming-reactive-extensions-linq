using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq.Mapping;

namespace DataBase101
{
    [Table]
    public class Publisher
    {
        [Column( IsPrimaryKey = true )]
        public string PublisherID { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public string City { get; set; }

        [Column]
        public string Url { get; set; }
    }
}
