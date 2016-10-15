<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* SelectMany 1:
 *
 * SelectMany is one of the most powerful Rx operators - it allows you to
 * substutute any item in an Observable with zero, one, or many items. This is a
 * powerful idea in Linq, but even moreso in Rx.
 *
 * In this example, we'll take every item and make a duplicate if it's an even
 * number. So for example, every '2' will show up as '2, 2', but a '3' will only
 * show up in the output as '3'.
 */

var input = new[] {1,2,3}.ToObservable();
var output = input.SelectMany(x => {

    // Here we want to return an Observable that we will "splice into" the final
    // stream:
    if (x % 2 == 0) {
        return new[] {x, x}.ToObservable();
    } else {
        return new[] { x }.ToObservable();
    }
});

output.Dump();
