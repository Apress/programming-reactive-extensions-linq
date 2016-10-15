<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Deferred Execution 1:
 *
 * Demonstrate that execution occurs when a collection is enumerated, *not*
 * when the expression is evaluated, which is sometimes really useful!
 */

var thisShouldTakeALongTime = Enumerable.Range(0, 1000 * 1000).Select(x => {
    Thread.Sleep(1000);
    return x * 10;
});

Console.WriteLine("The 1st number is " + thisShouldTakeALongTime.First());
