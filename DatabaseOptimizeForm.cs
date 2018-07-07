using System;
using System.Windows.Forms;
using ipmPMBasic;
using ipmControls;
using System.Drawing;

namespace ipmExtraFunctions
{
    public partial class DatabaseOptimizeForm : Form
    {
        ProgramManagerBasic M_Pm;
     
        public DatabaseOptimizeForm(ProgramManagerBasic pm)
        {
            M_Pm = pm;
            InitializeComponent();
            M_Pm.TranslateControl(this);
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            if (!checkShrink.Checked || !checkDefragment.Checked || !checkRebuildIndex.Checked)
                return;
            ProgressDispatcher.Activate();
            using (DBContext _db = new DBContext() { Timeout = 3600 })
            {
                if (checkShrink.Checked)
                    if (!_db.ExecuteSql(@" DECLARE @BaseName AS nvarchar(250) = (SELECT db_name())
                                      DECLARE @MainFile AS nvarchar(250) = (SELECT  name as Logical_File_Name FROM sys.database_files WHERE type = 0)
                                      DECLARE @LogFile AS nvarchar(250) = (SELECT  name as Logical_File_Name FROM sys.database_files WHERE type = 1)
                                      EXEC(
                                      'ALTER DATABASE ' + @BaseName + ' SET RECOVERY SIMPLE
                                      DBCC SHRINKFILE('+@MainFile+', 1)
                                      DBCC SHRINKFILE('+@LogFile+', 1)
                                      ')").HasValue)
                    {
                        ProgressDispatcher.Deactivate();
                        MessageBoxForm.Show(Application.ProductName, "შეცდომა ბაზის ოპტიმიზაციისას (შეკუმშვა)", _db.ErrorEx, null, SystemIcons.Error);
                        return;

                    }
                if (checkDefragment.Checked)
                    if (!_db.ExecuteSql(@"SET NOCOUNT ON;
                                    DECLARE @tablename varchar(255);
                                    DECLARE @execstr   varchar(400);
                                    DECLARE @objectid  int;
                                    DECLARE @indexid   int;
                                    DECLARE @frag      decimal;
                                    DECLARE @maxfrag   decimal;

                                    -- Decide on the maximum fragmentation to allow for.
                                    SELECT @maxfrag = 30.0;

                                    -- Declare a cursor.
                                    DECLARE tables CURSOR FOR
                                       SELECT TABLE_SCHEMA + '.' + TABLE_NAME
                                       FROM INFORMATION_SCHEMA.TABLES
                                       WHERE TABLE_TYPE = 'BASE TABLE';

                                    -- Create the table.
                                    CREATE TABLE #fraglist (
                                       ObjectName char(255),
                                       ObjectId int,
                                       IndexName char(255),
                                       IndexId int,
                                       Lvl int,
                                       CountPages int,
                                       CountRows int,
                                       MinRecSize int,
                                       MaxRecSize int,
                                       AvgRecSize int,
                                       ForRecCount int,
                                       Extents int,
                                       ExtentSwitches int,
                                       AvgFreeBytes int,
                                       AvgPageDensity int,
                                       ScanDensity decimal,
                                       BestCount int,
                                       ActualCount int,
                                       LogicalFrag decimal,
                                       ExtentFrag decimal);

                                    -- Open the cursor.
                                    OPEN tables;

                                    -- Loop through all the tables in the database.
                                    FETCH NEXT
                                       FROM tables
                                       INTO @tablename;

                                    WHILE @@FETCH_STATUS = 0
                                    BEGIN
                                    -- Do the showcontig of all indexes of the table
                                       INSERT INTO #fraglist 
                                       EXEC ('DBCC SHOWCONTIG (''' + @tablename + ''') 
                                          WITH FAST, TABLERESULTS, ALL_INDEXES, NO_INFOMSGS');
                                       FETCH NEXT
                                          FROM tables
                                          INTO @tablename;
                                    END;

                                    -- Close and deallocate the cursor.
                                    CLOSE tables;
                                    DEALLOCATE tables;

                                    -- Declare the cursor for the list of indexes to be defragged.
                                    DECLARE indexes CURSOR FOR
                                       SELECT ObjectName, ObjectId, IndexId, LogicalFrag
                                       FROM #fraglist
                                       WHERE LogicalFrag >= @maxfrag
                                          AND INDEXPROPERTY (ObjectId, IndexName, 'IndexDepth') > 0;

                                    -- Open the cursor.
                                    OPEN indexes;

                                    -- Loop through the indexes.
                                    FETCH NEXT
                                       FROM indexes
                                       INTO @tablename, @objectid, @indexid, @frag;

                                    WHILE @@FETCH_STATUS = 0
                                    BEGIN
                                       PRINT 'Executing DBCC INDEXDEFRAG (0, ' + RTRIM(@tablename) + ',
                                          ' + RTRIM(@indexid) + ') - fragmentation currently '
                                           + RTRIM(CONVERT(varchar(15),@frag)) + '%';
                                       SELECT @execstr = 'DBCC INDEXDEFRAG (0, ' + RTRIM(@objectid) + ',
                                           ' + RTRIM(@indexid) + ')';
                                       EXEC (@execstr);

                                       FETCH NEXT
                                          FROM indexes
                                          INTO @tablename, @objectid, @indexid, @frag;
                                    END;

                                    -- Close and deallocate the cursor.
                                    CLOSE indexes;
                                    DEALLOCATE indexes;

                                    -- Delete the temporary table.
                                    DROP TABLE #fraglist;
                                    ").HasValue)
                    {
                        ProgressDispatcher.Deactivate();
                        MessageBoxForm.Show(Application.ProductName, "შეცდომა ბაზის ოპტიმიზაციისას (დეფრაგმენტაცია)", _db.ErrorEx, null, SystemIcons.Error);
                        return;
                    }

                if (checkRebuildIndex.Checked)
                    if (!_db.ExecuteSql(@"DECLARE @TableName VARCHAR(255)
                                    DECLARE @sql NVARCHAR(500)
                                    DECLARE @fillfactor INT
                                     SET @fillfactor = 80
                                    DECLARE TableCursor CURSOR FOR
                                     SELECT OBJECT_SCHEMA_NAME([object_id])+'.'+name AS TableName
                                    FROM sys.tables
                                    OPEN TableCursor
                                    FETCH NEXT FROM TableCursor INTO @TableName
                                    WHILE @@FETCH_STATUS = 0
                                    BEGIN
                                     SET @sql = 'ALTER INDEX ALL ON ' + @TableName + ' REBUILD WITH (FILLFACTOR = ' + CONVERT(VARCHAR(3),@fillfactor) + ')'
                                    EXEC (@sql)
                                    FETCH NEXT FROM TableCursor INTO @TableName
                                    END
                                     CLOSE TableCursor
                                    DEALLOCATE TableCursor
                                    ").HasValue)
                    {
                        ProgressDispatcher.Deactivate();
                        MessageBoxForm.Show(Application.ProductName, "შეცდომა ბაზის ოპტიმიზაციისას (რე-ინდექსირება)", _db.ErrorEx, null, SystemIcons.Error);
                        return;
                    }
            }

                    ProgressDispatcher.Deactivate();
            MessageBoxForm.Show(Application.ProductName, "ოპტიმიზაცია დასრულებულია.", null, null, SystemIcons.Information);

        }
    }
}
