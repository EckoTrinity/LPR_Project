using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR_Project
{
    public static class Parser
    {
        public static Model ParseInput(string[] lines)
        {
            var model = new Model();

            // Parse the objective function
            var objectiveLine = lines[0].Split(' ');
            model.IsMaximization = objectiveLine[0].ToLower() == "max";
            model.ObjectiveCoefficients = objectiveLine.Skip(1).Select(double.Parse).ToArray();

            // Parse constraints
            var constraints = lines.Skip(1).Take(lines.Length - 2).ToArray();
            model.Constraints = constraints.Select(line => ParseConstraint(line)).ToArray();

            // Parse sign restrictions (optional)

            return model;
        }

        private static Constraint ParseConstraint(string line)
        {
            var parts = line.Split(' ');
            var coefficients = parts.Take(parts.Length - 2).Select(double.Parse).ToArray();
            var relation = parts[parts.Length - 2];
            var rhs = double.Parse(parts.Last());

            return new Constraint
            {
                Coefficients = coefficients,
                Relation = relation,
                RHS = rhs
            };
        }
    }
}
