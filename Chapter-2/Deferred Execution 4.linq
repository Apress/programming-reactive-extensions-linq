<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

var query = from num in list 
			where num < 7
			select num;
			
foreach ( var num in query )
{
	Console.WriteLine(num);
}
			
