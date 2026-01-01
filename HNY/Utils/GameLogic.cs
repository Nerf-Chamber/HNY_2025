using System.Text;

namespace HNY.Utils
{
    public class GameLogic
    {
        private string message = "Bon any nou Marina aka best patata :D";
        private int currentLetterPos = 0;

        private int freqLogic = 50;
        private int screenMargin = 5;
        private int timeBetweenAsteroids = 300;

        protected char asteroid;
        protected int speed = 2;
        protected int numCollisions = 0;
        protected string buildingMessage = string.Empty;
        protected (int, int) playerPosition;
        protected Queue<(int x, int y)> asteroids = new Queue<(int, int)>();

        public char SpaceShip { get; set; }
        protected int Width { get; set; }
        protected int Height { get; set; }
        protected CancellationTokenSource Cts { get; set; }
        protected CancellationToken Tk { get; set; }

        protected GameLogic(int width, int height, char spaceShip, CancellationTokenSource cts, CancellationToken tk) 
        { 
            Width = width;
            Height = height;
            SpaceShip = spaceShip;
            Cts = cts;
            Tk = tk;

            playerPosition = (Width/2, Console.WindowHeight - 2);
            asteroid = message[currentLetterPos];
        }

        // ---------- BASIC LOGIC ---------- 
        protected async Task GeneraLogicTask() 
        {
            while (!Tk.IsCancellationRequested)
            {
                lock (asteroids) 
                {
                    MoveAsteroids();
                    if (IsThereCollision())
                    {
                        buildingMessage += message[currentLetterPos];
                        currentLetterPos++;

                        if (currentLetterPos >= message.Length)
                        {
                            Cts.Cancel();
                            return;
                        }

                        if (message[currentLetterPos] == ' ')
                        {
                            buildingMessage += message[currentLetterPos];
                            currentLetterPos++;

                            if (currentLetterPos >= message.Length)
                            {
                                Cts.Cancel();
                                return;
                            }
                        }

                        asteroid = message[currentLetterPos];
                        asteroids.Clear();
                    }

                }
                await Task.Delay(freqLogic);
            }
        }
        protected async Task ShipMovement() 
        {
            while (!Tk.IsCancellationRequested)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.A:
                        if (playerPosition.Item1 > screenMargin) { playerPosition.Item1 -= speed; }
                        break;
                    case ConsoleKey.D:
                        if (playerPosition.Item1 < Width - screenMargin) { playerPosition.Item1 += speed; }
                        break;
                    case ConsoleKey.E:
                        ShootBullet();
                        break;
                    case ConsoleKey.Q:
                        Console.Clear();
                        Environment.Exit(0);
                        break;
                }
                await Task.Delay(1);
            }
        }
        protected async Task GenerateAsteroids() 
        {
            Random r = new Random();
            while (!Tk.IsCancellationRequested)
            {
                lock (asteroids) { asteroids.Enqueue((r.Next(screenMargin, Width - screenMargin), 0)); }
                await Task.Delay(timeBetweenAsteroids);
            }
        }

        // ---------- NOT TASKS ----------
        private void ShootBullet() { }
        private void MoveAsteroids() 
        {
            lock (asteroids)
            {
                int count = asteroids.Count;
                for (int i = 0; i < count; i++)
                {
                    var asteroid = asteroids.Dequeue();
                    asteroid = (asteroid.x, asteroid.y + 1);
                    if (asteroid.y < Console.WindowHeight - 1) { asteroids.Enqueue(asteroid); }
                }
            }
        }
        private bool IsThereCollision() 
        {
            int collisionRange = 2;

            lock (asteroids)
            {
                foreach (var ast in asteroids)
                {
                    if (ast.y == playerPosition.Item2 && Math.Abs(ast.x - playerPosition.Item1) <= collisionRange) { return true; }
                }
                return false;
            }
        }
    }
}