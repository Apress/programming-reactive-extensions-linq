<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Return 2:
 *
 * Show that Return Completes after returning its one value
 */

// Materialize lets us see the OnComplete, which normally isn't part of the
// values returned in the stream.
var input = Observable.Return(42).Materialize();

input.Dump();
