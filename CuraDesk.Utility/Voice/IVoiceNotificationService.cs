namespace CuraDesk.Utility.Voice
{
    public interface IVoiceNotificationService
    {
        Task<string> SendAppointmentConfirmationCallAsync(
            string patientPhoneNumber,
            string doctorName,
            string appointmentDate,
            string appointmentTime);
    }
}