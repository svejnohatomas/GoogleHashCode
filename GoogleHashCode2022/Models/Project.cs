namespace GoogleHashCode2022.Models
{
    internal class Project
    {
        public Project(string name, int duration, int score, int bestBefore, int rolesCount)
        {
            Name = name;
            Duration = duration;
            Score = score;
            BestBefore = bestBefore;
            RequiredContributorRoles = new List<Tuple<string, int>>(rolesCount);
        }

        public string Name { get; set; }
        public int Duration { get; set; }
        public int Score { get; set; }
        public int BestBefore { get; }
        public List<Tuple<string, int>> RequiredContributorRoles { get; set; }

        public LinkedList<Contributor> Contributors { get; set; } = new LinkedList<Contributor>();
    }
}
