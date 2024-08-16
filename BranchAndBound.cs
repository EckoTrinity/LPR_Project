using System;
using System.Collections.Generic;
using System.Linq;

namespace LPR381_Project
{
    internal class BranchAndBound
    {
        private List<List<double>> _initialTable;
        private int _variableColumnCount;

        private List<List<double>> ConvertArrayToList(double[,] array)
        {
            var list = new List<List<double>>();

            for (int row = 0; row < array.GetLength(0); row++)
            {
                var rowList = new List<double>();
                for (int col = 0; col < array.GetLength(1); col++)
                {
                    rowList.Add(array[row, col]);
                }
                list.Add(rowList);
            }
            return list;
        }

        public BranchAndBound(double[,] tableArray)
        {
            _initialTable = ConvertArrayToList(tableArray);
            _variableColumnCount = tableArray.GetLength(1) - tableArray.GetLength(0);
        }

        private bool IsBasicVariable(int columnIndex, List<List<double>> table)
        {
            var columnValues = new List<double>();
            int rowCount = table.Count;

            for (int row = 0; row < rowCount; row++)
            {
                double value = table[row][columnIndex];
                if (value == 0 || value == 1)
                {
                    columnValues.Add(value);
                }
                else
                {
                    return false;
                }
            }

            return columnValues.Sum() == 1;
        }

        private List<int> IdentifyBasicVariableRows(List<List<double>> table)
        {
            var basicVariableRows = new List<int>();
            int variableCount = table[0].Count - table.Count;

            for (int i = 0; i < variableCount; i++)
            {
                if (IsBasicVariable(i, table))
                {
                    for (int row = 0; row < table.Count; row++)
                    {
                        if (table[row][i] == 1)
                        {
                            basicVariableRows.Add(row);
                            break;
                        }
                    }
                }
            }
            return basicVariableRows;
        }

        private int SelectCuttingRow(List<List<double>> table)
        {
            var basicVariableRows = IdentifyBasicVariableRows(table);
            int columnCount = table[0].Count;
            var fractionalParts = new List<double>();
            var rowsToRemove = new List<int>();

            for (int i = 0; i < basicVariableRows.Count; i++)
            {
                double fractionalPart = Math.Round(table[basicVariableRows[i]][columnCount - 1] - Math.Floor(table[basicVariableRows[i]][columnCount - 1]), 3);
                if (fractionalPart != 0)
                {
                    fractionalParts.Add(Math.Abs(Math.Abs(fractionalPart) - 0.5));
                }
                else
                {
                    rowsToRemove.Add(i);
                }
            }

            for (int i = rowsToRemove.Count - 1; i >= 0; i--)
            {
                basicVariableRows.RemoveAt(rowsToRemove[i]);
            }

            double minFractionalPart = fractionalParts.Min();
            bool hasMultipleMinFractions = fractionalParts.Count(f => f == minFractionalPart) > 1;

            if (hasMultipleMinFractions)
            {
                int maxRHSIndex = 0;
                for (int i = 0; i < fractionalParts.Count; i++)
                {
                    if (fractionalParts[i] == minFractionalPart && table[basicVariableRows[i]][columnCount - 1] > table[basicVariableRows[maxRHSIndex]][columnCount - 1])
                    {
                        maxRHSIndex = basicVariableRows[i];
                    }
                }
                return maxRHSIndex;
            }
            else
            {
                return basicVariableRows[fractionalParts.IndexOf(minFractionalPart)];
            }
        }

        public List<List<double>> CreateConstraints(int rowIndex, int columnIndex, List<List<double>> table)
        {
            int columnCount = table[0].Count;
            var newConstraints = new List<List<double>>();

            double rhsValue = table[rowIndex][columnCount - 1];
            var lowerConstraint = Enumerable.Repeat(0.0, columnCount).ToList();
            double lowerRHS = Math.Floor(rhsValue);
            lowerConstraint[columnIndex] = 1.0;
            lowerConstraint[columnCount - 1] = lowerRHS;
            lowerConstraint.Insert(columnCount - 1, 1);

            newConstraints.Add(lowerConstraint);

            var upperConstraint = Enumerable.Repeat(0.0, columnCount).ToList();
            double upperRHS = Math.Ceiling(rhsValue);
            upperConstraint[columnIndex] = 1.0;
            upperConstraint[columnCount - 1] = upperRHS;
            upperConstraint.Insert(columnCount - 1, -1);

            newConstraints.Add(upperConstraint);

            return newConstraints;
        }

