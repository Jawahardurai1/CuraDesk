using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Model.Entities
{
    public class MedicalReport
    {

        public Guid MReportId { get; set; } = Guid.NewGuid();
        public Guid PatientUserId { get; set; }
        public string FileName { get; set; } = "";
        public string FilePath { get; set; } = "";
        public string DiagnosisTag { get; set; } = string.Empty;

        public DateTime ReportDate { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public User? Patient { get; set; }
        public Appointments? Appointment { get; set; }



    }
}
