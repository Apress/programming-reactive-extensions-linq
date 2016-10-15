<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Distinct 1:
 *
 * Distinct returns the set of unique items in a collection, removing any
 * duplicate items.
 */

var input = new[] {1,2,3,2,1,2,3,2,1,2,3,2,1};
input.Distinct().Dump();
