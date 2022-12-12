// See https://aka.ms/new-console-template for more information
using Tools.WMO;

Console.WriteLine("Hello, World!");

var files = Directory.GetFiles("OUTPUT", "*.wmo");

Console.WriteLine("Applying fixes...");

Parallel.ForEach(files, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, f =>
{
    WMO wmoFixer = new(f);
    wmoFixer.ApplyFixes();
});

Console.WriteLine("Done :) !");