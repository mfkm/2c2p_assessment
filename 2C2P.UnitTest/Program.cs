// See https://aka.ms/new-console-template for more information
List<int> someList = new List<int> { 3, 2, 4, 6, 8, 4, 5, 4, 3, 4, 5, 8, 0, 2, 1, 4, 6, 7, 5, 4 };
var uniqueVal = someList.Distinct().OrderBy(x => x).Select(x => new MyClass() { Number = x, Count = 0 }).ToList();
foreach (var val in uniqueVal)
    val.Count = someList.Where(x => val.Number == x).Count();
var highest = uniqueVal.OrderByDescending(x => x.Count).FirstOrDefault();
Console.WriteLine(highest.Number + ", " + highest.Count);
class MyClass
{
    public int Number { get; set; }
    public int Count { get; set; }
}


