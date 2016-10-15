<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Multicast, Publish, and IConnectableObservable
 *
 * Multicast and Publish are two methods that initially seem confusing - their
 * goal is to "return a Connectable Observable that shares a subscription to the
 * underlying source". 
 *
 * Before this makes sense, we have to understand subscription side-effects. In
 * a purely Functional world, there is no need for Publish or Multicast, as
 * the act of Subscribing would not affect the program in any way at all.
 * However, this is often not the case. FromEvent() will end up adding an Event
 * Handler for every subscription, Observable sources based on COM objects might
 * end up RefCounting the object, etc.
 *
 * Here's a simple example, using a Hot Observable (remember, that's a single
 * event stream), but explicit subscription side-effects added via Do().
 */


var sched = new TestScheduler();

var input = sched.CreateHotObservable(
    sched.OnNextAt(200, 1),
    sched.OnNextAt(300, 10),
    sched.OnNextAt(400, 100),
    sched.OnCompletedAt<int>(1100.0));

var sideEffected = input.Do(x => Console.WriteLine("Effects!"));

sideEffected.Subscribe(Console.WriteLine);
sideEffected.Subscribe(Console.WriteLine);

sched.Start();

