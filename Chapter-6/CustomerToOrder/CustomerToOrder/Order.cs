using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace CustomerToOrder
{
    [Table(Name="Orders")]
    class Order
    {
        [Column( IsPrimaryKey = true )]
        public int OrderID;

        [Column]
        public string CustomerID;

        private EntityRef<Customer> _customer;

        [Association( Storage = "_customer", ThisKey = "CustomerID" )]
        public Customer Customer
        {
            get { return this._customer.Entity; }
            set { this._customer.Entity = value; }
        }


    }
}
