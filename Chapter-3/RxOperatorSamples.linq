<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* How FromAsyncPattern works
 *
 * FromAsyncPattern takes the result of an async function and rebroadcasts it
 * onto an AsyncSubject - the advantage of this is that it turns the Hot
 * observable of an async call, and makes it Cold (only returning the last
 * result). This is really cool, because that means clients who subscribe after
 * the call returns *still* get the result.
 *
 * Let's see how we can take a Task and turn it into an Observable, similar to
 * what FromAsyncPattern does with the Begin/End pattern.
 */

public static class TaskObservifyMixin
{
	public static IObservable<T> Observify<T>(this Task<T> This)
	{
		var ret = new AsyncSubject<T>();
		
        // Bolt on a new Task that runs once the input task finishes
		This.ContinueWith(t => {
			if (t.Exception != null) {
				ret.OnError(t.Exception);
			} else {
				ret.OnNext(t.Result);
				ret.OnCompleted();
			}
		});
		
		return ret;
	}
}

void Main()
{
	var theTask = new Task<string>(() => {
		return RxBook.FetchWebpage("http://www.microsoft.com").First();
	});
	
	theTask.Start();
	
	var result = theTask.Observify();
	
	result.Dump();
	
	// Twiddle our thumbs until the task is finished
	while (!theTask.IsCompleted) {
		Thread.Sleep(1 * 1000);
	}
	
	
	result.Dump("If this was a Hot observable, I wouldn't see the text appear twice");
}


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

    << No Output >>


/* Concat 1:
 *
 * One thing to point out here, is that sometimes the *non* Extension method is
 * more readable than the extension method, especially for operations involving
 * 3+ Observables */

var firstPart = (new[] {1,2,3,4,5}).ToObservable();
var secondPart = (new[] {6,7,8,9,10}).ToObservable();
var thirdPart = (new[] {11,12,13,14,15}).ToObservable();

// extension
firstPart.Concat(secondPart.Concat(thirdPart)).Dump();

// non-extension
Observable.Concat(firstPart, secondPart, thirdPart).Dump();

    1
    2
    3
    4
    5
    6
    7
    8
    9
    10
    11
    12
    13
    14
    15


/* Concat 2:
 *
 * Concat can be very useful to serialize several async commands (i.e. run them
 * one at a time on a background thread, and wait for them all to complete in
 * order)
 */

var inputs = (new[] {
	"http://www.google.com",
	"http://www.yahoo.com",
	"http://www.bing.com",
});

inputs.Select(x => RxBook.FetchWebpage(x)).Concat().Dump();


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

    A 1
    B 1
    B 2


/* Defer
 *
 * Defer is a way to take a possibly Hot Observable and make it Cold via a Func.
 * This means, that we are creating an Observable that is only calculated when
 * someone actually Subscribes to it.
 *
 * Why would I want to do this? Well, remember that using Observable.Return
 * means that the value is calculated immediately, and maybe we want to be more
 * lazy about it (think perhaps looking up something in a database).
 */

int i = 2;

var input1 = Observable.Return(i);
var input2 = Observable.Defer(() => Observable.Return(i));

i = 10;

"Without Defer - captured 'i' when it was created".Dump();
input1.Subscribe(Console.WriteLine);

"Using Defer - we didn't capture 'i' until Dump()".Dump();
input2.Subscribe(Console.WriteLine);

    Without Defer - captured 'i' when it was created
    2

    Using Defer - we didn't capture 'i' until Dump()
    10


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

    { Time = 1200, Value = 5 } 
    { Time = 1300, Value = 10 } 


/* DistinctUntilChanged
 *
 * Not too complex - it's the Rx version of Posix's uniq, it filters out items
 * that were identical to the previous item. Note that this isn't quite the same
 * as LINQ's Distinct(), it doesn't guarantee numbers are *globally* distinct,
 * only that the value is different than the previous value.
 */


(new[] {1,1,1,1,2,2,2,2,3,4,5,5,5,5,1,1,1}).ToObservable()
    .DistinctUntilChanged()
    .Dump();

    1
    2
    3
    4
    5
    1


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

    5
    10
    20
    5
    10
    20


/* DoWhile 2:
 *
 * DoWhile is good for polling something in a loop using Defer (i.e. converting
 * something that's not very Rx'y into something usable in Rx).

// TODO: Figure out a sample fo this that doesn't have so many external
// dependencies


/* Expand 1:
 *
 * Expand is a new operator that's really useful in certain circumstances
 * related to walking trees asynchronously. Expand might be good for Chapter 6,
 * where we really melt people's brains
 */

// TODO: Take something based on the MeetingBill code, where we walk DLs which
// may or may not have more DLs


/* First 1:
 *
 * This is a very important operator and we will probably even introduce it
 * before Chapter 4, as it is the way to take a async method and make it sync.
 */

var input = (new[] {1,2,3,4,5}).ToObservable();
input.First().Dump();

    1


/* First 2:
 *
 * This sample is meant to illustrate an important concept, that First() can
 * deadlock if you use it on a Hot observable: since First() will block until an
 * item appears, if an item never appears the First() will block forever.
 */

