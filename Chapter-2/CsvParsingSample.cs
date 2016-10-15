
// Open a Tab-Separated Values file - this format is similar to
// comma-separated values, except that the fields are separated by a Tab
// character.
//
// Our input data is the raw file as an array of strings, one per line. We can
// use Linq to parse the raw data out of the file, as well as calculating
// interesting statistics and reports from the raw data. 

var lines = File.ReadAllLines(@"beer.tsv");


// First, we want to read the column names from the first line of the file. In a
// non-Linq world, we might write code that looks like this, to generate a
// Dictionary whose keys are the column names, and whose values are the index of
// the column (i.e. "from_region => 6")

int i = 0;
var headers = new Dictionary<string, int>();

foreach(var header in lines[0].Split('\t')) {
    headers[header] = i;
    i++;
}

headers.Dump("Imperatively make a dictionary of headers");


// Here's how we could write the same thing in a purely functional way - we will
// merge the list of column names with the Range 0...100. This means that we'll
// take one item from the left side, one item from the right side, then put them
// together. The list that is made will always be the length of the shorter list
// (the column names). 
//
// Zip makes it easier to traverse two lists in lock-step, since the Zip
// operator handles the case where the lists aren't the same length.

headers = Enumerable.Zip(
        lines.First().Split('\t'),
        Enumerable.Range(0,100),
        (header, index) => new {header, index})
    .ToDictionary(k => k.header, v => v.index);
    
headers.Dump("The functional version returns the same result");


// We'll use this in future queries - skip the header line and split the lines
// into fields. This demonstrates a key aspect of Linq and Rx: Composition. We
// can take our input and transform it into a building block, which we can reuse
// again in further queries.

IEnumerable<string[]> allRecords = lines.Skip(1).Select(line => line.Split('\t'));


// Let's do something straightforward - what countries are represented in the
// dataset?

var allCountries = allRecords
    .Select(fields => fields[headers["from_region"]])
    .Distinct();

allCountries.Dump("Countries in the dataset");


// Let's ask a more complicated question - what are the five oldest beers?
// What's really cool about Linq and Rx, is that we can see here how our
// *output* (the result we wanted) is related to our Input (the records in the
// dataset). If we were to write the same code imperatively, this clear relation
// between output and input would be lost and unclear.

var oldestBeers = allRecords
	.Select(x => new { Name = x[headers["name"]], FirstBrewed = x[headers["first_brewed"]], })
	.Where(x => !String.IsNullOrWhiteSpace(x.FirstBrewed))	
	.OrderBy(x => {
		int ret = Int32.MaxValue;
		return (Int32.TryParse(x.FirstBrewed, out ret) ? ret : Int32.MaxValue);
	})
	.Take(5);

oldestBeers.Dump("Oldest Five Beers");


// Here's a more complicated example - let's calculate which country brews the
// strongest beer. The key thing to understand is, that first we are going to
// group all of the beers by the country of origin. 
//
// Then, for each country, we want two piece of information: the name of the
// country, and the *average* beer strength. Since we want to reduce a list into
// a single item (a number), we will use Aggregate to calculate the average.
//
// This data isn't perfectly formatted, so we'll also use Where to ignore
// records that are missing information.

var byRegion = allRecords.GroupBy(items => items[headers["from_region"]]);
    
byRegion
    .Select(group => new { 
        Country = group.Key, 
        Strength = group
            .Select(g => g[headers["alcohol_content"]])
            .Where(x => !String.IsNullOrWhiteSpace(x))
            .Aggregate(0.0, (acc, x) => acc + Double.Parse(x) / group.Count())
    })
    .Where(x => x.Strength > 0.0)
    .OrderByDescending(x => x.Strength)
    .ThenBy(x => x.Country)
    .Dump();

// vim: ts=4 sw=4 tw=80 et :
