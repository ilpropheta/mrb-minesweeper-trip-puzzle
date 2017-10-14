using System.Collections.Generic;

/* author: Stefano Saraulli
 * MIT license. */

namespace Minesweeper
{
    public class MinesweeperNode
    {
        public MinesweeperNode(string label)
        {
            this.label = label;
            nearMines = 0;
            mined = false;
            covered = true;
            flag = false;
        }

        public string label;
        public int nearMines;
        public bool mined;
        public bool covered;  // Visible/not visible.
        public bool flag; //Mine marker, as in original minesweeper.
    }

    public class MinesweeperGraph
    {

        public MinesweeperGraph()
        {
            links = new Dictionary<MinesweeperNode, List<MinesweeperNode>>();
        }


        public void AddLink(MinesweeperNode from, MinesweeperNode to)
        {
            // NON-directed link.
            if (!links.ContainsKey(from))
                links.Add(from, new List<MinesweeperNode>());
            if (!links.ContainsKey(to))
                links.Add(to, new List<MinesweeperNode>());

            links[from].Add(to);
            links[to].Add(from);
        }


        public void ComputeNearMines()
        {
            foreach (MinesweeperNode n in links.Keys)
                foreach (MinesweeperNode near in links[n])
                    if (near.mined)
                        n.nearMines += 1;
        }

        public void UncoverSafeNodes(MinesweeperNode source)
        {
            Stack<MinesweeperNode> frontier = new Stack<MinesweeperNode>();
            HashSet<MinesweeperNode> visited = new HashSet<MinesweeperNode>();
            frontier.Push(source);

            while (frontier.Count != 0)
            {
                MinesweeperNode current = frontier.Pop();

                if (!current.mined)
                {
                    if (!visited.Contains(current))
                    {
                        visited.Add(current);
                        current.covered = false;
                        foreach (MinesweeperNode near in links[current])
                            frontier.Push(near);
                    }
                }
            }
        }

        /** Node -> neighbors. */
        Dictionary<MinesweeperNode, List<MinesweeperNode>> links;
    }
}
