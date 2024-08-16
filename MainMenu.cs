using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MetroSet_UI.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;
using MathNet.Symbolics;

namespace LPR381_Project
{
    public partial class MainMenu : MetroSetForm
    {
        List<string> lp = new List<string>();
        public MainMenu()
        {
            InitializeComponent();
        }
        private void MainMenu_Load(object sender, EventArgs e)
        {
            rtbFileOutput.BackColor = Color.FromArgb(30, 30, 30);
            rtbFileOutput.BorderColor = Color.FromArgb(30, 30, 30);

            rtbOutput.BackColor = Color.FromArgb(30, 30, 30);
            rtbOutput.BorderColor = Color.FromArgb(30, 30, 30);


            lblCASolution.ForeColor = Color.FromArgb(28, 131, 174);
            lblImport.ForeColor = Color.FromArgb(28, 131, 174);
            lblFileOutput.ForeColor = Color.FromArgb(28, 131, 174);
            lblSolve.ForeColor = Color.FromArgb(28, 131, 174);



            btnSolve.Enabled = false;
            cboMethod.Enabled = false;


            cbForm.Location = new System.Drawing.Point(1816, 4);
        }

        private void pnlDragnDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void pnlDragnDrop_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                    string[] lines;
                    rtbFileOutput.Text = "";
                    lp.Clear();
                    foreach (string filePath in droppedFiles)
                    {
                        lines = File.ReadAllLines(filePath);
                        foreach (var item in lines)
                        {
                            rtbFileOutput.AppendText(item + "\n");
                            lp.Add(item);
                        }
                    }

                    LinearModel lm = new LinearModel(lp.ToArray());
                    rtbFileOutput.AppendText("\nSIMPLEX CANONICAL FORM:\n");
                    rtbFileOutput.AppendText(lm.ObjFunctionToString() + "\n");
                    rtbFileOutput.AppendText(lm.CanonSimplexConstraintsToString() + "\n");

                    rtbFileOutput.AppendText("\nTWO-PHSES CANONICAL FORM:\n");
                    rtbFileOutput.AppendText(lm.WFunctionToString() + "\n");
                    rtbFileOutput.AppendText(lm.ObjFunctionToString() + "\n");
                    rtbFileOutput.AppendText(lm.CanonTwoPhaseConstraintsToString() + "\n");

                    rtbFileOutput.AppendText("\nDUALITY CANONICAL FORM:\n");
                    rtbFileOutput.AppendText(lm.CanonDualFunctionToString() + "\n");
                    rtbFileOutput.AppendText(lm.CanonDualConstraintsToString() + "\n");


                    btnSolve.Enabled = true;
                    cboMethod.Enabled = true;
                    cboMethod.SelectedIndex = 0;
                }
                catch (Exception)
                {
                    MessageBox.Show("The textfile was not in the correct format.\n" +
                            "Please ensure that each variable is separated by a space and there are not any additional spaces. All numbers except the rhs have signs (+/-). And should have a min/max at the start.\n"
                            , "Textfile error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = string.Empty;
                string[] lines = Array.Empty<string>();
                lp.Clear();
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        filePath = ofd.FileName;

                        lines = File.ReadAllLines(filePath);
                    }
                }
                rtbFileOutput.Text = "";
                foreach (var item in lines)
                {
                    rtbFileOutput.AppendText(item + "\n");
                    lp.Add(item);
                }
                LinearModel lm = new LinearModel(lp.ToArray());
                rtbFileOutput.AppendText("\nSIMPLEX CANONICAL FORM:\n");
                rtbFileOutput.AppendText(lm.ObjFunctionToString() + "\n");
                rtbFileOutput.AppendText(lm.CanonSimplexConstraintsToString() + "\n");

                rtbFileOutput.AppendText("\nTWO-PHSES CANONICAL FORM:\n");
                rtbFileOutput.AppendText(lm.WFunctionToString() + "\n");
                rtbFileOutput.AppendText(lm.ObjFunctionToString() + "\n");
                rtbFileOutput.AppendText(lm.CanonTwoPhaseConstraintsToString() + "\n");

