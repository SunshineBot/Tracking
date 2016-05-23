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
        private int index;
        public NextNode()
        {
            dirs = new int[RES.LOC_MAX];
            index = 0;
        }
        public NextNode(int current, int next, int distance)
        {
            this.current = current;
            this.next = next;
            this.distance = distance;
            dirs = new int[RES.LOC_MAX];
            index = 0;
        }

        public NextNode addNode(int node, int distance)
        {
            if (index + 1 < RES.LOC_MAX)
            {
                dirs[index++] = node;
                this.distance += distance;
            }
            return this;
        }

        public NextNode deleteNode()
        {
            dirs = new int[RES.LOC_MAX];
            distance = 0;
            index = 0;
            return this;
        }

        public void copyDirsTo(NextNode node){
            this.dirs.CopyTo(node.dirs, 0);
            node.index = this.index;
        }
    }
}
