using System;

namespace GoogleHashCode2020
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime startTime = DateTime.UtcNow;

            string[] inputFiles = {
                "a_example",
                "b_read_on",
                "c_incunabula",
                "d_tough_choices",
                "e_so_many_books",
                "f_libraries_of_the_world"
            };
            string inputRoot = @"C:\Users\Tomas\Documents\GitHub\SwanseaCompSci\GoogleHashCode\GoogleHashCode2020\input";
            string outputRoot = @"C:\Users\Tomas\Documents\GitHub\SwanseaCompSci\GoogleHashCode\GoogleHashCode2020\output";

            foreach (string item in inputFiles)
            {
                string loadPath = $@"{inputRoot}\{item}.txt";
                string savePath = $@"{outputRoot}\output_{item}.txt";
                _ = new Solution(loadPath, savePath);
            }

            DateTime endTime = DateTime.UtcNow;

            Console.WriteLine(endTime - startTime);
        }
    }
}
