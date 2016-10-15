<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

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
