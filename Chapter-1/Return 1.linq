<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Return 1:
 *
 * Can't get any simpler than this
 */

var input = Observable.Return(42);

input.Subscribe(x => Console.WriteLine("The number is {0}", x));
