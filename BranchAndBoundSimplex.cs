//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LPR_Project
//{
//    public static class BranchAndBoundSimplex
//    {
//        public static List<string> Solve(Model model)
//        {
//            var branches = new Queue<Model>();
//            branches.Enqueue(model);

//            List<string> bestSolution = null;
//            double bestObjective = double.NegativeInfinity;

//            while (branches.Count > 0)
//            {
//                var currentModel = branches.Dequeue();
//                var currentSolution = PrimalSimplex.Solve(currentModel);

//                if (!IsFeasible(currentSolution, currentModel))
//                {
//                    continue;
//                }

//                double currentObjective = ExtractObjective(currentSolution, currentModel);
//                if (IsIntegerSolution(currentSolution, currentModel) && currentObjective > bestObjective)
//                {
//                    bestObjective = currentObjective;
//                    bestSolution = currentSolution;

//                    // You may want to format the best solution for output here
//                }
//                else if (!IsIntegerSolution(currentSolution, currentModel))
//                {
//                    // Step 3: Branch on a fractional variable
//                    var (branch1, branch2) = Branch(currentModel, currentSolution);
//                    branches.Enqueue(branch1);
//                    branches.Enqueue(branch2);
//                }
//            }

//            // Format the result as needed for output
//            return bestSolution ?? new List<string> { "No feasible integer solution found." };
//        }

//        private static bool IsFeasible(List<string> solution, Model model)
//        {
//            var variableValues = ParseSolution(solution);

//            // Check each constraint
//            foreach (var constraint in model.Constraints)
//            {
//                double lhs = 0;
//                for (int i = 0; i < constraint.Coefficients.Length; i++)
//                {
//                    lhs += constraint.Coefficients[i] * variableValues[i];
//                }

//                // Compare lhs with rhs based on the constraint's operator
//                switch (constraint.Relation)
//                {
//                    case "<=":
//                        if (lhs > constraint.RHS) return false;
//                        break;
//                    case ">=":
//                        if (lhs < constraint.RHS) return false;
//                        break;
//                    case "=":
//                        if (lhs != constraint.RHS) return false;
//                        break;
//                }
//            }

//            return true;
//        }

//        private static double ExtractObjective(List<string> solution, Model model)
//        {
//            var variableValues = ParseSolution(solution);

//            // Calculate the objective function value
//            double objectiveValue = 0;
//            for (int i = 0; i < model.ObjectiveCoefficients.Length; i++)
//            {
//                objectiveValue += model.ObjectiveCoefficients[i] * variableValues[i];
//            }

//            return objectiveValue;
//        }

//        private static bool IsIntegerSolution(List<string> solution, Model model)
//        {
//            var variableValues = ParseSolution(solution);

//            // Check if all variables that should be integers are integers
//            for (int i = 0; i < model.SignRestrictions.Length; i++)
//            {
//                if (model.SignRestrictions[i] == "int" || model.SignRestrictions[i] == "bin")
//                {
//                    if (variableValues[i] != Math.Round(variableValues[i]))
//                    {
//                        return false; // Not an integer solution
//                    }
//                }
//            }

//            return true; // All required variables are integers
//        }

//        private static (Model, Model) Branch(Model model, List<string> solution)
//        {
//            var variableValues = ParseSolution(solution);

//            // Find the first fractional variable
//            int branchVariableIndex = -1;
//            for (int i = 0; i < variableValues.Length; i++)
//            {
//                if (model.SignRestrictions[i] == "int" || model.SignRestrictions[i] == "bin")
//                {
//                    if (variableValues[i] != Math.Round(variableValues[i]))
//                    {
//                        branchVariableIndex = i;
//                        break;
//                    }
//                }
//            }

//            if (branchVariableIndex == -1)
//            {
//                throw new InvalidOperationException("No fractional variable found to branch on.");
//            }

//            // Create two new models (subproblems) by adding constraints
//            var model1 = model.Clone();
//            var model2 = model.Clone();

//            // Add constraint: x_i <= floor(value)
//            var newConstraint1 = new Constraint
//            {
//                Coefficients = new double[model.ObjectiveCoefficients.Length],
//                Relation = "<=",
//                RHS = Math.Floor(variableValues[branchVariableIndex])
//            };
//            newConstraint1.Coefficients[branchVariableIndex] = 1;
//            model1.Constraints = model1.Constraints.Append(newConstraint1).ToArray();

//            // Add constraint: x_i >= ceil(value)
//            var newConstraint2 = new Constraint
//            {
//                Coefficients = new double[model.ObjectiveCoefficients.Length],
//                Relation = ">=",
//                RHS = Math.Ceiling(variableValues[branchVariableIndex])
//            };
//            newConstraint2.Coefficients[branchVariableIndex] = 1;
//            model2.Constraints = model2.Constraints.Append(newConstraint2).ToArray();

//            return (model1, model2);
//        }

//        private static double[] ParseSolution(List<string> solution)
//        {
//            // Initialize an array to hold variable values. Adjust size based on your needs.
//            var variableValues = new double[solution.Count];

//            for (int i = 0; i < solution.Count; i++)
//            {
//                // Assuming the format of each string is "x_i = value"
//                var parts = solution[i].Split('=');
//                if (parts.Length == 2)
//                {
//                    // Parse the value part and trim any whitespace
//                    if (double.TryParse(parts[1].Trim(), out double value))
//                    {
//                        variableValues[i] = value;
//                    }
//                    else
//                    {
//                        throw new FormatException($"Unable to parse value from solution string: {solution[i]}");
//                    }
//                }
//                else
//                {
//                    throw new FormatException($"Invalid format for solution string: {solution[i]}");
//                }
//            }

//            return variableValues;
//        }
//    }

//}
