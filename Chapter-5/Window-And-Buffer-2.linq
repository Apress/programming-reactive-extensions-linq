<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

//
// The same example, only illustrating how Window is different
//

var input = new[] {1,2,3,4,5}.ToObservable();

int i = 0;

// Subscribe to an Observable of Observables
input.Window(2).Subscribe(obs => {
    int current = ++i;
    Console.WriteLine("Started Observable {0}", current);

    // Subscribe to the inner Observable and print its items
    obs.Subscribe(
        item => Console.WriteLine("    {0}", item), 
        () => Console.WriteLine("Ended Observable {0}", current));
});

