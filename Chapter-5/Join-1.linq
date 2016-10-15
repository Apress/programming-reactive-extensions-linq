<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Join Patterns
 *
 * Join Patterns are like a superpowered version of Zip, they allow you to
 * combine streams using "And" and "Or"s, which let you match more complicated
 * patterns.
 *
 * The main pattern we can express is "Tell me when either of { A && B && C, D
 * && E } happens". To this end, we have three main methods to do this.
 *
 *      When - at the very end, says "Tell me when any of the patterns happen"
 *      And -  Creates a join pattern by saying "Wait until A and B produce an
 *             item." - you can keep writing 'And' to say "Wait for A+B+C"
 *      Then - Once you finish combining things using 'And', 'Then' works like
 *             Select: it allows you to combine the result into a value.
 */

// Arbitrary Example 1

var sched = new TestScheduler();

var lhs = sched.CreateColdObservable(
    sched.OnNextAt(200, 1),
    sched.OnNextAt(300, 10),
    sched.OnNextAt(400, 100),
    sched.OnNextAt(500, 1000),
    sched.OnNextAt(600, 10000),
    sched.OnCompletedAt<int>(1100.0));

var rhs = sched.CreateColdObservable(
    sched.OnNextAt(250, "A"),
    sched.OnNextAt(650, "B"),
    sched.OnNextAt(850, "C"),
    sched.OnCompletedAt<string>(1000));
    
var trigger = sched.CreateColdObservable(
    sched.OnNextAt(900, 4.4),
    sched.OnCompletedAt<double>(901));

