using System;
using System.Collections.Generic;
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
        public bool Multiplayer { get; set; }
        public int Sales { get; set; }
        public string StudioCountry { get; set; } 
        public string StudioCity { get; set; }
        public GameInfo(string name, string studio, string style, DateTime releaseDate, bool multiplayer, int sales, string studioCountry, string studioCity)
        {
            Name = name;
            Studio = studio;
            Style = style;
            ReleaseDate = releaseDate;
            Multiplayer = multiplayer;
            Sales = sales;
            StudioCountry = studioCountry;
            StudioCity = studioCity;
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

        public void DisplaySingleMultiplayerGamesCount()
        {
            var count = GameInfo.Count(g => !g.Multiplayer);
            Console.WriteLine($"Кількість однокористувацьких ігор: {count}");
        }

        public void DisplayMultiplayerGamesCount()
        {
            var count = GameInfo.Count(g => g.Multiplayer);
            Console.WriteLine($"Кількість багатокористувацьких ігор: {count}");
        }

        public void DisplayGameWithMaxSalesByStyle(string style)
        {
            var game = GameInfo.Where(g => g.Style == style).OrderByDescending(g => g.Sales).FirstOrDefault();
            if (game != null)
            {
                Console.WriteLine($"Гра з максимальною кількістю проданих ігор стилю '{style}':");
                game.Print();
            }
            else
            {
                Console.WriteLine($"Гри зі стилем '{style}' не знайдено.");
            }
        }

        public void DisplayTop5GamesByMaxSalesByStyle(string style)
        {
            var top5Games = GameInfo.Where(g => g.Style == style).OrderByDescending(g => g.Sales).Take(5).ToList();
            Console.WriteLine($"Топ-5 ігор за найбільшою кількістю продажів у стилі '{style}':");
            foreach (var game in top5Games)
            {
                game.Print();
                Console.WriteLine(" ");
            }
        }

        public void DisplayTop5GamesByMinSalesByStyle(string style)
        {
            var top5Games = GameInfo.Where(g => g.Style == style).OrderBy(g => g.Sales).Take(5).ToList();
            Console.WriteLine($"Топ-5 ігор за найменшою кількістю продажів у стилі '{style}':");
            foreach (var game in top5Games)
            {
                game.Print();
                Console.WriteLine(" ");
            }
        }

        public void DisplayFullGameInfo()
        {
            var games = GameInfo.ToList();
            Console.WriteLine("Повна інформація про гри:");
            foreach (var game in games)
            {
                game.Print();
                Console.WriteLine(" ");
            }
        }

        public void DisplayStudiosAndStylesWithMoreGames()
        {
            var studiosAndStyles = GameInfo.GroupBy(g => new { g.Studio, g.Style })
                                           .Where(group => group.Count() > 1)
                                           .Select(group => new
                                           {
                                               Studio = group.Key.Studio,
                                               Style = group.Key.Style,
                                               GamesCount = group.Count()
                                           }).ToList();

            Console.WriteLine("Назва кожної студії та стиль ігор, кількість яких переважає по створенню у цьому стилі:");
            foreach (var item in studiosAndStyles)
            {
                Console.WriteLine($"Студія: {item.Studio}, Стиль: {item.Style}, Кількість ігор: {item.GamesCount}");
            }
        }
    
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

            GameInfo game1 = new GameInfo("Genshin Impact", "miHoYo", "RPG", new DateTime(2023, 11, 08), true, 1000, "", "");
            GameInfo game2 = new GameInfo("Temple Run 2", "Imangi Studios", "Нескінченний бігун", new DateTime(2015, 01, 16), true, 900, "", "");
            GameInfo game3 = new GameInfo("The Legend of Zelda: Tears of the Kingdom", "Nintendo Entertainment Planning & Development", " Головоломка, Пригодницький бойовик, Файтинг, Шутер", new DateTime(2023, 03, 12), true, 800, "", "");
            GameInfo game4 = new GameInfo("Marvel's Spider-Man 2", " Insomniac Games", "Пригодницький бойовик, Файтинг, Платформер", new DateTime(2023, 10, 20), false, 700, "", "");
            GameInfo game5 = new GameInfo("FIFA 23", "Electronic Arts, EA Sports, EA Romania, EA Vancouver", "Спортивний симулятор, Симулятор, Ігрове моделювання", new DateTime(2022, 09, 27), false, 600, "", "");

            try
            {
                
                    using (var db = new GameDbContext())
                    {  db.SaveChanges();

                    var games = db.GameInfo.ToList();
                    foreach (var game in games)
                    {
                        game.Print();
                        Console.WriteLine(" ");
                    }
                        db.DisplaySingleMultiplayerGamesCount();
                        db.DisplayMultiplayerGamesCount();
                        db.DisplayGameWithMaxSalesByStyle("RPG");
                        db.DisplayTop5GamesByMaxSalesByStyle("RPG");
                        db.DisplayTop5GamesByMinSalesByStyle("RPG");
                        db.DisplayFullGameInfo();
                        db.DisplayStudiosAndStylesWithMoreGames();
                    }

                

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
    }
}