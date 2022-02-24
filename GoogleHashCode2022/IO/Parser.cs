using GoogleHashCode2022.Models;

namespace GoogleHashCode2022.IO
{
    internal class Parser
    {
        public Input ParseFile(string path)
        {
            Input input = null!;

            using (var reader = new StreamReader(path))
            {
                var firstLine = reader.ReadLine().Split(' ');

                var numberOfContributors = Convert.ToInt32(firstLine[0]);
                var numberOfProjects = Convert.ToInt32(firstLine[1]);

                input = new Input
                {
                    Contributors = new List<Contributor>(numberOfContributors),
                    Projects = new List<Project>(numberOfProjects)
                };

                for (int i = 0; i < numberOfContributors; i++)
                {
                    var contributorLine = reader.ReadLine().Split(' ');

                    var contributorName = contributorLine[0];
                    var contributorSkillsCount = Convert.ToInt32(contributorLine[1]);

                    var contributor = new Contributor(contributorName,
                                                      contributorSkillsCount);

                    for (int c = 0; c < contributorSkillsCount; c++)
                    {
                        var skillLine = reader.ReadLine().Split(' ');

                        var skillName = skillLine[0];
                        var skillLevel = Convert.ToInt32(skillLine[1]);

                        contributor.Skills.Add(skillName, skillLevel);
                    }

                    input.Contributors.Add(contributor);
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    var projectLine = reader.ReadLine().Split(' ');

                    var projectName = projectLine[0];
                    var projectDuration = Convert.ToInt32(projectLine[1]);
                    var projectScore = Convert.ToInt32(projectLine[2]);
                    var projectBestBefore = Convert.ToInt32(projectLine[3]);
                    var projectRolesCount = Convert.ToInt32(projectLine[4]);

                    var project = new Project(projectName,
                                              projectDuration,
                                              projectScore,
                                              projectBestBefore,
                                              projectRolesCount);

                    for (int r = 0; r < projectRolesCount; r++)
                    {
                        var roleLine = reader.ReadLine().Split(' ');

                        var roleSkillName = roleLine[0];
                        var roleSkillLevel = Convert.ToInt32(roleLine[1]);

                        project.RequiredContributorRoles.Add(
                            new Tuple<string, int>(roleSkillName, roleSkillLevel));
                    }

                    input.Projects.Add(project);
                }
            }

            return input;
        }
    }
}
