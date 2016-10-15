<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Deferred Execution 3:
 *
 * We can fix the broken code above by adding a ToArray operator to the
 * pipeline.
 * this turns the evenNumbersInSeries from an IEnumerable to an array and the array is populated before
 * the values are displayed
 */

int counter = 0;
var evenNumbersInSeries = Enumerable.Range(0, 10).Select(x => {
    int result = x + counter;
    counter++;
    return result;
}).ToArray();

// List the numbers in the series

Console.WriteLine("First Try:\n");
foreach(int i in evenNumbersInSeries) {
    Console.WriteLine(i);
}

// This time, because we added the ToArray(), we'll get the expected result
// every time.

Console.WriteLine("\nSecond Try:\n");
foreach(int i in evenNumbersInSeries) {
    Console.WriteLine(i);
}
