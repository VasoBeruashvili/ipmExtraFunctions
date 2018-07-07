using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Transactions;
using ipmPMBasic;

namespace IpmConverting
{
    public partial class SyncNecesittyForm : Form
    {
        private ProgramManagerBasic PM = null;

        private Dictionary<string, string> CheckItems = new Dictionary<string, string> 
        {
            {"PAR", "პარამეტრები"},
            {"PROD", "სასაქონლო სია"},
            {"CUS", "მყიდველები"},
            {"REST", "ნაშთები"},
            {"DOCS", "ოპერაციები"}
        };

     

        public SyncNecesittyForm(ProgramManagerBasic pm)
        {
            InitializeComponent();
            this.PM = pm;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FillTree()
        {
            m_Tree.BeginUpdate();
            m_Tree.Nodes.Clear();
            Dictionary<int, string> _stores = PM.GetDataManager().GetDictionary<int, string>("SELECT id, name FROM book.Stores WHERE path LIKE '0#1#3%' ORDER BY id ");
            if (!_stores.Any())
                return;
            foreach (KeyValuePair<int, string> store in _stores)
            {
                TreeNode chNode = new TreeNode
                {
                    Text = store.Value,
                    Tag = store.Key,
                    ImageIndex = 1,
                    SelectedImageIndex = 1,
                    Checked = true,
                    
                };
                chNode.Nodes.AddRange(CheckItems.Select(i => new TreeNode
                {
                    Text = i.Value,
                    Tag = i.Key,
                    Checked = true
                }).ToArray());
                m_Tree.Nodes.Add(chNode);
            }
            m_Tree.EndUpdate();
        }

        private void FillMenuStrip()
        {
            MenuCheck.DropDownItems.Clear();
            MenuUncheck.DropDownItems.Clear();

         



            List<ToolStripItem> it = new List<ToolStripItem>();
            it.Add(new ToolStripMenuItem { Text = "ყველა", Tag = true });
            it.Add(new ToolStripSeparator());
            it.AddRange(CheckItems.Select(i => new ToolStripMenuItem
            {
                Text = i.Value,
                Tag = i.Key

            }).ToArray());
            MenuCheck.DropDownItems.AddRange(it.ToArray());
            MenuCheck.DropDownItemClicked += (s, e) => 
            {
                if (e.ClickedItem.Tag is Boolean)
                {
                    OnCheckAll(m_Tree.Nodes, true);
                    m_Tree.CollapseAll();
                }
                else
                {
                    OnCheckAll(m_Tree.Nodes, false);
                    foreach (TreeNode nod in m_Tree.Nodes)
                    {
                        foreach (TreeNode nod_sub in nod.Nodes)
                        {
                            if (nod_sub.Tag == e.ClickedItem.Tag)
                            {
                                nod_sub.Checked = true;
                            }
                        }

                    }
                    m_Tree.ExpandAll();
                }

            };
            List<ToolStripItem> it2 = new List<ToolStripItem>();
            it2.Add(new ToolStripMenuItem { Text = "ყველა", Tag = false });
            it2.Add(new ToolStripSeparator());
            it2.AddRange(CheckItems.Select(i => new ToolStripMenuItem
            {
                Text = i.Value,
                Tag = i.Key
            }).ToArray());
            MenuUncheck.DropDownItems.AddRange(it2.ToArray());
            MenuUncheck.DropDownItemClicked += (s, e) =>
            {
                if (e.ClickedItem.Tag is Boolean)
                {
                    OnCheckAll(m_Tree.Nodes, false);
                    m_Tree.CollapseAll();
                }
                else
                {
                    OnCheckAll(m_Tree.Nodes, true);
                    foreach (TreeNode nod in m_Tree.Nodes)
                    {
                        foreach (TreeNode nod_sub in nod.Nodes)
                        {
                            if (nod_sub.Tag == e.ClickedItem.Tag)
                            {
                                nod_sub.Checked = false;
                            }
                        }
                    }
                    m_Tree.ExpandAll();
                }
            };
        }

        private void SyncNecesittyForm_Load(object sender, EventArgs e)
        {
            FillTree();
        }

        private void OnCheckAll(TreeNodeCollection nodes, bool check)
        {
            foreach (TreeNode node in nodes)
                node.Checked = check;
        }

        private void MenuCheckAll_Click(object sender, EventArgs e)
        {
            OnCheckAll(m_Tree.Nodes, true);
        }

        private void MenuUncheckAll_Click(object sender, EventArgs e)
        {
            OnCheckAll(m_Tree.Nodes, false);
        }



        private void m_Tree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            m_Tree.AfterCheck -= new TreeViewEventHandler(m_Tree_AfterCheck);
            TreeNode _current = e.Node;
            if (_current != null && _current.Level == 0)
                OnCheckAll(_current.Nodes, _current.Checked);
            m_Tree.AfterCheck += new TreeViewEventHandler(m_Tree_AfterCheck);
        }

        private bool OnSave()
        {
            m_Tree.EndUpdate();

            List<int> _users = PM.GetDataManager().GetList<int>("SELECT id FROM book.Users WHERE path LIKE '0#5#6%'");
            if (!_users.Any())
                return true;
            StringBuilder _execute = new StringBuilder();
            string tamplate = "INSERT INTO book.SyncNecessity (store_id, tag, user_id) VALUES {0}";

            foreach (TreeNode node in m_Tree.Nodes)
            {
                var sub = node.Nodes.OfType<TreeNode>().Where(a => a.Checked).ToList();
                if (!sub.Any())
                    continue;
                List<string> _params = new List<string>();
                _params.AddRange(sub.Where(s => Convert.ToString(s.Tag) != "PAR").Select(s => string.Format("({0}, '{1}', {2})", Convert.ToInt32(node.Tag), Convert.ToString(s.Tag), 0)));
                _params.AddRange(sub.Where(s => Convert.ToString(s.Tag) == "PAR").SelectMany(nod => _users, (nod, us) => string.Format("({0}, '{1}', {2})", Convert.ToInt32(node.Tag), Convert.ToString(nod.Tag), us)));
                _execute.AppendFormat(tamplate, string.Join(",", _params.ToArray()));
                _execute.AppendLine();
            }
            string _ex = _execute.ToString();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 3, 0) }))
            {
                if (!PM.GetDataManager().ExecuteSql("TRUNCATE TABLE book.SyncNecessity"))
                {
                    Transaction.Current.Rollback();
                    return false;
                }
                if (!string.IsNullOrEmpty(_ex))
                {
                    if (!PM.GetDataManager().ExecuteSql(_ex))
                    {
                        Transaction.Current.Rollback();
                        return false;
                    }
                }
                scope.Complete();
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (OnSave())
                this.Close();
        }

        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            FillMenuStrip();
        }


       

        
      
       
    }
}
