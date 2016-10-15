<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Range 1
 *
 * Nothing particularly exciting here
 */

var input = Observable.Range(1, 100);

input.Sum().Subscribe(x => Console.WriteLine("The Sum is {0}", x));
