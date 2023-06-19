using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tents_DFS_BFS
{
    class Node : IEquatable<Node>
    {
        public Node Parent { get; }
        public readonly Point coord;
        public readonly Point tree;
        public bool visit = false;
        public int generation { get; set; }
        private int? hashcode;


        public Node()
        {
            generation = 0;
        }
        
        public Node(Node node, Point p, Point ptree)
        {
            Parent = node;
            generation = node.generation + 1;
            coord = p;
            tree = ptree;

        }

        private IEnumerable<Node> AllNodes()
        {
            Node node = this;
            while (node.generation >= 1)
            {
                yield return node;
                node = node.Parent;
            }
        }

        public IEnumerable<Point> AllCoords()
        {
            foreach (Node item in AllNodes())
            {
                yield return item.coord;
            }
        }

        public IEnumerable<Point> AllTrees()
        {
            return AllNodes().Select(x => x.tree);
        }

        public bool Contains(Point point)
        {
            return AllCoords().Contains(point);
        }

        public bool ContainsTree(Point tree)
        {
            return AllTrees().Contains(tree);
        }

        public bool Equals(Node? other)
        {
            if (other == null)
                return false;
            if (other.generation != generation)
                return false;
            if (AllCoords().Except(other.AllCoords()).Any())
                return false;
            if (AllTrees().Except(other.AllTrees()).Any())
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            if (hashcode != null)
                return hashcode.Value;
            int coordshashcode = 19;
            int treeshashcode = 19;
            foreach (Node node in AllNodes())
            {
                coordshashcode += node.coord.GetHashCode();
                treeshashcode += node.tree.GetHashCode();
            }
            hashcode = HashCode.Combine(generation, coordshashcode, treeshashcode);
            return hashcode.Value;
        }

    }
}
