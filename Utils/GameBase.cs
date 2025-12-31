namespace HNY.Utils
{
    public class GameBase : GameLogic
    {
        private int FreqRender = 20;

        public GameBase(int width, int height, char spaceShip, CancellationTokenSource cts, CancellationToken tk)
            : base(width, height, spaceShip, cts, tk) { }

        public async Task RunGame()
        {
            Console.SetWindowSize(Width, Height);
            Console.SetBufferSize(Width, Height);
            Console.CursorVisible = false;

            Task renderTask = Task.Run(Render);
            Task logicTask = Task.Run(GeneraLogicTask);
            Task inputTask = Task.Run(ShipMovement);
            Task generateAsteroidsTask = Task.Run(GenerateAsteroids);

            await Task.WhenAny(renderTask, logicTask, inputTask, generateAsteroidsTask);
        }

        // ---------- RENDER FUNCTIONS ---------- 
        private async Task Render() 
        {
            while (!Tk.IsCancellationRequested)
            {
                Console.Clear();

                Console.Write(buildingMessage);

                // Player
                Console.SetCursorPosition(playerPosition.Item1, playerPosition.Item2);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(SpaceShip);
                // Asteroids
                lock (asteroids)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    foreach (var ast in asteroids)
                    {
                        Console.SetCursorPosition(ast.x, ast.y); 
                        Console.Write(asteroid);
                    }
                }
                // Hz
                await Task.Delay(FreqRender);
            }
        }

        public string GetBuiltMessage () { return buildingMessage; }
    }
}