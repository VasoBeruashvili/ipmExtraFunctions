using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ipmPMBasic;
using System.Collections;
using ipmControls;

namespace ipmExtraFunctions
{
    public partial class ExMSTemplatesForm : Form
    {
        ProgramManagerBasic PrograManager;
        int templateId, typeId;
        Hashtable columnValues = new Hashtable();
        Hashtable existingValues = new Hashtable();
        public ExMSTemplatesForm(ProgramManagerBasic pm, int template_id, int type_id, Hashtable values)
        {
            InitializeComponent();
            PrograManager = pm;
            existingValues = values;
            templateId = template_id;
            typeId = type_id;
            fillColumnValues(template_id);
            if (template_id != 0)
                setValues();
        }
        private ProgramManagerBasic GetProgramManager()
        {
            return PrograManager;
        }
        private void setValues()
        {
            var groupBoxes = pnlColumnValues.Controls.OfType<GroupBox>().AsEnumerable();

            foreach (GroupBox group in groupBoxes)
            {
                group.Controls.OfType<RadioButton>().ToList().ForEach(p => 
                    {
                        if (p.Text == existingValues[p.Tag.ToString()].ToString())
                        {
                            p.Checked = true;
                        }
                    });
            }

            var panels = pnlColumnValues.Controls.OfType<Panel>().AsEnumerable();
            foreach (Panel pnl in panels)
            {
                pnl.Controls.OfType<TextBoxDecimalInput>().ToList().ForEach(p=>
                {
                    p.Text = existingValues[p.Tag.ToString()].ToString();
                });

            }
        }
        private void fillColumnValues(int templateId)
        {
            pnlColumnValues.Controls.Clear();

            string sql = "SELECT col.id, col.db_name, col.name, col.is_interval FROM book.ExMSGeneralTypeColumns AS tc INNER JOIN book.ExMSColumns AS col ON tc.col_id=col.id WHERE tc.type_id=" + typeId;
            string subSql = "";
            DataTable subData;
            DataTable data = new DataTable();
            data = GetProgramManager().GetDataManager().GetTableData(sql);

            columnValues.Clear();
            
            foreach (DataRow dr in data.Rows)
            {
                
                if (!dr.Field<bool>("is_interval"))
                {
                    GroupBox groupColumn = new GroupBox();
                    groupColumn.AutoSize = true;
                    groupColumn.Dock = DockStyle.Top;
                    groupColumn.Text = dr.Field<string>("name");
                    pnlColumnValues.Controls.Add(groupColumn);
                    groupColumn.BringToFront();

                    subSql = "SELECT id, name FROM book.ExMSColumnValues WHERE col_id=" + dr.Field<int>("id");
                    subData = new DataTable();
                    subData = GetProgramManager().GetDataManager().GetTableData(subSql);
                    int x = 20, y = 20, counter = 0;
                    bool isFirst = true;
                    foreach (DataRow row in subData.Rows)
                    {

                        RadioButton rButton = new RadioButton();
                        rButton.Location = new Point(x, y);
                        rButton.Size=new Size(160, 25);
                        rButton.Tag = dr.Field<string>("db_name");
                        rButton.Text = row.Field<string>("name");                        

                        groupColumn.Controls.Add(rButton);
                        rButton.CheckedChanged += new EventHandler(radioBtn_checkChanged);
                        if ((templateId==0)&&isFirst)
                        {
                            rButton.Checked = true;
                            isFirst = false;
                        }                        
                        rButton.BringToFront();
                        x += 165;
                        counter++;
                        if (counter > 1)
                        {
                            counter = 0;
                            y += 27;
                            x = 20;
                        }
                    }

                }
                else
                {
                    Panel pnlInterval = new Panel();
                    pnlInterval.AutoSize = true;
                    pnlInterval.Dock = DockStyle.Top;
                    pnlColumnValues.Controls.Add(pnlInterval);
                    pnlInterval.BringToFront();

                    Label lblFrom = new Label();
                    lblFrom.Text = dr.Field<string>("name")+":";
                    lblFrom.Size = new Size(85, 30);
                    lblFrom.Location = new Point(7, 20);
                    pnlInterval.Controls.Add(lblFrom);

                    TextBoxDecimalInput txtFrom = new TextBoxDecimalInput();
                    txtFrom.Size = new Size(50, 30);
                    txtFrom.Location = new Point(95, 20);
                    txtFrom.Tag = dr.Field<string>("db_name") + "_from";
                    txtFrom.Text = "0.00";
                    txtFrom.TextChanged += new EventHandler(txt_Changed);
                    
                    columnValues.Add(txtFrom.Tag.ToString(), 0);
                    pnlInterval.Controls.Add(txtFrom);
                    
                    TextBoxDecimalInput txtTo = new TextBoxDecimalInput();
                    txtTo.Size = new Size(50, 30);
                    txtTo.Location = new Point(180, 20);
                    txtTo.Tag = dr.Field<string>("db_name") + "_to";
                    txtTo.Text = "0.00";
                    txtTo.TextChanged += new EventHandler(txt_Changed);

                    columnValues.Add(txtTo.Tag.ToString(), 0);
                    pnlInterval.Controls.Add(txtTo);
                }
                
            }

        }
      
        private void onAddTemplates()
        {
            columnValues.Add("type_id", typeId.ToString());

            string insertParatemets = string.Join(", ", columnValues.Keys.Cast<string>().ToArray());
            string insertValues = string.Join(", ", columnValues.Keys.Cast<string>().Select(p=>string.Format("@{0}", p)) .ToArray());

            string sql = @"INSERT INTO book.ExMSTemplates (" + insertParatemets + ") VALUES (" + insertValues + ")";

            
            if (!GetProgramManager().GetDataManager().ExecuteSql(sql, columnValues))
            {
                MessageBox.Show("შაბლონის შენახვა ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
        private void onEditTemplates(int template_id)
        {
            string updateParameters = string.Join(", ", columnValues.Keys.Cast<string>().Select(p => string.Format("{0}=@{0}", p)).ToArray());

            string sql = "UPDATE book.ExMSTemplates SET " + updateParameters + " WHERE id=" + template_id;
            if (!GetProgramManager().GetDataManager().ExecuteSql(sql, columnValues))
            {
                MessageBox.Show("შაბლონის გადახლება ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
        private void radioBtn_checkChanged(object sender, EventArgs e)
        {           
            RadioButton btn = (RadioButton)sender;
            if (btn.Checked)
            columnValues[btn.Tag.ToString()] = btn.Text;
        }

        private void btnSaveValues_Click(object sender, EventArgs e)
        {
            if (templateId==0)
            {
                onAddTemplates();
            }
            else
            {
                onEditTemplates(templateId);
            }
        }
        private void txt_Changed(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            decimal value;
            if (!decimal.TryParse(txt.Text, out value))
            {
                txt.Text = "0.00";
            }
            columnValues[txt.Tag.ToString()]= value;
        }
        private void btnCloseValues_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
