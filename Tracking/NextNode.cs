using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracking
{
    public class NextNode
    {
        public int current;
        public int next;
        public int distance;
        public int[] dirs;

        public NextNode()
        {

        }
        public NextNode(int current, int next, int distance)
        {
            this.current = current;
            this.next = next;
            this.distance = distance;
            dirs = new int[RES.LOC_MAX];
        }

        public NextNode addNode(int node)
        {
            dirs[dirs.Length] = node;
            return this;
        }

        public NextNode deleteNode()
        {
            dirs = new int[RES.LOC_MAX];
            return this;
        }
    }
}
