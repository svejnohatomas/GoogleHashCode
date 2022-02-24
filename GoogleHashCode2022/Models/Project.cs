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

        public int LatestDateStart() => BestBefore - Duration;
        public int MaxScore(int startDay)
        {
            return startDay + Duration - 1 < BestBefore
                ? Score
                : Score - ((startDay + Duration - 1) - BestBefore + 1);
        }

        public int? StartDay { get; set; }

        public bool IsProjectEndDay(int day)
        {
            return StartDay.HasValue
                ? day == StartDay.Value + Duration
                : false;
        }

        public LinkedList<Contributor> Contributors { get; set; } = new LinkedList<Contributor>();
    }
}
