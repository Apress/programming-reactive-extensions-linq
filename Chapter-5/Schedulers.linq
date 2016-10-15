<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Schedulers
 *
 * In both Rx and LINQ, one of the critical things to understand is, that you're
 * using Rx specifies *what* will happen*, not *in which context* it will
 * happen. That's really heady for anyone who hasn't done kernel programming, so
 * let me explain:
 *
 * In traditional imperative programming, any synchronization or running things
 * in another context requires an explicit action: your code will never suddenly
 * run in a background thread, you must type out "new Thread()" for code to run
 * in another thread. Locks don't happen magically, you must explicitly call
 * them out.
 *
 * Rx changes that - every time you write some code that runs in a Select() or
 * Where() operator, you should say to yourself, "I have no idea whether this is
 * on a UI thread, whether it's on a ThreadPool thread - who knows!". Rx will
 * happily run on any thread, many times a thread that's *different* than the
 * thread you created the pipeline on.
 *
 * If you only access local variables, or variables passed into your method,
 * this totally works! If you write in a purely functional way (i.e. you never
 * access variables outside of the method), or if you only use local variables
 * that you know won't be touched by more than one person at a time, this is
 * completely safe. 
 *
 * Rx is free to run your code wherever it wants, which is really powerful -
 * just like in LINQ where you can suddenly add AsParallel() and make your code
 * run on extra threads, with no other changes.
 *
 * However, many components aren't so flexible - if you're using technologies
 * such as WPF, Silverlight, or COM, they may require that objects are only
 * manipulated on certain threads. Or maybe, you are accessing an external
 * resource that can only be used by one person at a time. 
 *
 * By default, code is run in context - i.e. the fastest way to do some work is
 * to just do it immediately!  However, some operators such as Timer don't make
 * sense in an immediate context - they need to be deferred.
 *
 * To manipulate where code runs if we care, we use an IScheduler - operators
 * that have elements of concurrency such as ForkJoin or Timer will have an
 * IScheduler parameter, which lets you specify in which context the Timer will
 * run. Fortunately, the Rx team has already covered almost all of the cases you
 * care about:
 *
 *      * Scheduler.Immediate - Run the code immediately, don't schedule
 *      * Scheduler.NewThread - Run the code in a new thread
 *      * Scheduler.TaskPool - Run the code in a TPL Task - this is usually the
 *          one you want
 *      * Scheduler.ThreadPool - Run the code on the .NET 3.5 ThreadPool
 *      * Scheduler.CurrentThread - Like Immediate, but resolve dependencies so
 *          you don't deadlock yourself
 *      * DispatcherScheduler - Run the code on the WPF/Silverlight UI thread
 *      * ControlScheduler - Run the code on the WinForms UI thread
 */


Observable.Range(0, 20, Scheduler.TaskPool).Select(x => Thread.CurrentThread.ManagedThreadId).Dump("Taskpool Thread ID");

// Wait so we don't see results coming in from both queries in between each
// other
Thread.Sleep(5 * 1000);

Observable.Range(0, 20, Scheduler.Immediate).Select(x => Thread.CurrentThread.ManagedThreadId).Dump("Immediate Thread ID");

