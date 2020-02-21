using System;
using System.Linq;
using System.Threading;

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
            string root = @"C:\Users\Tomas\Documents\GitHub\svejnohatomas\GoogleHashCode\GoogleHashCode2020";

            Thread[] threads = new Thread[inputFiles.Length];

            for (int i = 0; i < inputFiles.Length; i++)
            {
                string item = inputFiles[i];
                string loadPath = $@"{root}\input\{item}.txt";
                string savePath = $@"{root}\output\output_{item}.txt";
                Solution solution = new Solution(loadPath, savePath);
                threads[i] = new Thread(new ThreadStart(solution.Start));
            }

            foreach (Thread item in threads)
            {
                item.Start();
                while (item.IsAlive) { }
            }

            while (threads.Any(x => x.IsAlive))
            {

            }

            DateTime endTime = DateTime.UtcNow;

            Console.WriteLine(endTime - startTime);
        }
    }
}
