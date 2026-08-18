using Microsoft.AspNetCore.Http;

namespace CuraDesk.Model.DTOs
{
    public class UploadReportMetadataDto
    {
        public IFormFile File { get; set; } = null!;
        public Guid? AppointmentId { get; set; }
        public string DiagnosisTag { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; }
    }

    public class MedicalReportResponseDto
    {
        public Guid ReportId { get; set; }
        public Guid PatientUserId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string DiagnosisTag { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}