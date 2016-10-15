<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* TakeUntil 1:
 *
 * TakeUntil is a version of TakeWhile that is signaled via another Observable -
 * in other words, it will "shut off the hose" when another Observable produces
 * any value.
 */

var input = new[] { 1, 2, 3 }.ToObservable();

input
    .Repeat()
    .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(2.0)))
    .Dump();
