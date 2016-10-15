/* Return 1:
 *
 * Can't get any simpler than this
 */

var input = Observable.Return(42);

input.Subscribe(x => Console.WriteLine("The number is {0}", x));


/* Return 2:
 *
 * Show that Return Completes after returning its one value
 */

// Materialize lets us see the OnComplete, which normally isn't part of the
// values returned in the stream.
var input = Observable.Return(42).Materialize();

input.Dump();


/* Return 3:
 *
 * Demonstrate that Return is a Cold Observable (i.e. that it produces a new
 * stream of events every time someone subscribes to it)
 */

var input = Observable.Return(42);
input.Subscribe(x => Console.WriteLine("Subscription 1: {0}", x));
input.Subscribe(x => Console.WriteLine("Subscription 2: {0}", x));


/* Range 1
 *
 * Nothing particularly exciting here
 */

var input = Observable.Range(1, 100);

input.Sum().Subscribe(x => Console.WriteLine("The Sum is {0}", x));


/* Timer 1
 *
 * Demonstrate a simple use of Observable.Timer 
 */

// We've specified that we want the timer to start *immediately*, that it should
// tick every second, and that we want the ticks to appear as Task<T>'s on other
// threads
var timer = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1.0), Scheduler.TaskPool);

timer.Take(5).Subscribe(x => Console.WriteLine(x % 2 == 0 ? "Tick" : "Tock"));

// Linqpad needs to wait while the tasks are running
Thread.Sleep(5500);


/* Start 1:
 *
 * Run an Action in the background and give us an Observable that represents the
 * background task. Equivalent to (new Task(() => {...})).Start()
 *
 * This sample also demonstrates how to use First() to block on an Observable.
 */

var task = Observable.Start(() => {
    Console.WriteLine("Hello World!");

    // Do something very time-consuming here
    Thread.Sleep(1000);
    return;
});

// Wait until the task is completed
task.First();

Console.WriteLine("We're Finished!");

// vim: ts=4 sw=4 tw=80 et :
