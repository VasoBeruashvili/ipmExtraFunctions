using ipmControls;
using ipmPMBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace ipmExtraFunctions
{
    public partial class EmailSendGeneralForm : Form
    {
        public ProgramManagerBasic ProgramManager {get; set;}
        DataTable m_DataEmails;
        Dictionary<char, string> alphabet;
        private void InitializeAlphabet()
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
                textLat += alphabet.ContainsKey(val) ? alphabet[val] : val.ToString();
            }
            if (textLat == string.Empty) textLat = text;

            return textLat;
        }
        public EmailSendGeneralForm()
        {
            InitializeComponent(); 
            txtBodyText.ScrollBars = ScrollBars.Vertical;
            InitializeAlphabet();
        }
        private void FillEmails()
        {
            int staffId = ProgramManager.GetDataManager().GetStaffIDByUserID(ProgramManager.GetUserID());

            string sql = "SELECT id, login, password, pop, smtp, pop_port, smtp_port, email_address, ssl, staff_id, bcc FROM book.StaffEMails WHERE staff_id=" + staffId + " order by id";
            m_DataEmails = ProgramManager.GetDataManager().GetTableData(sql);
            if (m_DataEmails == null || m_DataEmails.Rows.Count == 0)
                return;
                
            comboBoxEmailFrom.DataSource = m_DataEmails;
            comboBoxEmailFrom.ValueMember = "id";
            comboBoxEmailFrom.DisplayMember = "email_address";
            sql = "SELECT TOP(1) id FROM book.StaffEMails WHERE staff_id=" + staffId;
            int Id = ProgramManager.GetDataManager().GetIntegerValue(sql);
            comboBoxEmailFrom.SelectedValue = Id;
            txtBCC.Text = m_DataEmails.Rows.OfType<DataRow>().Where(x =>Convert.ToInt32(x["id"]) == Id).Select(x => x["bcc"].ToString()).First();           
        }
        private void OnAddRecipientsFromContacts()
        {
            string param = string.Empty;
            List<int> paramList = new List<int>();
            
                var list = ProgramManager.ShowSelectFormMulti("TABLE_CONTRAGENT", -1);
                if (list == null) 
                    return;

                paramList = list.ToList();
           
            if (paramList.Count == 0) 
                return;

            param = string.Join(",", paramList.ConvertAll<string>(p => p.ToString()).ToArray());

            string sql = @"SELECT ISNULL(CONVERT(NVARCHAR(MAX), contact.id), CONVERT(NVARCHAR(MAX), contr.id) +'-contr') AS id, 
                           ISNULL(contr.name, '') AS name, contr.code, ISNULL(contact.full_name, '') AS recipient_name, CASE WHEN contact.email IS NULL OR contact.email='' THEN ISNULL(contr.e_mail, '') ELSE contact.email END AS email 
                           FROM book.ContragentContacts AS contact
                           INNER JOIN book.Contragents AS contr ON contact.contragent_id=contr.id
                           WHERE contr.id IN (" + param + ")";

            DataTable data = new DataTable();
            data = ProgramManager.GetDataManager().GetTableData(sql);

            foreach (DataRow dr in data.Rows)
            {
                int count = m_GridRecipients.Rows.Cast<DataGridViewRow>().AsEnumerable().Where(p => p.Cells["contact_id"].Value.ToString() == dr["id"].ToString()).Count();
                if (count > 0) continue;
                List<string> emails = dr.Field<string>("email").Split(new char[] { ';', ',', '.' }).ToList();

                string param1 = dr.Field<string>("recipient_name") == string.Empty ? dr.Field<string>("name") : dr.Field<string>("recipient_name");

                foreach (string email in emails)
                {
                    m_GridRecipients.Rows.Add(dr.Field<string>("id"), dr.Field<string>("name"), dr.Field<string>("recipient_name"), email, "", convertToLat(param1), "", "False");
                }
            }
        }
      
        private void EmailSendGeneralForm_Load(object sender, EventArgs e)
        {
            FillEmails();
            ProgramManager.TranslateControl(this);
        }

        private void btnFromList_Click(object sender, EventArgs e)
        {
            OnAddRecipientsFromContacts();
        }

        private void btnManually_Click(object sender, EventArgs e)
        {
            m_GridRecipients.Rows.Add("", "", "", "", "", "", "", "False");
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

                string sql = @"SELECT ISNULL(CONVERT(NVARCHAR(MAX), contact.id), CONVERT(NVARCHAR(MAX), contr.id) +'-contr') AS id, 
                           ISNULL(contr.name, '') AS name, contr.code, ISNULL(contact.full_name, '') AS recipient_name, CASE WHEN contact.email IS NULL OR contact.email='' THEN ISNULL(contr.e_mail, '') ELSE contact.email END AS email 
                           FROM book.ContragentContacts AS contact
                           INNER JOIN book.Contragents AS contr ON contact.contragent_id=contr.id
                           WHERE contact.send_sms=1 AND contr.code IN (" + codes + ")";

                DataTable data = ProgramManager.GetDataManager().GetTableData(sql);

                if (data == null) return;

                List<string> emails;
                param1 = string.Empty;
                param2 = string.Empty;

                foreach (DataRow dr in data.Rows)
                {
                    int count = m_GridRecipients.Rows.Cast<DataGridViewRow>().AsEnumerable().Where(p => p.Cells["contact_id"].Value.ToString() == dr["id"].ToString()).Count();
                    if (count > 0) continue;
                    emails = dr.Field<string>("email").Split(new char[] { ';', ',', '.' }).ToList();


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
                    foreach (string email in emails)
                    {
                        m_GridRecipients.Rows.Add(dr.Field<string>("id"), dr.Field<string>("name"), dr.Field<string>("recipient_name"), email, "", string.IsNullOrEmpty(param1) ? dr.Field<string>("recipient_name") : convertToLat(param1), param2, "False");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("მონაცემების ჩასმა ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            _importData = ProgramManager.GetDataManager().GetTableDataFromExcel(dialog.FileName);
            if (_importData == null)
            {
                ProgressDispatcher.Deactivate();
                MessageBoxForm.Show(ProgramManager.GetTranslatorManager().Translate("შეცდომა"),
                ProgramManager.GetTranslatorManager().Translate("იმპორტი ვერ მოხერხდა!"), ProgramManager.GetTranslatorManager().Translate("ფაილი ვერ ჩაიტვირთა!"), null, SystemIcons.Error);
                return;
            }

            if (_importData.Rows.Count == 0)
            {
                ProgressDispatcher.Deactivate();
                MessageBoxForm.Show(ProgramManager.GetTranslatorManager().Translate("შეცდომა"),
                ProgramManager.GetTranslatorManager().Translate("იმპორტი ვერ მოხერხდა!"), ProgramManager.GetTranslatorManager().Translate("ფაილი არ შეიცავს ჩანაწერებს"), null, SystemIcons.Error);
                return;
            }

            if (!_importData.Columns.Contains(ProgramManager.GetTranslatorManager().Translate("ორგანიზაცია")) ||
                !_importData.Columns.Contains(ProgramManager.GetTranslatorManager().Translate("საკონტაქტო პირი")) ||
                !_importData.Columns.Contains(ProgramManager.GetTranslatorManager().Translate("ელ. ფოსტა")))
            {
                ProgressDispatcher.Deactivate();
                MessageBoxForm.Show(ProgramManager.GetTranslatorManager().Translate("შეცდომა"),
                ProgramManager.GetTranslatorManager().Translate("იმპორტი ვერ მოხერხდა!"), ProgramManager.GetTranslatorManager().Translate("ფაილი არ შეივაცს აუცილებელ ველებს!"), null, SystemIcons.Error);
                return;
            }

            int _columnCount = _importData.Columns.Count;
            _importData.Rows.OfType<DataRow>().ToList().ForEach(r =>
            {
                string _contactName = Convert.ToString(r[1]);
                string _nameField = _contactName;
                if (_columnCount >= 4 && !string.IsNullOrEmpty(Convert.ToString(r[3])))
                {
                    _nameField = Convert.ToString(r[3]);
                }

                m_GridRecipients.Rows.Add("", Convert.ToString(r[0]), _contactName, Convert.ToString(r[2]), "", _nameField, _columnCount >= 5 ? Convert.ToString(r[4]) : "", "False");
            });

            ProgressDispatcher.Deactivate();
            MessageBoxForm.Show(ProgramManager.GetTranslatorManager().Translate("ინფორმაცია"), ProgramManager.GetTranslatorManager().Translate("იმპორტი წარმატებით შესრულდა!"), null, null, SystemIcons.Information);
        }
        private void OnDeleteRecipient()
        {
            if (m_GridRecipients.SelectedRows.Count == 0) 
                return;

            if (MessageBox.Show(ProgramManager.GetTranslatorManager().Translate("გინდათ ჩანაწერის წაშლა?"), ProgramManager.GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (DataGridViewRow dgr in m_GridRecipients.SelectedRows)
                {
                    m_GridRecipients.Rows.Remove(dgr);
                }
            }
        }
       public void OnSendEmail()
        {
            if (comboBoxEmailFrom.SelectedValue == null)
                return;

            int _id = Convert.ToInt32(comboBoxEmailFrom.SelectedValue ?? 0);
                                        
            var mail_info = m_DataEmails.Rows.OfType<DataRow>().Where(x => Convert.ToInt32( x["id"])==_id).Select(x => new
            {
                SmtpHost = x["smtp"],
                SmtpPort = x["smtp_port"],
                EnableSsl = x["ssl"],
                SenderAddress = x["email_address"],
                Login = x["login"],
                Password = x["password"]
            }).FirstOrDefault();

            string SmtpHost = mail_info.SmtpHost.ToString();
            int SmtpPort = Convert.ToInt32(mail_info.SmtpPort);
            bool EnableSsl = Convert.ToString(mail_info.EnableSsl) == "1" ? true : false;
            string SenderAddress = mail_info.SenderAddress.ToString();
            string Login = mail_info.Login.ToString();
            string Password = FinaKeyGenerator.Decrypt(mail_info.Password.ToString());
            ProgressDispatcher.Activate();
            SendEmail(txtSubject.Text, SmtpHost, SmtpPort, EnableSsl, Login, Password);
            ProgressDispatcher.Deactivate();
            
               
        }
       public void SendEmail(string Subject, string SmtpHost, int SmtpPort, bool EnableSsl, string login, string password)
       {
           List<DataGridViewRow> data = m_GridRecipients.Rows.Cast<DataGridViewRow>().AsEnumerable().Where(p => p.Cells["status_value"].Value.ToString() == "False").ToList();
           if (data == null) return;
           
           foreach (DataGridViewRow _row in data)
           {
               string _senderAddress = Convert.ToString(_row.Cells["email"].Value);
               if (string.IsNullOrEmpty(_senderAddress))
                   continue;

               string bodyText = txtBodyText.Text;

               if (bodyText.Contains("<name>"))
                   bodyText = bodyText.Replace("<name>", _row.Cells["param1"].Value.ToString());

               if (bodyText.Contains("<param>"))
                   bodyText = bodyText.Replace("<param>", _row.Cells["param2"].Value.ToString());

               try
               {
                   using (MailMessage mailMessage = new MailMessage()
                   {
                       From = new MailAddress(_senderAddress),
                       Priority = MailPriority.High,
                       Body = bodyText,
                       Subject = Subject
                   })
                   {

                       SmtpClient smtpClient = new SmtpClient()
                       {
                           Host = SmtpHost,
                           Port = SmtpPort,
                           Credentials = new NetworkCredential(login, password),
                           EnableSsl = EnableSsl
                       };

                       mailMessage.To.Add(_senderAddress);

                       smtpClient.Send(mailMessage);
                       _row.Cells["status"].Value = ProgramManager.GetTranslatorManager().Translate("ელ. ფოსტა წარმატებით გაიგზავნა!");
                       _row.Cells["status"].Style.BackColor = Color.LightGreen;
                       _row.Cells["status_value"].Value = true;
                   }


               }
               catch
               {
                   _row.Cells["status"].Value = ProgramManager.GetTranslatorManager().Translate("ელ. ფოსტა ვერ გაიგზავნა!");
                   _row.Cells["status"].Style.BackColor = Color.LightPink;
                   _row.Cells["status_value"].Value = false;
               }
           }

       }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            OnDeleteRecipient();
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            OnSendEmail();
        }
    }
}