                rtbFileOutput.AppendText("\nDUALITY CANONICAL FORM:\n");
                rtbFileOutput.AppendText(lm.CanonDualFunctionToString() + "\n");
                rtbFileOutput.AppendText(lm.CanonDualConstraintsToString() + "\n");


                btnSolve.Enabled = true;
                cboMethod.Enabled = true;
                cboMethod.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("The textfile was not in the correct format.\n" +
                            "Please ensure that each variable is separated by a space and there are not any additional spaces. All numbers except the rhs have signs (+/-). And should have a min/max at the start.\n"
                            , "Textfile error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {

            LinearModel lm = new LinearModel(lp.ToArray());


            List<BranchTable> branches = new List<BranchTable>();
            List<string> headers = new List<string>();
            List<string> rowHeaders = new List<string>();
            double[,] finalTable = new double[lm.SimplexInitial.GetLength(0), lm.SimplexInitial.GetLength(1)];

            switch (cboMethod.SelectedIndex)
            {
                case 0:
                    if (lm.SignRes.Contains("int") || lm.SignRes.Contains("bin"))
                    {
                        MessageBox.Show("Solving Using Primal Simplex","Solver", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    SimplexSolver sp = new SimplexSolver(lm.SimplexInitial, lm.ProblemType);
                    List<double[,]> tables = sp.SolvePrimalSimplex();
                    finalTable = tables[tables.Count - 1];

                    rowHeaders.Add($"Z");

                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        headers.Add(kvp.Key);
                    }

                    int rowCount = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        rowHeaders.Add($"{rowCount}");
                        rowCount++;

                        foreach (var kvp in con.Where(x => !x.Key.Contains('X') && x.Key != "rhs" && x.Key != "sign"))
                        {
                            headers.Add(kvp.Key);
                        }
                    }
                    headers.Add("rhs");
                    int count = 1;
                    foreach (var table in tables)
                    {
                        BranchTable newTable = new BranchTable(count.ToString(), table, headers, rowHeaders);
                        branches.Add(newTable);
                        count++;
                    }

                    btnOutputClear_Click(sender, e);

                    break;
                case 1:
                    if (lm.SignRes.Contains("int") || lm.SignRes.Contains("bin"))
                    {
                        MessageBox.Show("Solving Using Dual Simplex", "Solver", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    SimplexSolver sd = new SimplexSolver(lm.SimplexInitial, lm.ProblemType);

                    List<double[,]> dualResult = sd.SolveDualSimplex();
                    finalTable = dualResult[dualResult.Count - 1];

                    rowHeaders.Add($"Z");

                    count = 1;
                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        headers.Add(kvp.Key);
                    }

                    rowCount = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        rowHeaders.Add($"{rowCount}");
                        rowCount++;

                        foreach (var kvp in con.Where(x => !x.Key.Contains('X') && x.Key != "rhs" && x.Key != "sign"))
                        {
                            headers.Add(kvp.Key);
                        }
                    }
                    headers.Add("rhs");

                    foreach (var table in dualResult)
                    {
                        BranchTable newTable = new BranchTable(count.ToString(), table, headers, rowHeaders);
                        branches.Add(newTable);
                        count++;
                    }
                    btnOutputClear_Click(sender, e);

                    break;
                case 2:
                    if (lm.SignRes.Contains("int") || lm.SignRes.Contains("bin"))
                    {
                        MessageBox.Show("Solving Using Branch and Bound", "Solver", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    sd = new SimplexSolver(lm.SimplexInitial, lm.ProblemType);

                    dualResult = sd.SolveDualSimplex();
                    finalTable = dualResult[dualResult.Count - 1];

                    bool branchable = false;

                    for (int i = 1; i < finalTable.GetLength(0); i++)
                    {
                        if (finalTable[i,finalTable.GetLength(1)-1] - Math.Floor(finalTable[i, finalTable.GetLength(1) - 1]) > 0)
                        {
                            branchable = true;
                        }
                    }

                    if (!branchable)
                    {
                        MessageBox.Show("This optimal soolution did not contain any fraction, thus no branch and bound could occur.");
                        break;
                    }

                    rowHeaders.Add($"Z");

                    count = 1;
                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        headers.Add(kvp.Key);
                    }

                    rowCount = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        rowHeaders.Add($"{rowCount}");
                        rowCount++;

                        foreach (var kvp in con.Where(x => !x.Key.Contains('X') && x.Key != "rhs" && x.Key != "sign"))
                        {
                            headers.Add(kvp.Key);
                        }
                    }
                    headers.Add("rhs");


                    BranchAndBound branchnBound = new BranchAndBound(finalTable);
                    btnOutputClear_Click(sender, e);
                    branchnBound.ExportSolutionsToFile(lm.ProblemType, headers, rowHeaders);
                    break;
                case 3:
                    CuttingPlaneSolver cp = new CuttingPlaneSolver(lm.SimplexInitial, lm.ProblemType, lm.SignRes.ToArray());


                    List<List<double[,]>> cpResultList = cp.SolveCuttingPlane();
                    List<double[,]> cpResult = new List<double[,]>();

                    string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
                    string filePath = Path.Combine(projectDirectory, "Solutions", "CuttingPlane.txt");

                    cp.SaveResultsToFile(filePath);

                    rowHeaders.Add($"Z");

                    count = 1;

                    foreach (var iteration in cpResultList)
                    {
                        foreach (var table in iteration)
                        {
                            cpResult.Add(table);
                        }
                    }
                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        headers.Add(kvp.Key);
                    }
                    rowCount = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        rowHeaders.Add($"{rowCount}");
                        rowCount++;


                        foreach (var kvp in con.Where(x => !x.Key.Contains('X') && x.Key != "rhs" && x.Key != "sign"))
                        {
                            headers.Add(kvp.Key);
                        }
                    }

