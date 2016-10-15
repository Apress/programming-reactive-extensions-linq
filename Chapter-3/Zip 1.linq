<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Zip 1:
 *
 * Zip allows us to merge two Observables, just like its Linq counterpart. Zip
 * creates two "slots" - whenever both slots have an item, it sends those two
 * items through the Selector function to create the result.
 *
 * One thing that's important to point out, is that Zip will throw away items
 * that it can't use, similar to CombineLatest (i.e. if the left side is already
 * full, it is replaced and the old one is lost). Unlike CombineLatest, Zip
 * won't yield anything until it has a *new* thing on both sides. 
 */

var leftSide = new[] {1,2,3,4}.ToObservable();
var rightSide = new[] { "A", "B", "C" }.ToObservable();

Observable.Zip(leftSide, rightSide, (num, letter) => num.ToString() + letter)
    .Dump();
