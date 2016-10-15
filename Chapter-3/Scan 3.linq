<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

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

