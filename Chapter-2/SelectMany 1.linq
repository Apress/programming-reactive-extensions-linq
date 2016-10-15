<Query Kind="Statements">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\Programming Reactive Extensions and LINQ\System.Reactive.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

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
