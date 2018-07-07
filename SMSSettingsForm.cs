using System;
using System.Windows.Forms;
using ipmPMBasic;


namespace ipmExtraFunctions
{
    public partial class SMSSettingsForm : Form
    {
        ProgramManagerBasic ProgramManager;
        new SendSMSForm ParentForm;      

        public SMSSettingsForm(ProgramManagerBasic PM, SendSMSForm p_form)
        {
            InitializeComponent();
            ProgramManager = PM;
            ParentForm = p_form;
            setExistingData();
        }
        private void setExistingData()
        {
            comboBoxServiceType.Items.Clear();
            comboBoxServiceType.Items.AddRange(new string[]
            {
                "Beeline", 
                "MagtiCom"
            });
            comboBoxServiceType.SelectedIndex =Convert.ToInt32(ProgramManager.GetConfigParamValue("SMSServiceType"));

            txtUserName.Text = ProgramManager.GetConfigParamValue("SMS_UserName");
            string password = FinaKeyGenerator.Decrypt(ProgramManager.GetConfigParamValue("SMS_Password"));
            txtPassword.Text = password == null ? "" : password;
            txtClientId.Text = ProgramManager.GetConfigParamValue("SMS_ClientId");
            txtServiceId.Text = ProgramManager.GetConfigParamValue("SMS_ServiceId");
        }
        private void onSave()
        {
            ProgramManager.GetDataManager().SetParamValue("SMS_UserName", txtUserName.Text);

            string password = FinaKeyGenerator.Encrypt(txtPassword.Text);
            ProgramManager.GetDataManager().SetParamValue("SMS_Password", password);
            ProgramManager.GetDataManager().SetParamValue("SMS_Password_Dec", txtPassword.Text);
            ProgramManager.GetDataManager().SetParamValue("SMS_ClientId", txtClientId.Text);
            ProgramManager.GetDataManager().SetParamValue("SMS_ServiceId",txtServiceId.Text);
            ProgramManager.GetDataManager().SetParamValue("SMSServiceType", Convert.ToString(comboBoxServiceType.SelectedIndex));

            ProgramManager.InitialConfigParamValue();

            ParentForm.UserName = txtUserName.Text;
            ParentForm.Password = txtPassword.Text;
            ParentForm.ClientId = txtClientId.Text;
            ParentForm.ServiceId = txtServiceId.Text;
            ParentForm.ServiceType = (SMSServiceType)comboBoxServiceType.SelectedIndex;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            onSave();
            Close();
        }
    }
}
