using LPR_Project;
using System;
using System.Collections.Generic;

namespace LPR_Project
{
    public static class PrimalSimplex
    {
        public static List<string> Solve(Model model)
        {
            List<string> result = new List<string>();

            // Step 1: Create the Canonical Form
            var canonicalForm = CreateCanonicalForm(model);
            result.Add("Canonical Form:");
            result.AddRange(PrintMatrix(canonicalForm));

            // Step 2: Perform the Simplex Method Iterations
            var finalTableau = SimplexMethod(canonicalForm);
            if (finalTableau == null)
            {
                result.Add("The problem is unbounded or infeasible.");
                return result;
            }

            result.Add("Final Tableau:");
            result.AddRange(PrintMatrix(finalTableau));

            // Step 3: Extract the Solution
            var solution = ExtractSolution(finalTableau, model);
            result.Add("Solution:");
            result.AddRange(solution);

            return result;
        }

        private static double[,] CreateCanonicalForm(Model model)
        {
            int numConstraints = model.Constraints.Length;
            int numVariables = model.ObjectiveCoefficients.Length;

            // Create the initial tableau (including slack variables)
            double[,] tableau = new double[numConstraints + 1, numVariables + numConstraints + 1];

            // Fill the objective row (last row)
            for (int j = 0; j < numVariables; j++)
            {
                tableau[numConstraints, j] = -model.ObjectiveCoefficients[j]; // negate since we minimize in Simplex
            }

            // Fill the constraint rows
            for (int i = 0; i < numConstraints; i++)
            {
                for (int j = 0; j < numVariables; j++)
                {
                    tableau[i, j] = model.Constraints[i].Coefficients[j];
                }

                // Add the slack variable
                tableau[i, numVariables + i] = 1;

                // Add the RHS of the constraint
                tableau[i, numVariables + numConstraints] = model.Constraints[i].RHS;
            }

            return tableau;
        }

        private static double[,] SimplexMethod(double[,] tableau)
        {
            int numRows = tableau.GetLength(0);
            int numCols = tableau.GetLength(1);

            while (true)
            {
                // Step 1: Identify the entering variable (most negative coefficient in the objective row)
                int enteringCol = -1;
                double mostNegative = 0;
                for (int j = 0; j < numCols - 1; j++)
                {
                    if (tableau[numRows - 1, j] < mostNegative)
                    {
                        mostNegative = tableau[numRows - 1, j];
                        enteringCol = j;
                    }
                }

                // If no negative coefficients, optimal solution found
                if (enteringCol == -1)
                {
                    break;
                }

                // Step 2: Identify the leaving variable (minimum positive ratio of RHS to entering column)
                int leavingRow = -1;
                double minRatio = double.PositiveInfinity;
                for (int i = 0; i < numRows - 1; i++)
                {
                    if (tableau[i, enteringCol] > 0)
                    {
                        double ratio = tableau[i, numCols - 1] / tableau[i, enteringCol];
                        if (ratio < minRatio)
                        {
                            minRatio = ratio;
                            leavingRow = i;
                        }
                    }
                }

                // If no valid leaving row, the problem is unbounded
                if (leavingRow == -1)
                {
                    return null;
                }

                // Step 3: Pivot on the identified element
                Pivot(tableau, leavingRow, enteringCol);
            }

            return tableau;
        }

        private static void Pivot(double[,] tableau, int pivotRow, int pivotCol)
        {
            int numRows = tableau.GetLength(0);
            int numCols = tableau.GetLength(1);

            double pivotElement = tableau[pivotRow, pivotCol];

            // Divide the pivot row by the pivot element
            for (int j = 0; j < numCols; j++)
            {
                tableau[pivotRow, j] /= pivotElement;
            }

            // Subtract multiples of the pivot row from all other rows
            for (int i = 0; i < numRows; i++)
            {
                if (i != pivotRow)
                {
                    double factor = tableau[i, pivotCol];
                    for (int j = 0; j < numCols; j++)
                    {
                        tableau[i, j] -= factor * tableau[pivotRow, j];
                    }
                }
            }
        }

        private static List<string> ExtractSolution(double[,] tableau, Model model)
        {
            List<string> solution = new List<string>();
            int numConstraints = model.Constraints.Length;
            int numVariables = model.ObjectiveCoefficients.Length;

            double[] variableValues = new double[numVariables];

            // Identify basic variables and their values
            for (int j = 0; j < numVariables; j++)
            {
                int count = 0;
                int row = -1;
                for (int i = 0; i < numConstraints; i++)
                {
                    if (tableau[i, j] == 1.0)
                    {
                        count++;
                        row = i;
                    }
                    else if (tableau[i, j] != 0.0)
                    {
                        count = 0;
                        break;
                    }
                }

                if (count == 1)
                {
                    variableValues[j] = tableau[row, numVariables + numConstraints];
                }
            }

            for (int i = 0; i < numVariables; i++)
            {
                solution.Add($"x{i + 1} = {Math.Round(variableValues[i], 3)}");
            }

            solution.Add($"Optimal objective value = {Math.Round(tableau[numConstraints, numVariables + numConstraints], 3)}");

            return solution;
        }

        private static List<string> PrintMatrix(double[,] matrix)
        {
            List<string> result = new List<string>();
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);

            for (int i = 0; i < numRows; i++)
            {
                string row = "";
                for (int j = 0; j < numCols; j++)
                {
                    row += Math.Round(matrix[i, j], 3).ToString().PadRight(10) + " ";
                }
                result.Add(row.Trim());
            }

            return result;
        }
    }
}
