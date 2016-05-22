using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracking
{
    public class Distance
    {
        private int currentNode;

        

        public Distance(int currentNode)
        {
            this.currentNode = currentNode;
        }

        public NextNode getNext(int finalNode)
        {
            NextNode nextNode = new NextNode();
            LinkedList<NextNode> S = new LinkedList<NextNode>();
            S.AddFirst(new NextNode(currentNode, currentNode, 0).addNode(currentNode));
            LinkedList<NextNode> U = new LinkedList<NextNode>();
            for (int i = 0; i < RES.LOC_MAX; i++)
            {
                if (i != currentNode)
                    U.AddLast(new NextNode(currentNode, i, RES.DISTANCE_MAX));
            }

            List<NextNode> disList = new List<NextNode>();
            List<NextNode> minDisList = new List<NextNode>();
            //int thisNode = currentNode;
            while (U.Count > 0)
            {
                //foreach (int l in U)
                //{
                    LinkedList<NextNode> neighbors = getNeighbors(1, U);
                    NextNode minNode;
                    int minDistance = 65535;
                    foreach(NextNode node in neighbors){
                        if (node.distance < minDistance)
                        {
                            minNode = node;
                            minDistance = node.distance;
                        }
                    }
                    foreach (NextNode node in minDisList)
                    {
                        //todo : nextnode中还需要记录路径=.=||
                    }
                //}
            }

            return nextNode;
        }

        private LinkedList<NextNode> getNeighbors(int currentNode, LinkedList<NextNode> edges)
        {
            
            LinkedList<NextNode> nodeList = new LinkedList<NextNode>();
            /*
            switch (currentNode)
            {
                case RES.BJ:
                    if (edges.Contains<int>(RES.TJ))
                        nodeList.AddLast(new NextNode(currentNode, RES.TJ, 137));
                    if (edges.Contains<int>(RES.HLJ))
                    nodeList.AddLast(new NextNode(currentNode, RES.HLJ, 1225));
                    if (edges.Contains<int>(RES.SD))
                    nodeList.AddLast(new NextNode(currentNode, RES.SD, 424));
                    if (edges.Contains<int>(RES.SC))
                    nodeList.AddLast(new NextNode(currentNode, RES.SC, 1788));
                    if (edges.Contains<int>(RES.HB))
                    nodeList.AddLast(new NextNode(currentNode, RES.HB, 1164));
                    break;
                case RES.HLJ:
                    if (edges.Contains<int>(RES.BJ))
                    nodeList.AddLast(new NextNode(currentNode, RES.BJ, 1225));
                    if (edges.Contains<int>(RES.TJ))
                    nodeList.AddLast(new NextNode(currentNode, RES.TJ, 1207));
                    break;
                case RES.TJ:
                    if (edges.Contains<int>(RES.HLJ))
                    nodeList.AddLast(new NextNode(currentNode, RES.HLJ, 1207));
                    if (edges.Contains<int>(RES.BJ))
                    nodeList.AddLast(new NextNode(currentNode, RES.BJ, 137));
                    if (edges.Contains<int>(RES.SD))
                    nodeList.AddLast(new NextNode(currentNode, RES.SD, 330));
                    if (edges.Contains<int>(RES.SH))
                    nodeList.AddLast(new NextNode(currentNode, RES.SH, 1084));
                    break;
                case RES.SD:
                    if (edges.Contains<int>(RES.BJ))
                    nodeList.AddLast(new NextNode(currentNode, RES.BJ, 424));
                    if (edges.Contains<int>(RES.TJ))
                    nodeList.AddLast(new NextNode(currentNode, RES.TJ, 330));
                    if (edges.Contains<int>(RES.SC))
                    nodeList.AddLast(new NextNode(currentNode, RES.SC, 1607));
                    if (edges.Contains<int>(RES.HB))
                    nodeList.AddLast(new NextNode(currentNode, RES.HB, 853));
                    if (edges.Contains<int>(RES.SH))
                    nodeList.AddLast(new NextNode(currentNode, RES.SH, 846));
                    break;
                case RES.SC:
                    if (edges.Contains<int>(RES.BJ))
                    nodeList.AddLast(new NextNode(currentNode, RES.BJ, 1788));
                    if (edges.Contains<int>(RES.SD))
                    nodeList.AddLast(new NextNode(currentNode, RES.SD, 1607));
                    if (edges.Contains<int>(RES.HB))
                    nodeList.AddLast(new NextNode(currentNode, RES.HB, 1154));
                    if (edges.Contains<int>(RES.GD))
                    nodeList.AddLast(new NextNode(currentNode, RES.GD, 1673));
                    break;
                case RES.HB:
                    if (edges.Contains<int>(RES.BJ))
                    nodeList.AddLast(new NextNode(currentNode, RES.BJ, 1164));
                    if (edges.Contains<int>(RES.SD))
                    nodeList.AddLast(new NextNode(currentNode, RES.SD, 853));
                    if (edges.Contains<int>(RES.SC))
                    nodeList.AddLast(new NextNode(currentNode, RES.SC, 1145));
                    if (edges.Contains<int>(RES.SH))
                    nodeList.AddLast(new NextNode(currentNode, RES.SH, 841));
                    if (edges.Contains<int>(RES.GD))
                    nodeList.AddLast(new NextNode(currentNode, RES.GD, 991));
                    break;
                case RES.SH:
                    if (edges.Contains<int>(RES.TJ))
                    nodeList.AddLast(new NextNode(currentNode, RES.TJ, 1084));
                    if (edges.Contains<int>(RES.SD))
                    nodeList.AddLast(new NextNode(currentNode, RES.SD, 846));
                    if (edges.Contains<int>(RES.HB))
                    nodeList.AddLast(new NextNode(currentNode, RES.HB, 841));
                    if (edges.Contains<int>(RES.GD))
                    nodeList.AddLast(new NextNode(currentNode, RES.GD, 1463));
                    break;
                case RES.GD:
                    if (edges.Contains<int>(RES.SC))
                    nodeList.AddLast(new NextNode(currentNode, RES.SC, 1673));
                    if (edges.Contains<int>(RES.HB))
                    nodeList.AddLast(new NextNode(currentNode, RES.HB, 991));
                    if (edges.Contains<int>(RES.SH))
                    nodeList.AddLast(new NextNode(currentNode, RES.SH, 1463));
                    break;
            }
            */

            
            return nodeList;
        }
        //private void addToList(LinkedList<NextNode> nodeList, LinkedList<int> edges, int node, int dist)
        //{
        //    if (edges.Contains<int>(node))
        //        nodeList.AddLast(new NextNode(node, dist));
        //}
    }
}
