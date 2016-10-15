<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

// Here, we create a three-way And - just like Zip, we need an item to fill all
// three "slots". Since the third stream only produces an item at 900ms, we will
// only get one result, at 900ms. 
//
// Here, we only have one pattern, but we could add additional patterns here, to
// say, "Either ThisComplexPattern *or* ThatComplexPattern *or*
// SomeThirdPattern"
var join = Observable.When(
    lhs.And(rhs).And(trigger).Then((l,r,t) => String.Format("{0}:{1}:{2}", l,r,t))
);

// Print the output
join.Timestamp(sched)
    .Select(x => new { Time = x.Timestamp.Millisecond, x.Value })
    .Dump();

sched.Start();

