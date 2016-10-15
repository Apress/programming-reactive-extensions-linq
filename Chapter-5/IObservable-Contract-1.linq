<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* The Contract of IObservable
 *
 * One thing that's critical to understand, is that the contract of IObservable
 * declares that once an observable sequence terminates with
 * OnCompleted/OnError, no more items can be produced. Let's prove it:
 */

var sched = new TestScheduler();

var input = sched.CreateColdObservable(
        sched.OnNextAt(200, 1),
        sched.OnNextAt(300, 10),
        sched.OnCompletedAt<int>(350.0),
        sched.OnNextAt(400, 100),
        sched.OnNextAt(500, 1000),
        sched.OnNextAt(600, 10000))
    .AsObservable();

input.Dump();

sched.Start();

