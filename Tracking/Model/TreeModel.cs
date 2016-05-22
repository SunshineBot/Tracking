//***************************************************
//
// 文件名（FileName）  ： TreeModel.cs
//
// 作者（Author）      ： zsm
//
// 创建时间（CreateAt）:  2013-03-18 14:23:58
//
// 描述（Description） ： 供TreeView实用的数据模型
//
//***************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tracking.Model
{
    public class TreeModel : INotifyPropertyChanged
    {

        public static readonly int BILL = 1;
        public static readonly int DATE = 0;

        #region 私有变量
        /// <summary>
        /// Id值
        /// </summary>
        private string _id;
        /// <summary>
        /// 显示的名称
        /// </summary>
        private string _name;
        /// <summary>
        /// 图标路径
        /// </summary>
        private string _icon;
        /// <summary>
        /// 选中状态
        /// </summary>
        private bool _isChecked;
        /// <summary>
        /// 折叠状态
        /// </summary>
        private bool _isExpanded;
        /// <summary>
        /// 子项
        /// </summary>
        private IList<TreeModel> _children;
        /// <summary>
        /// 父项
        /// </summary>
        private TreeModel _parent;
        /// <summary>
        /// 类型：date==0/bill==1
        /// </summary>
        private int _type;
        #endregion

        /// <summary>
        /// 构造
        /// </summary>
        public TreeModel(string name)
        {
            Children = new List<TreeModel>();
            _isChecked = false;
            IsExpanded = false;
            _icon = "/Images/16_16/folder_go.png";
            _name = name;
        }

        /// <summary>
        /// 键值
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 显示的字符
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        /// <summary>
        /// 指针悬停时的显示说明
        /// </summary>
        public string ToolTip
        {
            get
            {
                return String.Format("{0}-{1}", Id, Name);
            }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    NotifyPropertyChanged("IsChecked");

                    if (_isChecked)
                    {
                        //如果选中则父项也应该选中
                        if (Parent != null)
                        {
                            Parent.IsChecked = true;
                        }
                    }
                    else
                    {
                        //如果取消选中子项也应该取消选中
                        foreach (TreeModel child in Children)
                        {
                            child.IsChecked = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    //折叠状态改变
                    _isExpanded = value;
                    NotifyPropertyChanged("IsExpanded");
                }
            }
        }

        /// <summary>
        /// 父项
        /// </summary>
        public TreeModel Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// 子项
        /// </summary>
        public IList<TreeModel> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        /// <summary>
        /// 设置所有子项的选中状态
        /// </summary>
        /// <param name="isChecked"></param>
        public void SetChildrenChecked(bool isChecked)
        {
            foreach (TreeModel child in Children)
            {
                child.IsChecked = IsChecked;
                child.SetChildrenChecked(IsChecked);
            }
        }

        /// <summary>
        /// 设置所有子项展开状态
        /// </summary>
        /// <param name="isExpanded"></param>
        public void SetChildrenExpanded(bool isExpanded)
        {
            foreach (TreeModel child in Children)
            {
                child.IsExpanded = isExpanded;
                child.SetChildrenExpanded(isExpanded);
            }
        }

        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public int type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}