        private BranchTable ApplyConstraint(List<double> constraint, BranchTable branchTable, bool invertSign = false)
        {
            var table = ConvertArrayToList(branchTable.Table);
            int columnCount = table[0].Count;
            int rowCount = table.Count;
            var conflictingRows = new List<int>();

            for (int col = 0; col < columnCount; col++)
            {
                if (IsBasicVariable(col, table))
                {
                    double sum = 0;
                    int conflictingRow = 0;
                    for (int row = 0; row < rowCount; row++)
                    {
                        sum += table[row][col];
                        if (table[row][col] == 1)
                        {
                            conflictingRow = row;
                        }
                    }
                    if (sum != sum + constraint[col])
                    {
                        conflictingRows.Add(conflictingRow);
                    }
                }
            }

            for (int row = 0; row < rowCount; row++)
            {
                table[row].Insert(columnCount - 1, 0);
            }

            columnCount++;

            foreach (var conflictingRow in conflictingRows)
            {
                var newRow = new List<double>();
                for (int col = 0; col < columnCount; col++)
                {
                    double element = (table[conflictingRow][col] - constraint[col]);
                    if (invertSign)
                    {
                        element *= -1;
                    }
                    newRow.Add(element);
                }
                table.Add(newRow);
            }
            rowCount++;
            branchTable.Table = ConvertListToArray(table);
            return branchTable;
        }

        public double[,] ConvertListToArray(List<List<double>> list)
        {
            var array = new double[list.Count, list[0].Count];

            for (int row = 0; row < list.Count; row++)
            {
                for (int col = 0; col < list[row].Count; col++)
                {
                    array[row, col] = list[row][col];
                }
            }
            return array;
        }

        public void ExportSolutionsToFile(string problemType, List<string> columnHeaders, List<string> rowHeaders)
        {
            string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            string filePath = Path.Combine(projectDirectory, "Solutions", "BranchAndBound.txt");
            var branchQueue = new Queue<BranchTable>();
            var solutions = new List<BranchTable>();

            branchQueue.Enqueue(new BranchTable("0", ConvertListToArray(_initialTable), columnHeaders, rowHeaders));
            solutions.Add(new BranchTable("0", ConvertListToArray(_initialTable), columnHeaders, rowHeaders));

            while (branchQueue.Count > 0)
            {
                var currentBranch = branchQueue.Dequeue();
                var table = ConvertArrayToList(currentBranch.Table);

                int cuttingRow = SelectCuttingRow(table);
                var constraints = CreateConstraints(cuttingRow, table[cuttingRow].IndexOf(1), table);

                for (int i = 0; i < constraints.Count; i++)
                {
                    var newBranch = new BranchTable(currentBranch.Level + (i + 1), ConvertListToArray(table), columnHeaders, rowHeaders);
                    newBranch = ApplyConstraint(constraints[i], newBranch, i % 2 == 0);

                    var simplexSolver = new SimplexSolver(newBranch.Table, problemType);
                    var pivotTables = simplexSolver.SolveDualSimplex();
                    var updatedColumnHeaders = new List<string>(columnHeaders) { i % 2 == 0 ? "S" : "E" };
                    var updatedRowHeaders = new List<string>(rowHeaders) { (int.Parse(rowHeaders.Last()) + 1).ToString() };

                    foreach (var pivot in pivotTables)
                    {
                        solutions.Add(new BranchTable($"{newBranch.Level}{i + 1}", pivot, updatedColumnHeaders, updatedRowHeaders));
                    }

                    var optimalTable = pivotTables.Last();
                    var newSolutionBranch = new BranchTable($"{newBranch.Level}{i + 1}", optimalTable, updatedColumnHeaders, updatedRowHeaders);

                    bool hasFractionalPart = false;
                    for (int j = 0; j < optimalTable.GetLength(0); j++)
                    {
                        double rhsValue = Math.Round(optimalTable[j, optimalTable.GetLength(1) - 1], 3);
                        double floorValue = Math.Floor(rhsValue);
                        double fractionalPart = Math.Round(rhsValue - (floorValue < 0 ? 0 : floorValue), 3);
                        hasFractionalPart = fractionalPart > 0.001;
                        if (hasFractionalPart)
                        {
                            break;
                        }
                    }
                    if (hasFractionalPart && !AreTablesEqual(newSolutionBranch.Table, optimalTable))
                    {
                        branchQueue.Enqueue(newSolutionBranch);
                    }
                }
            }

            // Write solutions to file
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var solution in solutions)
                {
                    writer.WriteLine($"Branch: {solution.Level}");
                    var solutionTable = ConvertArrayToList(solution.Table);
                    foreach (var row in solutionTable)
                    {
                        writer.WriteLine(string.Join("\t", row.Select(val => val.ToString("F2"))));
                    }
                    writer.WriteLine();
                }
            }
        }

        private bool AreTablesEqual(double[,] table1, double[,] table2)
        {
            if (table1.GetLength(0) != table2.GetLength(0) || table1.GetLength(1) != table2.GetLength(1))
            {
                return false;
            }

            for (int row = 0; row < table1.GetLength(0); row++)
            {
                for (int col = 0; col < table1.GetLength(1); col++)
                {
                    if (table1[row, col] != table2[row, col])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
