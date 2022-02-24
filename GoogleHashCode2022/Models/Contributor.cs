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
    }
}