                    rowHeaders.Add($"{rowCount}");
                    headers.Add("S");
                    headers.Add("rhs");

                    foreach (var table in cpResult)
                    {
                        BranchTable newTable = new BranchTable(count.ToString(), table, headers, rowHeaders);
                        branches.Add(newTable);
                        count++;
                    }

                    btnOutputClear_Click(sender, e);

                    break;
                default:
                    MessageBox.Show("Invalid method selected, please try another method.", "Method Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            CriticalAnalysis ca = new CriticalAnalysis(lm.SimplexInitial, finalTable);
            rtbOutput.Text = "";
            rtbOutput.AppendText("\nCbv = ");

            string caOutput = "[\t";
            for (int i = 0; i < ca.cBV.Length; i++)
            {
                caOutput += $"{Math.Round(ca.cBV[i], 4)};\t";
            }
            caOutput += "]";
            rtbOutput.AppendText(caOutput + "\n");

            caOutput = "";
            Matrix<double> matrixB = Matrix<double>.Build.DenseOfArray(ca.B).Transpose();
            double[,] bArray = matrixB.ToArray();

            rtbOutput.AppendText("\n B =");
            for (int i = 0; i < bArray.GetLength(0); i++)
            {
                caOutput = "\t[\t";
                for (int j = 0; j < bArray.GetLength(1); j++)
                {
                    caOutput += $"{Math.Round(bArray[i, j], 4)};\t";
                }
                caOutput += "]";
                rtbOutput.AppendText(caOutput + "\n");
            }

            caOutput = "";

            rtbOutput.AppendText($"\n B-1 =");
            double[,] bInverseArray = ca.BInverse.Transpose().ToArray();
            for (int i = 0; i < bInverseArray.GetLength(0); i++)
            {
                caOutput = "\t[\t";
                for (int j = 0; j < bInverseArray.GetLength(1); j++)
                {
                    caOutput += $"{Math.Round(bInverseArray[i, j], 4)};\t";
                }
                caOutput += "]";
                rtbOutput.AppendText(caOutput + "\n");
            }


            rtbOutput.AppendText($"\nCbvB-1 =");
            caOutput = "[\t";
            double[,] cBVbInverse = ca.CbvBinverse.ToArray();
            for (int i = 0; i < cBVbInverse.GetLength(1); i++)
            {
                caOutput += $"{Math.Round(cBVbInverse[0, i], 4)};\t";
            }
            caOutput += "]";
            rtbOutput.AppendText(caOutput + "\n");


            rtbOutput.AppendText($"\nb =");
            caOutput = "";
            for (int i = 0; i < ca.z.Length; i++)
            {
                caOutput += $"\t[\t{Math.Round(ca.z[i], 4)}\t]\n";
            }
            rtbOutput.AppendText(caOutput + "\n");


        }

