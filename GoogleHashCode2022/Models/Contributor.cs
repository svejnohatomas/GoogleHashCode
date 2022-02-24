namespace GoogleHashCode2022.Models
{
    internal class Contributor
    {
        public Contributor(string name, int skillsCount)
        {
            Name = name;
            Skills = new Dictionary<string, int>(skillsCount);
        }

        public string Name { get; }
        public Dictionary<string, int> Skills { get; }

        public LinkedList<Project> Projects { get; } = new();

        public bool IsAvailable(int day)
        {
            var project = Projects.Last?.Value;

            return project is null
                ? true
                : project.StartDay + project.Duration < day;
        }

        public bool IsInProject(Project project)
        {
            return project == Projects.Last?.Value;
        }
    }
}
