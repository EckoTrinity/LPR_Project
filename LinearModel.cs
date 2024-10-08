﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    class LinearModel
    {
        public string ProblemType { get; set; }
        public string DualProblemType { get; set; }
        public List<string> SignRes { get; set; }
        public List<string> DualSignRes { get; set; }
        public Dictionary<string, double> ObjectiveFunction { get; set; }
        public Dictionary<string, double> WFunction { get; set; }
        public Dictionary<string, double> DualityFunction { get; set; }
        public List<Dictionary<string, string>> DualityConstraints { get; set; }
        public List<Dictionary<string, string>> ConstraintsSimplex { get; set; }
        public List<Dictionary<string, string>> ConstraintsTwoPhase { get; set; }
        public List<int> TwoPhaseArtificialColumns { get; set; }
        public List<double[,]> SimplexTables { get; set; }
        public List<Dictionary<int, int>> SimplexTablesPivots { get; set; }
        public List<double[,]> TwoPhaseTables { get; set; }
        public List<Dictionary<int, int>> TwoPhaseTablesPivots { get; set; }


        public double[,] SimplexInitial { get; set; }
        public double[,] TwoPhaseInitial { get; set; }
        public double[,] DualityInitial { get; set; }

        public LinearModel(string[] lines)
        {
            this.ObjectiveFunction = new Dictionary<string, double>();
            this.WFunction = new Dictionary<string, double>();
            this.DualityFunction = new Dictionary<string, double>();

            this.DualityConstraints = new List<Dictionary<string, string>>();
            this.ConstraintsSimplex = new List<Dictionary<string, string>>();
            this.ConstraintsTwoPhase = new List<Dictionary<string, string>>();
            this.TwoPhaseArtificialColumns = new List<int>();


            this.SimplexTables = new List<double[,]>();
            this.SimplexTablesPivots = new List<Dictionary<int, int>>();

            this.TwoPhaseTables = new List<double[,]>();
            this.TwoPhaseTablesPivots = new List<Dictionary<int, int>>();


            this.SignRes = new List<string>();
            this.DualSignRes = new List<string>();

            List<string> model = new List<string>(lines);

            List<string> objFunc = new List<string>(model[0].Split(" "));

            model.RemoveAt(0);

            this.ProblemType = objFunc[0];
            objFunc.RemoveAt(0);

            this.DualProblemType = this.ProblemType == "max" ? "min" : "max";


            for (int i = 1; i <= objFunc.Count; i++)
            {
                this.ObjectiveFunction.Add($"X{i}", double.Parse(objFunc[i - 1]));

                Dictionary<string, string> newEntry = new Dictionary<string, string>
                {
                    { "rhs", objFunc[i - 1] }
                };
                this.DualityConstraints.Add(newEntry);
            }

            int decVars = this.ObjectiveFunction.Count;

            List<string> signRes = new List<string>(model[model.Count - 1].Split(" "));
            this.SignRes = signRes;
            model.RemoveAt(model.Count - 1);

            for (int i = 0; i < signRes.Count; i++)
            {
                switch (signRes[i])
                {
                    case "+":
                        {
                            if (this.ProblemType == "max")
                            {
                                this.DualityConstraints[i].Add("sign", ">=");
                            }
                            else
                            {
                                this.DualityConstraints[i].Add("sign", "<=");
                            }
                        }
                        break;
                    case "-":
                        {
                            if (this.ProblemType == "max")
                            {
                                this.DualityConstraints[i].Add("sign", ">=");
                            }
                            else
                            {
                                this.DualityConstraints[i].Add("sign", "<=");
                            }
                        }
                        break;
                    case "urs":
                        {
                            this.DualityConstraints[i].Add("sign", "=");
                        }

                        break;
                    default:
                        {
                            if (this.ProblemType == "max")
                            {
                                this.DualityConstraints[i].Add("sign", ">=");
                            }
                            else
                            {
                                this.DualityConstraints[i].Add("sign", "<=");
                            }
                        }
                        break;
                }
            }

            int coeffCounter = 1;
            foreach (var constraint in model)
            {
                Dictionary<string, string> conRow = new Dictionary<string, string>();
                List<string> con = new List<string>(constraint.Split(" "));

                for (int i = 1; i <= objFunc.Count; i++)
                {
                    conRow.Add($"X{i}", con[i - 1]);
                    this.DualityConstraints[i - 1].Add($"Y{this.DualityConstraints[i - 1].Count - 1}", con[i - 1]);
                }

                conRow.Add("sign", con[con.Count - 2]);

                switch (con[con.Count - 2])
                {
                    case ">=":
                        {
                            if (this.ProblemType == "max")
                            {
                                this.DualSignRes.Add("-");
                            }
                            else
                            {
                                this.DualSignRes.Add("+");
                            }
                        }
                        break;
                    case "<=":
                        {
                            if (this.ProblemType == "max")
                            {
                                this.DualSignRes.Add("+");
                            }
                            else
                            {
                                this.DualSignRes.Add("-");
                            }
                        }
                        break;
                    case "=":
                        {
                            this.DualSignRes.Add("urs");
                        }
                        break;
                }

                conRow.Add("rhs", con[con.Count - 1]);
                this.DualityFunction.Add($"Y{coeffCounter}", double.Parse(con[con.Count - 1]));
                coeffCounter++;

                this.ConstraintsSimplex.Add(new Dictionary<string, string>(conRow));
                this.ConstraintsTwoPhase.Add(new Dictionary<string, string>(conRow));
            }

            for (int i = 0; i < signRes.Count; i++)
            {
                string key = $"X{i + 1}";
                switch (signRes[i])
                {
                    case "-":
                        {
                            this.ObjectiveFunction[key] *= -1;
                            foreach (var constraint in this.ConstraintsSimplex)
                            {
                                constraint[key] = (double.Parse(constraint[key]) * -1).ToString();
                            }

                            foreach (var con in this.ConstraintsTwoPhase)
                            {
                                con[key] = (double.Parse(con[key]) * -1).ToString();
                            }
                        }
                        break;

                    case "urs":
                        {
                            decVars++;
                            this.ObjectiveFunction.Add($"{key}-", this.ObjectiveFunction[key] * -1);
                            foreach (var constraint in this.ConstraintsSimplex)
                            {
                                constraint.Add($"{key}-", (double.Parse(constraint[key]) * -1).ToString());
                            }

                            foreach (var con in this.ConstraintsTwoPhase)
                            {
                                con.Add($"{key}-", (double.Parse(con[key]) * -1).ToString());
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            for (int i = 0; i < this.DualSignRes.Count; i++)
            {
                string xKey = $"X{i + 1}";
                string yKey = $"Y{i + 1}";
                switch (this.DualSignRes[i])
                {
                    case "-":
                        {
                            foreach (var con in this.DualityConstraints)
                            {
                                con[yKey] = (double.Parse(con[yKey]) * -1).ToString();
                            }
                        }
                        break;
                    case "urs":
                        {
                            this.DualityFunction.Add($"{yKey}-", this.DualityFunction[yKey] * -1);

                            foreach (var con in this.DualityConstraints)
                            {
                                con.Add($"{yKey}-", (double.Parse(con[yKey]) * -1).ToString());
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            List<Dictionary<string, string>> tempConstraints = new List<Dictionary<string, string>>();

            foreach (var constraint in this.ConstraintsSimplex)
            {
                switch (constraint["sign"])
                {
                    case "<=":
                        {
                            constraint.Add("S", "+1");
                        }
                        break;

                    case ">=":
                        {
                            constraint.Add("E", "+1");
                            foreach (var kvp in constraint.Where(x => x.Key.Contains('X') || x.Key.Contains("rhs")))
                            {
                                constraint[kvp.Key] = (double.Parse(constraint[kvp.Key]) * -1).ToString();
                            }
                        }
                        break;

                    case "=":
                        {
                            Dictionary<string, string> temp = new Dictionary<string, string>(constraint);
                            constraint.Add("S", "+1");

                            foreach (var kvp in temp.Where(x => x.Key.Contains('X') || x.Key.Contains("rhs")))
                            {
                                temp[kvp.Key] = (double.Parse(temp[kvp.Key]) * -1).ToString();
                            }

                            temp.Add("E", "+1");

                            tempConstraints.Add(temp);
                        }
                        break;
                }
            }

            foreach (var tempConstraint in tempConstraints)
            {
                this.ConstraintsSimplex.Add(tempConstraint);
            }

            tempConstraints.Clear();
            foreach (var constraint in this.DualityConstraints)
            {
                switch (constraint["sign"])
                {
                    case "<=":
                        {
                            constraint.Add("S", "+1");
                        }
                        break;

                    case ">=":
                        {
                            constraint.Add("E", "+1");
                            foreach (var kvp in constraint.Where(x => x.Key.Contains('Y') || x.Key.Contains("rhs")))
                            {
                                constraint[kvp.Key] = (double.Parse(constraint[kvp.Key]) * -1).ToString();
                            }
                        }
                        break;

                    case "=":
                        {
                            Dictionary<string, string> temp = new Dictionary<string, string>(constraint);
                            constraint.Add("S", "+1");

                            foreach (var kvp in temp.Where(x => x.Key.Contains('Y') || x.Key.Contains("rhs")))
                            {
                                temp[kvp.Key] = (double.Parse(temp[kvp.Key]) * -1).ToString();
                            }

                            temp.Add("E", "+1");

                            tempConstraints.Add(temp);
                        }
                        break;

                }
            }

            foreach (var tempConstraint in tempConstraints)
            {
                this.DualityConstraints.Add(tempConstraint);
            }


            int numGreaterThan = 0;
            foreach (var constraint in this.ConstraintsTwoPhase)
            {
                switch (constraint["sign"])
                {
                    case "<=":
                        {
                            constraint.Add("S", "+1");
                        }
                        break;

                    case ">=":
                        {
                            numGreaterThan += 2;
                            constraint.Add("E", "-1");
                            constraint.Add("A", "+1");
                        }
                        break;
                    case "=":
                        {
                            constraint.Add("A", "+1");
                        }
                        break;
                }
            }


            int counter = 1;
            foreach (var constraint in this.ConstraintsTwoPhase)
            {
                counter++;
                if (constraint.ContainsKey("A"))
                {
                    foreach (var kvp in constraint.Where(x => !x.Value.Contains('=') && x.Key != "A"))
                    {
                        string key = kvp.Key;
                        if (key == "E")
                        {
                            key = "E" + counter;
                        }

                        if (this.WFunction.ContainsKey(key))
                        {
                            if (key == "rhs")
                            {
                                this.WFunction[key] += double.Parse(kvp.Value) * 1;
                            }
                            else
                            {
                                this.WFunction[key] += double.Parse(kvp.Value) * -1;
                            }

                        }
                        else
                        {
                            if (key == "rhs")
                            {
                                this.WFunction.Add(key, double.Parse(kvp.Value) * 1);
                            }
                            else
                            {
                                this.WFunction.Add(key, double.Parse(kvp.Value) * -1);
                            }

                        }
                    }
                }

            }


            int dualCount = 0;
            this.DualityInitial = new double[decVars + 1, decVars + this.DualityConstraints.Count + 1];
            foreach (var kvp in this.DualityFunction)
            {
                this.DualityInitial[0, dualCount] = kvp.Value * -1;
                dualCount++;
            }

            int count = 0;
            this.SimplexInitial = new double[this.ConstraintsSimplex.Count + 1, decVars + this.ConstraintsSimplex.Count + 1];
            if (numGreaterThan >= 1)
            {
                this.TwoPhaseInitial = new double[this.ConstraintsTwoPhase.Count + 2, decVars + this.ConstraintsTwoPhase.Count + numGreaterThan];
            }
            else
            {
                this.TwoPhaseInitial = new double[this.ConstraintsTwoPhase.Count + 2, decVars + this.ConstraintsTwoPhase.Count + numGreaterThan + 1];
            }


            foreach (var kvp in this.ObjectiveFunction)
            {
                this.SimplexInitial[0, count] = kvp.Value * -1;
                this.TwoPhaseInitial[1, count] = kvp.Value * -1;
                count++;
            }

            if (this.WFunction.Count != 0)
            {

                count = 0;
                foreach (var kvp in this.WFunction.Where(x => x.Key.Contains('X')))
                {
                    this.TwoPhaseInitial[0, count] = kvp.Value * -1;
                    count++;
                }
                foreach (var kvp in this.WFunction.Where(x => x.Key.Contains('E')))
                {

                    int value = int.Parse(kvp.Key[1].ToString());
                    Console.WriteLine(value);

                    this.TwoPhaseInitial[0, count + value - 2] = kvp.Value * -1;

                }

                for (int i = 0; i < this.ConstraintsTwoPhase.Count; i++)
                {
                    if (this.ConstraintsTwoPhase[i].ContainsKey("A"))
                    {
                        this.TwoPhaseArtificialColumns.Add(decVars + numGreaterThan - 1 + i);
                    }
                }
                this.TwoPhaseInitial[0, this.TwoPhaseInitial.GetLength(1) - 1] = this.WFunction["rhs"];
            }

            int row = 1;
            foreach (var con in this.ConstraintsSimplex)
            {
                count = 0;
                foreach (var kvp in con.Where(x => x.Key.Contains('X')))
                {
                    this.SimplexInitial[row, count] = double.Parse(kvp.Value);
                    count++;
                }
                if (con.ContainsKey("S"))
                {
                    this.SimplexInitial[row, count + row - 1] = double.Parse(con["S"]);
                }
                else if (con.ContainsKey("E"))
                {
                    this.SimplexInitial[row, count + row - 1] = double.Parse(con["E"]);
                }
                this.SimplexInitial[row, this.SimplexInitial.GetLength(1) - 1] = double.Parse(con["rhs"]);
                row++;
            }

            row = 1;
            foreach (var con in this.DualityConstraints)
            {
                count = 0;
                foreach (var kvp in con.Where(x => x.Key.Contains('Y')))
                {
                    this.DualityInitial[row, count] = double.Parse(kvp.Value);
                    count++;
                }
                if (con.ContainsKey("S"))
                {
                    this.DualityInitial[row, count + row - 1] = double.Parse(con["S"]);
                }
                else if (con.ContainsKey("E"))
                {
                    this.DualityInitial[row, count + row - 1] = double.Parse(con["E"]);
                }
                this.DualityInitial[row, this.DualityInitial.GetLength(1) - 1] = double.Parse(con["rhs"]);
                row++;
            }

            row = 2;
            int constraintNumber = 1;
            foreach (var con in this.ConstraintsTwoPhase)
            {
                count = 0;
                foreach (var kvp in con.Where(x => x.Key.Contains('X')))
                {
                    this.TwoPhaseInitial[row, count] = double.Parse(kvp.Value);
                    count++;
                }

                if (con.ContainsKey("S"))
                {
                    if (constraintNumber == 1 && numGreaterThan >= 1)
                    {
                        this.TwoPhaseInitial[row, count - 1 + constraintNumber] = double.Parse(con["S"]);
                    }
                    else if (numGreaterThan <= 0)
                    {
                        this.TwoPhaseInitial[row, count + row - 2] = double.Parse(con["S"]);
                    }
                    else
                    {
                        this.TwoPhaseInitial[row, count + constraintNumber] = double.Parse(con["S"]);
                    }

                }

                if (con.ContainsKey("E"))
                {
                    if (con.ContainsKey("A"))
                    {
                        this.TwoPhaseInitial[row, count - 1 + constraintNumber] = double.Parse(con["E"]);
                        count++;
                        this.TwoPhaseInitial[row, count - 1 + constraintNumber] = double.Parse(con["A"]);

                    }
                }

                if (con.ContainsKey("A"))
                {
                    if (!con.ContainsKey("E"))
                    {
                        if (constraintNumber == row - 2)
                        {
                            this.TwoPhaseInitial[row, count - 1 + constraintNumber] = double.Parse(con["A"]);
                        }
                        else
                        {
                            this.TwoPhaseInitial[row, count + constraintNumber] = double.Parse(con["A"]);
                        }
                    }

                }
                this.TwoPhaseInitial[row, this.TwoPhaseInitial.GetLength(1) - 1] = double.Parse(con["rhs"]);
                row++;
                constraintNumber++;
            }
        }

        private double[,] PivotTable(double[,] initialTableArray, int pivotColumnIndex, int pivotRowIndex)
        {
            double[,] outTableArray = new double[initialTableArray.GetLength(0), initialTableArray.GetLength(1)];

            for (int i = 0; i < initialTableArray.GetLength(0); i++)
            {
                for (int j = 0; j < initialTableArray.GetLength(1); j++)
                {
                    double value = initialTableArray[i, j] - (initialTableArray[i, pivotColumnIndex] * (initialTableArray[pivotRowIndex, j] / initialTableArray[pivotRowIndex, pivotColumnIndex]));
                    if (value.ToString().Contains(",99999"))
                    {
                        value = Math.Ceiling(value);
                    }
                    else if (value.ToString().Contains(",00000"))
                    {
                        value = Math.Floor(value);
                    }
                    else
                    {
                        value = Math.Round(value, 15);
                    }

                    outTableArray[i, j] = value;
                }
            }

            for (int i = 0; i < initialTableArray.GetLength(0); i++)
            {
                for (int j = 0; j < initialTableArray.GetLength(1); j++)
                {
                    double value = initialTableArray[pivotRowIndex, j] / initialTableArray[pivotRowIndex, pivotColumnIndex];
                    if (value.ToString().Contains(",99999"))
                    {
                        value = Math.Ceiling(value);
                    }
                    else if (value.ToString().Contains(",00000"))
                    {
                        value = Math.Floor(value);
                    }
                    else
                    {
                        value = Math.Round(value, 15);
                    }

                    outTableArray[pivotRowIndex, j] = value;
                }
            }

            return outTableArray;
        }

        public string ObjFunctionToString()
        {
            List<string> objectiveTerms = new List<string>();


            foreach (var kvp in this.ObjectiveFunction)
            {
                string sign = (kvp.Value < 0) ? " - " : " + ";
                double absValue = Math.Abs(kvp.Value);
                string term = $"{sign}{absValue}{kvp.Key}";
                objectiveTerms.Add(term);
            }

            return $"{this.ProblemType} Z =" + string.Join("", objectiveTerms);
        }

        public string CanonObjFunctionToString()
        {
            List<string> objectiveTerms = new List<string>();


            foreach (var kvp in this.ObjectiveFunction)
            {
                string sign = (kvp.Value * -1 < 0) ? " - " : " + ";
                double absValue = Math.Abs(kvp.Value);
                string term = $"{sign}{absValue}{kvp.Key}";
                objectiveTerms.Add(term);
            }


            return $"{this.ProblemType.ToUpper()} Z" + string.Join("", objectiveTerms) + " = 0";
        }

        public string CanonDualFunctionToString()
        {
            List<string> objectiveTerms = new List<string>();


            foreach (var kvp in this.DualityFunction)
            {
                string sign = (kvp.Value * -1 < 0) ? " - " : " + ";
                double absValue = Math.Abs(kvp.Value);
                string term = $"{sign}{absValue}{kvp.Key}";
                objectiveTerms.Add(term);
            }


            return $"{this.DualProblemType.ToUpper()} W" + string.Join("", objectiveTerms) + " = 0";
        }


        public string WFunctionToString()
        {
            List<string> objectiveTerms = new List<string>();

            foreach (var kvp in this.WFunction.Where(x => !x.Key.Contains("rhs") && !x.Key.Contains('=')))
            {
                string sign = (kvp.Value * -1 < 0) ? " - " : " + ";
                double absValue = Math.Abs(kvp.Value);
                string term = $"{sign}{absValue}{kvp.Key}";
                objectiveTerms.Add(term);
            }


            if (this.WFunction.Count > 0)
            {
                return $"W" + string.Join("", objectiveTerms) + $" = {this.WFunction["rhs"]}";
            }
            else
            {
                return "No Two Phase Possible: W - 0";
            }

        }


        public string CanonSimplexConstraintsToString()
        {
            List<string> objectiveTerms = new List<string>();


            foreach (var con in this.ConstraintsSimplex)
            {
                string term = "";
                foreach (var kvp in con.Where(x => x.Key.Contains('X')))
                {
                    string sign = (double.Parse(kvp.Value) < 0) ? " - " : " + ";
                    double absValue = Math.Abs(double.Parse(kvp.Value));

                    term += $"{sign}{absValue}{kvp.Key}";

                }

                foreach (var kvp in con.Where(x => x.Key.Contains('S') || x.Key.Contains('E')))
                {
                    string sign = (double.Parse(kvp.Value) < 0) ? " - " : " + ";
                    double absValue = Math.Abs(double.Parse(kvp.Value));

                    term += $" {sign}{absValue}{kvp.Key}";

                }

                term += $" = {con["rhs"]}";
                objectiveTerms.Add(term);

            }
            return string.Join("\n", objectiveTerms);
        }
        public string CanonDualConstraintsToString()
        {
            List<string> objectiveTerms = new List<string>();


            foreach (var con in this.DualityConstraints)
            {
                string term = "";
                foreach (var kvp in con.Where(x => x.Key.Contains('Y')))
                {
                    string sign = (double.Parse(kvp.Value) < 0) ? " - " : " + ";
                    double absValue = Math.Abs(double.Parse(kvp.Value));

                    term += $"{sign}{absValue}{kvp.Key}";

                }

                foreach (var kvp in con.Where(x => x.Key.Contains('S') || x.Key.Contains('E')))
                {
                    string sign = (double.Parse(kvp.Value) < 0) ? " - " : " + ";
                    double absValue = Math.Abs(double.Parse(kvp.Value));

                    term += $" {sign}{absValue}{kvp.Key}";

                }

                term += $" = {con["rhs"]}";
                objectiveTerms.Add(term);

            }
            return string.Join("\n", objectiveTerms);
        }

        public string CanonTwoPhaseConstraintsToString()
        {
            List<string> objectiveTerms = new List<string>();


            foreach (var con in this.ConstraintsTwoPhase)
            {
                string term = "";
                foreach (var kvp in con.Where(x => x.Key.Contains('X')))
                {
                    string sign = (double.Parse(kvp.Value) < 0) ? " - " : " + ";
                    double absValue = Math.Abs(double.Parse(kvp.Value));

                    term += $"{sign}{absValue}{kvp.Key}";

                }

                foreach (var kvp in con.Where(x => x.Key.Contains('S') || x.Key.Contains('E') || x.Key.Contains('A')))
                {
                    string sign = (double.Parse(kvp.Value) < 0) ? " - " : " + ";
                    double absValue = Math.Abs(double.Parse(kvp.Value));

                    term += $" {sign}{absValue}{kvp.Key}";

                }

                term += $" = {con["rhs"]}";
                objectiveTerms.Add(term);

            }
            return string.Join("\n", objectiveTerms);
        }
    }
}