        public bool CheckFeasibility(List<double[,]> result)
        {
            double[,] finalTable = result.ElementAt(result.Count - 1);
            // Infeasible solution.
            double sum = 0;
            for (int i = 0; i < finalTable.GetLength(0); i++)
            {
                for (int j = 0; j < finalTable.GetLength(1); j++)
                {
                    sum += finalTable[i, j];
                }
            }
            if (sum == 0)
            {
                MessageBox.Show("The solution is infeasible given the problem and the method used.\n\nTry another method or inspect the Linear programming problem.", "Infeasible solution", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Unbounded solution.
            for (int i = 0; i < finalTable.GetLength(1); i++)
            {
                if (finalTable[0, i] < 0)
                {
                    MessageBox.Show("The solution is unbounded given the problem.\n\nTry inspecing the Linear programming problem for any variables that can be unbounded.", "Unbounded solution", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            // Multiple solutions.
            for (int i = 0; i < finalTable.GetLength(1); i++)
            {
                if (finalTable[0, i] == 0)
                {
                    sum = 0;
                    for (int j = 0; j < finalTable.GetLength(0); j++)
                    {
                        sum += finalTable[j, i];
                    }
                    if (sum != 1)
                    {
                        MessageBox.Show("There are multiple solutions for the given problem.\n\nKeep in mind only one of these possible solutions will be displayed.", "Multiple solutions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                }
            }
            return true;
        }

        public List<List<double>> ArrayToList(double[,] table)
        {
            // Populate Nested lists with values from the array
            List<List<double>> listTable = new List<List<double>>();

            for (int i = 0; i < table.GetLength(0); i++)
            {
                List<double> row = new List<double>();
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    row.Add(table[i, j]);
                }
                listTable.Add(row);
            }
            return listTable;
        }
        public double[,] ListToArray(List<List<double>> table)
        {
            // Populate Nested lists with values from the array
            double[,] listTable = new double[table.Count, table[0].Count];

            for (int i = 0; i < table.Count; i++)
            {
                for (int j = 0; j < table[i].Count; j++)
                {
                    listTable[i, j] = table[i][j];
                }
            }
            return listTable;
        }
        private bool IsBasic(int columnIndex, List<List<double>> table, int rows)
        {
            List<double> column = new List<double>();
            //Grab values from column
            for (int i = 0; i < rows; i++)
            {
                // If element is one or zero add to list (straight forward) 
                // BUT if the element is anything else then it is a non-basic variable (IsBasic = false = non-basic and vice versa)
                if (table[i][columnIndex] == 0 || table[i][columnIndex] == 1)
                {
                    column.Add(table[i][columnIndex]);
                }
                else
                {
                    return false;
                }
            }
            // Sum of basic variable column = 1 (accounts for example: 0 1 1 which would pass the previous test)
            return column.Sum() == 1;
        }

        private void cboCAChangeRow_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnOutputClear_Click(object sender, EventArgs e)
        {

        }

        private void btnCAOutputClear_Click(object sender, EventArgs e)
        {
            rtbOutput.Text = "";
        }
    }
}