var input = Observable.Return(4).Delay(TimeSpan.FromMilliseconds(10 * 1000));

"Start".Dump();

input.First().Dump();

"Finish - you won't see this until at least one item shows up!".Dump();

    Start
    4
    Finish - you won't see this until at least one item shows up!


/* First 3:
 *
 * Show how to take an Observable async function and make it sync
 */


// If we were to write this as a normal function, it would look like:
//
// public static IObservable<int> SomeFunc() 
// {
// }

Func<IObservable<int>> someFunc = new Func<IObservable<int>>(() => {
    // Pretend this was some time-consuming async calculation
    return Observable.Return(5).Delay(TimeSpan.FromSeconds(5));
});

// Now, let's make a synchronous version of this function, using the async one
// as our actual implementation, so now our method would normally look like:
//
// public static int SomeFuncSync()
// {
// }

Func<int> someFuncSync = new Func<int>(() => {
    // Use First to wait until the async method returns
    return someFunc().First();
});


someFuncSync().Dump();


/* ForkJoin 1:
 *
 * TODO: 
 */

var inputs = (new[] {
    "http://www.google.com",
    "http://www.yahoo.com",
    "http://www.bing.com",
});

Observable.ForkJoin(inputs.Select(x => RxBook.FetchWebpage(x))).Dump();


/* ForkJoin 2:
 *
 * TODO: 
 */


var inputs = (new[] {
    "http://www.google.com",
    "http://www.duckduckgo.com",
    "http://www.yahoo.com",
    "http://www.bing.com",
});

IObservable<string[]> output = Observable.ForkJoin(inputs.Select(url => 
    RxBook.FetchWebpage(url).Select(content =>
        new { url, content })));

foreach(var item in output.First()) {
    String.Format("'{0}' is of size {1}", item.url, item.content.Length).Dump();
}


/* Merge 1
 *
 * This is the simplest example of Merge, just take 5 results and return them
 */

Observable.Merge(
    Observable.Return(1),
    Observable.Return(2),
    Observable.Return(3),
    Observable.Return(4)
).Subscribe(x => {
    Console.WriteLine("Number {0}", x);
});

    Number 1
    Number 2
    Number 3
    Number 4


/* Merge 2
 *
 * This is a much more advanced example - Merge can take an
 * IObservable<IObservable<T>> and merge them down into an IObservable<T> -
 * it's the 2nd half of how SelectMany works.
 */

var inputs = (new[] {
    "http://www.google.com",
    "http://www.duckduckgo.com",
    "http://www.yahoo.com",
    "http://www.bing.com",
});

var output = inputs.Select(x => RxBook.FetchWebpage(x)).Merge();
output.Dump();


/* Multicast 1:
 *
 * Multicast plays an Observable onto a Subject - this is useful for making Cold
 * Observables into Hot Observables. Practically speaking, it's also useful in
 * class implementations where you want to return "an Observable that is based
 * on this other Observable, but every once in awhile I want to signal it
 * by-hand"
 */

// IEnumerable<T>.ToObservable() is a *Cold* Observable - every time you
// subscribe to it, you get a new copy.

var input = new[] {1,2,3}.ToObservable();

var output = input.Multicast(new Subject<int>());

// This doesn't do anything until Connect is called to actually connect the
// input to the Subject.
output.Dump();

Util.ReadLine("Press any key to continue");

output.Connect();


/* Scan 1:
 *
 * Scan is similar to Aggregate - however, whereas Aggregate only returns
 * exactly *one* value, Scan returns the accumulator value with every OnNext of
 * the input. This method is great for "running total" type of calculations.
 */

var stockChanges = new[] { 0.005, 0.01, 0.016, -0.021, -0.005, 0.03, -0.04, 0.00, -0.02, -0.005 }.ToObservable();

var initialPrice = 24.815;

var runningTotal = stockChanges.Scan(initialPrice, (acc, x) => acc + x);

runningTotal.Dump();

    24.82
    24.83
    24.846
    24.825
    24.82
    24.85
    24.81
    24.81
    24.79
    24.785


/* Scan 2:
 *
 * Scan can also be used in more clever ways, such as this example that
 * implements a reference count using two Subjects.
 *
 * (Select 'C# Program' here since we use a function)
 */

IObservable<int> ThrowIfBelowZero(int refCount)
{
	if (refCount >= 0) {
		return Observable.Return(refCount);
	}
	return Observable.Throw<int>(new Exception("Refcount dropped below Zero!"));
}

void Main()
{	
	var AddRef = new Subject<Unit>();
	var Release = new Subject<Unit>();
	
	var referenceCount = Observable.Merge(
			AddRef.Select(_ => 1),
			Release.Select(_ => -1))
		.Scan(0, (acc, x) => acc + x)
		.SelectMany(x => ThrowIfBelowZero(x));
	
	referenceCount.Subscribe(x => Console.WriteLine("Current RefCount is {0}", x));
	
	AddRef.OnNext(Unit.Default);
	AddRef.OnNext(Unit.Default);
	Release.OnNext(Unit.Default);
	AddRef.OnNext(Unit.Default);
	Release.OnNext(Unit.Default);
	Release.OnNext(Unit.Default);
	Release.OnNext(Unit.Default);
}

    Current RefCount is 1
    Current RefCount is 2
    Current RefCount is 1
    Current RefCount is 2
    Current RefCount is 1
    Current RefCount is 0
        
    Exception!


