using ipmControls;
using ipmPMBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace ipmExtraFunctions
{
    public partial class ContragentRelationElementForm : Form
    {
        public ProgramManagerBasic ProgramManager { get; set; }
        private List<Client> m_ClientList;
        List<RelClient> m_Clients = new List<RelClient>();
        public ContragentRelationElementForm()
        {
            InitializeComponent();
            checkGenerateTypes.Checked = true;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        public void FillTypes()
        {
            ProgressDispatcher.Activate();

            string _sql = "UPDATE book.ContragentRelations SET type=0";
            if (!ProgramManager.GetDataManager().ExecuteSql(_sql))
            {
                ProgressDispatcher.Deactivate();
                MessageBoxForm.Show("შეცდომა", "სტატუსების განახლება ვერ მოხერხდა!", ProgramManager.GetDataManager().ErrorEx, null, SystemIcons.Error);
                return;
            }

            _sql = @"SELECT r.id, r.child_id AS ChildId, r.parent_id AS ParentId, r.path, ISNULL(c.name,'') AS Name,
                            (SELECT COUNT(rr.id) FROM book.ContragentRelations AS rr 
                             WHERE rr.parent_id=r.child_id AND 
                            (SELECT COUNT(id) FROM book.ContragentRelations WHERE parent_id=rr.child_id)<5 ) AS ChildrenCount, r.type, CAST(0 AS BIT) AS IsChanged
                            FROM book.ContragentRelations AS r 
                            INNER JOIN book.Contragents AS p ON r.parent_id=p.id
                            INNER JOIN book.Contragents AS c ON r.child_id=c.id
                            WHERE (SELECT COUNT(id) FROM book.ContragentRelations WHERE parent_id=r.child_id)>=5
                            ORDER BY LEN(r.path) DESC";

            m_Clients = ProgramManager.GetDataManager().GetList<RelClient>(_sql);
            if (m_Clients == null || m_Clients.Count == 0)
            {
                ProgressDispatcher.Deactivate();
                return;
            }

            int _count = 0;

            foreach (RelClient _c in m_Clients)
            {
                _count = m_Clients.Where(c => c.ParentId == _c.ChildId && c.ChildrenCount >= 5).Count();
                if (_c.ChildrenCount >= 5 )
                {
                    if (_c.Type != 1)
                    {
                        _c.Type = 1;
                        _c.IsChanged = true;
                    }
                }
            }

            foreach (RelClient _c in m_Clients)
            {
                _count = m_Clients.Where(c => c.ParentId == _c.ChildId && c.Type == 1).Count();
                if (_c.ChildrenCount >= 10 && _count >= 2)
                {
                    if (_c.Type != 2)
                    {
                        _c.Type = 2;
                        _c.IsChanged = true;
                    }
                }

            }


            foreach (RelClient _c in m_Clients)
            {
                if (_c.ChildrenCount < 15)
                    continue;

                _count = m_Clients.Where(c => c.ParentId == _c.ChildId && c.Type == 2).Count();
                if (_count >= 2)
                {
                    if (_c.Type != 3)
                    {
                        _c.Type = 3;
                        _c.IsChanged = true;
                    }

                }
            }

            foreach (RelClient _c in m_Clients)
            {
                if (_c.ChildrenCount < 40)
                    continue;

                _count = m_Clients.Where(c => c.ParentId == _c.ChildId && c.Type == 3).Count();
                if (_count >= 2)
                {
                    if (_c.Type != 4)
                    {
                        _c.Type = 4;
                        _c.IsChanged = true;
                    }
                }
            }

            List<RelClient> _changedClients = m_Clients.Where(c => c.IsChanged).ToList();
            foreach (RelClient _cc in _changedClients)
            {
                _sql = "UPDATE book.ContragentRelations SET type=" + _cc.Type + " WHERE id=" + _cc.Id;
                if (!ProgramManager.GetDataManager().ExecuteSql(_sql))
                {
                    ProgressDispatcher.Deactivate();
                    MessageBoxForm.Show("შეცდომა", "ვერ მოხარხდა იერარქიის განახლება!", ProgramManager.GetDataManager().ErrorEx, null, SystemIcons.Error);
                    return;
                }
            }
            ProgressDispatcher.Deactivate();
            
        }
        private bool FillRelations(TreeNode node, int id, TreeView TW)
        {
            string sqlText = "";

            if (id != 0)
            {
                sqlText = @"SELECT r.*, c.name FROM book.ContragentRelations AS r 
                            INNER JOIN book.Contragents AS c ON r.child_id=c.id
                            WHERE r.parent_id= " + id.ToString() + " ORDER BY r.id";
            }
            else
            {
                sqlText = @"SELECT r.*, c.name FROM book.ContragentRelations AS r 
                            INNER JOIN book.Contragents AS c ON r.child_id=c.id
                            WHERE r.parent_id = 0 ORDER BY r.id";

                DataTable tableinvNodes = ProgramManager.GetDataManager().GetTableData(sqlText);

                if (tableinvNodes == null)
                    return false;

                if (tableinvNodes.Rows.Count == 0)
                    return false;

                int idd;
                foreach (DataRow row in tableinvNodes.Rows)
                {

                    idd = int.Parse(row["child_id"].ToString());
                    TreeNode chNode = new TreeNode();
                    chNode.Name = row["name"].ToString();
                    chNode.Text = row["name"].ToString();

                    if (checkGenerateTypes.Checked)
                    {
                        RelClient _cc = m_Clients.Where(p => p.ChildId == idd).FirstOrDefault();
                        if (_cc != null)
                        {
                            switch (_cc.Type)
                            {
                                case 1: chNode.Name += " (კოორდინატორი)"; break;
                                case 2: chNode.Name += " (უფროსი კოორდინატორი)"; break;
                                case 3: chNode.Name += " (მენეჯერი)"; break;
                                case 4: chNode.Name += " (დირექტორი)"; break;
                            }
                        }

                    }

                    chNode.Tag = new KeyValuePair<string, string>(row["child_id"].ToString(), row["path"].ToString());
                    chNode.ToolTipText = "";
                    chNode.ImageIndex = 0;
                    chNode.SelectedImageIndex = 0;
                    if (node == null)
                    {
                        TW.Nodes.Add(chNode);
                    }
                    else
                        node.Nodes.Add(chNode);

                    FillRelations(chNode, idd, TW);
                }

                return true;
            }

            DataTable tb = ProgramManager.GetDataManager().GetTableData(sqlText);

            if (tb == null)
                return false;
            if (tb.Rows.Count == 0)
                return false;

            foreach (DataRow row in tb.Rows)
            {
                int idd = -1;
                idd = int.Parse(row["child_id"].ToString());
                TreeNode chNode = new TreeNode();
                chNode.Name = row["name"].ToString();
                chNode.Text = row["name"].ToString();

                if (checkGenerateTypes.Checked)
                {
                    RelClient _cc = m_Clients.Where(p => p.ChildId == idd).FirstOrDefault();
                    if (_cc != null)
                    {
                        switch (_cc.Type)
                        {
                            case 1: chNode.Name += " (კოორდინატორი)"; break;
                            case 2: chNode.Name += " (უფროსი კოორდინატორი)"; break;
                            case 3: chNode.Name += " (მენეჯერი)"; break;
                            case 4: chNode.Name += " (დირექტორი)"; break;
                        }
                    }
                }


                chNode.Tag = new KeyValuePair<string, string>(row["child_id"].ToString(), row["path"].ToString());
                chNode.ToolTipText = "";
                chNode.ImageIndex = 0;
                chNode.SelectedImageIndex = 0;

                if (node == null)
                {
                    TW.Nodes.Add(chNode);
                }
                else
                    node.Nodes.Add(chNode);
                if (idd != id)
                    FillRelations(chNode, idd, TW);
            }

            return true;
        }

        private void ContragentRelationElementForm_Load(object sender, EventArgs e)
        {
            Fill();
        }
        private void Fill()
        {
            if (checkGenerateTypes.Checked)
                FillTypes();

            ProgressDispatcher.Activate();
            FillClientList();
            TreeNode mainNode = new TreeNode();
            m_Tree.Nodes.Clear();
            mainNode.Name = ProgramManager.GetTranslatorManager().Translate("ყველა");
            mainNode.Text = ProgramManager.GetTranslatorManager().Translate("ყველა");
            mainNode.Tag = null;
            mainNode.ImageIndex = 1;
            mainNode.SelectedImageIndex = 1;
            m_Tree.Nodes.Add(mainNode);           
            FillRelations(mainNode, 0, m_Tree);            
            ExpandTree();
            ProgressDispatcher.Deactivate();
        }
        private void FillClientList()
        {
            string _sql = @"SELECT c.id, c.name, ISNULL(c.code,'') AS Code, ISNULL(c.address,'') AS Address, (COALESCE(d.debetCycle,0) - COALESCE(d.creditCycle,0)) AS Debt, 
                            ISNULL(c.tel,'') AS Tel, ISNULL(parent.name, '') AS ParentName 
                            FROM book.Contragents AS c 
                            INNER JOIN book.ContragentRelations AS r ON c.id=r.child_id
                            LEFT JOIN book.Contragents AS parent ON parent.id=r.parent_id AND parent.id<>0
                            OUTER APPLY
		                    (SELECT
				                    ROUND((SELECT SUM(e.amount*e.rate) FROM doc.Entries e INNER JOIN doc.GeneralDocs g ON e.general_id = g.id WHERE e.a1=c.id AND ISNULL(g.is_deleted,0)=0 AND (e.debit_acc = c.account OR e.debit_acc =  c.account2 )),3) AS debetCycle,
				                    ROUND((SELECT SUM(e.amount*e.rate) FROM doc.Entries e INNER JOIN doc.GeneralDocs g ON e.general_id = g.id WHERE e.b1=c.id AND ISNULL(g.is_deleted,0)=0 AND (e.credit_acc = c.account OR e.credit_acc = c.account2 )),3) AS creditCycle
		                    )AS d ";

            m_ClientList = ProgramManager.GetDataManager().GetList<Client>(_sql);
            if (m_ClientList != null)
                lblFullDebt.Text = string.Concat(ProgramManager.GetTranslatorManager().Translate("სრული დავალიანება:"), " ", m_ClientList.Select(p => p.Debt).Sum().ToString("F2"), " ", ProgramManager.GetTranslatorManager().Translate("ლარი"));
        }
       /* public void ExpandAllNodes(TreeNode Node)
        {
            m_Tree.ExpandAll();
            foreach (TreeNode nd in Node.Nodes)
            {
                nd.Expand();
                ExpandAllNodes(nd);
            }
        }*/
        public void ExpandTree()
        {
            TreeNodeCollection NodeCollection = m_Tree.Nodes;
            foreach (TreeNode n in NodeCollection)
            {
                n.Expand();                           
            }
            SetTreeNodesCount();
        }
        public void SetNodesCount(TreeNode Node)
        {          
            foreach (TreeNode nd in Node.Nodes)
            {
                nd.Text = string.Concat(nd.Name, " (", nd.GetNodeCount(true), ")");
                SetNodesCount(nd);
            }
        }
        public void SetTreeNodesCount()
        {
            TreeNodeCollection NodeCollection = m_Tree.Nodes;//[0].Nodes;
            foreach (TreeNode n in NodeCollection)
            {
                n.Text = string.Concat(n.Name," (", n.GetNodeCount(true), ")");
                SetNodesCount(n);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TreeNode _node = m_Tree.SelectedNode;
            string sqlText;          

            int _contragentId = ProgramManager.ShowSelectForm("TABLE_CONTRAGENT:CUSTOMER", -1);
            if (_contragentId <= 0)
                return;

            string _contragentName = ProgramManager.GetDataManager().GetContragentNameByID(_contragentId);

            Hashtable m_sqlParams = new Hashtable();
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 10, 0) }))
            {
                if (_node == null || _node.Tag == null)
                {
                    string path = "0#" + _contragentId.ToString();
                    sqlText = "INSERT INTO book.ContragentRelations (parent_id, child_id, path, create_date, user_id) VALUES (@parent_id, @child_id, @path, @create_date, @user_id)";
                    m_sqlParams.Clear();
                    m_sqlParams.Add("@parent_id", 0);
                    m_sqlParams.Add("@child_id", _contragentId);
                    m_sqlParams.Add("@path", path);
                    m_sqlParams.Add("@create_date", DateTime.Now);
                    m_sqlParams.Add("@user_id", ProgramManager.GetUserID());
                    if (!ProgramManager.GetDataManager().ExecuteSql(sqlText, m_sqlParams))
                    {
                        Transaction.Current.Rollback();
                        MessageBoxForm.Show("", ProgramManager.GetTranslatorManager().Translate("შენახვა ვერ მოხერხდა!"), ProgramManager.GetDataManager().ErrorEx, null, SystemIcons.Error);
                        return;
                    }

                    tran.Complete();
                    TreeNode mynode = new TreeNode();
                    mynode.Name = _contragentName;
                    mynode.Text = _contragentName;
                    mynode.Tag = new KeyValuePair<string, string>(_contragentId.ToString(), path);

                    if (_node == null)
                        m_Tree.Nodes.Add(mynode);
                    else
                        _node.Nodes.Add(mynode);

                    FillClientList();
                    SetTreeNodesCount();

                    return;
                }
                if (_node != null)
                {
                    int _parentId = 0;
                    KeyValuePair<string, string> ? _tag = (KeyValuePair<string, string>)_node.Tag;
                    if (!_tag.HasValue)
                        return;

                 
                    if (!int.TryParse(_tag.Value.Key, out _parentId))
                        return;

                    if (_parentId <= 0)
                        return;

                    string _path = Convert.ToString(_tag.Value.Value);
                    if (string.IsNullOrEmpty(_path))
                        return;

                    List<string> _pathItems = _path.Split('#').ToList();
                    if(_pathItems.Contains(_contragentId.ToString()))                  
                    {
                        MessageBoxForm.Show("", ProgramManager.GetTranslatorManager().Translate("კლიენტი უკვე გამოყენებულია ზედა იერარქიაში!"), null, null, SystemIcons.Error);
                        return;
                    }                   

                    _path = string.Concat(_path, "#", _contragentId);
                    sqlText = "INSERT INTO book.ContragentRelations (parent_id, child_id, path, create_date, user_id) VALUES (@parent_id, @child_id, @path, @create_date, @user_id)";

                    m_sqlParams.Clear();

                    m_sqlParams.Add("@parent_id", _parentId);
                    m_sqlParams.Add("@child_id", _contragentId);
                    m_sqlParams.Add("@path", _path);
                    m_sqlParams.Add("@create_date", DateTime.Now);
                    m_sqlParams.Add("@user_id", ProgramManager.GetUserID());

                    if (!ProgramManager.GetDataManager().ExecuteSql(sqlText, m_sqlParams))
                    {
                        Transaction.Current.Rollback();
                        MessageBoxForm.Show("", ProgramManager.GetTranslatorManager().Translate("შენახვა ვერ მოხერხდა!"), ProgramManager.GetDataManager().ErrorEx, null, SystemIcons.Error);
                        return;
                    }

                    TreeNode mynode = new TreeNode();
                    mynode.Name = _contragentName;
                    mynode.Text = _contragentName;
                    mynode.Tag =new KeyValuePair<string, string>(_contragentId.ToString(),  _path);

                    _node.Nodes.Add(mynode);
                    FillClientList();                   
                    SetTreeNodesCount();
                    tran.Complete();
                }


            }
        }
        private bool DeleteRelation(int childId, int parentId)
        {
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 10, 0) }))
            {               
                string _sql = "DELETE FROM book.ContragentRelations WHERE child_id=" + childId + " AND parent_id=" + parentId;
                if (!ProgramManager.GetDataManager().ExecuteSql(_sql))
                {
                    Transaction.Current.Rollback();
                    return false;
                }

                tran.Complete();
                return true;
            }
        }        
        public bool DeleteNode(TreeNode node, int parentId)
        {
            if (node == null || node.Tag == null)
                return true;

            int _id;
            KeyValuePair<string, string>? _tag = (KeyValuePair<string, string>)(node.Tag);
            if (!_tag.HasValue)
                return false;

            if (!int.TryParse(_tag.Value.Key, out _id))
                return false;

            if (!DeleteRelation(_id, parentId))
                return false;

            return true;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            TreeNode _selectedNode = m_Tree.SelectedNode;
            if (_selectedNode == null||_selectedNode.Tag==null)
                return;
           
            if (_selectedNode.Nodes.Count>0)
            {
                MessageBoxForm.Show("", string.Concat(ProgramManager.GetTranslatorManager().Translate("ჩანაწერი შეიცავს ქვეჩანაწერებს!"), "\n", ProgramManager.GetTranslatorManager().Translate("წაშლა დაუშვებელია!")), null, null, SystemIcons.Exclamation);
                return;
            }
            bool? _result = MessageBoxForm.ShowDialog("", ProgramManager.GetTranslatorManager().Translate("გსურთ კლიენტის წაშლა?"), SystemIcons.Question);
            if (!_result.HasValue || !_result.Value)
                return;


            KeyValuePair<string, string>? _tag = (KeyValuePair<string, string>)_selectedNode.Tag;
            if (!_tag.HasValue)
                return;

            if (_tag.Value.Key == "0")
                return;

            string[] strs =_tag.Value.Value.Split('#');
            int parent_id = int.Parse(strs[strs.Length - 2]);

            if (!DeleteNode(_selectedNode, parent_id))
            {
                MessageBoxForm.Show("", ProgramManager.GetTranslatorManager().Translate("წაშლა ვერ მოხერხდა!"), ProgramManager.GetDataManager().ErrorEx, null, SystemIcons.Error);
                return;
            }

            m_Tree.Nodes.Remove(_selectedNode);
            FillClientList();          
            SetTreeNodesCount();          
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            TreeNode _selectedNode = m_Tree.SelectedNode;
            if (_selectedNode == null || Convert.ToString(_selectedNode.Tag) == "0")
                return;

            int _id;
            if (!int.TryParse(_selectedNode.Name, out _id))
                return;

            string[] strs = Convert.ToString(_selectedNode.Tag).Split('#');
            int parent_id = int.Parse(strs[strs.Length - 2]);
            string _path = string.Join("#", strs.Take(strs.Length - 1).ToArray());

            int _contragentId = ProgramManager.ShowSelectForm("TABLE_CONTRAGENT:CUSTOMER", _id);
            if (_contragentId <= 0)
                return;

            string _contragentName = ProgramManager.GetDataManager().GetContragentNameByID(_contragentId);
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 10, 0) }))
            {

                string _sql = "UPDATE book.ContragentRelations SET parent_id=" + _contragentId + " WHERE parent_id=" + _id;
                if (!ProgramManager.GetDataManager().ExecuteSql(_sql))
                {
                    Transaction.Current.Rollback();
                    return;
                }

                _sql = "UPDATE book.ContragentRelations SET child_id=" + _contragentId + " WHERE  parent_id=" + parent_id + " AND child_id=" + _id;
                if (!ProgramManager.GetDataManager().ExecuteSql(_sql))
                {
                    Transaction.Current.Rollback();
                    return;
                }

                tran.Complete();
            }

            _selectedNode.Text = _contragentName;
            _selectedNode.Name = _contragentId.ToString();
            _selectedNode.Tag = string.Concat(_path, "#", _contragentId);
        }
        private void ShowClientInformation()
        {
            if (m_ClientList == null)
                return;

            TreeNode _node = m_Tree.SelectedNode;
            if (_node == null || _node.Tag == null)
                return;

            KeyValuePair<string, string>? _tag = (KeyValuePair<string, string>)_node.Tag;
            if (!_tag.HasValue)
                return;

            txtName.Text = "";
            txtDebt.Text = "0.00";
            txtParentName.Text = "";
            txtCode.Text = "";
            txtTel.Text = "";
            txtAddress.Text = "";

            int _id = Convert.ToInt32(_tag.Value.Key);
            Client _client=m_ClientList.Where(c => c.Id == _id).FirstOrDefault();
            if (_client == null)
                return;
            
            txtName.Text = _client.Name;
            txtDebt.Text = _client.Debt.ToString("F2");
            txtParentName.Text = _client.ParentName;
            txtCode.Text = _client.Code;
            txtTel.Text = _client.Tel;
            txtAddress.Text = _client.Address;
        }
        private void OnSearch()
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
                return;

            string _contragentName = txtSearch.Text.Trim();
            if (chckSearchByCode.Checked)
                _contragentName = m_ClientList.Where(c => c.Code.Contains(_contragentName)).Select(c => c.Name).FirstOrDefault();

            SearchNode(_contragentName);
        }
        private void SearchNode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            TreeNode _foundNode = m_Tree.Nodes[0].Nodes.OfType<TreeNode>().Where(n => n.Name.Contains(text)).FirstOrDefault();
            if (_foundNode != null)
            {
                m_Tree.SelectedNode = _foundNode;
                m_Tree.Select();
                ShowClientInformation();
                return;
            }
            foreach (TreeNode _node in m_Tree.Nodes[0].Nodes)
            {
                if (SearchNode(_node, text))
                    return;
            }
        }
        private bool SearchNode(TreeNode node, string text)
        {
            if (node == null)
                return false;

            if (string.IsNullOrEmpty(text))
                return false;

            if (node.Name.Contains(text))
            {
                m_Tree.SelectedNode = node;
                m_Tree.Select();
                ShowClientInformation();
                return true;
            }
            foreach (TreeNode _nd in node.Nodes)
            {
                if (SearchNode(_nd, text))
                    return true;
            }
            return false;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            OnSearch();
        }

        private void m_Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowClientInformation();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OnSearch();

        }

        private void btnRefResh_Click(object sender, EventArgs e)
        {
            Fill();
        }
    }
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public double Debt { get; set; }
        public string Tel { get; set; }
        public string ParentName { get; set; }
    }
    public class RelClient
    {
        public int Id { get; set; }
        public int ChildId { get; set; }
        public int ParentId { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public int ChildrenCount { get; set; }
        public byte Type { get; set; }
        public bool IsChanged { get; set; }
    }
}
