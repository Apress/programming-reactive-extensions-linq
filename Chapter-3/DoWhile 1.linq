<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* DoWhile 1:
 *
 * A pretty useful operator actually, once you grok it, especially when you
 * combine it with Defer - DoWhile keeps repeating the sequence over and over
 * (as if via Repeat()), until the Func is false. 
 * Because we Subscribe repeatedly, using DoWhile with a Hot observable like an
 * Event, you'll just deadlock yourself.
 */

var sched = new TestScheduler();

var input = sched.CreateColdObservable(
    sched.OnNextAt(200, 5),
    sched.OnNextAt(300, 10),
    sched.OnNextAt(400, 20),
    sched.OnNextAt(500, 30),
    sched.OnNextAt(600, 35),
    sched.OnCompletedAt<int>(1000)
);

int times = 3;
input.DoWhile(() => (--times) > 0).Dump();

sched.Start();
