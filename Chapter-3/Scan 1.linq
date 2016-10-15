<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Scan 1:
 *
 * Scan is similar to Aggregate - however, whereas Aggregate only returns
 * exactly *one* value, Scan returns the accumulator value with every OnNext of
 * the input. This method is great for "running total" type of calculations.
 */

var stockChanges = new[] { 0.005, 0.01, 0.016, -0.021, -0.005, 0.03, -0.04, 0.00, -0.02, -0.005 }.ToObservable();

var initialPrice = 24.815;

var runningTotal = stockChanges.Scan(initialPrice, (acc, x) => acc + x);

runningTotal.Dump();
