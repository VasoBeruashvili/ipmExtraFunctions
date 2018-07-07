using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ipmPMBasic;

namespace ipmExtraFunctions
{
    public partial class ConstructionForm : Form
    {
        int i, lastValue;
        ProgramManagerBasic pm;
        string path = "0#1#10#11";
        string lastName;

        public ConstructionForm(ProgramManagerBasic pm)
        {
            InitializeComponent();

            this.pm = pm;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FillComboTemplate();
        }

        private ProgramManagerBasic GetProgramManager()
        {
            return pm;
        }

        private void FillComboTemplate()
        {
            string sql = "SELECT id, name FROM book.ConstructionTemplates";
            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

            comboTemplate.ValueMember = "id";
            comboTemplate.DisplayMember = "name";
            comboTemplate.DataSource = data;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnAddTemplate_Click(object sender, EventArgs e)
        {
            ConstructionTemplateListForm form = new ConstructionTemplateListForm(GetProgramManager());
            if (form.ShowDialog() == DialogResult.OK)
            {
                FillComboTemplate();
                comboTemplate.SelectedValue = form.TemplateID;
                //GenerateTemplate(form.TemplateID);
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            GenerateComboBox(-1);
        }

        private void btnRealization_Click(object sender, EventArgs e)
        {
            OnRealize();
        }

        private void OnRealize()
        {
            int doc_id = 1;
            Bitmap bmp = new Bitmap(Properties.Resources.FINA);
            IntPtr hicon = bmp.GetHicon();

            ipmDocForms.ProductOut.ProductOutDocForm out_form = new ipmDocForms.ProductOut.ProductOutDocForm();

            out_form.Icon = Icon.FromHandle(hicon);
            out_form.ShowIcon = true;
            out_form.SetProgramManager = GetProgramManager();
            out_form.Tag = "DOC_PRODUCTOUT";
            out_form.SetupControls();
            out_form.Text = GetProgramManager().GetTranslatorManager().Translate("საქონლის გაყიდვა :") + " " + GetProgramManager().GetTranslatorManager().Translate("ახალი");
            out_form.StartPosition = FormStartPosition.CenterScreen;
            doc_id = out_form.GetDocMaxID();

            foreach (Control c in pnlRight.Controls)
                foreach (Control cc in c.Controls)
                    foreach (Control ccc in cc.Controls)
                        if (ccc is ComboBox)
                        {
                            if (((ComboBox)ccc).SelectedValue == null)
                                continue;

                            out_form.AddConstructionProducts(DateTime.Now, (int)((ComboBox)ccc).SelectedValue, ((ComboBox)ccc).Text, doc_id);
                        }

            //this.m_AskBeforeClose = false;
            if (GetProgramManager().GetTempString() != "AUTOREALIZE")
                out_form.Show();
        }

        private void GenerateTemplate(int templateId)
        {
            if (templateId == -1)
                return;

            string sql = "SELECT gp.path, " +
               "ctf.id, " +
               "ctf.product_group_id " +
               "FROM book.ConstructionTemplates ct " +
               "INNER JOIN book.ConstructionTemplatesFlow ctf ON ctf.template_id = ct.id " +
               "INNER JOIN book.GroupProducts gp ON gp.id = ctf.product_group_id " +
               "WHERE ct.id = " + templateId;

            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

            if (data == null)
                return;

            i = 0;
            pnlLeft.Controls.Clear();
            pnlRight.Controls.Clear();

            foreach (DataRow rw in data.Rows)
                GenerateComboBox(Convert.ToInt32(rw["product_group_id"]));
        }

        private void GenerateComboBox(int groupProdId)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Top;
            panel.Size = new Size(305, 31);
            panel.Name = "pnlRight_" + i;
            pnlRight.Controls.Add(panel);

            Panel paneSepLeft = new Panel();
            paneSepLeft.Dock = DockStyle.Left;
            paneSepLeft.Size = new Size(5, 26);

            Panel paneSepRight = new Panel();
            paneSepRight.Dock = DockStyle.Right;
            paneSepRight.Size = new Size(5, 26);

            Panel paneSepMid = new Panel();
            paneSepMid.Dock = DockStyle.Right;
            paneSepMid.Size = new Size(4, 26);

            Panel paneSep = new Panel();
            paneSep.Dock = DockStyle.Top;
            paneSep.Size = new Size(200, 5);

            Panel panelButton = new Panel();
            panelButton.Dock = DockStyle.Right;
            panelButton.Size = new Size(70, 26);

            Panel panelNum = new Panel();
            panelNum.Dock = DockStyle.Right;
            panelNum.Size = new Size(83, 26);

            Panel panelPrice = new Panel();
            panelPrice.Dock = DockStyle.Right;
            panelPrice.Size = new Size(83, 26);

            Panel panelSelf = new Panel();
            panelSelf.Dock = DockStyle.Right;
            panelSelf.Size = new Size(83, 26);

            Panel paneSepNum = new Panel();
            paneSepNum.Dock = DockStyle.Right;
            paneSepNum.Size = new Size(4, 26);

            Panel paneSepPrice = new Panel();
            paneSepPrice.Dock = DockStyle.Right;
            paneSepPrice.Size = new Size(4, 26);

            Panel paneSepSelf = new Panel();
            paneSepSelf.Dock = DockStyle.Right;
            paneSepSelf.Size = new Size(4, 26);

            Panel pnlCombo = new Panel();
            pnlCombo.Dock = DockStyle.Fill;
            pnlCombo.Size = new Size(200, 5);

            ComboBox comboBox = new ComboBox();
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.Dock = DockStyle.Fill;
            comboBox.Name = "comboRight_" + i;
            comboBox.FormattingEnabled = true;
            comboBox.Size = new Size(258, 26);
            comboBox.SelectedValueChanged += new EventHandler(comboBoxProduct_SelectedValueChanged);
            panel.Controls.Add(pnlCombo);

            ipmControls.TextBoxDecimalInput textBoxNum = new ipmControls.TextBoxDecimalInput();
            textBoxNum.Dock = DockStyle.Fill;
            textBoxNum.Name = "txtNum_" + i;
            textBoxNum.Size = new Size(83, 26);
            textBoxNum.TextChanged += new EventHandler(textBox_TextChanged);
            textBoxNum.Text = "1";

            ipmControls.TextBoxDecimalInput textBoxPrice = new ipmControls.TextBoxDecimalInput();
            textBoxPrice.Dock = DockStyle.Fill;
            textBoxPrice.Name = "txtPrice_" + i;
            textBoxPrice.Size = new Size(83, 26);
            textBoxPrice.TextChanged += new EventHandler(textBox_TextChanged);
            textBoxPrice.Text = "0.00";

            ipmControls.TextBoxDecimalInput textBoxSelf = new ipmControls.TextBoxDecimalInput();
            textBoxSelf.Dock = DockStyle.Fill;
            textBoxSelf.Name = "txtSelf_" + i;
            textBoxSelf.Size = new Size(83, 26);
            textBoxSelf.ReadOnly = true;
            textBoxSelf.TextChanged += new EventHandler(textBox_TextChanged);
            textBoxSelf.Text = "0.00";

            Button button = new Button();
            button.Image = global::ipmExtraFunctions.Properties.Resources.delete_16;
            button.Dock = DockStyle.Fill;
            button.Name = "button_" + i;
            button.Size = new Size(70, 26);
            button.Text = "წაშლა";
            button.TextImageRelation = TextImageRelation.ImageBeforeText;
            button.UseVisualStyleBackColor = true;
            button.Click += new EventHandler(button_Click);
            panel.Controls.Add(paneSepNum);
            panel.Controls.Add(panelNum);
            panel.Controls.Add(paneSepPrice);
            panel.Controls.Add(panelPrice);
            panel.Controls.Add(paneSepSelf);
            panel.Controls.Add(panelSelf);
            panel.Controls.Add(paneSepMid);
            panel.Controls.Add(panelButton);
            panel.Controls.Add(paneSepLeft);
            panel.Controls.Add(paneSepRight);
            panel.Controls.Add(paneSep);
            pnlCombo.Controls.Add(comboBox);
            panelNum.Controls.Add(textBoxNum);
            panelPrice.Controls.Add(textBoxPrice);
            panelSelf.Controls.Add(textBoxSelf);
            panelButton.Controls.Add(button);

            Panel panel1 = new Panel();
            panel1.Dock = DockStyle.Top;
            panel1.Size = new Size(305, 31);
            panel1.Name = "pnlLeft_" + i;
            pnlLeft.Controls.Add(panel1);

            Panel paneSep1 = new Panel();
            paneSep1.Dock = DockStyle.Top;
            paneSep1.Size = new Size(200, 5);

            ComboBox comboBox1 = new ComboBox();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Dock = DockStyle.Right;
            comboBox1.FormattingEnabled = true;
            comboBox1.Name = "comboLeft_" + i;
            comboBox1.Size = new Size(120, 26);
            comboBox1.SelectedValueChanged += new EventHandler(comboBox_SelectedValueChanged);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(paneSep1);
            i++;

            FillComboGroupProducts(comboBox1, groupProdId);
        }

        private void SetPrices(int productId, int index)
        {
            string[] txtBox;

            foreach (Control c in pnlRight.Controls)
                foreach (Control cc in c.Controls)
                    foreach (Control ccc in cc.Controls)
                        if (ccc is TextBox)
                        {
                            txtBox = ccc.Name.Split('_');

                            if (txtBox.Length != 2)
                                continue;

                            if (txtBox[0].Equals("txtPrice") && txtBox[1].Equals(index.ToString()))
                            {
                                ccc.Text = Convert.ToString(GetProgramManager().GetDataManager().GetProductPriceByID(productId.ToString(), "3"));
                                continue;
                            }

                            if (txtBox[0].Equals("txtSelf") && txtBox[1].Equals(index.ToString()))
                            {
                                ccc.Text = Convert.ToString(GetProgramManager().GetDataManager().GetProductSelfCost(productId, 1, DateTime.Now));
                                continue;
                            }
                        }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            SetSums();
        }

        private void button_Click(object sender, EventArgs e)
        {
            DeleteComboBoxByName(((Button)sender).Name);
            SetSums();
        }

        private void SetSums()
        {
            float num = 0, price = 0, self = 0;
            string[] name;

            foreach (Control c in pnlRight.Controls)
                foreach (Control cc in c.Controls)
                    foreach (Control ccc in cc.Controls)
                        if (ccc is TextBox)
                        {
                            if (string.IsNullOrEmpty(ccc.Text))
                                continue;

                            name = ccc.Name.Split('_');

                            if (name.Length != 2)
                                continue;

                            if (name[0].Equals("txtNum"))
                            {
                                num = num + Convert.ToSingle(ccc.Text);
                                continue;
                            }

                            if (name[0].Equals("txtPrice"))
                            {
                                price = price + Convert.ToSingle(ccc.Text);
                                continue;
                            }

                            if (name[0].Equals("txtSelf"))
                            {
                                self = self + Convert.ToSingle(ccc.Text);
                                continue;
                            }
                        }

            lblNum.Text = num.ToString();
            lblPrice.Text = price.ToString();
            lblSelf.Text = self.ToString();
        }

        void comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedValue == null)
                return;

