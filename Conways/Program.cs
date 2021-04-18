using System;
using System.Text;
using System.Threading;

namespace Conways
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new GameOfLife (0, 0);
            game.InsertRandomLives();
            Console.WriteLine(game.DrawGeneration());

            for (; ; )
            {
                game.ApplyRulesAndGenerateNewGeneration();
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine(game.DrawGeneration());
            }
        }
    }
    class GameOfLife
    {
        bool[,] Generation;
        Random random = new Random();

        public GameOfLife (int numberOfRows, int numberColumns)
        {
            CreateFirstGeneration(numberOfRows, numberColumns);
        }

        private void CreateFirstGeneration(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
            {
                rows = 40;
                cols = 80;
            }
            Generation = new bool[rows, cols];
        }


        public void InsertRandomLives() => Iterator((row, col) => { Generation[row, col] = random.Next(0, 10) == 1; });


        public int CountNeighbours(int row, int col)
        {
            var count = default(int);

            count += row + 1 < Generation.GetLength(0) && Generation[row + 1, col] ? 1 : 0;
            count += row - 1 <= Generation.GetLowerBound(0) && Generation[row - 1, col] ? 1 : 0;
            count += col + 1 < Generation.GetLength(0) && Generation[row, col + 1] ? 1 : 0;
            count += col - 1 >= Generation.GetLowerBound(1) && Generation[row, col -1] ? 1 : 0;
            count += row + 1 < Generation.GetLength(0) && col + 1 < Generation.GetLength(1) && Generation[row + 1, col + 1] ? 1 : 0;
            count += row - 1 >= Generation.GetLowerBound(0) && col - 1 >= Generation.GetLowerBound(1) && Generation [row - 1, col - 1] ? 1 : 0;
            count += row + 1 < Generation.GetLength(0) && col - 1 >= Generation.GetLowerBound (1) && Generation [row + 1, col - 1] ? 1 : 0;
            count += row - 1 >= Generation.GetLowerBound(0) && col + 1 < Generation.GetLength(1) && Generation[row - 1, col + 1] ? 1 : 0;

            return count;

        }

        public void ApplyRulesAndGenerateNewGeneration()
        {
            var newGen = new bool[Generation.GetLength(0), Generation.GetLength(1)];

            Iterator((row, col) =>
            {
                if (Generation[row, col])
                {
                    if (CountNeighbours(row, col) < 2)
                    {
                        newGen[row,col] = !Generation[row, col];

                    }
                    else if (CountNeighbours(row, col) == 2 || CountNeighbours(row, col) == 3)
                    {
                        newGen[row,col] = Generation[row, col];
                    }
                    else if (CountNeighbours(row, col) > 3)
                    {
                        newGen[row,col] = !Generation[row, col];
                    }
                    else if (CountNeighbours(row, col) == 3)
                    {
                        newGen[row,col] = !Generation[row, col];
                    }
                }
            });

            Generation = (bool[,])newGen.Clone ();

        }

        public string DrawGeneration()
        {
            var builder = new StringBuilder();

            Iterator ((row, col) => 
            {
                builder.Append(Generation[row, col] ? "#" : " ");
            },
            () => 
            {
                builder.AppendLine();
            
            });
            return builder.ToString();
        }
        private void Iterator(Action<int, int> statement, Action statement2 = null)
        {
            for (var row = 0; row < Generation.GetLength(0); row++)
            {
                for (var col = 0; col < Generation.GetLength(1); col++)
                {
                    statement(row, col);
                }
                if (statement2 != null)
                {
                    statement2();
                }
            }
        }
    }
}
