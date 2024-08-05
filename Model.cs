using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR_Project
{
    public class Model
    {
        public bool IsMaximization { get; set; }
        public double[] ObjectiveCoefficients { get; set; }
        public Constraint[] Constraints { get; set; }
    }

    public class Constraint
    {
        public double[] Coefficients { get; set; }
        public string Relation { get; set; }
        public double RHS { get; set; }
    }
}
