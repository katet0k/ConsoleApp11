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
        public int Sales { get; set;  }

        public GameInfo(string name, string studio, string style, DateTime releaseDate, bool multiplayer, int sales)
        {
            Name = name;
            Studio = studio;
            Style = style;
            ReleaseDate = releaseDate;
            Multiplayer = multiplayer;
            Sales = sales;
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
        public GameInfo FindGameByName(string name)
        {
            var game = this.GameInfo.Where(x => x.Name == name).FirstOrDefault();
            return game;
        }

        public GameInfo FindGamesByStudioName(string studioName)
        {
            var game = this.GameInfo.Where(x => x.Studio == studioName).FirstOrDefault();
            return game;
        }

        public GameInfo FindGameByStudioNameAndGameName(string studioName, string gameName)
        {
            var game = this.GameInfo.Where(x => x.Studio == studioName && x.Name == gameName).FirstOrDefault();
            return game;
        }
        public GameInfo FindGamesByStyle(string style)
        {
            var game = this.GameInfo.Where(x => x.Style == style).FirstOrDefault();
            return game;
        }
        public GameInfo FindGamesByReleaseYear(int releaseYear)
        {
            var game = this.GameInfo.Where(x => x.ReleaseDate.Year == releaseYear).FirstOrDefault();
            return game;
        }

        public GameInfo FindSinglePlayerGames()
        {
           var game = this.GameInfo.Where(x => x.Multiplayer == false).FirstOrDefault();
          return game;
        }
        public GameInfo FindMultiplayerGames()
        {
            var game = this.GameInfo.Where(x => x.Multiplayer == true).FirstOrDefault();
            return game;
        }
        public GameInfo FindGameWithMaxSales()
        {
            var game = this.GameInfo.Where(x => x.Sales == this.GameInfo.Max(x => x.Sales)).FirstOrDefault();
            return game;
        }

        public GameInfo FindGameWithMinSales()
        {
            var game = this.GameInfo.Where(x => x.Sales == this.GameInfo.Min(x => x.Sales)).FirstOrDefault();
            return game;
        }
        public IEnumerable<GameInfo> FindTop3PopularGames()
        {
            return this.GameInfo.OrderByDescending(x => x.Sales).Take(3);
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

            GameInfo game1 = new GameInfo("Genshin Impact", "miHoYo", "RPG", new DateTime(2023, 11, 08), true, 1000);
            GameInfo game2 = new GameInfo("Temple Run 2", "Imangi Studios", "Нескінченний бігун", new DateTime(2015, 01, 16), true, 900);
            GameInfo game3 = new GameInfo("The Legend of Zelda: Tears of the Kingdom", "Nintendo Entertainment Planning & Development", " Головоломка, Пригодницький бойовик, Файтинг, Шутер", new DateTime(2023, 03, 12), true, 800);
            GameInfo game4 = new GameInfo("Marvel's Spider-Man 2", " Insomniac Games", "Пригодницький бойовик, Файтинг, Платформер", new DateTime(2023, 10, 20), false, 700);
            GameInfo game5 = new GameInfo("FIFA 23", "Electronic Arts, EA Sports, EA Romania, EA Vancouver", "Спортивний симулятор, Симулятор, Ігрове моделювання", new DateTime(2022, 09, 27), false, 600);

            try
            {
                using (var db = new GameDbContext())
                {
                    
                    db.SaveChanges();

                    var games = db.GameInfo.ToList();

                    foreach (var game in games)
                    {
                        game.Print();
                        Console.WriteLine(" ");
                    }

                    Console.WriteLine("1 - Завдання 1");
                    Console.WriteLine("2 - Завдання 2");
                    Console.WriteLine("3 - Завдання 3");
                    Console.WriteLine("");
                    int num;
                    do
                    {
                        Console.WriteLine("Введіть цифру:");
                        num = Convert.ToInt32(Console.ReadLine());
                        switch (num)
                        {
                            case 1:
                                {
                                    
                                    int num1;
                                    do
                                    { 
                                        Console.WriteLine("1 - Пошук інформації за назвою гри.");
                                        Console.WriteLine("2 - Пошук ігор за назвою студії.");
                                        Console.WriteLine("3 - Пошук інформації за назвою студії та гри.");
                                        Console.WriteLine("4 - Пошук ігор за стилем.");
                                        Console.WriteLine("5 - Пошук ігор за роком релізу.");
                                        
                                        Console.WriteLine("");
                                        Console.WriteLine("Введіть цифру:");
                                        num1 = Convert.ToInt32(Console.ReadLine());
                                        switch (num1)
                                        {
                                            case 0:
                                                break;

                                            case 1:
                                                {
                                                    Console.WriteLine("Введіть назву гри -->");
                                                    string nameGame = Console.ReadLine();
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGameByName(nameGame);

                                                    if (game != null)
                                                    {
                                                        game.Print(); 
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                            case 2:
                                                {
                                                    Console.WriteLine("Введіть студію гри -->");
                                                    string studioName = Console.ReadLine();
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGamesByStudioName(studioName);

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                            case 3:
                                                {
                                                    Console.WriteLine("Введіть назву гри -->");
                                                    string name = Console.ReadLine();
                                                    Console.WriteLine(" ");
                                                    Console.WriteLine("Введіть студію гри -->");
                                                    string studioName = Console.ReadLine();
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGameByStudioNameAndGameName(name, studioName);

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                            case 4:
                                                {
                                                    Console.WriteLine("Введіть стиль гри -->");
                                                    string style = Console.ReadLine();
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGamesByStyle(style);

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                            case 5:
                                                {
                                                    Console.WriteLine("Введіть рік релізу гри -->");
                                                    int releaseYear = Convert.ToInt32(Console.ReadLine()); ;
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGamesByReleaseYear(releaseYear);

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                        }
                                    } while (num1 != 0);
                                }break;
                            case 2:
                                {
                                    int num2;
                                    do
                                    { 

                                        Console.WriteLine(" 1 - Відображення інформації про всі однокористувацькі ігри. ");
                                        Console.WriteLine(" 2 - Відображення інформації про всі багатокористувацькі ігри. ");
                                        Console.WriteLine(" 3 - Показати гру з максимальною кількістю проданих ігор. ");
                                        Console.WriteLine(" 4 - Показати гру з мінімальною кількістю проданих ігор. ");
                                        Console.WriteLine(" 5 - Відображення Топ-3 найпопулярніших ігор. ");
                                        Console.WriteLine(" 6 - Відображення Топ-3 найнепопулярніших ігор ");

                                        Console.WriteLine("");
                                        Console.WriteLine("Введіть цифру:");
                                        num2 = Convert.ToInt32(Console.ReadLine());
                                        switch (num2)
                                        {
                                            case 0:
                                                break;

                                            case 1:
                                                {
                                                    Console.WriteLine("Всі однокористувацькі ігри");
                                                    Console.WriteLine(" ");
                                                    var game = db.FindSinglePlayerGames();

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                            case 2:
                                                {
                                                    Console.WriteLine("Всі багатокористувацькі ігри");
                                                    Console.WriteLine(" ");
                                                    var game = db.FindMultiplayerGames();

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                            case 3:
                                                {
                                                    Console.WriteLine("Максимальна кількістю проданих ігор");
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGameWithMaxSales();

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                    }


                                                }
                                                break;
                                            case 4:
                                                {
                                                    Console.WriteLine("Мінімальна кількістю проданих ігор");
                                                    Console.WriteLine(" "); 
                                                    var game = db.FindGameWithMinSales();

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                    }

                                                }
                                                break;
                                            case 5:
                                                {
                                                    Console.WriteLine("Топ-3 найпопулярніших ігор");
                                                    Console.WriteLine(" ");
                                                    var games5 = db.FindTop3PopularGames();

                                                    foreach (var game in games5)
                                                    {
                                                        game.Print(); 
                                                        Console.WriteLine(" ");
                                                    }

                                                }
                                                break;
                                            case 6:
                                                {
                                                }
                                                break;
                                        }
                                    } while (num2 != 0);
                                }
                                break;
                            case 3:
                                {

                                    int num3;
                                    do
                                    {
                                        Console.WriteLine("1 - Додавання нової гри.Перед додаванням потрібно");
                                        Console.WriteLine("перевірити, чи не існує вже така гра.");
                                        Console.WriteLine("2 -  Зміна даних в існуючій грі.Користувач може змінити будьякий параметр гри.");
                                        Console.WriteLine("3 - Видалити гру. Пошук гри для видалення проводиться за");
                                        Console.WriteLine("назвою гри та студії. Перед видаленням гри, додаток має");
                                        Console.WriteLine("запитати користувача, чи потрібно видаляти гру.");
                                        
                                        Console.WriteLine("");
                                        Console.WriteLine("Введіть цифру:");
                                        num3 = Convert.ToInt32(Console.ReadLine());
                                        switch (num3)
                                        {
                                            case 0:
                                                break;

                                            case 1:
                                                {
                                                    Console.WriteLine("name:");
                                                    string name = Console.ReadLine();
                                                    Console.WriteLine("studio:");
                                                    string studio = Console.ReadLine();
                                                    Console.WriteLine("style:");
                                                    string style = Console.ReadLine();
                                                    Console.WriteLine("releaseDate:");
                                                    DateTime releaseDate = DateTime.Parse(Console.ReadLine());
                                                    Console.WriteLine("multiplayer:");
                                                    bool multiplayer = bool.Parse(Console.ReadLine());
                                                    Console.WriteLine("sales:");
                                                    int sales = int.Parse(Console.ReadLine());

                                                    var game = db.GameInfo.Where(x => x.Name == name && x.Studio == studio).FirstOrDefault();

                                                    if (game == null)
                                                    {
                                                        game = new GameInfo(name, studio, style, releaseDate, multiplayer, sales);
                                                        db.GameInfo.Add(game);
                                                        db.SaveChanges();
                                                    }

                                                }
                                                break;
                                            case 2:
                                                {
                                                    Console.WriteLine("Введіть назву гри для пошуку:");
                                                    string name = Console.ReadLine();
                                                    Console.WriteLine("Введіть студію гри для пошуку:");
                                                    string studio = Console.ReadLine();

                                                    var game = db.GameInfo.Where(x => x.Name == name && x.Studio == studio).FirstOrDefault();

                                                    if (game != null)
                                                    {

                                                        Console.WriteLine("name:");
                                                        string newName = Console.ReadLine();
                                                        Console.WriteLine("studio:");
                                                        string newStudio = Console.ReadLine();
                                                        Console.WriteLine("style:");
                                                        string newStyle = Console.ReadLine();
                                                         Console.WriteLine("releaseDate:");
                                                        DateTime newReleaseDate = DateTime.Parse(Console.ReadLine());
                                                        Console.WriteLine("multiplayer:");
                                                        bool newMultiplayer = bool.Parse(Console.ReadLine());
                                                        Console.WriteLine("sales:");
                                                        int newSales = int.Parse(Console.ReadLine());

                                                        game.Name = newName;
                                                        game.Studio = newStudio;
                                                        game.Style = newStyle;
                                                        game.ReleaseDate = newReleaseDate;
                                                        game.Multiplayer = newMultiplayer;
                                                        game.Sales = newSales;

                                                        db.SaveChanges();
                                                    }

                                                }
                                                break;
                                            case 3:
                                                {
                                                    Console.WriteLine("Введіть назву гри для пошуку:");
                                                    string name = Console.ReadLine();
                                                    Console.WriteLine("Введіть студію гри для пошуку:");
                                                    string studio = Console.ReadLine();
                                                     
                                                    var game = db.GameInfo.Where(x => x.Name == name && x.Studio == studio).FirstOrDefault();
                                                     
                                                    if (game != null)
                                                    {
                                                        Console.WriteLine("Ви дійсно хочете видалити гру {0} від {1}?", name, studio);
                                                        var response = Console.ReadLine();

                                                        if (response == "так")
                                                        { 
                                                            db.GameInfo.Remove(game);
                                                            db.SaveChanges();
                                                        }
                                                    }

                                                }break;
                                        }
                                    } while (num3 != 0);
                                }
                                break;

                        }
                 
                    } while (num != 0) ;
                }
               
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
    }
}
