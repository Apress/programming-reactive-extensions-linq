<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* DistinctUntilChanged
 *
 * Not too complex - it's the Rx version of Posix's uniq, it filters out items
 * that were identical to the previous item. Note that this isn't quite the same
 * as LINQ's Distinct(), it doesn't guarantee numbers are *globally* distinct,
 * only that the value is different than the previous value.
 */


(new[] {1,1,1,1,2,2,2,2,3,4,5,5,5,5,1,1,1}).ToObservable()
    .DistinctUntilChanged()
    .Dump();
