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
        

        // Повна інформація про матч
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

        // Інформація про матчі у конкретну дату
        public void ShowMatchesByDate(DateTime date)
        {
            var matchesByDate = Matches.Where(match => match.Date.Date == date.Date).ToList();

            foreach (var match in matchesByDate)
            {
                Console.WriteLine($"Match ID: {match.MatchId}, Teams: {match.Team1.TeamName} vs {match.Team2.TeamName}, " +
                                  $"Goals: {match.Team1Goals}-{match.Team2Goals}, Scorer: {match.Scorer}, Date: {match.Date}");
            }
        }

        // Усі матчі конкретної команди
        public void ShowMatchesByTeam(string teamName)
        {
            var teamMatches = Matches.Where(match => match.Team1.TeamName == teamName || match.Team2.TeamName == teamName).ToList();

            foreach (var match in teamMatches)
            {
                Console.WriteLine($"Match ID: {match.MatchId}, Teams: {match.Team1.TeamName} vs {match.Team2.TeamName}, " +
                                  $"Goals: {match.Team1Goals}-{match.Team2Goals}, Scorer: {match.Scorer}, Date: {match.Date}");
            }
        }

        // Усі гравці, які забили голи у конкретну дату
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
                context.ShowFullMatchInfo();
                context.ShowMatchesByDate(DateTime.Now);
                context.ShowMatchesByTeam("Real Madrid");
                context.ShowScorersByDate(DateTime.Now);
            }
        }
    }

}
