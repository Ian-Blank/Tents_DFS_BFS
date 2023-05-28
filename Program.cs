// See https://aka.ms/new-console-template for more information
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;

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
Console.WriteLine("Please enter amount of tents in the puzzle: ");
int tents = int.Parse(Console.ReadLine());
for (int i = 0; i < 1; i++)
{
    Tents puzzle = new Tents(size, tents);
    puzzle.display();
}


class Tents
{
    int[,] grid;
    int[] rows;
    int[] cols;

    public Tents(int size, int tents)
    {
        grid = new int[size,size];
        rows = new int[size]; 
        cols = new int[size];

        Random rdn = new Random();
        int x = rdn.Next(0,size);
        int y = rdn.Next(0,size);
        int limit = 1000;
        while (tents > 0)
        {
            if (grid[x, y] == 0)
            {
                
                int randomBool = rdn.Next(2);
                int tent = rdn.Next(3) - 1;
                while (tent == 0)
                {
                    tent = rdn.Next(3) - 1;
                }
               
                if (randomBool == 0 && tentcheck(x + tent, y) && inGrid(x+tent,y))
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
                    cols[y+ tent]++;
                    tents--;

                }
                x = rdn.Next(size);
                y = rdn.Next(size);
            }
            else
            {
                if (x + 1 < grid.GetLength(0))
                    x++;
                else if (y + 1 < grid.GetLength(0))
                    y++;
            }
            limit--;
        }


    }

    private bool tentcheck(int x, int y)
    {
        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                if (inGrid(x+i,y+j))
                    if (grid[x + i, y + j] == 1)
                        return false;
            }
        return true;
    }

    private bool inGrid(int x,int y)
    {
        if (x < grid.GetLength(0) && x >= 0 && y < grid.GetLength(0) && y >= 0)
            return true;
        return false;
    }

    public Tents(int[,] puzzle, int[] col, int[] row)
    {
        grid = puzzle;
        rows = row;
        cols = col;
    }

    public void display()
    {
        Console.WriteLine(" " + string.Join("", cols));
        for (int i = 0; i < rows.Length; i++)
        {
            Console.Write(rows[i]);
            for (int j = 0; j < cols.Length; j++)
            {
                //Console.Write("|");
                switch (grid[i,j])
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.Green;
                        break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.BackgroundColor = ConsoleColor.Red;
                        break;
                    case 3:
                        Console.ForegroundColor= ConsoleColor.DarkYellow;
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        break;
                }
                Console.Write(this.grid[i, j]);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.Write("\n");
        }
    }
}