            if (lastValue == Convert.ToInt32(((ComboBox)sender).SelectedValue) && lastName.Equals(((ComboBox)sender).Name))
                return;

            FillComboProducts(((ComboBox)sender).Name, Convert.ToInt32(((ComboBox)sender).SelectedValue));

            lastValue = Convert.ToInt32(((ComboBox)sender).SelectedValue);
            lastName = ((ComboBox)sender).Name;
        }

        void comboBoxProduct_SelectedValueChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedValue == null)
                return;

            string[] combo = ((ComboBox)sender).Name.Split('_');

            if (combo.Length != 2)
                return;

            SetPrices((int)((ComboBox)sender).SelectedValue, int.Parse(combo[1]));
        }

        private void FillComboProducts(string comboName, int groupProdId)
        {
            ComboBox comboBox = GetComboBoxByName(comboName);

            if (comboBox == null)
                return;

            string sql = @"SELECT DISTINCT p.id, p.name FROM book.Products p
                         LEFT JOIN book.GroupProducts gp ON gp.id = " + groupProdId +
                         " WHERE p.path = gp.path OR p.path LIKE (gp.path + '#%')";
            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

            comboBox.ValueMember = "id";
            comboBox.DisplayMember = "name";
            comboBox.DataSource = data;
        }

        private ComboBox GetComboBoxByName(string comboName)
        {
            string[] parentName = comboName.Split('_');

            if (parentName.Length != 2)
                return null;

            foreach (Control c in pnlRight.Controls)
                foreach (Control cc in c.Controls)
                    foreach (Control ccc in cc.Controls)
                        if (ccc is ComboBox)
                        {
                            string[] targetName = ccc.Name.Split('_');

                            if (targetName.Length != 2)
                                return null;

                            if (targetName[1].Equals(parentName[1]))
                                return (ComboBox)ccc;
                        }

            return null;
        }

        private void DeleteComboBoxByName(string btnName)
        {
            string[] parentName = btnName.Split('_');

            if (parentName.Length != 2)
                return;

            foreach (Control c in pnlLeft.Controls)
                if (c is Panel)
                {
                    string[] targetName = c.Name.Split('_');

                    if (targetName.Length != 2)
                        continue;

                    if (targetName[1].Equals(parentName[1]))
                    {
                        pnlLeft.Controls.Remove(c);
                        break;
                    }
                }

            foreach (Control c in pnlRight.Controls)
                if (c is Panel)
                {
                    string[] targetName = c.Name.Split('_');

                    if (targetName.Length != 2)
                        continue;

                    if (targetName[1].Equals(parentName[1]))
                    {
                        pnlRight.Controls.Remove(c);
                        break;
                    }
                }
        }

        private void FillComboGroupProducts(ComboBox comboBox, int groupProdId)
        {
            string sql = "SELECT id, name FROM book.GroupProducts WHERE path IN(" + GetProductPaths() + ")";
            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

            if (data == null)
                return;

            if (data.Rows.Count == 0)
                return;

            comboBox.ValueMember = "id";
            comboBox.DisplayMember = "name";
            comboBox.DataSource = data;

            if (groupProdId != -1)
                comboBox.SelectedValue = groupProdId;
        }

        private string GetProductPaths()
        {
            string paths = string.Empty;
            string sql = "SELECT path FROM book.GroupProducts WHERE path LIKE '" + this.path + "#%'";
            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

            foreach (DataRow row in data.Rows)
            {
                if (Convert.ToString(row["path"]).Split('#').Length == 5)
                    paths = paths + "'" + row["path"] + "'" + ",";
            }

            return paths.TrimEnd(',');
        }

        private void comboTemplate_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboTemplate.SelectedValue == null)
                return;

            if (comboTemplate.SelectedValue.GetType() != typeof(int))
                return;

            lastValue = 0;
            lastName = string.Empty;

            GenerateTemplate((int)comboTemplate.SelectedValue);
        }
    }
}
