using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace CustomerToOrder
{
    class NorthWndDataContext : DataContext
    {
        public Table<Customer> Customers;
        public Table<Order> Orders;
        public NorthWndDataContext( string connection ) :
            base( connection ) { }
    }
}
