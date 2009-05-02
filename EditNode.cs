using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Spaetzel.UtilityLibrary;
using System.Web.UI.HtmlControls;
using FredCK.FCKeditorV2;
using Spaetzel.UtilityLibrary.Types;

namespace Spaetzel.Controls
{
    [DefaultProperty("Item")]
    [ToolboxData("<{0}:EditNode runat=server></{0}:EditNode>")]
    public abstract class EditNode : ValidatedForm
    {
        protected HtmlTable _mainTable;
        private Label _nameLabel;

        protected Label NameLabel
        {
            get { return _nameLabel; }
            set { _nameLabel = value; }
        }
        private TextBox _name;
        private FCKeditor _description;
        protected Label _descriptionLabel;



        protected Node PrefillItem
        {
            set {
                EnsureChildControls();

                _name.Text = value.Name;

                if (_description != null)
                {
                    _description.Value = value.Description;
                }


            }
        }

  

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            CreateMainTable();

      
        }

        protected virtual void CreateMainTable()
        {
            _mainTable = new HtmlTable();
            this.Controls.Add(_mainTable);

            CreateNameRows();


            CreateDescriptionRow();


      /*      bool isEditor = false;

            List<PermissionGroup> groups = Users.GetCurrentUserPermissionGroups();

            isEditor = groups.Contains(PermissionGroup.Editor) || groups.Contains(PermissionGroup.Admin);

            if (isEditor)
            {
                CreateEditorRows();
            }*/
        }

     /*   protected virtual void CreateEditorRows()
        {
            _featuredLevel = new DropDownList();

            for (int i = 10; i > 0; i--)
            {
                _featuredLevel.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ListItem notFeatured = new ListItem("Not Featured", "0");
            notFeatured.Selected = true;
            _featuredLevel.Items.Add(notFeatured);

            _mainTable.Rows.Add(GenerateTableRow("Featured Level", false, _featuredLevel, ""));
        }*/


        public Unit DescriptionWidth
        {
            get
            {
                EnsureChildControls();
                return _description.Width;
            }
            set
            {
                EnsureChildControls();
                _description.Width = value;
            }
        }


        public Unit DescriptionHeight
        {
            get
            {
                EnsureChildControls();
                if (_description != null)
                {
                    return _description.Height;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                EnsureChildControls();
                if (_description != null)
                {
                    _description.Height = value;
                }
            }
        }

        protected virtual void CreateDescriptionRow()
        {
            _descriptionLabel = new Label();
            _descriptionLabel.Text = "Description";

            _description = new FCKeditor();
            _description.ToolbarSet = "Basic";
            _description.Width = Unit.Pixel(500);
            //_description.BaseHref = "http://skylab/castroller";
            _description.BasePath = "~/fckeditor/";

            _mainTable.Rows.Add(GenerateTableRow(_descriptionLabel, false, _description, null));
        }

        protected virtual void CreateNameRows()
        {
            _nameLabel = new Label();
            _nameLabel.Text = "Name";

            _name = new TextBox()
            {
                ID = "name"
            };

            _name.Width = Unit.Pixel(400);
            _mainTable.Rows.Add(GenerateTableRow(_nameLabel, true, _name, null));

 
        }


        protected Node FillItem(Node output)
        {
            output.Name = _name.Text.Trim();

            if (_description != null)
            {
                output.Description = _description.Value.Trim();
            }
            return output;

                
        }

       

    }
}
