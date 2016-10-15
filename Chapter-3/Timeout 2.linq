<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Timeout 2:
 *
 * Let's see Timeout in combination with Retry and Defer, to make a highly
 * reliable web service call. We use the Defer here, because Retry only makes
 * sense with Cold observables.
 */

// Your web connection might actually be quick enough to pull google.com in time
// - if so, try changing the timeout to a lower value, or go to an airport and
// use their Wifi

Observable.Defer(() => RxBook.FetchWebpage("http://www.google.com"))
	.Timeout(TimeSpan.FromMilliseconds(750))
	.Retry(3)
	.OnErrorResumeNext(Observable.Return("Couldn't fetch the Website"))
	.Dump();
