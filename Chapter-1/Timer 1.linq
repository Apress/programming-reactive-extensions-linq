<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

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
