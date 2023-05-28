// See https://aka.ms/new-console-template for more information
int[] collumn = new int[] {3,0,0,2,0,2};
int[] allrows = new int[] { 2, 0, 2, 1, 1, 1 };
int[,] grid1 = new int[,] 
{   {0,3,3,0,0,0},
    {3,0,0,0,0,0},
    {0,0,0,0,0,0},
    {3,0,0,0,0,3},
    {0,0,0,3,0,0},
    {0,0,0,0,3,0} };


Console.WriteLine("Please enter size of the puzzle: ");
int size = int.Parse(Console.ReadLine());
Tents puzzle = new Tents(grid1,collumn,allrows);
puzzle.display();

class Tents
{
    int[,] grid;
    int[] rows;
    int[] cols;

    public Tents(int size)
    {
        grid = new int[size,size];
        rows = new int[size]; 
        cols = new int[size];
    }


    public Tents(int[,] puzzle, int[] col, int[] row)
    {
        grid = puzzle;
        rows = row;
        cols = col;
    }

    public void display()
    {
        Console.WriteLine("  " + string.Join(" ", cols));
        for (int i = 0; i < rows.Length; i++)
        {
            Console.Write(rows[i]);
            for (int j = 0; j < cols.Length; j++)
            {
                Console.Write("|");
                switch (grid[i,j])
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case 3:
                        Console.ForegroundColor= ConsoleColor.DarkYellow;
                        break;
                }
                Console.Write(this.grid[i, j]);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.Write("\n");
        }
    }
}

