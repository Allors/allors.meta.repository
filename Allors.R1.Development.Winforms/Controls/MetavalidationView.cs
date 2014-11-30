// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetavalidationView.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.WinForms.Controls
{
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    using Allors.R1.Meta;

    using Meta;

    public class MetavalidationView : UserControl
    {
        private readonly Hashtable errorsByDomain;
        private Container components;
        private DataGrid dataGrid;
        private DataGridTableStyle dataGridTableStyle;
        private DataSet dataSet;
        private DataTable dataTable;
        private DataColumn domainDataColumn;
        private DataGridTextBoxColumn domainDataGridTextBoxColumn;
        private DataColumn kindDataColumn;
        private DataGridTextBoxColumn kindDataGridTextBoxColumn;
        private DataColumn memberDataColumn;
        private DataGridTextBoxColumn memberDataGridTextBoxColumn;
        private DataColumn messageDataColumn;
        private DataGridTextBoxColumn messageDataGridTextBoxColumn;
        private DataColumn sourceDataColumn;
        private DataGridTextBoxColumn sourceDataGridTextBoxColumn;

        public MetavalidationView()
        {
            InitializeComponent();

            errorsByDomain = new Hashtable();
        }

        public void Validate(Domain domain)
        {
            var validationLog = domain.Validate();
            this.errorsByDomain[domain] = validationLog.Errors;
            this.UpdateView();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataSet = new DataSet();
            this.dataTable = new DataTable();
            this.domainDataColumn = new DataColumn();
            this.sourceDataColumn = new DataColumn();
            this.kindDataColumn = new DataColumn();
            this.memberDataColumn = new DataColumn();
            this.messageDataColumn = new DataColumn();
            this.dataGrid = new DataGrid();
            this.dataGridTableStyle = new DataGridTableStyle();
            this.domainDataGridTextBoxColumn = new DataGridTextBoxColumn();
            this.sourceDataGridTextBoxColumn = new DataGridTextBoxColumn();
            this.memberDataGridTextBoxColumn = new DataGridTextBoxColumn();
            this.kindDataGridTextBoxColumn = new DataGridTextBoxColumn();
            this.messageDataGridTextBoxColumn = new DataGridTextBoxColumn();
            ((ISupportInitialize) (this.dataSet)).BeginInit();
            ((ISupportInitialize) (this.dataTable)).BeginInit();
            ((ISupportInitialize) (this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // dataSet
            // 
            this.dataSet.DataSetName = "MetaValidationDataSet";
            this.dataSet.Locale = new CultureInfo("nl-BE");
            this.dataSet.Tables.AddRange(new DataTable[] {this.dataTable});
            // 
            // dataTable
            // 
            this.dataTable.Columns.AddRange(new DataColumn[] {this.domainDataColumn, this.sourceDataColumn, this.kindDataColumn, this.memberDataColumn, this.messageDataColumn});
            this.dataTable.TableName = "Errors";
            // 
            // domainDataColumn
            // 
            this.domainDataColumn.ColumnName = "Domain";
            // 
            // sourceDataColumn
            // 
            this.sourceDataColumn.ColumnName = "Source";
            // 
            // kindDataColumn
            // 
            this.kindDataColumn.ColumnName = "Kind";
            // 
            // memberDataColumn
            // 
            this.memberDataColumn.ColumnName = "Member";
            // 
            // messageDataColumn
            // 
            this.messageDataColumn.ColumnName = "Message";
            // 
            // dataGrid
            // 
            this.dataGrid.CaptionText = "Errors";
            this.dataGrid.DataMember = "Errors";
            this.dataGrid.DataSource = this.dataSet;
            this.dataGrid.Dock = DockStyle.Fill;
            this.dataGrid.HeaderForeColor = SystemColors.ControlText;
            this.dataGrid.Location = new Point(0, 0);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.Size = new Size(784, 232);
            this.dataGrid.TabIndex = 0;
            this.dataGrid.TableStyles.AddRange(new DataGridTableStyle[] {this.dataGridTableStyle});
            // 
            // dataGridTableStyle
            // 
            this.dataGridTableStyle.DataGrid = this.dataGrid;
            this.dataGridTableStyle.GridColumnStyles.AddRange(new DataGridColumnStyle[] {this.domainDataGridTextBoxColumn, this.sourceDataGridTextBoxColumn, this.memberDataGridTextBoxColumn, this.kindDataGridTextBoxColumn, this.messageDataGridTextBoxColumn});
            this.dataGridTableStyle.HeaderForeColor = SystemColors.ControlText;
            this.dataGridTableStyle.MappingName = "Errors";
            this.dataGridTableStyle.ReadOnly = true;
            // 
            // domainDataGridTextBoxColumn
            // 
            this.domainDataGridTextBoxColumn.Format = "";
            this.domainDataGridTextBoxColumn.FormatInfo = null;
            this.domainDataGridTextBoxColumn.HeaderText = "Domain";
            this.domainDataGridTextBoxColumn.MappingName = "Domain";
            this.domainDataGridTextBoxColumn.NullText = "";
            this.domainDataGridTextBoxColumn.ReadOnly = true;
            this.domainDataGridTextBoxColumn.Width = 120;
            // 
            // sourceDataGridTextBoxColumn
            // 
            this.sourceDataGridTextBoxColumn.Format = "";
            this.sourceDataGridTextBoxColumn.FormatInfo = null;
            this.sourceDataGridTextBoxColumn.HeaderText = "Source";
            this.sourceDataGridTextBoxColumn.MappingName = "Source";
            this.sourceDataGridTextBoxColumn.NullText = "";
            this.sourceDataGridTextBoxColumn.ReadOnly = true;
            this.sourceDataGridTextBoxColumn.Width = 120;
            // 
            // memberDataGridTextBoxColumn
            // 
            this.memberDataGridTextBoxColumn.Format = "";
            this.memberDataGridTextBoxColumn.FormatInfo = null;
            this.memberDataGridTextBoxColumn.HeaderText = "Member";
            this.memberDataGridTextBoxColumn.MappingName = "Member";
            this.memberDataGridTextBoxColumn.NullText = "";
            this.memberDataGridTextBoxColumn.ReadOnly = true;
            this.memberDataGridTextBoxColumn.Width = 120;
            // 
            // kindDataGridTextBoxColumn
            // 
            this.kindDataGridTextBoxColumn.Format = "";
            this.kindDataGridTextBoxColumn.FormatInfo = null;
            this.kindDataGridTextBoxColumn.HeaderText = "Kind";
            this.kindDataGridTextBoxColumn.MappingName = "Kind";
            this.kindDataGridTextBoxColumn.NullText = "";
            this.kindDataGridTextBoxColumn.ReadOnly = true;
            this.kindDataGridTextBoxColumn.Width = 120;
            // 
            // messageDataGridTextBoxColumn
            // 
            this.messageDataGridTextBoxColumn.Format = "";
            this.messageDataGridTextBoxColumn.FormatInfo = null;
            this.messageDataGridTextBoxColumn.HeaderText = "Message";
            this.messageDataGridTextBoxColumn.MappingName = "Message";
            this.messageDataGridTextBoxColumn.NullText = "";
            this.messageDataGridTextBoxColumn.ReadOnly = true;
            this.messageDataGridTextBoxColumn.Width = 250;
            // 
            // MetavalidationView
            // 
            this.Controls.Add(this.dataGrid);
            this.Name = "MetavalidationView";
            this.Size = new Size(784, 232);
            ((ISupportInitialize) (this.dataSet)).EndInit();
            ((ISupportInitialize) (this.dataTable)).EndInit();
            ((ISupportInitialize) (this.dataGrid)).EndInit();
            this.ResumeLayout(false);
        }

        private void UpdateView()
        {
            this.dataTable.Clear();

            foreach (DictionaryEntry entry in this.errorsByDomain)
            {
                var domain = (Domain) entry.Key;
                var validationErrors = (ValidationError[]) entry.Value;
                foreach (ValidationError metaValidationError in validationErrors)
                {
                    var row = this.dataTable.NewRow();
                    row[this.domainDataColumn] = domain.ToString();

                    row[this.sourceDataColumn] = metaValidationError.Source.ToString();
                    var relationType = metaValidationError.Source as RelationType;
                    if (relationType != null)
                    {
                        try
                        {
                            row[this.sourceDataColumn] = relationType.AssociationType.ObjectType + "." + relationType.RoleType.RootName;
                        }
                        catch
                        {
                        }
                    }

                    row[this.kindDataColumn] = metaValidationError.Kind.ToString();
                    row[this.memberDataColumn] = metaValidationError.Members.Length > 0
                                                     ? metaValidationError.Members[0].ToString()
                                                     : null;
                    row[this.messageDataColumn] = metaValidationError.Message;

                    this.dataTable.Rows.Add(row);
                }
            }
        }
    }
}