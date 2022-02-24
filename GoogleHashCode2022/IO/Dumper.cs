using GoogleHashCode2022.Models;
using System.Text;

namespace GoogleHashCode2022.IO
{
    internal class Dumper
    {
        public void Dump(Input input, FileInfo fileInfo)
        {
            Directory.CreateDirectory(fileInfo.FullName.Replace("Input", "Output").Replace(fileInfo.Name, ""));

            var projects = GetProjects(input.Projects);

            using (var writer = new StreamWriter(fileInfo.FullName.Replace("Input", "Output").Replace(".in.txt", ".out.txt")))
            {
                writer.WriteLine(projects.Count());

                foreach (var project in projects)
                {
                    writer.WriteLine(project.Name);

                    var sb = new StringBuilder();
                    foreach (var item in project.Contributors)
                    {
                        sb.Append($"{item.Name} ");
                    }
                    sb.Remove(sb.Length - 1, 1);

                    writer.WriteLine(sb.ToString());
                }
            }
        }

        public IEnumerable<Project> GetProjects(IEnumerable<Project> projects)
        {
            return projects.Where(x => x.Contributors.Count > 0);
        }
    }
}
