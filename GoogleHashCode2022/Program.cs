// See https://aka.ms/new-console-template for more information

using GoogleHashCode2022.IO;
using GoogleHashCode2022.Logic;

Console.WriteLine($"Program started at {DateTime.UtcNow}\n");

DirectoryInfo directoryInfo = new("./Input");
var files = directoryInfo.GetFiles();
foreach (var file in files)
{
    var input = new Parser().ParseFile(file.FullName);

    Console.WriteLine($"Processing file: {file.Name}:");
    Console.WriteLine($"- Number of projects: {input.Projects.Count}");
    Console.WriteLine($"- Number of contributors: {input.Contributors.Count}\n");

    Computer.Compute(input);

    new Dumper().Dump(input, file);
}

Console.WriteLine($"Program finished at {DateTime.UtcNow}\n");