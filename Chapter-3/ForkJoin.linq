<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

var inputs = (new[] {
    "http://www.google.com",
    "http://www.duckduckgo.com",
    "http://www.yahoo.com",
    "http://www.bing.com",
});

IObservable<string[]> output = Observable.ForkJoin(inputs.Select(url => 
    RxBook.FetchWebpage(url).Select(content =>
        new { url, content })));

foreach(var item in output.First()) {
    String.Format("'{0}' is of size {1}", item.url, item.content.Length).Dump();
}
