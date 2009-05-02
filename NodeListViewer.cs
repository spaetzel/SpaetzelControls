using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Spaetzel.UtilityLibrary.Database;
using System.Text.RegularExpressions;
using Spaetzel.UtilityLibrary.Types;

namespace Spaetzel.Controls
{
    public enum ListType { List, Bar, Tile, SmallTile, TextList, SingleLine };
    public abstract class NodeListViewer : Panel
    {

        private NodeQuery _query = null;
        private uint _pageNum = 0;
        private uint _itemsPerPage = 15;
        private ListType _listType = ListType.List;
        private string _title = "";
        private Pager _pager;
        private bool _showPager = false;
        private bool _showDescriptions = true;
        private List<NodeListItem> _listItems;

        public List<NodeListItem> ListItems
        {
            get { return _listItems; }
        }

        public bool ShowDescriptions
        {
            get { return _showDescriptions; }
            set
            {
                _showDescriptions = value;
                if (ChildControlsCreated)
                {
                    foreach (var curItem in ListItems)
                    {
                        curItem.ShowDescription = value;
                    }
                }
            }
        }

        public DateTime? MinDateAdded
        {
            get
            {
                if (ViewState["minDateAdded"] == null)
                {
                    return null;
                }
                else
                {
                    return (DateTime?)ViewState["minDateAdded"];
                }
            }
            set
            {
                ViewState["minDateAdded"] = value;
            }
        }

        public NodeListViewer()
        {
            if (Context.Request.QueryString["page"] != null)
            {
                PageNum = Convert.ToUInt32(Context.Request.QueryString["page"]);
               
            }
            else if (Context.Request.RawUrl.Contains("page="))
            {
                string pattern = "page=([0-9]+)";

                var matches = Regex.Matches(Context.Request.RawUrl, pattern);

                try
                {

                    PageNum = Convert.ToUInt32(matches[0].Groups[1].Value);

                }
                catch
                {
                    PageNum = 1;
                }

            }
            else
            {
                PageNum = 1;
            }
        }
        public bool ShowPager
        {
            get { return _showPager; }
            set
            {
                _showPager = value;
                if (ChildControlsCreated)
                {
                    _pager.Visible = value;
                }
            }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }


        public ListType ListType
        {
            get { return _listType; }
            set { _listType = value; }
        }

        protected void ItemsSet()
        {
            if (ChildControlsCreated)
            {
                DisplayListItems();
            }


        }
        public abstract int ItemCount
        {
            get;
        }

        protected abstract Node GetItem(int index);

        protected virtual void BuildQuery()
        {

        }

        private void DisplayListItems()
        {
            _listItems = new List<NodeListItem>();

            if (this.ItemCount == 0)
            {
                BuildQuery();
            }


            int count = 0;

            try
            {
                count = this.ItemCount;
            }
            catch
            {
            }

            for (int i = 0; i < count; i++)
            {
                NodeListItem curItem = GenerateItem(GetItem(i));

                curItem.ShowDescription = ShowDescriptions;

                if (curItem != null)
                {
                    // Set the format for the item
                    curItem.ListType = this.ListType;

                    this.Controls.Add(curItem);
                    ListItems.Add(curItem);
                }
            }

        }




        public NodeQuery Query
        {
            get { return _query; }
            set
            {
                _query = value;

                if (value != null)
                {
                    if (_query.Count > 0)
                    {
                        this.ItemsPerPage = (uint)_query.Count;
                    }
                    else
                    {
                        Query.Count = this.ItemsPerPage;
                    }

                    //  if (this.PageNum > 1)
                    {
                        Query.PageNum = PageNum;
                    }

                    DoQuery();

                    if (ChildControlsCreated)
                    {
                        DisplayListItems();
                    }
                }
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            switch (ListType)
            {
                case ListType.Bar:
                    this.CssClass = "listBar";
                    break;
                case ListType.Tile:
                case ListType.SmallTile:
                    this.CssClass = "tileList";
                    break;
                case ListType.TextList:
                    this.CssClass = "textList";
                    break;
                case ListType.List:
                default:
                    this.CssClass = "itemList";
                    break;
            }

            CreateTitleLabel();

            DisplayListItems();

            CreatePager();

        }

        protected abstract int DoCountQuery();

        private void CreatePager()
        {
            _pager = new Pager();
            _pager.CurPage = PageNum;

            if (ItemCount < ItemsPerPage)
            {
                _pager.NumPages = 1;
            }
            else
            {
                if (Query == null)
                {
                    BuildQuery();
                }


                if (Query != null)
                {

                    _pager.NumPages = (uint)Math.Ceiling((double)DoCountQuery() / (double)ItemsPerPage);
                }
            }

            if (_pager.NumPages <= 1)
            {
                ShowPager = false;
                _pager.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                _pager.Visible = ShowPager;
                this.Controls.Add(_pager);
            }
        }

        protected virtual void CreateTitleLabel()
        {
            if (Title != "")
            {

                Label titleLabel = new Label();
                titleLabel.Text = Title;

                switch (ListType)
                {
                    case ListType.Bar:
                        titleLabel.CssClass = "listBarTitle";
                        break;
                }

                this.Controls.Add(titleLabel);
            }
        }

        public uint PageNum { get { return _pageNum; } set { _pageNum = value; } }
        public uint ItemsPerPage { get { return _itemsPerPage; } set { _itemsPerPage = value; } }

        protected abstract void DoQuery();

        public void DisplayList()
        {
            if (Query == null)
            {
                BuildQuery();
            }

            if (Query != null)
            {
                DoQuery();
            }
        }

        protected abstract NodeListItem GenerateItem(Node item);


    }
}
