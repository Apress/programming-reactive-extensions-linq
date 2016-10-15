<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Return 3:
 *
 * Demonstrate that Return is a Cold Observable (i.e. that it produces a new
 * stream of events every time someone subscribes to it)
 */

var input = Observable.Return(42);
input.Subscribe(x => Console.WriteLine("Subscription 1: {0}", x));
input.Subscribe(x => Console.WriteLine("Subscription 2: {0}", x));
