using GoogleHashCode2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleHashCode2022.Logic
{
    internal static class Computer
    {
        public static void Compute(Input input)
        {
            var project = input.Projects.FirstOrDefault();

            for (int i = 0; i < 3; i++)
            {
                project!.Contributors.AddLast(input.Contributors[i]);
            }
        }
    }
}
