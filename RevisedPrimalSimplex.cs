using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace LPR_Project
{
    public static class RevisedPrimalSimplex
    {
        public static List<string> Solve(Model model)
        {
            List<string> result = new List<string>();

            try
            {
                // Step 1: Create Initial Canonical Form and Basis
                var (B, N, basicVariables, nonBasicVariables) = Initialize(model);

                result.Add("Initial Basis:");
                result.AddRange(PrintBasis(basicVariables, model));

                // Step 2: Perform Revised Simplex Iterations
                while (true)
                {
                    // Step 2a: Calculate reduced costs
                    var reducedCosts = CalculateReducedCosts(N, model.ObjectiveCoefficients, B, nonBasicVariables);

                    result.Add("Reduced Costs:");
                    result.AddRange(reducedCosts.Select(c => c.ToString("F2")).ToList());

                    // Step 2b: Determine entering variable (most negative reduced cost)
                    int enteringIndex = Array.IndexOf(reducedCosts, reducedCosts.Min());

                    if (enteringIndex == -1 || reducedCosts[enteringIndex] >= 0)
                    {
                        result.Add("Optimal solution found.");
                        break; // Optimal solution found
                    }

                    // Step 2c: Compute direction vector (B^-1 * N entering column)
                    var enteringColumn = ExtractColumn(N, enteringIndex);
                    var B_inv = B.Inverse();
                    var directionVector = MultiplyMatrixVector(B_inv, enteringColumn);

                    // Step 2d: Determine leaving variable using minimum ratio test
                    int leavingIndex = -1;
                    double minRatio = double.PositiveInfinity;
                    for (int i = 0; i < basicVariables.Length; i++)
                    {
                        if (directionVector[i] > 0)
                        {
                            double ratio = basicVariables[i] / directionVector[i];
                            if (ratio < minRatio)
                            {
                                minRatio = ratio;
                                leavingIndex = i;
                            }
                        }
                    }

                    if (leavingIndex == -1)
                    {
                        result.Add("The problem is unbounded.");
                        return result;
                    }

                    // Step 2e: Update Basis
                    B = UpdateBasis(B, directionVector, leavingIndex);
                    basicVariables[leavingIndex] = model.Constraints[leavingIndex].RHS;
                    N = UpdateNonBasicVariables(N, B, ExtractColumn(N, enteringIndex), enteringIndex);
                    nonBasicVariables[enteringIndex] = basicVariables[leavingIndex];

                    result.Add("Updated Basis:");
                    result.AddRange(PrintBasis(basicVariables, model));
                }

                // Step 3: Extract and Print Final Solution
                var finalSolution = ExtractSolution(B, model, basicVariables);
                result.Add("Final Solution:");
                result.AddRange(finalSolution);
            }
            catch (Exception ex)
            {
                result.Add($"An error occurred: {ex.Message}");
            }

            return result;
        }

        private static (Matrix<double> B, Matrix<double> N, double[] basicVariables, double[] nonBasicVariables) Initialize(Model model)
        {
            int numConstraints = model.Constraints.Length;
            int numVariables = model.ObjectiveCoefficients.Length;

            // Initialize basis matrix B (identity matrix for slack variables)
            var B = Matrix<double>.Build.DenseIdentity(numConstraints);
            var basicVariables = new double[numConstraints];

            // Initialize non-basis matrix N (original constraint coefficients)
            var N = Matrix<double>.Build.Dense(numConstraints, numVariables);

            for (int i = 0; i < numConstraints; i++)
            {
                basicVariables[i] = model.Constraints[i].RHS;

                for (int j = 0; j < numVariables; j++)
                {
                    N[i, j] = model.Constraints[i].Coefficients[j];
                }
            }

            var nonBasicVariables = model.ObjectiveCoefficients;

            return (B, N, basicVariables, nonBasicVariables);
        }

        private static double[] CalculateReducedCosts(Matrix<double> N, double[] c, Matrix<double> B, double[] nonBasicVariables)
        {
            int numNonBasic = nonBasicVariables.Length;
            double[] reducedCosts = new double[numNonBasic];

            var B_inverse = B.Inverse();

            for (int j = 0; j < numNonBasic; j++)
            {
                var N_j = ExtractColumn(N, j);
                var B_inv_N_j = MultiplyMatrixVector(B_inverse, N_j);
                reducedCosts[j] = nonBasicVariables[j] - DotProduct(c, B_inv_N_j);
            }

            return reducedCosts;
        }

        private static Matrix<double> UpdateBasis(Matrix<double> B, double[] directionVector, int leavingIndex)
        {
            if (directionVector.Length != B.RowCount)
                throw new ArgumentException("Direction vector length does not match the number of rows in B.");

            var newB = B.Clone();

            for (int i = 0; i < B.RowCount; i++)
            {
                newB[i, leavingIndex] = directionVector[i];
            }

            return newB;
        }

        private static Matrix<double> UpdateNonBasicVariables(Matrix<double> N, Matrix<double> B, double[] enteringColumn, int enteringIndex)
        {
            if (enteringColumn.Length != N.RowCount)
                throw new ArgumentException("Entering column length does not match the number of rows in N.");

            var newN = N.Clone();
            var newColumn = MultiplyMatrixVector(B, enteringColumn);

            for (int i = 0; i < N.RowCount; i++)
            {
                newN[i, enteringIndex] = newColumn[i];
            }

            return newN;
        }

        private static double[] ExtractColumn(Matrix<double> matrix, int colIndex)
        {
            if (colIndex < 0 || colIndex >= matrix.ColumnCount)
                throw new IndexOutOfRangeException("Column index is out of range.");

            var column = new double[matrix.RowCount];
            for (int i = 0; i < matrix.RowCount; i++)
            {
                column[i] = matrix[i, colIndex];
            }

            return column;
        }

        private static double DotProduct(double[] vector1, double[] vector2)
        {
            if (vector1.Length != vector2.Length)
                throw new ArgumentException("Vectors must be of the same length.");

            return vector1.Zip(vector2, (v1, v2) => v1 * v2).Sum();
        }

        private static double[] MultiplyMatrixVector(Matrix<double> matrix, double[] vector)
        {
            if (matrix.ColumnCount != vector.Length)
                throw new ArgumentException("Matrix column count must match vector length.");

            var result = new double[matrix.RowCount];

            for (int i = 0; i < matrix.RowCount; i++)
            {
                result[i] = 0;
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    result[i] += matrix[i, j] * vector[j];
                }
            }

            return result;
        }

        private static List<string> PrintBasis(double[] basicVariables, Model model)
        {
            var result = new List<string>();
            for (int i = 0; i < basicVariables.Length; i++)
            {
                result.Add($"x{i + 1} = {basicVariables[i]}");
            }
            return result;
        }

        private static List<string> ExtractSolution(Matrix<double> B, Model model, double[] basicVariables)
        {
            var solution = new List<string>();

            double objectiveValue = 0.0;

            for (int i = 0; i < model.ObjectiveCoefficients.Length; i++)
            {
                objectiveValue += model.ObjectiveCoefficients[i] * basicVariables[i];
            }

            solution.Add($"Objective Value = {objectiveValue:F2}");

            for (int i = 0; i < basicVariables.Length; i++)
            {
                solution.Add($"x{i + 1} = {basicVariables[i]}");
            }

            return solution;
        }
    }
}
