<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* SelectMany 2:
 *
 * Here's the lightbulb moment - if an Observable represents both an "Observable
 * list", and a Future async task, we can have a Observable List of Futures -
 * whose type would be IObservable<IObservable<T>> - how to we convert a list of
 * lists into a list? SelectMany!
 *
 * In practice, SelectMany allows you to chain web service or other async calls
 * - passing the result of one service to another.
 */

var inputs = (new[] {
    "http://www.google.com",
    "http://www.duckduckgo.com",
    "http://www.yahoo.com",
    "http://www.bing.com",
}).ToObservable();

var output = inputs.SelectMany(x => RxBook.FetchWebpage(x));
output.Dump();
