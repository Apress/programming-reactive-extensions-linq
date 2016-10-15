<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Multicast 1:
 *
 * Multicast plays an Observable onto a Subject - this is useful for making Cold
 * Observables into Hot Observables. Practically speaking, it's also useful in
 * class implementations where you want to return "an Observable that is based
 * on this other Observable, but every once in awhile I want to signal it
 * by-hand"
 */

// IEnumerable<T>.ToObservable() is a *Cold* Observable - every time you
// subscribe to it, you get a new copy.

var input = new[] {1,2,3}.ToObservable();

var output = input.Multicast(new Subject<int>());

// This doesn't do anything until Connect is called to actually connect the
// input to the Subject.
output.Dump();

Util.ReadLine("Press any key to continue");

output.Connect();
