using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using Spaetzel.UtilityLibrary;

namespace Spaetzel.Controls
{
    public class ValidatedForm : CompositeControl
    {
        private List<Label> _labels;
        private List<Control> _controls;
        private List<BaseValidator> _validators;
        public ValidatedForm()
        {
            _labels = new List<Label>();
            _controls = new List<Control>();
            _validators = new List<BaseValidator>();
        }

        protected void AddLabel(Label newLabel)
        {
            _labels.Add(newLabel);
        }

        protected void AddControl(Control newControl)
        {
            _controls.Add(newControl);
        }

        protected void AddValidator(BaseValidator newValidator)
        {
            newValidator.Display = ValidatorDisplay.None;

            _validators.Add(newValidator);

            this.Controls.Add(newValidator);
        }

        protected HtmlTableRow GenerateTableRow(Label label, bool required, Control control, Label description)
        {
            if (required)
            {
                if (control.ID.IsNullOrEmpty())
                {
                    throw new ArgumentException(String.Format("ID Must be entered for required fields, {0}", label.Text));
                }

                RequiredFieldValidator validator = new RequiredFieldValidator()
                {
                    ControlToValidate = control.ID,
                    ErrorMessage = String.Format("{0} is required", label.Text)
                };
                AddValidator(validator);
            }
            return GenerateUnvalidatedTableRow(label, required, control, description);
        }

        protected HtmlTableRow GenerateUnvalidatedTableRow(Label label, bool required, Control control, Label description)
        {
            HtmlTableRow row = new HtmlTableRow();

            HtmlTableCell labelCell = new HtmlTableCell();
            AddLabel(label);
            label.CssClass = "formLabel";
            labelCell.Controls.Add(label);

            if (description != null)
            {
                AddLabel(description);
                description.CssClass = "formDescription";
                labelCell.Controls.Add(description);

            }

            if (required)
            {
                labelCell.Controls.Add(GenerateRequiredLabel());
            }

            row.Cells.Add(labelCell);

            HtmlTableCell controlCell = new HtmlTableCell();
            AddControl(control);
            controlCell.Controls.Add(control);

            row.Cells.Add(controlCell);

            

            return row;
        }


        protected HtmlTableRow GenerateTableRow(string label, bool required, Control control)
        {
            return GenerateTableRow(label, required, control, "");
        }

        protected HtmlTableRow GenerateTableRow(string label, bool required, Control control, string description)
        {
            Label labelLabel = new Label();
            labelLabel.Text = label;

            Label descriptionLabel = new Label();
            descriptionLabel.Text = description;

            return GenerateTableRow(labelLabel, required, control, descriptionLabel);
        }

   

        private Label GenerateRequiredLabel()
        {
            Label output = new Label();
            output.Text = "*";
            output.ForeColor = System.Drawing.Color.Red;
            AddLabel(output);

            return output;
        }

    }
}
