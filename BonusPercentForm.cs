using ipmPMBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ipmExtraFunctions
{
    public partial class BonusPercentForm : Form
    {
        public ProgramManagerBasic ProgramManager { get; set; }
        public BonusPercentForm()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ProgramManager.GetDataManager().SetParamValue("PrcFromGroup", txtPrcFromGroup.Text);
            ProgramManager.GetDataManager().SetParamValue("PrcFromCoords", txtPrcFromCoords.Text);
            ProgramManager.GetDataManager().SetParamValue("PrcFromSeniorCoords", txtPrcFromSeniorCoords.Text);
            ProgramManager.GetDataManager().SetParamValue("PrcFromManagers", txtPrcFromManagers.Text);

            this.DialogResult = DialogResult.OK;
        }

        private void BonusPercentForm_Load(object sender, EventArgs e)
        {
            txtPrcFromGroup.Text = ProgramManager.GetDataManager().GetParamValue("PrcFromGroup");
            txtPrcFromCoords.Text = ProgramManager.GetDataManager().GetParamValue("PrcFromCoords");
            txtPrcFromSeniorCoords.Text = ProgramManager.GetDataManager().GetParamValue("PrcFromSeniorCoords");
            txtPrcFromManagers.Text = ProgramManager.GetDataManager().GetParamValue("PrcFromManagers");
        }
    }
}
