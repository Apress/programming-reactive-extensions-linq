<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* That sucks!
 *
 * This aspect of Observable seems quite impractical for those who are new to
 * Rx; how am I supposed to get anything done if whenever anything goes wrong,
 * my entire pipeline gets torn down? 
 *
 * If every Observable was a Hot observable, you'd be right - however, remember
 * that Cold observables get a new copy every time someone evaluates them via
 * Subscribe(), First(), or other methods that eventually return a value. This
 * means that Observables that you want to reuse should strive to be Cold
 * observables.
 *
 * This is the core reason why methods such as Repeat and Retry work - they
 * continually resubscribe to their input whenever it ends, making the
 * *appearance* of an infinite stream, even when their input ends via an error.
 *
 * However, they do this by deciding what to do about the error case, either
 * making it disappear (Retry), or by following Abort semantics (Repeat).
 * Rx makes you explicitly think about what *should* happen when errors occur -
 * do you want to make them disappear? Replace the output with a 'null' value?
 * Tear everything down? All of these are viable options based on the scenario,
 * but you have to think about it!
 *
 * Here's how we could take advantage of Defer() to ask for a new Observable
 * every time something bad happens.
 */

int counter = 0;
var input = Observable.Defer(() => {
    // Simulate an Observable that sometimes dies
    if (++counter % 2 == 0) {
        return Observable.Throw<int>(new Exception("Aieeee!"));
    } else {
        return Observable.Return(42);
    }
});

input.Subscribe(x => Console.WriteLine(x), ex => Console.WriteLine(ex.Message));

input.Subscribe(x => Console.WriteLine(x), ex => Console.WriteLine(ex.Message));

input.Subscribe(x => Console.WriteLine(x), ex => Console.WriteLine(ex.Message));

