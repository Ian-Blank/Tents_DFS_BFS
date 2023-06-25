using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tents_DFS_BFS
{
    class Tents
    {
        public int[,] grid;
        public int[] rows;
        public int[] cols;
        public int[] calcrows;
        public int[] calccols;
        public int length;
        public int seed;

        public Tents(Tents puzzle)
        {
            rows = puzzle.rows;
            cols = puzzle.cols;
            length = puzzle.length;
            grid = new int[length, length];
            for (int i = 0; i < length; i++)
                for (int j = 0; j < length; j++)
                {
                    grid[i, j] = puzzle.grid[i, j];
                }
        }
        public Tents(int size, int tents)
        {
            grid = new int[size, size];
            rows = new int[size];
            cols = new int[size];
            calcrows = new int[size];
            calccols = new int[size];
            length = size;

            Random seedrandom = new Random();
            seed = seedrandom.Next();
            Random rdn = new Random(seed);
            int x = rdn.Next(0, size);
            int y = rdn.Next(0, size);
            int limit = 1000;
            while (tents > 0 && limit > 0)
            {
                if (grid[x, y] == 0)
                {

                    int randomBool = rdn.Next(2);
                    int tent = rdn.Next(3) - 1;
                    while (tent == 0)
                    {
                        tent = rdn.Next(3) - 1;
                    }

                    if (randomBool == 0 && tentcheck(x + tent, y) && inGrid(x + tent, y))
                    {
                        grid[x, y] = 3;
                        grid[x + tent, y] = 1;
                        rows[x + tent]++;
                        cols[y]++;
                        tents--;
                    }
                    else if (tentcheck(x, y + tent) && inGrid(x, y + tent))
                    {
                        grid[x, y] = 3;
                        grid[x, y + tent] = 1;
                        rows[x]++;
                        cols[y + tent]++;
                        tents--;

                    }
                    x = rdn.Next(size);
                    y = rdn.Next(size);
                }
                else
                {
                    x = rdn.Next(size);
                    y = rdn.Next(size);
                }
                limit--;
            }
            for (int i = 0; i < length; i++)
                for (int j = 0; j < length; j++)
                {
                    if (grid[i, j] == 1) grid[i, j] = 0;
                }
        }

        public bool tentcheck(int x, int y)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (inGrid(x + i, y + j))
                        if (grid[x + i, y + j] == 1)
                            return false;
                }
            return true;
        }

        public bool inGrid(int x, int y)
        {
            if (x < grid.GetLength(0) && x >= 0 && y < grid.GetLength(0) && y >= 0)
                return true;
            return false;
        }

        public bool hasTent(int x, int y)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (inGrid(x + i, y + j) && Math.Abs(i + j) == 1)
                        if (grid[x + i, y + j] == 1)
                            return true;
                }
            return false;
        }

        private (int[] rowcounts, int[] colcounts) CountColRows(Node node)
        {
            Array.Clear(calcrows);
            Array.Clear(calccols);
            int[] rowCount = calcrows;
            int[] colCount = calccols;

            foreach (Point p in node.AllCoords())
            {
                rowCount[p.X]++;
                colCount[p.Y]++;
            }
            return (rowCount, colCount);
        }
        public bool checkRowsCols(Node node)
        {
            var (rowCount, colCount) = CountColRows(node);
            for (int i = 0; i < length - 1; i++)
            {
                if (cols[i] < colCount[i] || rows[i] < rowCount[i])
                    return false;
            }
            return true;
        }

        public bool isSolution(Node node)
        {
            var (rowCount, colCount) = CountColRows(node);
            return rowCount.SequenceEqual(rows) && colCount.SequenceEqual(cols);
        }
        public Tents(int[,] puzzle, int[] col, int[] row)
        {
            grid = puzzle;
            rows = row;
            cols = col;
            length = grid.GetLength(0);
        }

        public void placeTents(Node node)
        {

            foreach (Point p in node.AllCoords())
            {
                grid[p.X, p.Y] = 1;
            }
            display();
            foreach (Point p in node.AllCoords())
            {
                grid[p.X, p.Y] = 0;
            }
        }
        public void display()
        {
            Console.WriteLine(" " + string.Join(" ", cols));
            for (int i = 0; i < rows.Length; i++)
            {
                Console.Write(rows[i]);
                for (int j = 0; j < cols.Length; j++)
                {
                    //Console.Write("|");
                    switch (grid[i, j])
                    {
                        case 0:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write("  ");
                            break;
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write("▲");
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.Write("  ");
                            break;
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.Write("\n");
            }
        }
    }
}
