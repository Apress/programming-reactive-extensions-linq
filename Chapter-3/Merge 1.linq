<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Merge 1
 *
 * This is the simplest example of Merge, just take 5 results and return them
 */

Observable.Merge(
    Observable.Return(1),
    Observable.Return(2),
    Observable.Return(3),
    Observable.Return(4)
).Subscribe(x => {
    Console.WriteLine("Number {0}", x);
});
