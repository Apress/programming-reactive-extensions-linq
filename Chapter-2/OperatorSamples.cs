/* Deferred Execution 1:
 *
 * Demonstrate that execution occurs when a collection is enumerated, *not*
 * when the expression is evaluated, which is sometimes really useful!
 */

var thisShouldTakeALongTime = Enumerable.Range(0, 1000 * 1000).Select(x => {
    Thread.Sleep(1000);
    return x * 10;
});

Console.WriteLine("The 1st number is " + thisShouldTakeALongTime.First());

    The 1st number is 0


/* Deferred Execution 2:
 *
 * Sometimes deferred execution *isn't* useful, let's see how deferred
 * execution could cause us to have unexpected results:
 */

int counter = 0;
var evenNumbersInSeries = 
    Enumerable.Range(0, 10).Select(
	x => 
    {
       int result = x + counter;
       counter++;
       return result;
    });

// List the numbers in the series

Console.WriteLine("First Try:\n");
foreach(int i in evenNumbersInSeries) 
{
    Console.WriteLine(i);
}

// We're running the same code again here, we'll certainly get the same
// result, right?

Console.WriteLine("\nSecond Try:\n");
foreach(int i in evenNumbersInSeries) 
{
    Console.WriteLine(i);
}

    First Try:

    0
    2
    4
    6
    8
    10
    12
    14
    16
    18

    Second Try:

    10
    12
    14
    16
    18
    20
    22
    24
    26
    28


/* Deferred Execution 3:
 *
 * We can fix the broken code above by adding a ToArray operator to the
 * pipeline.
 * this turns the evenNumbersInSeries from an IEnumerable to an array and the array is populated before
 * the values are displayed
 */

int counter = 0;
var evenNumbersInSeries = Enumerable.Range(0, 10).Select(x => {
    int result = x + counter;
    counter++;
    return result;
}).ToArray();

// List the numbers in the series

Console.WriteLine("First Try:\n");
foreach(int i in evenNumbersInSeries) {
    Console.WriteLine(i);
}

// This time, because we added the ToArray(), we'll get the expected result
// every time.

Console.WriteLine("\nSecond Try:\n");
foreach(int i in evenNumbersInSeries) {
    Console.WriteLine(i);
}

    First Try:

    0
    2
    4
    6
    8
    10
    12
    14
    16
    18

    Second Try:

    0
    2
    4
    6
    8
    10
    12
    14
    16
    18


/* Take 1:
 *
 * Return a subset of the items in the collection.
 */

var input = new[] {1,2,3,4,5,4,3,2,1};
var output = input.Take(5).Select(x => x * 10);

output.Dump();

    10
    20
    30
    40
    50


/* Distinct 1:
 *
 * Distinct returns the set of unique items in a collection, removing any
 * duplicate items.
 */

var input = new[] {1,2,3,2,1,2,3,2,1,2,3,2,1};
input.Distinct().Dump();

    1
    2
    3


/* SelectMany 1:
 *
 * SelectMany lets you expand each item into either zero, one, or many items.
 * This is one of the more difficult operators to understand, but also one of
 * the most powerful.
 *
 * One way to think of how this operator works, is that it "flattens" a list
 * of lists - so if we have [[1,2,3], [4], [5, 6]], the result will be
 * [1,2,3,4,5,6]. This is easier to understand, but also hides some of
 * SelectMany's interesting uses. It's often better to think of SelectMany as,
 * "For each item in this list, I can replace it with whatever I want -
 * nothing, a single item, or another list". 
 *
 * This method will be even more useful for us when we look at the Reactive
 * version of it, where it is instrumental to helping us chain calls to
 * asynchronous methods (i.e. call 'A', pass the result to 'B', pass its
 * result to 'C', etc). 
 *
 * In this example, we will write a recursive method that finds all of the
 * files in a folder. 
 */

IEnumerable<string> GetFilesInAllSubdirectories(string root)
{
    var di = new System.IO.DirectoryInfo(root);

    // This line is the interesting one to grok - we are taking the stream of
    // all directories in the current folder, and for each one, expanding it
    // into all of the files in that directory. 

    return di.GetDirectories()
        .SelectMany(x => GetFilesInAllSubdirectories(x.FullName))
        .Concat(di.GetFiles().Select(x => x.FullName)); 
}

void Main()
{
    var allFilesOnDesktop = GetFilesInAllSubdirectories(
        System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

    allFilesOnDesktop.Dump();
}


// vim: ts=4 sw=4 tw=80 et :
