using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ConsoleApp1
{
    public class TournamentTable
    {
        [Key]
        public int Id { get; set; }
        public string TeamName { get; set; }
        public string City { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }

        public TournamentTable(string teamName, string city, int wins, int losses, int draws)
        {
            TeamName = teamName;
            City = city;
            Wins = wins;
            Losses = losses;
            Draws = draws;
        }
        public void Print()
        {
            Console.WriteLine($"Назва команди: {TeamName}");
            Console.WriteLine($"Місто команди: {City}");
            Console.WriteLine($"Кількість перемог: {Wins}");
            Console.WriteLine($"Кількість поразок: {Losses}");
            Console.WriteLine($"Кількість ігор у нічию: {Draws}");

        }
    }

    public class TournamentDbContext : DbContext
    {
        public string connectionString = @"Data Source=Gryhoriy\SQLEXPRESS;Initial Catalog=TournamentTableDB;Integrated Security=True;Encrypt=False";

        public TournamentDbContext()
        {
        }

        public DbSet<TournamentTable> TournamentTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo culture = new CultureInfo("uk-UA");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Console.OutputEncoding = Encoding.UTF8;

            TournamentTable tournament1= new TournamentTable("Барселона", "Барселона", 10, 2, 4);
            TournamentTable tournament2 = new TournamentTable("Реал Мадрид", "Мадрид", 8, 4, 2);
            TournamentTable tournament3 = new TournamentTable("Атлетіко Мадрид", "Мадрид", 6, 4, 4);

            try
            {
                using (var db = new TournamentDbContext())
                {
                    db.TournamentTable.Add(tournament1);
                    db.TournamentTable.Add(tournament2);
                    db.TournamentTable.Add(tournament3);
                    db.SaveChanges();

                    var tables = db.TournamentTable.ToList();

                    foreach (var table in tables)
                    {
                        Console.WriteLine("Під'єднано до бази");
                        table.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
    }
}