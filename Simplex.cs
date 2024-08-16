using System;
using System.Collections.Generic;
using System.Text;

namespace LPR381_Project
{
    internal class SimplexSolver
    {
        private double[,] initialTableau;
        private string optimizationType;

        public SimplexSolver(double[,] initialTableau, string optimizationType)
        {
            this.InitialTableau = initialTableau;
            this.OptimizationType = optimizationType;
        }

        public double[,] InitialTableau { get => initialTableau; set => initialTableau = value; }
        public string OptimizationType { get => optimizationType; set => optimizationType = value; }

        // Method to perform pivot operation on the tableau.
        private double[,] PerformPivot(double[,] tableau, int pivotColIndex, int pivotRowIndex)
        {
            double[,] newTableau = new double[tableau.GetLength(0), tableau.GetLength(1)];

            // Perform the pivot operations
            for (int row = 0; row < tableau.GetLength(0); row++)
            {
                for (int col = 0; col < tableau.GetLength(1); col++)
                {
                    double elementValue = tableau[row, col] - (tableau[row, pivotColIndex] * (tableau[pivotRowIndex, col] / tableau[pivotRowIndex, pivotColIndex]));
                    if (elementValue.ToString().Contains(".9999"))
                    {
                        elementValue = Math.Ceiling(elementValue);
                    }
                    newTableau[row, col] = elementValue;
                }
            }

            // Normalize the pivot row
            for (int col = 0; col < tableau.GetLength(1); col++)
            {
                double elementValue = tableau[pivotRowIndex, col] / tableau[pivotRowIndex, pivotColIndex];
                if (elementValue.ToString().Contains(".9999"))
                {
                    elementValue = Math.Ceiling(elementValue);
                }
                newTableau[pivotRowIndex, col] = elementValue;
            }

            return newTableau;
        }

        // Dual Simplex Algorithm
        public List<double[,]> SolveDualSimplex()
        {
            List<double[,]> tableaus = new List<double[,]>();
            tableaus.Add(InitialTableau);
            bool optimalSolutionFound = false;

            do
            {
                double[,] currentTableau = tableaus[tableaus.Count - 1];
                int rowCount = currentTableau.GetLength(0);
                int colCount = currentTableau.GetLength(1);

                int pivotRow = -1;
                double minRatio = double.MaxValue;
                int pivotCol = -1;

                // Find the row with the most negative RHS value
                double minRHS = 0;
                for (int row = 1; row < rowCount; row++)
                {
                    double rhsValue = currentTableau[row, colCount - 1];
                    if (rhsValue < minRHS)
                    {
                        pivotRow = row;
                        minRHS = rhsValue;
                    }
                }

                if (minRHS >= 0)
                {
                    // Pivot column selection for maximization problems
                    if (OptimizationType == "max")
                    {
                        double minCost = 0;
                        for (int col = 0; col < colCount - 1; col++)
                        {
                            double costValue = currentTableau[0, col];
                            if (costValue < minCost)
                            {
                                pivotCol = col;
                                minCost = costValue;
                            }
                        }
                    }
                    // Pivot column selection for minimization problems
                    else
                    {
                        double maxCost = 0;
                        for (int col = 0; col < colCount - 1; col++)
                        {
                            double costValue = currentTableau[0, col];
                            if (costValue > maxCost)
                            {
                                pivotCol = col;
                                maxCost = costValue;
                            }
                        }
                    }

                    if (pivotCol == -1)
                    {
                        optimalSolutionFound = true;
                        break;
                    }

                    // Find the pivot row
                    minRatio = double.MaxValue;
                    for (int row = 1; row < rowCount; row++)
                    {
                        double ratio = currentTableau[row, colCount - 1] / currentTableau[row, pivotCol];
                        if (ratio < minRatio && ratio >= 0)
                        {
                            pivotRow = row;
                            minRatio = ratio;
                        }
                    }

                    if (pivotRow == -1)
                    {
                        optimalSolutionFound = true;
                        break;
                    }

                    tableaus.Add(PerformPivot(currentTableau, pivotCol, pivotRow));
                }
                else
                {
                    if (pivotRow == -1)
                    {
                        optimalSolutionFound = true;
                        break;
                    }

                    // Find the pivot column for the dual simplex method
                    for (int col = 0; col < colCount - 1; col++)
                    {
                        double ratio = Math.Abs(currentTableau[0, col] / currentTableau[pivotRow, col]);
                        if (currentTableau[pivotRow, col] <= 0 && ratio < minRatio)
                        {
                            minRatio = ratio;
                            pivotCol = col;
                        }
                    }

                    if (pivotCol == -1)
                    {
                        optimalSolutionFound = true;
                        break;
                    }

                    tableaus.Add(PerformPivot(currentTableau, pivotCol, pivotRow));
                }

            } while (!optimalSolutionFound);

            string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            string filePath = Path.Combine(projectDirectory, "Solutions", "DualSimplex.txt");
            WriteTableausToFile(tableaus, filePath);

            return tableaus;
        }

