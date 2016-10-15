<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Defer
 *
 * Defer is a way to take a possibly Hot Observable and make it Cold via a Func.
 * This means, that we are creating an Observable that is only calculated when
 * someone actually Subscribes to it.
 *
 * Why would I want to do this? Well, remember that using Observable.Return
 * means that the value is calculated immediately, and maybe we want to be more
 * lazy about it (think perhaps looking up something in a database).
 */

int i = 2;

var input1 = Observable.Return(i);
var input2 = Observable.Defer(() => Observable.Return(i));

i = 10;

"Without Defer - captured 'i' when it was created".Dump();
input1.Subscribe(Console.WriteLine);

"Using Defer - we didn't capture 'i' until Dump()".Dump();
input2.Subscribe(Console.WriteLine);
