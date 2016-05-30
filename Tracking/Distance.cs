using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tracking
{
    public class Distance
    {
        private int currentNode;
        private LinkedList<NextNode> S;
        public static readonly int[][] distanceMatrix;
        static Distance(){
            //todo : 从文件读取距离矩阵。
            StreamReader file = new StreamReader(@"distanceMatrix.txt");
            String[][] strArray = new String[RES.LOC_MAX][];
            distanceMatrix = new int[RES.LOC_MAX][];
            for (int i = 0; i < strArray.Length; i++ )
            {
                strArray[i] = file.ReadLine().Split(new char[] { '\t' });
                distanceMatrix[i] = new int[RES.LOC_MAX];
                for(int j = 0; j < strArray[i].Length; j++)
                {
                    distanceMatrix[i][j] = Int32.Parse(strArray[i][j]);
                }
            }
        }
        public Distance(int currentNode)
        {
            
            this.currentNode = currentNode;
        }

        public NextNode calcMinDistance()
        {
            NextNode nextNode = new NextNode();
            S = new LinkedList<NextNode>();
            S.AddFirst(new NextNode(currentNode, currentNode, 0));
            LinkedList<NextNode> U = new LinkedList<NextNode>();
            for (int i = 0; i < RES.LOC_MAX; i++)
            {
                if (i != currentNode)
                    U.AddLast(new NextNode(currentNode, i, RES.DISTANCE_MAX));
            }

            List<NextNode> disList = new List<NextNode>();
            List<NextNode> minDisList = new List<NextNode>();
            while (U.Count > 0)
            {
                NextNode minNode = findNeighbors(S.Last<NextNode>(), U, S);
                S.AddLast(minNode);
                U.Remove(minNode);
            }
            return nextNode;
        }

        public NextNode getNextNode(int finalNode)
        {
            
            foreach (NextNode node in S)
            {
                if (node.next == finalNode)
                {
                    return new NextNode(currentNode, node.dirs[0], getDistance(currentNode, node.dirs[0]));
                    //foreach (NextNode n in S)
                    //{
                    //    if(n.next == node.dirs[0])
                    //        return new NextNode(currentNode, node.next, n.distance);
                    //}
                }
                    
            }
            return new NextNode();
        }

        private NextNode findNeighbors(NextNode thisNode, LinkedList<NextNode> edges, LinkedList<NextNode> S)
        {

            int currentNode = thisNode.next;
            switch (currentNode)
            {
                case RES.BJ:
                    foreach (NextNode n in edges)
                    {
                        setIt(n, thisNode, RES.TJ, getDistance(currentNode, RES.TJ));
                        setIt(n, thisNode, RES.HLJ, getDistance(currentNode, RES.HLJ));
                        setIt(n, thisNode, RES.SD, getDistance(currentNode, RES.SD));
                        setIt(n, thisNode, RES.SC, getDistance(currentNode, RES.SC));
                        setIt(n, thisNode, RES.HB, getDistance(currentNode, RES.HB));
                    }
                    break;
                case RES.HLJ:
                    foreach (NextNode n in edges)
                    {
                        setIt(n, thisNode, RES.BJ, getDistance(currentNode, RES.BJ));
                        setIt(n, thisNode, RES.TJ, getDistance(currentNode, RES.TJ));
                    }
                    break;
                case RES.TJ:
                    foreach (NextNode n in edges)
                    {
                        setIt(n, thisNode, RES.HLJ, getDistance(currentNode, RES.HLJ));
                        setIt(n, thisNode, RES.BJ, getDistance(currentNode, RES.BJ));
                        setIt(n, thisNode, RES.SD, getDistance(currentNode, RES.SD));
                        setIt(n, thisNode, RES.SH, getDistance(currentNode, RES.SH));
                    }
                    break;
                case RES.SD:
                    foreach (NextNode n in edges)
                    {
                        setIt(n, thisNode, RES.BJ, getDistance(currentNode, RES.BJ));
                        setIt(n, thisNode, RES.TJ, getDistance(currentNode, RES.TJ));
                        setIt(n, thisNode, RES.SC, getDistance(currentNode, RES.SC));
                        setIt(n, thisNode, RES.HB, getDistance(currentNode, RES.HB));
                        setIt(n, thisNode, RES.SH, getDistance(currentNode, RES.SH));
                    }
                    break;
                case RES.SC:
                    foreach (NextNode n in edges)
                    {
                        setIt(n, thisNode, RES.BJ, getDistance(currentNode, RES.BJ));
                        setIt(n, thisNode, RES.SD, getDistance(currentNode, RES.SD));
                        setIt(n, thisNode, RES.HB, getDistance(currentNode, RES.HB));
                        setIt(n, thisNode, RES.GD, getDistance(currentNode, RES.GD));
                    }
                    break;
                case RES.HB:
                    foreach (NextNode n in edges)
                    {
                        setIt(n, thisNode, RES.BJ, getDistance(currentNode, RES.BJ));
                        setIt(n, thisNode, RES.SD, getDistance(currentNode, RES.SD));
                        setIt(n, thisNode, RES.SC, getDistance(currentNode, RES.SC));
                        setIt(n, thisNode, RES.SH, getDistance(currentNode, RES.SH));
                        setIt(n, thisNode, RES.GD, getDistance(currentNode, RES.GD));
                    }
                    break;
                case RES.SH:
                    foreach (NextNode n in edges)
                    {
                        setIt(n, thisNode, RES.TJ, getDistance(currentNode, RES.TJ));
                        setIt(n, thisNode, RES.SD, getDistance(currentNode, RES.SD));
                        setIt(n, thisNode, RES.HB, getDistance(currentNode, RES.HB));
                        setIt(n, thisNode, RES.GD, getDistance(currentNode, RES.GD));
                    }
                    break;
                case RES.GD:
                    foreach (NextNode n in edges)
                    {
                        setIt(n, thisNode, RES.SC, getDistance(currentNode, RES.SC));
                        setIt(n, thisNode, RES.HB, getDistance(currentNode, RES.HB));
                        setIt(n, thisNode, RES.SH, getDistance(currentNode, RES.SH));
                    }
                    break;
            }
            NextNode minNode = edges.First<NextNode>();
            for (int i = 1; i < edges.Count(); i++)
            {
                if (minNode.distance > edges.ElementAt<NextNode>(i).distance)
                {
                    minNode = edges.ElementAt<NextNode>(i);
                }
            }
            return minNode;
        }
        //private void addToList(LinkedList<NextNode> nodeList, LinkedList<int> edges, int node, int dist)
        //{
        //    if (edges.Contains<int>(node))
        //        nodeList.AddLast(new NextNode(node, dist));
        //}
        private static void setIt(NextNode nodeU, NextNode thisNode, int city, int distance)
        {
            if (nodeU.next != thisNode.next && nodeU.next == city)
            {
                if (thisNode.distance + distance < nodeU.distance)
                {
                    nodeU.distance = thisNode.distance;
                    thisNode.copyDirsTo(nodeU);
                    nodeU.addNode(city, distance);
                }
            }
        }

        public int getDistance(int n1, int n2){
            return distanceMatrix[n1][n2];
        }

    }
}
