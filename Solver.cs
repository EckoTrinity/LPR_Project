using Google.OrTools.LinearSolver;
using LPR_Project;
using System.Collections.Generic;

namespace LPR_Project
{
    public static class Solver
    {
        public static List<string> Solve(Model model)
        {
            List<string> result = new List<string>();

            var solver = Google.OrTools.LinearSolver.Solver.CreateSolver("GLOP");

            // Define variables
            List<Variable> variables = new List<Variable>();
            for (int i = 0; i < model.ObjectiveCoefficients.Length; i++)
            {
                Variable var = solver.MakeNumVar(0.0, double.PositiveInfinity, $"x{i}");
                variables.Add(var);
            }

            // Define constraints
            foreach (var constraint in model.Constraints)
            {
                LinearExpr expr = solver.MakeNumVar(0.0, 0.0, "dummy"); // Initialize with a dummy variable
                for (int i = 0; i < constraint.Coefficients.Length; i++)
                {
                    expr += constraint.Coefficients[i] * variables[i];
                }

                switch (constraint.Relation)
                {
                    case "<=":
                        solver.Add(expr <= constraint.RHS);
                        break;
                    case "=":
                        solver.Add(expr == constraint.RHS);
                        break;
                    case ">=":
                        solver.Add(expr >= constraint.RHS);
                        break;
                }
            }

            // Define objective
            Objective objective = solver.Objective();
            for (int i = 0; i < model.ObjectiveCoefficients.Length; i++)
            {
                objective.SetCoefficient(variables[i], model.ObjectiveCoefficients[i]);
            }
            if (model.IsMaximization)
            {
                objective.SetMaximization();
            }
            else
            {
                objective.SetMinimization();
            }

            // Solve the model
            Google.OrTools.LinearSolver.Solver.ResultStatus resultStatus = solver.Solve();

            if (resultStatus == Google.OrTools.LinearSolver.Solver.ResultStatus.OPTIMAL)
            {
                result.Add("Solution:");
                for (int i = 0; i < variables.Count; i++)
                {
                    result.Add($"{variables[i].Name()} = {variables[i].SolutionValue()}");
                }
                result.Add($"Optimal objective value = {solver.Objective().Value()}");
            }
            else
            {
                result.Add("No optimal solution found.");
            }

            return result;
        }
    }
}
