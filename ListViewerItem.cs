using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Spaetzel.UtilityLibrary;
using Spaetzel.UtilityLibrary.Types;

namespace Spaetzel.Controls
{
	public abstract class NodeListItem : Panel
	{
        private Node _item = null;

        private HyperLink _title;
        protected Panel _titlePanel;

        private ListType _listType = ListType.List;
        private Panel _descriptionPanel = null;

        private bool _showDescription = true;
      

        public bool ShowDescription
        {
            get { return _showDescription; }
            set
            {
                _showDescription = value;
                if (ChildControlsCreated)
                {
                    _descriptionPanel.Visible = value;
                }
            }
        }

        protected Panel TitlePanel
        {
            get
            {
                return _titlePanel;
            }
        }

        protected Panel DescriptionPanel
        {
            get { return _descriptionPanel; }
            set { _descriptionPanel = value; }
        }

        public ListType ListType
        {
            get { return _listType; }
            set { _listType = value; }
        }


       
        protected HyperLink Title
        {
          get { return _title; }
          set { _title = value; }
        }

        public NodeListItem()
        {
        }

        public NodeListItem(Node item)
            : base()
        {
           // _item = item;
            Item = item;
        }


     

        public Node Item
        {
            get
            {
                if (_item == null && ViewState["itemId"] != null)
                {
                    _item = GetItem((int)ViewState["itemId"]);
                }
                return _item;
            }
            set
            {
                if (value != null)
                {
                    ViewState["itemId"] = value.Id;
                    _item = value;
                }
                
            }
        }

        protected abstract Node GetItem(int itemId);

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (Item != null)
            {


                if (ListType == ListType.List)
                {
                    CreateTitlePanel();
                }

                switch (ListType)
                {
                    case ListType.List:
                        this.CssClass = "listItem";
                        break;
                    case ListType.Bar:
                        this.CssClass = "barItem";
                        break;
                    case ListType.Tile:
                        this.CssClass = "tile";
                        break;
                    case ListType.TextList:
                        this.CssClass = "textItem";
                        break;
                    case ListType.SmallTile:
                        this.CssClass = "smallTile";
                        break;
                    case ListType.SingleLine:
                        this.CssClass = "singleLine";
                        break;

                        
                }

                if (ListType != ListType.SingleLine)
                {
                   

                    if (ListType != ListType.SmallTile)
                    {
                        CreateDescriptionPanel();
                    }
                }
                else
                {
                    CreateSingleLine();
                }

            }
        }

        protected virtual void CreateSingleLine()
        {
            throw new NotImplementedException("Single line is not implemented for this type");
        }



    protected virtual void CreateDescriptionPanel()
    {


        _descriptionPanel = new Panel()
        {
            Visible = ShowDescription
        };
      

            switch (ListType)
            {
                case ListType.Tile:
                case ListType.Bar:
                case ListType.TextList:
                    HyperLink link = new HyperLink();
                    link.Text = Item.Name;
                    link.NavigateUrl = Item.ViewPath;
                    link.CssClass = "title";
                    _descriptionPanel.Controls.Add(link);
                    break;
                default:
                    CreateDescriptionTopContent();
                    Label description = new Label();
                    description.Text = FormatDescription();
                    _descriptionPanel.Controls.Add(description);
                    _descriptionPanel.CssClass = "itemContent";

                    break;
            }

            this.Controls.Add(_descriptionPanel);
        
    }

    protected virtual string FormatDescription(bool stripTags)
    {
        if (stripTags)
        {
            return Item.Description.StripTags().Shorten(500);
        }
        else
        {
            return Item.Description.Shorten(500);
        }
    }

    protected virtual string FormatDescription()
    {
        return FormatDescription(true);
    }

        protected virtual void CreateDescriptionTopContent()
        {
    }

        protected virtual int TitleShortenChars
        {
            get
            {
                return 50;
            }
        }

    protected virtual void CreateTitlePanel()
        {
              
            _titlePanel = new Panel();
            _titlePanel.CssClass = "itemHeader";

            _title = new HyperLink();
            _title.Text = Item.Name.Shorten(TitleShortenChars,5);
            _title.NavigateUrl = Item.ViewPath;
            _title.CssClass = "title";

            _titlePanel.Controls.Add(_title);

            this.Controls.Add(_titlePanel);
        }

   

	}
}
