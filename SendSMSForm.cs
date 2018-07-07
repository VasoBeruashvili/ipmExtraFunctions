using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ipmPMBasic;
using System.Net;
using System.IO;
using ipmControls;

namespace ipmExtraFunctions
{
    public partial class SendSMSForm : Form
    {
        List<Recipient> recipients = new List<Recipient>();
        ProgramManagerBasic ProgramManager;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
        public string ServiceId { get; set; }
        public SMSServiceType ServiceType { get; set; }

        HttpWebRequest Request;
        HttpWebResponse Response;
        string bodyText, URL;
        bool Successful;
        List<int> Ids = new List<int>();
        Dictionary<char, string> alphabet;
        public SendSMSForm(ProgramManagerBasic PM, List<int> ids)
        {
            InitializeComponent();
            ProgramManager = PM;
            txtBodyText.ScrollBars = ScrollBars.Vertical;
            initializeAlphabet();
            SetSMSParameters();
            GetProgramManager().TranslateControl(this);            
            if (ids != null)
            {
                Ids = ids;
                onAddRecipientsFromContacts(true);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.V:
                    {
                        PasteData();
                        break;
                    }
            }

            return base.ProcessCmdKey(ref msg, keyData);

        }
        private void PasteData()
        {
            DataTable DataContragents = new DataTable();
            DataContragents.Columns.Add("code", typeof(string));
            DataContragents.Columns.Add("param1", typeof(string));
            DataContragents.Columns.Add("param2", typeof(string));

            try
            {

                string dataFromClipboard = Clipboard.GetText();
                if (dataFromClipboard == "")
                    return;

                string[] rows = dataFromClipboard.Split('\n');
                if (rows.Length == 0) return;

                string code = string.Empty, param1 = string.Empty, param2 = string.Empty;
                foreach (string row in rows)
                {
                    string[] values = row.Split('\t');
                    if (values.Length == 0) continue;

                    if (values[0] == string.Empty) continue;

                    code = values[0].Trim();

                    if (values.Length >= 2)
                        param1 = values[1];

                    if (values.Length >= 3)
                        param2 = values[2];

                    DataContragents.Rows.Add(code, param1, param2);

                    code = string.Empty;
                    param1 = string.Empty;
                    param2 = string.Empty;
                }

                if (DataContragents.Rows.Count == 0) return;

                string codes = string.Join(", ", DataContragents.Rows.Cast<DataRow>().Select(p => string.Format("'{0}'", p.Field<string>("code"))).ToArray());

                string sql =@"SELECT ISNULL(CONVERT(NVARCHAR(MAX), contact.id), CONVERT(NVARCHAR(MAX), contr.id) +'-contr') AS id, 
                           ISNULL(contr.name, '') AS name, contr.code, ISNULL(contact.full_name, '') AS recipient_name, CASE WHEN contact.tel IS NULL OR contact.tel='' THEN ISNULL(contr.tel, '') ELSE contact.tel END AS tel 
                           FROM book.ContragentContacts AS contact
                           INNER JOIN book.Contragents AS contr ON contact.contragent_id=contr.id
                           WHERE contact.send_sms=1 AND contr.code IN (" + codes + ")";

                DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

                if (data == null) return;

                List<string> numbers;
                param1 = string.Empty;
                param2 = string.Empty;

                foreach (DataRow dr in data.Rows)
                {
                    int count = m_GridRecipients.Rows.Cast<DataGridViewRow>().AsEnumerable().Where(p => p.Cells["contact_id"].Value.ToString() == dr["id"].ToString()).Count();
                    if (count > 0) continue;
                    numbers = getTelFormatFromString(dr.Field<string>("tel"));
                    

                    var currentRow = DataContragents.Rows.Cast<DataRow>().Where(p => p.Field<string>("code") == dr.Field<string>("code")).FirstOrDefault();
                    if (currentRow != null)
                    {
                        param1 = currentRow.Field<string>("param1");
                        if (param1 == string.Empty)
                        {
                            param1 = dr.Field<string>("recipient_name") == string.Empty ? dr.Field<string>("name") : dr.Field<string>("recipient_name");
                        }
                        param2 = currentRow.Field<string>("param2");
                    }
                    foreach (string number in numbers)
                    {
                        m_GridRecipients.Rows.Add(dr.Field<string>("id"), dr.Field<string>("name"), dr.Field<string>("recipient_name"), number, "", string.IsNullOrEmpty(param1)? dr.Field<string>("recipient_name"): convertToLat(param1), param2, "False");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("მონაცემების ჩასმა ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void initializeAlphabet()
        {
            alphabet = new Dictionary<char, string>();
            alphabet.Add('ა', "a");
            alphabet.Add('ბ', "b");
            alphabet.Add('გ', "g");
            alphabet.Add('დ', "d");
            alphabet.Add('ე', "e");
            alphabet.Add('ვ', "v");
            alphabet.Add('ზ', "z");
            alphabet.Add('თ', "t");
            alphabet.Add('ი', "i");
            alphabet.Add('კ', "k");
            alphabet.Add('ლ', "l");
            alphabet.Add('მ', "m");
            alphabet.Add('ნ', "n");
            alphabet.Add('ო', "o");
            alphabet.Add('პ', "p");
            alphabet.Add('ჟ', "jh");
            alphabet.Add('რ', "r");
            alphabet.Add('ს', "s");
            alphabet.Add('ტ', "t");
            alphabet.Add('უ', "u");
            alphabet.Add('ფ', "f");
            alphabet.Add('ქ', "q");
            alphabet.Add('ღ', "g");
            alphabet.Add('ყ', "k");
            alphabet.Add('შ', "sh");
            alphabet.Add('ჩ', "ch");
            alphabet.Add('ც', "c");
            alphabet.Add('ძ', "dz");
            alphabet.Add('წ', "ts");
            alphabet.Add('ჭ', "tch");
            alphabet.Add('ხ', "kh");
            alphabet.Add('ჯ', "j");
            alphabet.Add('ჰ', "h");

        }
        private string convertToLat(string text)
        {
            if (text == string.Empty) return text;

            string textLat = string.Empty;
            char[] characters = text.ToCharArray();
            foreach (char val in characters)
            {
                textLat += alphabet.ContainsKey(val)? alphabet[val]:val.ToString();
            }
            if (textLat == string.Empty) textLat = text;

                return textLat;
        }
        private void SetSMSParameters()
        {
            UserName = ProgramManager.GetConfigParamValue("SMS_UserName");
            string password = FinaKeyGenerator.Decrypt(ProgramManager.GetConfigParamValue("SMS_Password"));
            Password = password == null ? "" : password;
            ClientId = ProgramManager.GetConfigParamValue("SMS_ClientId");
            ServiceId = ProgramManager.GetConfigParamValue("SMS_ServiceId");

            int _serviceTypeId = Convert.ToInt32(ProgramManager.GetConfigParamValue("SMSServiceType"));
            ServiceType = (SMSServiceType)_serviceTypeId;
        }

        private string onSendRequest(string url, ref bool successful)
        {
            try
            {
                this.Request = (HttpWebRequest)WebRequest.Create(url);
                this.Request.Method = "GET";
                this.Request.ContentType = "application/charset=utf-8";
                this.Response = (HttpWebResponse)this.Request.GetResponse();
                string Result;

                using (StreamReader resultStream = new StreamReader(this.Response.GetResponseStream(), Encoding.UTF8))
                {
                    string code = resultStream.ReadToEnd();
                    if (code == null) return "შეტყობინება ვერ გაიგზავნა!";

                    code = code.Split('-').FirstOrDefault().Trim();
                    switch (code)
                    {
                        case "0000": Result = GetProgramManager().GetTranslatorManager().Translate("შეტყობინება წარმატებით გაიგზავნა!"); successful = true; break;
                        case "0001": Result = GetProgramManager().GetTranslatorManager().Translate("შეტყობინების პარამეტრები არასწორია!"); successful = false; break;
                        case "0003": Result = GetProgramManager().GetTranslatorManager().Translate("არასწორი მოთხოვნა!"); successful = false; break;
                        case "0005": Result = GetProgramManager().GetTranslatorManager().Translate("ცარიელი შეტყობინება დაუშვებელია!"); successful = false; break;
                        case "0006": Result = GetProgramManager().GetTranslatorManager().Translate("არასწორი პრეფიქსი!"); successful = false; break;
                        case "0007": Result = GetProgramManager().GetTranslatorManager().Translate("მიმღების ნომერი არასწორია!"); successful = false; break;
                        default: Result = GetProgramManager().GetTranslatorManager().Translate("შეტყობინება ვერ გაიგზავნა!"); successful = false; break;
                    }
                }
                return Result;
            }
            catch (Exception)
            {
                return "შეტყობინება ვერ გაიგზავნა!";
            }
        }
        private void onSendSMS()
        {
            if (m_GridRecipients.Rows.Count == 0) return;

            if (txtBodyText.Text == string.Empty)
            {
                MessageBox.Show("მიუთითეთ შეტყობინების ტექსტი!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var data = m_GridRecipients.Rows.Cast<DataGridViewRow>().AsEnumerable().Where(p => p.Cells["status_value"].Value.ToString() == "False");
            if (data == null) return;

            foreach (var row in data)
            {
                List<string> numberTo = getTelFormatFromString(row.Cells["tel_number"].Value.ToString());

                bodyText = txtBodyText.Text;

                if (bodyText.Contains("<name>"))
                    bodyText = bodyText.Replace("<name>", row.Cells["param1"].Value.ToString());

                if (bodyText.Contains("<param>"))
                    bodyText = bodyText.Replace("<param>", row.Cells["param2"].Value.ToString());
                foreach (string number in numberTo)
                {
                    if (ServiceType == SMSServiceType.Beeline)
                    {
                        bodyText = convertToLat(bodyText);
                        URL = "http://msg.ge/bi/sendsms.php?username=" + this.UserName + @"&password=" +
                              this.Password + @"&client_id=" + this.ClientId + "&service_id=" + this.ServiceId +
                              "&to=" + number + "&text=" + Uri.EscapeDataString(bodyText);
                    }
                    else
                    {
                        URL = "http://81.95.160.47/mt/oneway?username=" + this.UserName + @"&password=" +
                              this.Password + @"&client_id=" + this.ClientId + "&service_id=" + this.ServiceId +
                              "&to=" + number + "&text=" + bodyText+"&coding=2";
                    }
                   
                    row.Cells["status"].Value = onSendRequest(URL, ref Successful);
                    row.Cells["status_value"].Value = Successful;
                    row.Cells["status"].Style.BackColor = Successful ? Color.LightGreen : Color.LightPink;
                }
            }

         
        }
        private ProgramManagerBasic GetProgramManager()
        {
            return ProgramManager;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtBodyText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.A))
                txtBodyText.SelectAll();
        }
        private List<string> getTelFormatFromString(string txt)
        {
            List<string> telNumbers = new List<string>();

            var numbers = txt.Split(new char[] { ';', ',', '.' }).AsEnumerable();

            string digitalNumber;
            foreach (string number in numbers)
            {
                digitalNumber = new string(number.ToCharArray().Where(p => char.IsDigit(p)).ToArray());
                if ((digitalNumber.Length < 12) && (digitalNumber.Length != 0))
                {
                    digitalNumber = "995" + digitalNumber;
                }
                telNumbers.Add(digitalNumber);
            }
            
            return telNumbers;
        }
       
        private void onAddRecipientsFromContacts(bool atStart)
        {
            string param = string.Empty;
            List<int> paramList = new List<int>();
            if (atStart)
            {
                paramList = Ids;
            }
            else
            {
                var list=GetProgramManager().ShowSelectFormMulti("TABLE_CONTRAGENT", -1);
                if (list == null) return;
                paramList =  list.ToList();                            
            }
            if (paramList.Count == 0) return;

            param = string.Join(",", paramList.ConvertAll<string>(p => p.ToString()).ToArray());

            string sql = @"SELECT CONVERT(NVARCHAR(MAX), contr.id) +'-contr' AS id, 
                           ISNULL(contr.name, '') AS name, ISNULL(contr.name, '') AS recipient_name, ISNULL(contr.tel, '') tel                           
                           FROM book.Contragents AS contr 
                           WHERE ISNULL(contr.tel, '')<>'' AND contr.id IN (" + param + @")

                           UNION ALL

                           SELECT ISNULL(CONVERT(NVARCHAR(MAX), contact.id), CONVERT(NVARCHAR(MAX), contr.id) +'-contr') AS id, 
                           ISNULL(contr.name, '') AS name, ISNULL(contact.full_name, '') AS recipient_name, CASE WHEN contact.tel IS NULL OR contact.tel='' THEN ISNULL(contr.tel, '') ELSE contact.tel END AS tel 
                           FROM book.ContragentContacts AS contact
                           INNER JOIN book.Contragents AS contr ON contact.contragent_id=contr.id
                           WHERE contact.send_sms=1 AND contr.id IN (" + param + @")";



            DataTable data = new DataTable();
            data = GetProgramManager().GetDataManager().GetTableData(sql);

            foreach (DataRow dr in data.Rows)
            {
                int count = m_GridRecipients.Rows.Cast<DataGridViewRow>().AsEnumerable().Where(p => p.Cells["contact_id"].Value.ToString() == dr["id"].ToString()).Count();
                if (count > 0) continue;
                List<string> numbers = getTelFormatFromString(dr.Field<string>("tel"));
               
                string param1 = dr.Field<string>("recipient_name") == string.Empty ? dr.Field<string>("name") : dr.Field<string>("recipient_name");

                foreach (string number in numbers)
                {
                    m_GridRecipients.Rows.Add(dr.Field<string>("id"), dr.Field<string>("name"), dr.Field<string>("recipient_name"), number, "", convertToLat(param1), "", "False");
                }
            }
        }
              
        private void onDeleteRecipient()
        {
            if (m_GridRecipients.SelectedRows.Count == 0) return;

            if (MessageBox.Show(GetProgramManager().GetTranslatorManager().Translate("გინდათ ჩანაწერის წაშლა?"),GetProgramManager().GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
            {
                foreach (DataGridViewRow dgr in m_GridRecipients.SelectedRows)
                {
                    m_GridRecipients.Rows.Remove(dgr);
                }
            }
        }        
        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (SMSSettingsForm Form = new SMSSettingsForm(GetProgramManager(), this))
            {
                Form.ShowDialog();
            }
        }
       
        private void btnSend_Click(object sender, EventArgs e)
        {
            onSendSMS();
        }

      

        private void btnFromList_Click(object sender, EventArgs e)
        {
            onAddRecipientsFromContacts(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            onDeleteRecipient();
        }

        private void btnManually_Click(object sender, EventArgs e)
        {
            m_GridRecipients.Rows.Add("", "", "", "", "", "","", "False");
        }

        private void m_GridRecipients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            if (new List<int>() { 3, 5, 6 }.Contains(e.ColumnIndex))
            {
                //m_GridRecipients.CurrentCell = m_GridRecipients.Rows[e.RowIndex].Cells[e.ColumnIndex];
                m_GridRecipients.BeginEdit(true);
            }
        }

        private void btnImportFromExcel_Click(object sender, EventArgs e)
        {
            
            DataTable _importData = new DataTable();          
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel Files|*.xls;*.xlsx;*";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            ProgressDispatcher.Activate();
            _importData = GetProgramManager().GetDataManager().GetTableDataFromExcel(dialog.FileName);
            if (_importData == null)
            {
                ProgressDispatcher.Deactivate();
                MessageBoxForm.Show(GetProgramManager().GetTranslatorManager().Translate("შეცდომა"), 
                GetProgramManager().GetTranslatorManager().Translate("იმპორტი ვერ მოხერხდა!"), GetProgramManager().GetTranslatorManager().Translate("ფაილი ვერ ჩაიტვირთა!"), null, SystemIcons.Error);
                return;
            }

            if (_importData.Rows.Count == 0)
            {
                ProgressDispatcher.Deactivate();
                MessageBoxForm.Show(GetProgramManager().GetTranslatorManager().Translate("შეცდომა"),
                GetProgramManager().GetTranslatorManager().Translate("იმპორტი ვერ მოხერხდა!"), GetProgramManager().GetTranslatorManager().Translate("ფაილი არ შეიცავს ჩანაწერებს"), null, SystemIcons.Error);
                return;
            }

            if (!_importData.Columns.Contains(GetProgramManager().GetTranslatorManager().Translate("ორგანიზაცია")) ||
                !_importData.Columns.Contains(GetProgramManager().GetTranslatorManager().Translate("საკონტაქტო პირი")) ||
                !_importData.Columns.Contains(GetProgramManager().GetTranslatorManager().Translate("ტელეფონი")))
            {
                ProgressDispatcher.Deactivate();
                MessageBoxForm.Show(GetProgramManager().GetTranslatorManager().Translate("შეცდომა"),
                GetProgramManager().GetTranslatorManager().Translate("იმპორტი ვერ მოხერხდა!"), GetProgramManager().GetTranslatorManager().Translate("ფაილი არ შეივაცს აუცილებელ ველებს!"), null, SystemIcons.Error);
                return;
            }

            int _columnCount = _importData.Columns.Count;
             _importData.Rows.OfType<DataRow>().ToList().ForEach(r =>
                {
                    string _contactName=Convert.ToString(r[1]);
                    string _nameField = _contactName;
                    if (_columnCount >= 4 && !string.IsNullOrEmpty(Convert.ToString(r[3])))
                    {
                        _nameField = Convert.ToString(r[3]);
                    }
                    
                    m_GridRecipients.Rows.Add("", Convert.ToString(r[0]), _contactName, Convert.ToString(r[2]), "", _nameField, _columnCount >= 5 ? Convert.ToString(r[4]) : "", "False");
                });

             ProgressDispatcher.Deactivate();
             MessageBoxForm.Show(GetProgramManager().GetTranslatorManager().Translate("ინფორმაცია"), GetProgramManager().GetTranslatorManager().Translate("იმპორტი წარმატებით შესრულდა!"), null, null, SystemIcons.Information);
        }
       
    }
}
