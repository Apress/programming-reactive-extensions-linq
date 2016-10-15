<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* 9 overloads and 10 Buffer implementations covering a variety of use-cases are
 * all derived from this one method - that's pretty powerful!
 *
 * Here's how we could use the core Window method - it's important to point out
 * that despite the fact that I'm using time to determine the shape of my
 * Windows, I could use *any* IObservable.
 */

var sched = new TestScheduler();

var input = sched.CreateColdObservable(
    sched.OnNextAt(205, 1),
    sched.OnNextAt(305, 10),
    sched.OnNextAt(405, 100),
    sched.OnNextAt(505, 1000),
    sched.OnNextAt(605, 10000),
    sched.OnCompletedAt<int>(1100.0));

int i = 0;
var windows = input.Window(
        // We're going to start a window every 100 milliseconds..
        Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(100), sched).Take(7),
        // ..and then close it 50ms later.
        x => Observable.Timer(TimeSpan.FromMilliseconds(50), sched));

windows
    .Timestamp(sched)
    .Subscribe(obs => {
        int current = ++i;
        Console.WriteLine("Started Observable {0} at {1}ms", current, obs.Timestamp.Millisecond);

        // Subscribe to the inner Observable and print its items
        obs.Value.Subscribe(
            item => Console.WriteLine("    {0} at {1}ms", item, sched.Now.Millisecond), 
            () => Console.WriteLine("Ended Observable {0} at {1}ms\n", current, sched.Now.Millisecond));
    });
    
sched.Start();

