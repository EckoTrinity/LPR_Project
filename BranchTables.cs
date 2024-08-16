using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPR381_Project
{
    internal class BranchTable
    {
        string level;
        double[,] table;
        public List<string> headers;
        List<string> rowHeaders;

        DataGridView dataGrid;
        public BranchTable(BranchTable table) 
        {
            this.headers = table.headers;
            this.level = table.level;
            this.rowHeaders = table.rowHeaders;
            this.table = table.table;
            this.DataGrid = table.DataGrid;
        }
        public BranchTable(string level, double[,] table, List<string> headers, List<string> rowHeaders)
        {
            this.Level = level;
            this.Table = table;
            this.headers = headers;
            this.rowHeaders = rowHeaders;
        }

        public double[,] Table { get => table; set => table = value; }
        public string Level { get => level; set => level = value; }
        public DataGridView DataGrid { get => dataGrid; set => dataGrid = value; }
    }
}