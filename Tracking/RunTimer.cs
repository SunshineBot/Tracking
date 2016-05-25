using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tracking.Model;

namespace Tracking
{
    public class RunTimer
    {
        public List<TreeModel> runningList;

        private Timer timer;
        private Distance[] nodes;
        private ZsmTreeView[][] treeList;
        public window_company context
        {
            get;
            set;
        }
        public Boolean Enabled
        {
            get { return timer.Enabled; }
            set { timer.Enabled = value; }
        }
        public RunTimer(ZsmTreeView[][] treeList)
        {
            this.treeList = treeList;
            runningList = new List<TreeModel>();
            timer = new Timer();
            timer.Enabled = false;
            timer.Interval = 500;
            timer.Elapsed += OnTimerEvent;
            timer.AutoReset = true;
            nodes = new Distance[RES.LOC_MAX];
            for (int i = 0; i < RES.LOC_MAX; i++)
            {
                nodes[i] = new Distance(i);
            }
        }

        public void addToRunList(IList<TreeModel> data)
        {
            foreach (TreeModel t in data)
            {
                addToRunList(t);
            }
        }

        public void addToRunList(TreeModel data)
        {
            int src = Int32.Parse(data.Name.Substring(0, 2));
            int dst = Int32.Parse(data.Name.Substring(2, 2));
            data.trackRoute = nodes[data.src].getNextNode(data.dst);
        }

        private void OnTimerEvent(Object source, ElapsedEventArgs e)
        {
            foreach (TreeModel t in runningList)
            {
                t.trackRoute.distance -= 100;
                if (t.trackRoute.distance <= 0)
                {
                    //到达目的地
                    if (t.trackRoute.next == t.dst)
                    {
                        NextNode n = t.trackRoute;
                        //写入数据库
                        DBO.newRecord(t.Name, t.trackRoute.current, t.trackRoute.next, 3);
                        //修改UI表项
                        ZsmTreeView currentTree = treeList[n.current][1];
                        ZsmTreeView nextTree = treeList[n.next][2];
                        currentTree.tvZsmTree.BeginInit();
                        nextTree.tvZsmTree.BeginInit();
                        foreach (TreeModel dateNode in currentTree.ItemsSourceData)
                        {
                            if (dateNode.Children.Contains(t))
                            {
                                dateNode.Children.Remove(t);
                                IList<TreeModel> nextSource = nextTree.ItemsSourceData;
                                int dateIndex = nextSource.Count();
                                while (dateIndex-- > 0)
                                {
                                    TreeModel date2 = nextSource.ElementAt<TreeModel>(dateIndex);
                                    if (dateNode.Name.Equals(date2.Name))
                                        break;
                                }
                                if (dateIndex == -1)
                                {
                                    TreeModel newDate = new TreeModel(dateNode.Name);
                                    newDate.type = TreeModel.DATE;
                                    newDate.Children = new List<TreeModel>();
                                    newDate.Children.Add(t);
                                    nextSource.Add(newDate);
                                }
                                else
                                {
                                    nextSource.ElementAt<TreeModel>(dateIndex).Children.Add(t);
                                }
                            }
                        }
                        foreach (ZsmTreeView[] trees in treeList)
                        {
                            trees[1].menuUnSelectAll_Click(null, null);
                        }
                        currentTree.tvZsmTree.EndInit();
                        nextTree.tvZsmTree.EndInit();

                        runningList.Remove(t);
                    }
                    //到达下一站
                    else
                    {
                        NextNode n = nodes[t.trackRoute.next].getNextNode(t.dst);
                        //写入数据库
                        DBO.newRecord(t.Name, t.trackRoute.current, t.trackRoute.next, 2);
                        //修改UI表项
                        treeList[n.current][1].tvZsmTree.BeginInit();
                        treeList[n.next][1].tvZsmTree.BeginInit();
                        foreach (TreeModel dateNode in treeList[n.current][1].ItemsSourceData)
                        {
                            if (dateNode.Children.Contains(t))
                            {
                                dateNode.Children.Remove(t);
                                IList<TreeModel> nextSource = treeList[n.next][1].ItemsSourceData;
                                int dateIndex = nextSource.Count();
                                while(dateIndex-->0)
                                {
                                    TreeModel date2 = nextSource.ElementAt<TreeModel>(dateIndex);
                                    if (dateNode.Name.Equals(date2.Name))
                                        break;
                                }
                                if (dateIndex == -1)
                                {
                                    TreeModel newDate = new TreeModel(dateNode.Name);
                                    newDate.type = TreeModel.DATE;
                                    newDate.Children = new List<TreeModel>();
                                    newDate.Children.Add(t);
                                    nextSource.Add(newDate);
                                }
                                else
                                {
                                    nextSource.ElementAt<TreeModel>(dateIndex).Children.Add(t);
                                }
                            }
                        }
                        foreach (ZsmTreeView[] trees in treeList)
                        {
                            trees[1].menuUnSelectAll_Click(null, null);
                        }
                        treeList[n.current][1].tvZsmTree.EndInit();
                        treeList[n.next][1].tvZsmTree.EndInit();
                        //进入下一节点
                        t.trackRoute = n;
                    }
                }
            }
        }
    }
}
