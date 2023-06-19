// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Tents_DFS_BFS;

int[] allrows = new int[] {1,1,0,0,0};
int[] collumn = new int[] { 1, 0, 1, 0, 0};
int[,] grid1 = new int[,] 
{   {3,3,0,0,0},
    {0,0,0,0,0},
    {0,0,0,0,0},
    {0,0,0,0,0},
    {0,0,0,0,0}};
void BFS(Tents puzzle)
{
    Queue<Node> q = new Queue<Node>();
    HashSet<Node> visited = new HashSet<Node>();
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
        depth = node.generation;

        for (int i = 0; i < puzzle.length; i++)
        {
            for (int j = 0; j < puzzle.length; j++)
            {
                Point tree = new Point(i,j);
                if (puzzle.grid[i, j] == 3 && !node.ContainsTree(tree))
                {
                    
                    for (int x = -1; x <= 1; x++)
                        for (int y = -1; y <= 1; y++)

                            if (Math.Abs(x + y) == 1)
                            {
                                int ni = x + i;
                                int nj = y + j;
                                Point newpoint = new Point(ni, nj);
                                Node newnode = new Node(node, newpoint, tree);
                                if (puzzle.inGrid(ni, nj) && tentcheck(node, ni, nj) && puzzle.checkRowsCols(newnode) && puzzle.grid[ni, nj] == 0)
                                {
                                    if(visited.Add(newnode))
                                        q.Enqueue(newnode);
                                }
                            }
                }
            }
            
        }
        if (puzzle.isSolution(node))
        {
            puzzle.placeTents(node);
            Console.WriteLine("Solution Found");

        }
    }
    Console.WriteLine($" Breadth-First search results: depth = {depth}||  number of iterations = {iterations}");
}

void DFS(Tents puzzle)
{
    Node start = new Node();
    int iterations = 0;
    recDFS(puzzle,start,ref iterations, new HashSet<Node>());
}

void recDFS(Tents puzzle,Node node, ref int counter,HashSet<Node> visited)
{
    counter++;
    if (puzzle.isSolution(node))
    {
        puzzle.placeTents(node);
        Console.WriteLine("Solution Found");
        Console.WriteLine(" Depth-First search results: depth = " + node.generation + "||  number of iterations = " + counter);
    }
    else
    {
        for (int i = 0; i < puzzle.length; i++)
        {
            for (int j = 0; j < puzzle.length; j++)
            {
                Point tree = new Point(i, j);
                if (puzzle.grid[i, j] == 3 && !node.ContainsTree(tree))
                {

                    for (int x = -1; x <= 1; x++)
                        for (int y = -1; y <= 1; y++)

                            if (Math.Abs(x + y) == 1)
                            {
                                int ni = x + i;
                                int nj = y + j;
                                Point newpoint = new Point(ni, nj);
                                Node newnode = new Node(node, newpoint, tree);
                                if (puzzle.inGrid(ni, nj) && tentcheck(node, ni, nj) && puzzle.checkRowsCols(newnode) && puzzle.grid[ni, nj] == 0)
                                {
                                    if (visited.Add(newnode))
                                        recDFS(puzzle, newnode,  ref counter, visited);
                                        

                                }
                            }
                }
            }

        }
    }
}

Console.WriteLine("Please enter size of the puzzle: ");
int size = int.Parse(Console.ReadLine());
Console.WriteLine("Please enter amount of tents in the puzzle: ");
int tents = int.Parse(Console.ReadLine());
for (int i = 0; i < 1; i++)
{
    Tents puzzle = new Tents(size,tents);
    //Tents puzzle = new Tents(grid1,collumn,allrows);
    puzzle.display();
    BFS(puzzle);
    DFS(puzzle);
    Console.WriteLine($"Puzzle Seed :{puzzle.seed}");
    
}

bool tentcheck(Node node, int x, int y)
{
    if (node == null)
    {
        return true;
    }
    foreach (Point coord in node.AllCoords())
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



