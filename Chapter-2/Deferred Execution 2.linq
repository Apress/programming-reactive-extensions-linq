<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Deferred Execution 2:
 *
 * Sometimes deferred execution *isn't* useful, let's see how deferred
 * execution could cause us to have unexpected results:
 */

int counter = 0;
var evenNumbersInSeries = 
    Enumerable.Range(0, 10).Select(
	x => 
    {
       int result = x + counter;
       counter++;
       return result;
    });

// List the numbers in the series

Console.WriteLine("First Try:\n");
foreach(int i in evenNumbersInSeries) 
{
    Console.WriteLine(i);
}

// We're running the same code again here, we'll certainly get the same
// result, right?

Console.WriteLine("\nSecond Try:\n");
foreach(int i in evenNumbersInSeries) 
{
    Console.WriteLine(i);
}
