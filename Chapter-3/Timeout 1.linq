<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Timeout 1:
 *
 * Timeout allows you to either terminate an Observable via an Exception, or
 * replace the Observable with another Observable. This is very useful for web
 * service calls. 
 *
 * When using this operator with a TestScheduler, it's important to remember
 * that you pass in the Scheduler argument to Timeout!
 */

var sched = new TestScheduler();

var input = sched.CreateColdObservable(
	sched.OnNextAt(200, 5),
	sched.OnNextAt(300, 10),
	sched.OnCompletedAt<int>(1000)
);

input.Timeout(TimeSpan.FromMilliseconds(500), sched).Dump();

sched.Start();
