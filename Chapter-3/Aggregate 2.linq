<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Aggregate 2:
 *
 * The main point of this sample is to illustrate that Rx's Aggregate returns an
 * IObservable<T> - this means that Observables that don't complete will never
 * return anything when sent through Aggregate!
 */

var sched = new TestScheduler();

var input = sched.CreateColdObservable(
    sched.OnNextAt(200, 5),
    sched.OnNextAt(300, 10)
    //sched.OnCompletedAt<int>(1000)
);

input.Aggregate(0, (acc, x) => acc + x).Dump();

sched.Start();
