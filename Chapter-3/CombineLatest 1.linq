<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* CombineLatest 1:
 * 
 * This one is going to be a little trickier to explain. CombineLatest is kind
 * of like Zip; it allocates two "slots" for each side - nothing happens until
 * we have at least one value for both slots.
 *
 * Once both slots are filled, both values are sent through the Selector (the
 * 3rd parameter). So far, we're just like Zip; however, unlike Zip,
 * CombineLatest "remembers" the latest value from both sides - so if a new
 * value comes in on Left Side, the Selector gets the new Left Side, and
 * whatever the last Right Side value was.
 *
 * So, in some senses, this operator is kind of like 'Or' - "Any time either
 * Observable changes, give me the latest value of Both sides". 
 */

var sched = new TestScheduler();

var leftSide = sched.CreateColdObservable(
    sched.OnNextAt(200, "A"),
    sched.OnNextAt(300, "B"),
    sched.OnCompletedAt<string>(1000)
);

var rightSide = sched.CreateColdObservable(
    sched.OnNextAt(50, "0"),
    sched.OnNextAt(100, "1"),
    sched.OnNextAt(500, "2"),
    sched.OnCompletedAt<string>(1000)
);

var result = Observable.CombineLatest(leftSide, rightSide,
    (left, right) => left + " " + right);

result.Dump();
sched.Start();
