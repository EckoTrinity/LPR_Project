using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPR381_Project
{
    internal class CuttingPlaneSolver
    {
        private double[,] linearProgram;
        private string objectiveType;
        private string[] variableRestrictions;

        public CuttingPlaneSolver(double[,] linearProgram, string objectiveType, string[] variableRestrictions)
        {
            this.LinearProgram = linearProgram;
            this.ObjectiveType = objectiveType;
            this.VariableRestrictions = variableRestrictions;
        }

        public double[,] LinearProgram { get => linearProgram; set => linearProgram = value; }
        public string ObjectiveType { get => objectiveType; set => objectiveType = value; }
        public string[] VariableRestrictions { get => variableRestrictions; set => variableRestrictions = value; }

        // CUTTING PLANE ALGORITHM
        public List<List<double[,]>> SolveCuttingPlane()
        {
            List<List<double[,]>> allIterations = new List<List<double[,]>>();

            SimplexSolver initialSimplex = new SimplexSolver(LinearProgram, ObjectiveType);
            List<double[,]> simplexIteration = initialSimplex.SolveDualSimplex();
            allIterations.Add(simplexIteration);
            LinearProgram = simplexIteration[simplexIteration.Count - 1];

            List<bool> skipRows = new List<bool>();
            for (int i = 0; i < LinearProgram.GetLength(0); i++)
            {
                skipRows.Add(false);
            }

            bool continueIterations = true;
            while (continueIterations)
            {
                int selectedRow = 0;
                double bestValue = 0;
                double smallestFractional = 10;

                // Identify the row to apply the cutting plane.
                for (int i = 1; i < LinearProgram.GetLength(0); i++)
                {
                    if (!skipRows[i])
                    {
                        double rhsValue = LinearProgram[i, LinearProgram.GetLength(1) - 1];
                        double fractionalValue = Math.Round(rhsValue % 1 * 10, 0);

                        if (fractionalValue >= 5)
                        {
                            fractionalValue -= 5;
                        }
                        else
                        {
                            fractionalValue = 5 - fractionalValue;
                        }

                        if (fractionalValue < smallestFractional)
                        {
                            selectedRow = i;
                            smallestFractional = fractionalValue;
                            bestValue = rhsValue;
                        }
                        else if (fractionalValue == smallestFractional && rhsValue > bestValue)
                        {
                            selectedRow = i;
                            smallestFractional = fractionalValue;
                            bestValue = rhsValue;
                        }
                    }
                }

                if (smallestFractional == 5)
                {
                    continueIterations = false;
                    break;
                }

                for (int j = 0; j < LinearProgram.GetLength(1); j++)
                {
                    if (LinearProgram[selectedRow, j] == 1)
                    {
                        if (j < VariableRestrictions.Length)
                        {
                            if (VariableRestrictions[j] != "int")
                            {
                                double columnSum = 0;
                                for (int k = 0; k < LinearProgram.GetLength(0); k++)
                                {
                                    columnSum += LinearProgram[k, j];
                                }
                                if (columnSum == 1)
                                {
                                    skipRows[selectedRow] = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (skipRows[selectedRow])
                {
                    continue;
                }

                // Generate the cut row.
                List<double> cutRow = new List<double>();
                for (int i = 0; i < LinearProgram.GetLength(1); i++)
                {
                    if (i == LinearProgram.GetLength(1) - 1)
                    {
                        cutRow.Add(1);
                    }
                    else if (LinearProgram[selectedRow, i] > 0)
                    {
                        cutRow.Add((LinearProgram[selectedRow, i] % 1) * -1);
                    }
                    else if (LinearProgram[selectedRow, i] < 0)
                    {
                        cutRow.Add((1 + (LinearProgram[selectedRow, i] % 1)) * -1);
                    }
                    else
                    {
                        cutRow.Add(0);
                    }
                }

                // Add the new cut row to the LP table.
                double[,] newLinearProgram = new double[LinearProgram.GetLength(0) + 1, LinearProgram.GetLength(1) + 1];

                for (int i = 0; i < newLinearProgram.GetLength(0); i++)
                {
                    for (int j = 0; j < LinearProgram.GetLength(1); j++)
                    {
                        if (i == newLinearProgram.GetLength(0) - 1)
                        {
                            for (int k = 0; k < cutRow.Count; k++)
                            {
                                newLinearProgram[i, k] = cutRow[k];
                            }
                            break;
                        }
                        else
                        {
                            if (j == LinearProgram.GetLength(1) - 1)
                            {
                                newLinearProgram[i, j] = 0;
                                newLinearProgram[i, j + 1] = LinearProgram[i, j];
                            }
                            else
                            {
                                newLinearProgram[i, j] = LinearProgram[i, j];
                            }
                        }
                    }
                }

                SimplexSolver iterationSimplex = new SimplexSolver(newLinearProgram, ObjectiveType);
                List<double[,]> newTables = iterationSimplex.SolveDualSimplex();
                LinearProgram = newTables[newTables.Count - 1];
                skipRows.Add(false);
                allIterations.Add(newTables);

                // Check for any remaining fractional parts.
                bool integerSatisfied = true;
                for (int i = 1; i < LinearProgram.GetLength(0); i++)
                {
                    if (LinearProgram[i, LinearProgram.GetLength(1) - 1] % 1 != 0 && !skipRows[i])
                    {
                        for (int j = 0; j < VariableRestrictions.Length; j++)
                        {
                            if (LinearProgram[i, j] == 1)
                            {
                                integerSatisfied = false;
                            }
                        }
                    }
                }

                if (integerSatisfied)
                {
                    continueIterations = false;
                }
            }

            return allIterations;
        }

        public void SaveResultsToFile(string filePath)
        {
            string results = FormatResults();

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(results);
                }
                Console.WriteLine("Results have been successfully saved to " + filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving the results: " + ex.Message);
            }
        }

        // Format results into a string.
        public string FormatResults()
        {
            List<List<double[,]>> iterations = SolveCuttingPlane();
            int iterationCount = 0;
            StringBuilder resultOutput = new StringBuilder();

            foreach (var iteration in iterations)
            {
                iterationCount++;
                resultOutput.AppendLine($"OPTIMAL SOLUTION: {iterationCount}");
                foreach (var table in iteration)
                {
                    for (int i = 0; i < table.GetLength(0); i++)
                    {
                        for (int j = 0; j < table.GetLength(1); j++)
                        {
                            resultOutput.Append(Math.Round(table[i, j], 2)).Append("\t");
                        }
                        resultOutput.AppendLine();
                    }
                    resultOutput.AppendLine();
                }
                resultOutput.AppendLine();
            }

            return resultOutput.ToString();
        }
    }
}
