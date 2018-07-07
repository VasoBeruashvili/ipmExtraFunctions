using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ipmPMBasic;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Collections;
using System.IO;


namespace ipmExtraFunctions
{
    public partial class EmailSendForm : Form
    {
        ProgramManagerBasic ProgramManager;
        DataTable DataEmails, DataTemplates;
        List<string> contragentEmails = new List<string>();
        List<string> emailBCC = new List<string>();
        bool templatesAreInitialized = false;
        List<int> contagentIds = new List<int>();
        List<string> Paths = new List<string>();       
        public EmailSendForm(List<int> ids, ProgramManagerBasic PM, string file_path)
        {
            InitializeComponent();
            ProgramManager = PM;
            contagentIds = ids;
            fillEmails();
            fillTemplates();
            templatesAreInitialized = true;
            txtBodyText.ScrollBars = ScrollBars.Vertical;
            getContagentEmails();
            if(!string.IsNullOrEmpty(file_path))
                attachFromReport(file_path);
            PM.TranslateControl(this);
            this.Text = PM.GetTranslatorManager().Translate("ელ. ფოსტის გაგზავნა:") + " " + PM.GetTranslatorManager().Translate("ახალი");
        }
        private ProgramManagerBasic GetProgramManager()
        {
            return ProgramManager;
        }
        public void attachFromReport(string fileName)
        {
            m_GridFiles.Rows.Clear();
            System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(fileName);
            string name = Path.GetFileName(fileName);
            m_GridFiles.Rows.Add(icon, name, fileName);
        }
        private void getTemplateBodyText(string id)
        {
          txtBodyText.Text=DataTemplates.Rows.Cast<DataRow>().Where(x => x["id"].ToString()==id).Select(x => x["body_text"].ToString()).First();
          txtSubject.Text = DataTemplates.Rows.Cast<DataRow>().Where(x => x["id"].ToString() == id).Select(x => x["subject"].ToString()).First();
        }
        private void getEmailBCC(string id)
        {
            txtBCC.Text = DataEmails.Rows.Cast<DataRow>().Where(x => x["id"].ToString() == id).Select(x => x["bcc"].ToString()).First() ;
        }
        private void getContagentEmails()
        {
            if (contagentIds.Count <= 0)
                return;
            string Ids = string.Join(",", contagentIds.ConvertAll<string>(x=>x.ToString()).ToArray());
            string sql = "SELECT id, name, e_mail FROM book.Contragents WHERE id in (" + Ids + ") order by id";
            DataTable Data = GetProgramManager().GetDataManager().GetTableData(sql);
            foreach (DataRow dr in Data.Rows)
            {
                if (!string.IsNullOrEmpty(dr["e_mail"].ToString()))
                contragentEmails.Add(dr["e_mail"].ToString());
            }
            txtEmailTo.Text = string.Join(",", contragentEmails.ToArray());
        }
        private void onEditEmailTo()
        {
            contragentEmails.Clear();
            foreach (string email in txtEmailTo.Text.Split(','))
            {
                if(!string.IsNullOrEmpty(email))
                    contragentEmails.Add(email);
            }            
        }
        private void onEditBCC()
        {
            emailBCC.Clear();
            foreach (string BCC in txtBCC.Text.Split(','))
            {
                if (!string.IsNullOrEmpty(BCC))
                    emailBCC.Add(BCC);
            }
        }
        private void onAttach()
        {
            OpenFileDialog OpenDialog = new OpenFileDialog();
            OpenDialog.Multiselect = true;
            OpenDialog.Filter = "All Files|*.*";
            
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
              
                foreach (string file in OpenDialog.FileNames)
                {
                   
                    int n=m_GridFiles.Rows.Cast<DataGridViewRow>().Where(x=>x.Cells[file_path.Index].Value.ToString()==file).Count();
                    if (n == 0)
                    {
                        System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(file);
                        string file_name = Path.GetFileName(file);
                        m_GridFiles.Rows.Add(icon, file_name, file);
                    }
                }
            
            }           

        }
        private string GenerateDocNum()
        {
            string sqlText = "SELECT MAX(doc_num) as res FROM doc.GeneralDocs INNER JOIN config.DocTypes ON doc.GeneralDocs.doc_type = config.DocTypes.id WHERE config.DocTypes.tag='DOC_CRM_ACTION' ";
            Int64 max_value = GetProgramManager().GetDataManager().GetInt64Value(sqlText) + 1;
            return max_value.ToString();
        }
        private bool insertActionInCRM(int contragent_id)
        {
            int doc_type = GetProgramManager().GetDataManager().GetDocTypeByTag("DOC_CRM_ACTION");
            if (doc_type <= 0)
                return false;

            string docNumber = GenerateDocNum();

            int doc_num = 0;
            if (!int.TryParse(docNumber, out doc_num))
                return false;

            int contact_id = 0;
            int client_id = 0;
            int.TryParse(Convert.ToString(contragent_id), out client_id);

            int general_id = GetProgramManager().GetDataManager().Insert_GeneralDocs(DateTime.Now, string.Empty, doc_num, doc_type,
              "მოქმედება № " + docNumber, 0, 1, 1, 0, GetProgramManager().GetUserID(), 0, client_id, contact_id, 0, false, 1,1,0);
            if (general_id <= 0)
                return false;

            Hashtable mParams = new Hashtable();
            string sql = @"INSERT INTO doc.CRMActions (general_id,incident_type,comment) 
                         VALUES(@general_id,@incident_type,@comment)";
            mParams.Add("@general_id", general_id);
            mParams.Add("@incident_type", 1);
            mParams.Add("@comment", txtBodyText.Text);
            if (!GetProgramManager().GetDataManager().ExecuteSql(sql, mParams))
                return false;

            if (!GetProgramManager().GetDataManager().ExecuteSql("DELETE FROM doc.GeneralDocsAttachments WHERE general_id=" + general_id))
                return false;

            string file_name = string.Empty;
            byte[] file;
            Hashtable M_params = new Hashtable();
            foreach (string file_path in Paths)
            {
                M_params.Clear();                
                file = File.ReadAllBytes(file_path);

                file_name = file_path.Split('\\').Last().ToString();

                M_params.Add("@file_name", file_name);
                M_params.Add("@file", file);
                M_params.Add("@general_id", general_id);

                sql = "INSERT INTO doc.GeneralDocsAttachments (general_id,name,data_file) VALUES(@general_id,@file_name,@file)";
                if (!GetProgramManager().GetDataManager().ExecuteSql(sql, M_params))
                    return false;

            }
            return true;
        }
        public void onSendEmail(string id)
        {
            if (string.IsNullOrEmpty(txtEmailTo.Text.Trim()))
            {
                MessageBox.Show(GetProgramManager().GetTranslatorManager().Translate("გთხოვთ მიუთითოთ ელ.ფოსტის მისამართი!"), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string MessageText = !string.IsNullOrEmpty(txtBodyText.Text) ? txtBodyText.Text : "(no text)";
            bool enableCRM =GetProgramManager().GetConfigParamValue("CRM") == "0" ? false : true;
            Paths.AddRange(m_GridFiles.Rows.Cast<DataGridViewRow>().Select(x => x.Cells[file_path.Index].Value.ToString()).ToList());
            var mail_info = DataEmails.Rows.OfType<DataRow>().Where(x => x["id"].ToString().Equals(id)).Select(x => new
                {
                    SmtpHost = x["smtp"],
                    SmtpPort = x["smtp_port"],
                    EnableSsl = x["ssl"],
                    SenderAddress = x["email_address"],
                    Login = x["login"],
                    Password= x["password"]
                }).FirstOrDefault();

            string SmtpHost = mail_info.SmtpHost.ToString();
            int SmtpPort = Convert.ToInt32(mail_info.SmtpPort);
            bool EnableSsl = Convert.ToString(mail_info.EnableSsl) == "1" ? true : false;
            string SenderAddress = mail_info.SenderAddress.ToString();
            string Login = mail_info.Login.ToString();
            string Password = FinaKeyGenerator.Decrypt(mail_info.Password.ToString());
            if (SendEmail(Paths, txtSubject.Text, SmtpHost, SmtpPort, EnableSsl, SenderAddress, Login, Password, contragentEmails, emailBCC, MessageText))
            {
                if (enableCRM)
                {
                    if (contagentIds.Count != 0)
                    {
                        foreach (int contragent_id in contagentIds)
                        {
                            insertActionInCRM(contragent_id);
                        }
                    }
                    else
                    {
                        insertActionInCRM(0);
                    }
                }
                MessageBox.Show(GetProgramManager().GetTranslatorManager().Translate("ელ.ფოსტა წარმატებით გაიგზავნა!"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                MessageBox.Show(GetProgramManager().GetTranslatorManager().Translate("ელ.ფოსტა ვერ გაიგზავნა!"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            comboBoxTemplate.SelectedValue = 0;
            txtBodyText.Clear();
        }
        public bool SendEmail(List<string> file, string Subject, string SmtpHost, int SmtpPort, bool EnableSsl, string SenderAddress, string login, string password, List<string> ReceiverAddress, List<string> BCC, string MessageText)
        {
           
                Attachment fileAttachment;
                using (MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(SenderAddress),
                    Priority = MailPriority.High,
                    Body = MessageText,
                    Subject = Subject
                })
                {
                    try
                    {
                        SmtpClient smtpClient = new SmtpClient()
                        { 
                            Host = SmtpHost,
                            Port = SmtpPort,
                            Credentials = new NetworkCredential(login, password),
                            EnableSsl = EnableSsl
                        };
                        file.ForEach(filePath =>
                        {
                            if (!string.IsNullOrEmpty(filePath))
                            {
                                fileAttachment = new Attachment(filePath, MediaTypeNames.Application.Octet);
                                ContentDisposition disposition = fileAttachment.ContentDisposition;
                                disposition.CreationDate = System.IO.File.GetCreationTime(filePath);
                                disposition.ModificationDate = System.IO.File.GetLastWriteTime(filePath);
                                disposition.ReadDate = System.IO.File.GetLastAccessTime(filePath);
                                mailMessage.Attachments.Add(fileAttachment);
                            }
                        });
                        ReceiverAddress.ForEach(toEmail => { mailMessage.To.Add(toEmail); });
                        BCC.ForEach(bcc => { mailMessage.Bcc.Add(bcc);});
                        smtpClient.Send(mailMessage);

                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    return true;
                }
        }
        private void fillEmails()
        {
            int staffId= GetProgramManager().GetDataManager().GetStaffIDByUserID(GetProgramManager().GetUserID());

            string sql = "SELECT id, login, password, pop, smtp, pop_port, smtp_port, email_address, ssl, staff_id, bcc FROM book.StaffEMails WHERE staff_id="+staffId+" order by id";
            DataEmails = new DataTable();
            DataEmails = GetProgramManager().GetDataManager().GetTableData(sql);           
            comboBoxEmailFrom.DataSource = DataEmails;
            comboBoxEmailFrom.ValueMember = "id";
            comboBoxEmailFrom.DisplayMember = "email_address";
            sql = "SELECT TOP(1) id FROM book.StaffEMails WHERE staff_id=" + staffId;
            int Id = GetProgramManager().GetDataManager().GetIntegerValue(sql);
            comboBoxEmailFrom.SelectedValue = Id;
            try
            {
                getEmailBCC(Convert.ToString(comboBoxEmailFrom.SelectedValue));
            }
            catch
            {
            }
        }
        private void fillTemplates()
        {
            string sql = "SELECT id, name, body_text, subject FROM book.EmailTemplates UNION ALL SELECT 0 AS id, '' AS name, '' AS body_text, '' AS subject order by id";
            DataTemplates = new DataTable();
            DataTemplates = GetProgramManager().GetDataManager().GetTableData(sql);
            comboBoxTemplate.DataSource = DataTemplates;
            comboBoxTemplate.ValueMember = "id";
            comboBoxTemplate.DisplayMember = "name";

        }
        private void onDelete()
        {
            foreach (DataGridViewRow dgr in m_GridFiles.SelectedRows)
            {
                m_GridFiles.Rows.Remove(dgr);
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBoxTemplate_SelectedValueChanged(object sender, EventArgs e)
        {
            if (templatesAreInitialized)
            getTemplateBodyText(comboBoxTemplate.SelectedValue.ToString());
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(comboBoxEmailFrom.SelectedValue)!="0") 
            onSendEmail(comboBoxEmailFrom.SelectedValue.ToString());
        }



        private void btnAddFile_Click(object sender, EventArgs e)
        {
            onAttach();
        }

        private void btnDeleteEmail_Click(object sender, EventArgs e)
        {
            onDelete();
        }

        private void txtEmailTo_TextChanged(object sender, EventArgs e)
        {
            onEditEmailTo();
        }

        private void txtBCC_TextChanged(object sender, EventArgs e)
        {
            onEditBCC();
        }

        private void comboBoxEmailFrom_SelectedValueChanged(object sender, EventArgs e)
        {
            if(templatesAreInitialized)
            getEmailBCC(comboBoxEmailFrom.SelectedValue.ToString());
        }

        private void txtBodyText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control|Keys.A))
                txtBodyText.SelectAll();
        }  
    }
}
