using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace CustomerToOrder
{
    class Program
    {
        static void Main( string[ ] args )
        {
            var db = new NorthWndDataContext( "L:\\Downloads\\Northwind\\NORTHWND.mdf" );
            var q = from cust in db.Customers
                    from ord in db.Orders
                    select new
                    {
                        cust.CustomerID,
                        cust.City,
                        ord.OrderID
                    };

            foreach (var record in q)
            {
                Console.WriteLine( record );
            }
        }
    }
}
