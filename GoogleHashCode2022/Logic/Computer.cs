using GoogleHashCode2022.Models;

namespace GoogleHashCode2022.Logic
{
    internal class Computer
    {
        public void Compute(Input input)
        {
            var latestProject = input.Projects.OrderByDescending(x => x.BestBefore + x.Duration).First();
            var simulationDuration = latestProject.BestBefore + latestProject.Duration;

            for (int day = 0; day < simulationDuration; day++)
            {
                var projects = GetProjects(input.Projects, day);
                if (projects.Count() == 0)
                {
                    break;
                }

                foreach (var project in projects)
                {
                    var contributors = GetContributors(project, input.Contributors, project.RequiredContributorRoles, day);
                    if (contributors is not null)
                    {
                        project.Contributors = contributors;

                        foreach (var contributor in contributors)
                        {
                            contributor.Projects.AddLast(project);
                        }

                        project.StartDay = day;
                    }
                }
            }
        }

        public IEnumerable<Project> GetProjects(IEnumerable<Project> projects, int day)
        {
            return projects
                .Where(x => x.StartDay is null)
                .OrderByDescending(x => x.MaxScore(day))
                .ThenBy(x => x.RequiredContributorRoles.Count);
        }

        public LinkedList<Contributor>? GetContributors(Project project, IEnumerable<Contributor> contributors, List<Tuple<string, int>> requiredContributorRoles, int day)
        {
            var resultContributors = new LinkedList<Contributor>();

            // Get available contributors
            contributors = contributors.Where(x => x.IsAvailable(day));

            if (contributors.Count() < requiredContributorRoles.Count)
            {
                return null;
            }

            foreach (var role in requiredContributorRoles)
            {
                var skillName = role.Item1;
                var skillLevel = role.Item2;

                var roleContributors = contributors
                    .Where(x => x.IsInProject(project) == false)
                    .OrderByDescending(x => GetContributorScore(x, skillName, skillLevel));

                if (roleContributors is null)
                {
                    return null;
                }

                if (roleContributors.Any() && GetContributorScore(roleContributors.First(), skillName, skillLevel) > 0)
                {
                    resultContributors.AddLast(roleContributors.First());
                } 
                else
                {
                    return null;
                }
            }

            return resultContributors;
        }

        private int GetContributorScore(Contributor contributor, string skillName, int skillLevel, bool hasMentor = false)
        {
            if (contributor.Skills.ContainsKey(skillName) == false)
            {
                return 0;
            }

            var contributorSkillLevel = contributor.Skills[skillName];

            if (contributorSkillLevel > skillLevel)
            {
                return 1;
            }
            else if (contributorSkillLevel == skillLevel)
            {
                return 2;
            }
            else if (contributorSkillLevel == skillLevel - 1 && hasMentor)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
    }
}