        // Primal Simplex Algorithm
        public List<double[,]> SolvePrimalSimplex()
        {
            List<double[,]> tableaus = new List<double[,]>();
            tableaus.Add(InitialTableau);
            bool optimalSolutionFound = false;

            do
            {
                double[,] currentTableau = tableaus[tableaus.Count - 1];
                int rowCount = currentTableau.GetLength(0);
                int colCount = currentTableau.GetLength(1);

                int pivotRow = -1;
                double minRatio = double.MaxValue;
                int pivotCol = -1;

                double minRHS = 0;
                for (int row = 1; row < rowCount; row++)
                {
                    double rhsValue = currentTableau[row, colCount - 1];
                    if (rhsValue < minRHS)
                    {
                        pivotRow = row;
                        minRHS = rhsValue;
                    }
                }

                if (minRHS >= 0)
                {
                    if (OptimizationType == "max")
                    {
                        double minCost = 0;
                        for (int col = 0; col < colCount - 1; col++)
                        {
                            double costValue = currentTableau[0, col];
                            if (costValue < minCost)
                            {
                                pivotCol = col;
                                minCost = costValue;
                            }
                        }
                    }
                    else
                    {
                        double maxCost = 0;
                        for (int col = 0; col < colCount - 1; col++)
                        {
                            double costValue = currentTableau[0, col];
                            if (costValue > maxCost)
                            {
                                pivotCol = col;
                                maxCost = costValue;
                            }
                        }
                    }

                    if (pivotCol == -1)
                    {
                        optimalSolutionFound = true;
                        break;
                    }

                    minRatio = double.MaxValue;
                    for (int row = 1; row < rowCount; row++)
                    {
                        double ratio = currentTableau[row, colCount - 1] / currentTableau[row, pivotCol];
                        if (ratio < minRatio && ratio >= 0)
                        {
                            pivotRow = row;
                            minRatio = ratio;
                        }
                    }

                    if (pivotRow == -1)
                    {
                        optimalSolutionFound = true;
                        break;
                    }

                    tableaus.Add(PerformPivot(currentTableau, pivotCol, pivotRow));
                }
                else
                {
                    tableaus.Add(new double[rowCount, colCount]);
                    break;
                }

            } while (!optimalSolutionFound);

            string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            string filePath = Path.Combine(projectDirectory, "Solutions", "PrimalSimplex.txt");
            WriteTableausToFile(tableaus, filePath);

            return tableaus;
        }

        private void WriteTableausToFile(List<double[,]> tableaus, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                int iteration = 1;
                foreach (var tableau in tableaus)
                {
                    writer.WriteLine($"Iteration {iteration}:");
                    for (int row = 0; row < tableau.GetLength(0); row++)
                    {
                        for (int col = 0; col < tableau.GetLength(1); col++)
                        {
                            writer.Write(tableau[row, col].ToString("F4") + "\t");
                        }
                        writer.WriteLine();
                    }
                    writer.WriteLine();
                    iteration++;
                }
            }
        }
    }
}
