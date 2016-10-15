<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Aggregate 1:
 *
 * Simple example of using Aggregate to calculate the sum of an IObservable.
 * Important to point out that we reduce an IObservable of 'n' items into an
 * IObservable of one item.
 */

var sched = new TestScheduler();

var input = sched.CreateColdObservable(
	sched.OnNextAt(200, 5),
	sched.OnNextAt(300, 10),
	sched.OnCompletedAt<int>(1000)
);

input.Aggregate(0, (acc, x) => 
{
	Console.WriteLine("Acc: {0}, x: {1}", acc, x);
	return acc + x;	
}).Subscribe(Console.WriteLine);

sched.RunToMilliseconds(201);
sched.RunToMilliseconds(999);

Util.ReadLine("Press any key to continue");

sched.RunToMilliseconds(1001);
