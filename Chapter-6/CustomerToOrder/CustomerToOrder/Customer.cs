using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace CustomerToOrder
{
    [Table(Name="Customers")]
    class Customer
    {
        [Column( IsPrimaryKey = true )]
        public string CustomerID { get; set; }

        [Column]
        public string CompanyName { get; set; }

        [Column]
        public string ContactName { get; set; }

        [Column]
        public string City { get; set; }

        private EntitySet<Order> _orders;

        [Association( Storage = "_orders", OtherKey = "CustomerID" )]
        public EntitySet<Order> Orders
        {
            get { return this._orders; }
            set { _orders.Assign( value ); }
        }



    }
}
