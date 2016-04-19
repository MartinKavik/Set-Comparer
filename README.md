# SetComparer

## Library provides
- **a)** A way to accept a string representing a new set of values delimited by comma sign “,” (e.g. “1,2,3”) and return true/false if the given set is a duplicate of a set seen before

**Example code**
```c#
IntSetComparer comp = new IntSetComparer();
bool add1 = comp.AddUniqueSet("1,2,3");     // unique
bool add2 = comp.AddUniqueSet("1,3,2");     // duplicate
Console.WriteLine("Add1 = {0}\nAdd2 = {1}", add1, add2);
```
**Output**
```
Add1 = True         // set is unique
Add2 = False        // set is duplicate
```
---
- **b)** A way to return an information on number of duplicates and non-duplicates seen so far

**Example code**
```c#
// ... (code from a) ...
(new List<string>{
    "56,69,26,0",   // unique
    "3,2,1",        // 2. duplicate
    "2,156",        // unique
    "156,2",        // duplicate
    "32"            // unique
}).ForEach(s => comp.AddUniqueSet(s));
int nonDuplicates = comp.GetUniquesCount();
int duplicates = comp.GetDuplicatesCount();            
Console.WriteLine("Non-duplicates = {0}\nDuplicates = {1}", nonDuplicates, duplicates);
```
**Output**
```
Non-duplicates = 4
Duplicates = 3
```
---
- **c)** A way to list members of the most frequent duplicate group seen so far

**Example code**
```c#
// ... (code from a,b) ...
Console.WriteLine("Most frequent duplicate group: {0}", comp.GetMostFrequentSet());
```
**Output**
```
Most frequent duplicate group: 1,2,3
```
---
- **d)** A way to return human readable report on list of invalid inputs seen so far

**Example code**
```c#
// ... (code from a,b,c) ...
(new List<string>{
    "56,,0",  
    null,              
    "156,ab"
}).ForEach(s => comp.AddUniqueSet(s));
comp.GetInvalidInputsReport(Console.Out);
```
**Output**
```xml
<?xml version="1.0" encoding="ibm852"?>
<Report xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ReportItem>
    <Timestamp>2016-03-18 17:56:08.013</Timestamp>
    <Set>56,,0</Set>
  </ReportItem>
  <ReportItem>
    <Timestamp>2016-03-18 17:56:08.028</Timestamp>
    <Set>null</Set>
  </ReportItem>
  <ReportItem>
    <Timestamp>2016-03-18 17:56:08.036</Timestamp>
    <Set>156,ab</Set>
  </ReportItem>
</Report>
```
---
***Notes*** 
- Examples are available in project *ConsoleApp*, *Program.cs*
- Complexity of default algorithm for duplicates detection is **O(N)**

---
## Extensions
It is possible to extend the library by adding support for other data-types.

**How to create extension** (data type *string*):

**1)** Create class derived from *SetBase<string>*

*Sets/**StringSet**.cs*
```c#
using System.Collections.Generic;
using System.Linq;

namespace SetComparer.Sets
{
    public class StringSet : SetBase<string>
    {
        public StringSet(List<string> set) : base(set) { }
        public StringSet(string set) : base(set) { }
        public StringSet() { }

        public override List<string> Parse(string set)
        {
            if (set == null)
            {
                ThrowAndLogFormatException(set);
            }
            return set.Split(',').ToList();
        }
    }
}
```
**2)** Override *IsDuplicate* and *ToString* methods if necessary

**3)** Create class derived from *SetComparer<StringSet, string>*

*Comparers/**StringSetComparer**.cs*
```c#
using SetComparer.Sets;

namespace SetComparer.Comparers
{
    public class StringSetComparer : SetComparer<StringSet, string> { }
}
```
**4)** Add comments and unit tests

## 3rd-party libraries
- [**DeepCloner**](https://www.nuget.org/packages/DeepCloner/) in project *SetComparerTesting*

## Development
- IDE: **Microsoft Visual Studio Community 2015**
- Target framework: **.Net Framework 4.5.2** (default)
- Markdown editor: [**dillinger.io**](http://dillinger.io/)
- OS: **Windows 10** x64

## Testing
- Project: **SetComparerTesting**
- Unit tests cover class ***IntSetComparer*** (resp. its ancestor ***SetComparer****\<IntSet, int\>*)
- Files used in tests: ***/TestFiles/*** (*Note*: **input.txt** is encoded in *Windows-1252 (ANSI)*)
