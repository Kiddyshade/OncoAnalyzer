using CsvHelper.Configuration;
using OncoAnalyzer.Models;

namespace OncoAnalyzer.Services
{
    public sealed class PatientCsvMap:ClassMap<Patient>
    {
        public PatientCsvMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Age).Name("Age");
            Map(m => m.Diagnosis).Name("Diagnosis");
        }
           
    }
}
