using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OncoAnalyzer.Models
{
    public class Biomarker
    {
        public string Name { get; set; } // Biomarker name (e.g., PSA, CA-125)
        public double Value { get; set; } // Test result value
        public DateTime TestDate { get; set; } // Date of the test
    }
}
