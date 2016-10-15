<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

/* Window and Buffer
 *
 * Window and Buffer (Window's easier to use yet less powerful cousin) are
 * operators that allow you to cut an input stream into "samples" in a variety
 * of very powerful ways.
 *
 * Let's talk about Window first - the core idea of Window is, that we have an
 * input stream, and a number of "Windows" that have a start and end. For
 * example, "From 3pm to 4:30pm", or maybe "from when they click the button,
 * until they let go" - another common Window is simply, "once we've seen 'n'
 * items".
 *
 * Window returns an IObservable<IObservable<T>> - a "future list of future
 * lists"; the outer sequence represents a list of windows, and each item in the
 * Window list is a list of items that happened in the Window. Confused yet? :)
 *
 * While Window is very powerful, you often don't care about the *timing* of the
 * items in each window, you just want to find out the list of things that fell
 * into each Window. This is where Buffer comes in - it works just like Window,
 * but returns an easier-to-manage, yet less informative type:
 * IObservable<IList<T>>. In other words, "an Observable of Lists", telling us
 * what items fell into each Window.
 *
 * Even though these methods can be really complex, there are overloads that
 * cover a lot of the practical use-cases that are much easier to grok.
 */


//
// The simplest Buffer we can possibly do - just take items two at a time
//

var input = new[] {1,2,3,4,5}.ToObservable();

// Split the input into pieces, with a buffer size of two
int i = 0;
input.Buffer(2).Subscribe(list => {
    // We've got the items, but we've lost the timings of when they happened
    // since list is just a simple IList
    list.Dump(String.Format("List {0}", ++i));
});
