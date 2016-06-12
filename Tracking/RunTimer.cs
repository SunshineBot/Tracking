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
        private System.Threading.SynchronizationContext sync;
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

        public RunTimer setSynchronizationContext(System.Threading.SynchronizationContext sync)
        {
            this.sync = sync;
            return this;
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
            data.trackRoute = nodes[data.src].getNextNode(data.dst);
        }

        private void OnTimerEvent(Object source, ElapsedEventArgs e)
        {
            this.Enabled = false;
            

            if (runningList.Count() <= 0)
                return;
            foreach (Distance d in nodes)
            {
                d.calcMinDistance();
            }
            List<TreeModel> toRemove = new List<TreeModel>();
            foreach (TreeModel t in runningList)
            {
                if (t.trackRoute == null)
                    t.trackRoute = nodes[t.src].getNextNode(t.dst);
                t.trackRoute.distance -= 100;
                if (t.trackRoute.distance <= 0)
                {
                    this.Enabled = false;
                    //到达目的地
                    if (t.trackRoute.next == t.dst)
                    {
                        NextNode n = t.trackRoute;
                        //写入数据库
                        DBO.newRecord(t.Name, t.trackRoute.current, t.trackRoute.next, RES.SENDING);
                        DBO.newRecord(t.Name, t.trackRoute.next, t.trackRoute.next, RES.ARRIVED);
                        //修改UI表项
                        ZsmTreeView currentTree = treeList[n.current][1];
                        ZsmTreeView nextTree = treeList[n.next][2];
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
                        toRemove.Add(t);
                        if (t.IsChecked)
                            context.moveToArrivedCheckedList(t);
                    }
                    //到达下一站
                    else
                    {
                        NextNode n = nodes[t.trackRoute.next].getNextNode(t.dst);
                        //写入数据库
                        DBO.newRecord(t.Name, t.trackRoute.current, t.trackRoute.next, RES.SENDING);
                        //修改UI表项
                        ZsmTreeView currentTree = treeList[n.current][1];
                        ZsmTreeView nextTree = treeList[n.next][1];
                        foreach (TreeModel dateNode in treeList[t.trackRoute.current][1].ItemsSourceData)
                        {
                            if (dateNode.Children.Contains(t))
                            {
                                dateNode.Children.Remove(t);
                                IList<TreeModel> nextSource = treeList[t.trackRoute.next][1].ItemsSourceData;
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
                        //进入下一节点
                        t.trackRoute = n;
                    }
                }
            }
            foreach(TreeModel t in toRemove)
                runningList.Remove(t);
            this.Enabled = true;
            foreach (ZsmTreeView[] trees in treeList)
                foreach (ZsmTreeView tree in trees)
                {
                    sync.Post(context.startUpdateUI, tree);
                    sync.Post(context.endUpdateUI, tree);
                }
                    
        }
    }
}
