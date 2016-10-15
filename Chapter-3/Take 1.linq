<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Take 1:
 *
 * Return a subset of the items in the collection.
 */

var input = new[] {1,2,3,4,5,4,3,2,1}.ToObservable();
var output = input.Take(5).Select(x => x * 10);

output.Dump();
