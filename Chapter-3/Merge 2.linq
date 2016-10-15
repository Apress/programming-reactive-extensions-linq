<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Merge 2
 *
 * This is a much more advanced example - Merge can take an
 * IObservable<IObservable<T>> and merge them down into an IObservable<T> -
 * it's the 2nd half of how SelectMany works.
 */

var inputs = (new[] {
    "http://www.google.com",
    "http://www.duckduckgo.com",
    "http://www.yahoo.com",
    "http://www.bing.com",
});

var output = inputs.Select(x => RxBook.FetchWebpage(x)).Merge();
output.Dump();
