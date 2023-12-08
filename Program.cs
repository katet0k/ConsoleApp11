using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
    public class GameInfo
    {
        [Key]
        public int GameId { get; set; }
        public string Name { get; set; }
        public string Studio { get; set; }
        public string Style { get; set; }
        public DateTime ReleaseDate { get; set; }

        public GameInfo(string name, string studio, string style, DateTime releaseDate)
        {
            Name = name;
            Studio = studio;
            Style = style;
            ReleaseDate = releaseDate;
        }
        public void Print()
        {
            Console.WriteLine($"Назва гри: {Name}");
            Console.WriteLine($"Студія: {Studio}");
            Console.WriteLine($"Стиль: {Style}");
            Console.WriteLine($"Дата релізу: {ReleaseDate}");

        }
    }

    public class GameDbContext : DbContext
    {
        public string connectionString = @"Data Source=Gryhoriy\SQLEXPRESS;Initial Catalog=DBGame;Integrated Security=True;Encrypt=False";

        public GameDbContext()
        {
        }

        public DbSet<GameInfo> GameInfo { get; set; }

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

            GameInfo game1 = new GameInfo("Genshin Impact", "miHoYo", "RPG", new DateTime(2023, 11, 08));

            try
            {
                using (var db = new GameDbContext())
                {
                    db.GameInfo.Add(game1);
                    db.SaveChanges();

                    var games = db.GameInfo.ToList();

                    foreach (var game in games)
                    {
                        Console.WriteLine("Під'єднано до бази");
                        game.Print();
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
