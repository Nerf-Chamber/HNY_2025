using HNY.Utils;

namespace HNY
{
    public class Program
    {
        public static async Task Main()
        {
            const string Menu = "\n\n HNY MINI ASTEROID CATCH!" +
                "\n ------------------------" +
                "\n\n Microgame where You have to catch the letter-asteroids falling to form a secret phrase :D" +
                "\n To move use WASD (Only A and D but xD)" +
                "\n\n [1] - Play" +
                "\n [2] - Customize your space ship!" +
                "\n [Q] - Quit" +
                "\n\n Write your option: ";
            const string Invalid = "\n Invalid option! Try again!";
            const string Link = "dffqj://xpawu.ykkycu.ekz/oacu/x/7ApXCznROvYGMLq3082Pb-gNUn6zTXO9u/waug?hjq=jdbpavy";
            const string SpaceShipMessage = "\n\n Type the char You want for your space ship: ";
            const string SecretMessageCompleted = "\n The secret message was...";
            const string GameOver = "\n Thanks for playing!" +
                "\n [Press any key to exit]";

            bool exit = false;
            char spaceShip = '?';
            char option;
            string completeMessage = string.Empty;

            CancellationTokenSource cts = new CancellationTokenSource();
            GameBase game = new GameBase(100, 50, spaceShip, cts, cts.Token);

            Console.CursorVisible = true;

            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(Menu);
                option = Console.ReadKey().KeyChar;
                option = Char.ToUpper(option);

                switch (option)
                {
                    case '1':
                        await game.RunGame();

                        exit = true;
                        break;
                    case '2':
                        Console.Write(SpaceShipMessage);

                        spaceShip = Console.ReadKey().KeyChar;
                        spaceShip = Char.ToUpper(spaceShip);

                        game.SpaceShip = spaceShip;
                        break;
                    case 'Q':
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine(Invalid);
                        break;
                }
            }

            Thread.Sleep(50);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(SecretMessageCompleted);
            Console.WriteLine($"\n {game.GetBuiltMessage()}");
            Console.WriteLine($" dffqj://xpawu.ykkycu.ekz/oacu/x/7ApXCznROvYGMLq3082Pb-gNUn6zTXO9u/waug?hjq=jdbpavy");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(GameOver);
            Console.ReadKey();
        }
    }
}