/* Scan 3:
 *
 * We can even make this code even more readable if we turn ThrowIfBelowZero
 * into its own Operator, via writing our own Extension Method.
 */

public static class ThrowObservableMixin
{
    public static IObservable<int> ThrowIfBelowZero(this IObservable<int> This)
    {
        return This.SelectMany(refCount => {
            if (refCount >= 0) {
                return Observable.Return(refCount);
            }
            return Observable.Throw<int>(new Exception("Refcount dropped below Zero!"));
        });
    }
}

void Main()
{	
	var AddRef = new Subject<Unit>();
	var Release = new Subject<Unit>();
	
	var referenceCount = Observable.Merge(
			AddRef.Select(_ => 1),
			Release.Select(_ => -1))
		.Scan(0, (acc, x) => acc + x)
        .ThrowIfBelowZero();
	
	referenceCount.Subscribe(x => Console.WriteLine("Current RefCount is {0}", x));
	
	AddRef.OnNext(Unit.Default);
	AddRef.OnNext(Unit.Default);
	Release.OnNext(Unit.Default);
	AddRef.OnNext(Unit.Default);
	Release.OnNext(Unit.Default);
	Release.OnNext(Unit.Default);
	Release.OnNext(Unit.Default);
}


/* SelectMany 1:
 *
 * SelectMany is one of the most powerful Rx operators - it allows you to
 * substutute any item in an Observable with zero, one, or many items. This is a
 * powerful idea in Linq, but even moreso in Rx.
 *
 * In this example, we'll take every item and make a duplicate if it's an even
 * number. So for example, every '2' will show up as '2, 2', but a '3' will only
 * show up in the output as '3'.
 */

var input = new[] {1,2,3}.ToObservable();
var output = input.SelectMany(x => {

    // Here we want to return an Observable that we will "splice into" the final
    // stream:
    if (x % 2 == 0) {
        return new[] {x, x}.ToObservable();
    } else {
        return new[] { x }.ToObservable();
    }
});

output.Dump();


/* SelectMany 2:
 *
 * Here's the lightbulb moment - if an Observable represents both an "Observable
 * list", and a Future async task, we can have a Observable List of Futures -
 * whose type would be IObservable<IObservable<T>> - how to we convert a list of
 * lists into a list? SelectMany!
 *
 * In practice, SelectMany allows you to chain web service or other async calls
 * - passing the result of one service to another.
 */

var inputs = (new[] {
    "http://www.google.com",
    "http://www.duckduckgo.com",
    "http://www.yahoo.com",
    "http://www.bing.com",
}).ToObservable();

var output = inputs.SelectMany(x => RxBook.FetchWebpage(x));
output.Dump();


/* Take 1:
 *
 * Return a subset of the items in the collection.
 */

var input = new[] {1,2,3,4,5,4,3,2,1}.ToObservable();
var output = input.Take(5).Select(x => x * 10);

output.Dump();


/* TakeUntil 1:
 *
 * TakeUntil is a version of TakeWhile that is signaled via another Observable -
 * in other words, it will "shut off the hose" when another Observable produces
 * any value.
 */

var input = new[] { 1, 2, 3 }.ToObservable();

input
    .Repeat()
    .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(2.0)))
    .Dump();


    1
    2
    3
    1
    2
    3
    1
    2
    3
    ... two seconds of this ...


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


/* Timeout 2:
 *
 * Let's see Timeout in combination with Retry and Defer, to make a highly
 * reliable web service call. We use the Defer here, because Retry only makes
 * sense with Cold observables.
 */

// Your web connection might actually be quick enough to pull google.com in time
// - if so, try changing the timeout to a lower value, or go to an airport and
// use their Wifi

Observable.Defer(() => RxBook.FetchWebpage("http://www.google.com"))
	.Timeout(TimeSpan.FromMilliseconds(750))
	.Retry(3)
	.OnErrorResumeNext(Observable.Return("Couldn't fetch the Website"))
	.Dump();


/* Zip 1:
 *
 * Zip allows us to merge two Observables, just like its Linq counterpart. Zip
 * creates two "slots" - whenever both slots have an item, it sends those two
 * items through the Selector function to create the result.
 *
 * One thing that's important to point out, is that Zip will throw away items
 * that it can't use, similar to CombineLatest (i.e. if the left side is already
 * full, it is replaced and the old one is lost). Unlike CombineLatest, Zip
 * won't yield anything until it has a *new* thing on both sides. 
 */

var leftSide = new[] {1,2,3,4}.ToObservable();
var rightSide = new[] { "A", "B", "C" }.ToObservable();

Observable.Zip(leftSide, rightSide, (num, letter) => num.ToString() + letter)
    .Dump();
	

// vim: ts=4 sw=4 tw=80 et :
