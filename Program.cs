// See https://aka.ms/new-console-template for more information
using System.ComponentModel;
using System.Drawing;
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
void BFS(Tents puzzle)
{
    Queue<Node> q = new Queue<Node>();
    Node root= new Node();
    q.Enqueue(root);
    Node node;
    int iterations = 0;
    int depth = 0;
    while (q.Count > 0)
    {
        iterations++;
        
        node = q.Dequeue();
        int qlen = q.Count;
        if(node != null)
            depth = node.generation;

        for (int i = 0; i < puzzle.length; i++)
        {
            for (int j = 0; j < puzzle.length; j++)
            {
                if (puzzle.grid[i, j] == 3 && !puzzle.hasTent(i, j))
                {
                    for (int x = -1; x <= 1; x++)
                        for (int y = -1; y <= 1; y++)
                            if (Math.Abs(x + y) == 1)
                            {
                                List<Point> succesor = new List<Point>(node.coords);
                                Point point = new Point(x + i, j + y);
                                succesor.Add(point);
                                if (puzzle.inGrid(i + x, j + y) && tentcheck(node, x + i, y + j) && puzzle.checkRowsCols(succesor))
                                {
                                    Node node1 = new Node(node,point);
                                    q.Enqueue(node1);
                                }
                                else if (puzzle.inGrid(i + x, j + y))
                                {
                                    break;
                                }
                            }
                }
            }
        }
        if (node.generation >= 3) foreach (Point item in node.coords)
            {
                Console.WriteLine("(" + item.X + "," + item.Y + ")");
                Console.Write("(" + item.X + "," + item.Y + ")");
            };        
    }
    Console.WriteLine("depth = " + depth + "||  number of iterations = " + iterations);
}

Console.WriteLine("Please enter size of the puzzle: ");
int size = int.Parse(Console.ReadLine());
Console.WriteLine("Please enter amount of tents in the puzzle: ");
int tents = int.Parse(Console.ReadLine());
for (int i = 0; i < 1; i++)
{
    Tents puzzle = new Tents(size, tents);
    puzzle.display();
    BFS(puzzle);
    
}

bool tentcheck(Node node, int x, int y)
{
    if (node == null)
    {
        return true;
    }
    foreach (Point coord in node.coords)
    {
        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                if (coord.X == x + i && coord.Y == y + j)
                    return false;
            }
    }
    return true;
}

class Tents
{
    public int[,] grid;
    public int[] rows;
    public int[] cols;
    public int length;

    public Tents(Tents puzzle)
    {
        rows = puzzle.rows;
        cols = puzzle.cols;
        length = puzzle.length;
        grid = new int[length,length];
        for (int i = 0; i < length; i++)
            for (int j = 0; j < length; j++)
            {
                grid[i, j] = puzzle.grid[i, j];
            }
    }
    public Tents(int size, int tents)
    {
        grid = new int[size,size];
        rows = new int[size]; 
        cols = new int[size];
        length = size;

        Random rdn = new Random();
        int x = rdn.Next(0,size);
        int y = rdn.Next(0,size);
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
               
                if (randomBool == 0 && tentcheck(x + tent, y) && inGrid(x+tent,y))
                {
                    grid[x, y] = 3;
                    grid[x+ tent, y] = 1;
                    rows[x + tent]++;
                    cols[y]++;
                    tents--;
                }
                else if (tentcheck(x, y + tent) && inGrid(x, y + tent))
                {
                    grid[x, y] = 3;
                    grid[x, y+tent] = 1;
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
        for (int i = 0; i < length; i++)
            for (int j = 0; j < length; j++)
            {
                if (grid[i,j] == 1) grid[i,j] = 0;
            }

    }

    public bool tentcheck(int x, int y)
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

    public bool inGrid(int x,int y)
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
                if (inGrid(x + i, y + j) && Math.Abs(i+j) == 1)
                    if (grid[x + i, y + j] == 1)
                        return true;
            }
        return false;
    }

    public bool checkRowsCols(List<Point> list)
    {
        int rowCount = 0;
        int colCount = 0;

        foreach (Point p in list)
        {
            for (int i = 0; i < length; i++)
            {
                rowCount = 0;
                colCount = 0;
                for (int j = 0; j < length; j++)
                {
                    if ((p.X == i)) rowCount++;
                    if ((p.Y == j)) colCount++;

                }
                if (cols[i] < colCount || rows[i] < rowCount)
                    return false;
            }
        }
        
        return true;
    }
    public Tents(int[,] puzzle, int[] col, int[] row)
    {
        grid = puzzle;
        rows = row;
        cols = col;
    }

    public void placeTents(List<Point> list)
    {
        foreach (Point p in list)
        {
            grid[p.X, p.Y] = 1;
        }
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

class Node
{
    public List<Point> coords = new List<Point>();
    public bool visit = false;
    public int generation { get; set; }

    public Node()
    {
        generation = 0;
    }

    public Node(Node node, Point coord)
    {
        if (node != null)
        {
            coords = new List<Point>(node.coords);
            generation = node.generation + 1;
            coords.Add(coord);
        } else
        {
            coords.Add(coord);
            generation = 1;
        }
    }
    public bool Contains(Point point)
    {
        if(coords.Contains(point)) return true;
        return false;
    }


}



