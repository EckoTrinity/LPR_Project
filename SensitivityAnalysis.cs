using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR_Project
{
    public static class SensitivityAnalysis
    {
        public static List<string> Perform(Model model, List<string> solution)
        {
            List<string> analysisResults = new List<string>();

            // Implement sensitivity analysis logic here
            analysisResults.Add("Sensitivity Analysis:");
            analysisResults.Add("Objective Coefficient Sensitivity: ...");
            analysisResults.Add("Right-hand-side Sensitivity: ...");
            // ...

            return analysisResults;
        }
    }
}
