using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OncoAnalyzer.Models
{
    public class Patient
    {
        public int Id { get; set; } // Unique Patient ID
        public string Name { get; set; } // Patient Name
        public int Age { get; set; } //Patient Age
        public string Diagnosis { get; set; } // Cancer Type Diagnosis
    }
}
