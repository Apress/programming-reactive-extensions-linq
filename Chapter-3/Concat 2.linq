<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Concat 1:
 *
 * One thing to point out here, is that sometimes the *non* Extension method is
 * more readable than the extension method, especially for operations involving
 * 3+ Observables */

var firstPart = (new[] {1,2,3,4,5}).ToObservable();
var secondPart = (new[] {6,7,8,9,10}).ToObservable();
var thirdPart = (new[] {11,12,13,14,15}).ToObservable();

// extension
firstPart.Concat(secondPart.Concat(thirdPart)).Dump();

// non-extension
Observable.Concat(firstPart, secondPart, thirdPart).Dump();
