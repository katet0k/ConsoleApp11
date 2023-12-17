using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public List<Player> Players { get; set; }
        public List<Match> Matches { get; set; }
        public int TotalGoalsScored => Matches.Sum(m => m.Team1Id == TeamId ? m.Team1Goals : m.Team2Goals);
        public int TotalGoalsConceded => Matches.Sum(m => m.Team1Id == TeamId ? m.Team2Goals : m.Team1Goals);

    }

    public class Player
    {
        public int PlayerId { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public int JerseyNumber { get; set; }
        public string Position { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public List<Goal> Goals { get; set; }
    }
    public class Goal
    {
        public int GoalId { get; set; }
        public int PlayerId { get; set; }
        public Player Scorer { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }
    }

    public class Match
    {
        public int MatchId { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public int Team1Goals { get; set; }
        public int Team2Goals { get; set; }
        public string Scorer { get; set; }
        public DateTime Date { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
    }

    public class FootballDbContext : DbContext
    {
        public string ConnectionString { get; set; } = @"Data Source=Gryhoriy\SQLEXPRESS;Initial Catalog=Spanish_football;Integrated Security=True;Encrypt=False";

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        

        public void ShowFullMatchInfo()
        {
            var matchesInfo = Matches.Select(match => new
            {
                MatchId = match.MatchId,
                Team1 = match.Team1.TeamName,
                Team2 = match.Team2.TeamName,
                Team1Goals = match.Team1Goals,
                Team2Goals = match.Team2Goals,
                Scorer = match.Scorer,
                Date = match.Date
            }).ToList();

            foreach (var matchInfo in matchesInfo)
            {
                Console.WriteLine($"Match ID: {matchInfo.MatchId}, Teams: {matchInfo.Team1} vs {matchInfo.Team2}, " +
                                  $"Goals: {matchInfo.Team1Goals}-{matchInfo.Team2Goals}, Scorer: {matchInfo.Scorer}, Date: {matchInfo.Date}");
            }
        }

        public void ShowMatchesByDate(DateTime date)
        {
            var matchesByDate = Matches.Where(match => match.Date.Date == date.Date).ToList();

            foreach (var match in matchesByDate)
            {
                Console.WriteLine($"Match ID: {match.MatchId}, Teams: {match.Team1.TeamName} vs {match.Team2.TeamName}, " +
                                  $"Goals: {match.Team1Goals}-{match.Team2Goals}, Scorer: {match.Scorer}, Date: {match.Date}");
            }
        }

        public void ShowMatchesByTeam(string teamName)
        {
            var teamMatches = Matches.Where(match => match.Team1.TeamName == teamName || match.Team2.TeamName == teamName).ToList();

            foreach (var match in teamMatches)
            {
                Console.WriteLine($"Match ID: {match.MatchId}, Teams: {match.Team1.TeamName} vs {match.Team2.TeamName}, " +
                                  $"Goals: {match.Team1Goals}-{match.Team2Goals}, Scorer: {match.Scorer}, Date: {match.Date}");
            }
        }

        public void ShowScorersByDate(DateTime date)
        {
            var scorersByDate = Players
                .Where(player => Matches.Any(match => match.Scorer == player.FullName && match.Date.Date == date.Date))
                .ToList();

            foreach (var scorer in scorersByDate)
            {
                Console.WriteLine($"Player ID: {scorer.PlayerId}, Name: {scorer.FullName}, Team: {scorer.Team.TeamName}");
            }
        }
        public void DisplayTop3ScorersForTeam(int teamId)
        {
            var topScorers = Players.Where(p => p.TeamId == teamId)
                                    .OrderByDescending(p => p.Goals.Count)
                                    .Take(3)
                                    .ToList();

            Console.WriteLine($"Топ-3 бомбардири команди {teamId}:");
            foreach (var scorer in topScorers)
            {
                Console.WriteLine($"Гравець: {scorer.FullName}, Кількість голів: {scorer.Goals.Count}");
            }
        }

        public void DisplayBestScorerForTeam(int teamId)
        {
            var bestScorer = Players.Where(p => p.TeamId == teamId)
                                    .OrderByDescending(p => p.Goals.Count)
                                    .FirstOrDefault();

            if (bestScorer != null)
            {
                Console.WriteLine($"Найкращий бомбардир команди {teamId}:");
                Console.WriteLine($"Гравець: {bestScorer.FullName}, Кількість голів: {bestScorer.Goals.Count}");
            }
            else
            {
                Console.WriteLine($"Немає бомбардирів у команді {teamId}.");
            }
        }

        public void DisplayTop3ScorersInLeague()
        {
            var topScorers = Players.OrderByDescending(p => p.Goals.Count)
                                    .Take(3)
                                    .ToList();

            Console.WriteLine("Топ-3 бомбардири усього чемпіонату:");
            foreach (var scorer in topScorers)
            {
                Console.WriteLine($"Гравець: {scorer.FullName}, Кількість голів: {scorer.Goals.Count}");
            }
        }

        public void DisplayBestScorerInLeague()
        {
            var bestScorer = Players.OrderByDescending(p => p.Goals.Count)
                                    .FirstOrDefault();

            if (bestScorer != null)
            {
                Console.WriteLine($"Найкращий бомбардир усього чемпіонату:");
                Console.WriteLine($"Гравець: {bestScorer.FullName}, Кількість голів: {bestScorer.Goals.Count}");
            }
            else
            {
                Console.WriteLine("Немає бомбардирів у чемпіонаті.");
            }
        }
        public void DisplayTop3TeamsByGoalsScored()
        {
            var topTeams = Teams.OrderByDescending(t => t.TotalGoalsScored)
                                .Take(3)
                                .ToList();

            Console.WriteLine("Топ-3 команди за кількістю забитих голів:");
            foreach (var team in topTeams)
            {
                Console.WriteLine($"Команда: {team.TeamName}, Забито голів: {team.TotalGoalsScored}");
            }
        }

        public void DisplayTeamWithMostGoalsScored()
        {
            var topTeam = Teams.OrderByDescending(t => t.TotalGoalsScored)
                               .FirstOrDefault();

            if (topTeam != null)
            {
                Console.WriteLine($"Команда, яка забила найбільше голів:");
                Console.WriteLine($"Команда: {topTeam.TeamName}, Забито голів: {topTeam.TotalGoalsScored}");
            }
            else
            {
                Console.WriteLine("Немає даних про команди.");
            }
        }

        public void DisplayTop3TeamsByGoalsConceded()
        {
            var topTeams = Teams.OrderBy(t => t.TotalGoalsConceded)
                                .Take(3)
                                .ToList();

            Console.WriteLine("Топ-3 команди за кількістю пропущених голів:");
            foreach (var team in topTeams)
            {
                Console.WriteLine($"Команда: {team.TeamName}, Пропущено голів: {team.TotalGoalsConceded}");
            }
        }

        public void DisplayTeamWithLeastGoalsConceded()
        {
            var topTeam = Teams.OrderBy(t => t.TotalGoalsConceded)
                               .FirstOrDefault();

            if (topTeam != null)
            {
                Console.WriteLine($"Команда, яка пропустила найменше голів:");
                Console.WriteLine($"Команда: {topTeam.TeamName}, Пропущено голів: {topTeam.TotalGoalsConceded}");
            }
            else
            {
                Console.WriteLine("Немає даних про команди.");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new FootballDbContext())
            {
                Console.WriteLine("Топ-3 найкращих бомбардирів команди:");
                context.DisplayTop3ScorersForTeam(teamId);

                Console.WriteLine("\nНайкращий бомбардир команди:");
                context.DisplayBestScorerForTeam(teamId);

                Console.WriteLine("\nТоп-3 найкращих бомбардирів усього чемпіонату:");
                context.DisplayTop3ScorersInLeague();

                Console.WriteLine("\nНайкращий бомбардир усього чемпіонату:");
                context.DisplayBestScorerInLeague();

                Console.WriteLine("Покажіть Топ-3 команди, які забили найбільше голів:");
                context.DisplayTop3TeamsByGoalsScored();

                Console.WriteLine("\nПокажіть команду, яка забила найбільше голів:");
                context.DisplayTeamWithMostGoalsScored();

                Console.WriteLine("\nПокажіть Топ-3 команди, які пропустили найменше голів:");
                context.DisplayTop3TeamsByGoalsConceded();

                Console.WriteLine("\nПокажіть команду, яка пропустила найменше голів:");
                context.DisplayTeamWithLeastGoalsConceded();
            }
        }
    }

}
