<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Delay
 *
 * This is an easy one to grok :)  One thing that's important, is that Delay
 * needs a Scheduler to run, since it's a deferred operation. Scheduler.TaskPool
 * is a good choice.
 */

var sched = new TestScheduler();

var input = sched.CreateColdObservable(
    sched.OnNextAt(200, 5),
    sched.OnNextAt(300, 10),
    sched.OnCompletedAt<int>(1000)
);

input.Delay(TimeSpan.FromMilliseconds(1000), sched)
    .Timestamp(sched)
    .Select(x => new { Time = (x.Timestamp - start).TotalMilliseconds, Value = x.Value })
    .Dump();

sched.Start();
