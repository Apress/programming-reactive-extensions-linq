<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

List<int> ints = new List<int>() 
	 { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15 };

var query = from i in ints
   where i % 2 == 0
   select i;
			
foreach ( var j in query )
   Console.Write ("{0} ", j); 
