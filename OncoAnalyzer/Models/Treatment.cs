
namespace OncoAnalyzer.Models
{
    public class Treatment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string TreatmentType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } //Nullable EndDate
        public string Response { get; set; }   //Nullable Response
    }
}
