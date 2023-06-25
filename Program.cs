// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
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
int nsolution = 0;

int MeanBFS = 0;
int MeanDFS = 0;
int Meanhyb = 0;
int intervalBFS = 0;
int intervalDFS = 0;
int intervalhyb = 0;
int old = 0;
void BFS(Tents puzzle)
{
    Queue<Node> q = new Queue<Node>();
    HashSet<Node> visited = new HashSet<Node>();
    Node root= new Node();
    q.Enqueue(root);
    Node node;
    int iterations = 0;
    old = 0;
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
            // puzzle.placeTents(node);
            if (MeanBFS == 0)
                old = iterations;
            MeanBFS = iterations;
            
            nsolution++;
            Console.WriteLine($" Breadth-First search results: depth = {depth}||  number of iterations = {iterations}");
        }
    }
    intervalBFS = MeanBFS - old;
}

void DFS(Tents puzzle)
{
    old = 0;
    Node start = new Node();
    int iterations = 0;
    recDFS(puzzle,start,ref iterations, new HashSet<Node>());
    intervalDFS = MeanDFS - old;
}

void recDFS(Tents puzzle,Node node, ref int counter,HashSet<Node> visited)
{
    counter++;
    if (puzzle.isSolution(node))
    {
        //puzzle.placeTents(node);
        if (MeanBFS == 0)
            old = counter;
        MeanDFS = counter;
        
        nsolution++;
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

void hybrid(Tents puzzle, int limit)
{
    Node start = new Node();
    old = 0;
    int iterations = 0;
    recHybrid(puzzle, start, new HashSet<Node>(), ref iterations, limit);
    intervalhyb = Meanhyb - old;
}

void recHybrid(Tents puzzle, Node node, HashSet<Node> visited, ref int counter, int limit)
{
    
    Queue<Node> q = new Queue<Node>();
    q.Enqueue(node);
    while (q.Count > 0) 
    {
        counter++;
        node = q.Dequeue();
        if (puzzle.isSolution(node))
        {
            //puzzle.placeTents(node);
            if (Meanhyb == 0)
                old = counter;
            Meanhyb = counter;
            nsolution++;
            Console.WriteLine($" Hybrid search results: Limit = {limit}|| depth = {node.generation}||  number of iterations = {counter}");
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
                                        {
                                            if (node.generation >= limit)
                                            {
                                                    q.Enqueue(newnode);
                                            }
                                            else
                                            {
                                                recHybrid(puzzle, newnode, visited, ref counter,limit);
                                            }
                                        }


                                    }
                                }
                    }
                }

            }
        }
    }
}

Console.WriteLine("Please enter size of the puzzle: ");
int size = int.Parse(Console.ReadLine());
Console.WriteLine("Please enter number of tents in the puzzle: ");
int tents = int.Parse(Console.ReadLine());
Console.WriteLine("Please enter the number of puzzles : ");
int n = int.Parse(Console.ReadLine());

int totalsolutions = 0; 
int[,] results = new int[n,5];
int[,]intervalresults = new int[n,5];
for (int i = 0; i < n; i++)
{
    Tents puzzle = new Tents(size,tents);
    //Tents puzzle = new Tents(grid1,collumn,allrows);
    
    BFS(puzzle);
    results[i,0] = MeanBFS ;
    intervalresults[i, 0] = intervalBFS;
    totalsolutions += nsolution;
    nsolution = 0;
    DFS(puzzle);
    intervalresults[i, 1] = intervalDFS;
    results[i, 1] = MeanDFS;
    hybrid(puzzle, tents - 2);
    intervalresults[i, 2] = intervalhyb;
    results[i, 2] = Meanhyb;
    Meanhyb = 0;
    hybrid(puzzle, tents - 3);
    intervalresults[i, 3] = intervalhyb;
    results[i, 3] = Meanhyb;
    Meanhyb = 0;
    hybrid(puzzle, tents - 4);
    intervalresults[i, 4] = intervalhyb;
    results[i, 4] = Meanhyb;
    Console.WriteLine($"Puzzle Seed :{puzzle.seed}");
    Console.WriteLine($"\n");

}
Console.WriteLine($"Mean iterations over number of solutions: ");
int total = 0;
for (int i = 0; i < 5; i++)
{

        total = 0;
        for (int j = 0; j < results.GetLength(0); j++)
        {
            total += results[j, i];
        }

        Console.WriteLine($"results[{i}] = {total / totalsolutions} ");
}
total = 0;
for (int i = 0; i < 5; i++)
{

    total = 0;
    for (int j = 0; j < results.GetLength(0); j++)
    {
        total += intervalresults[j, i];
    }

    Console.WriteLine($"interval results[{i}] = {total / totalsolutions} ");
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



