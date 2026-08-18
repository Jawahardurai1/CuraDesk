using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace CuraDesk.Utility.Voice
{
    public class VoiceNotificationService : IVoiceNotificationService
    {
        private readonly TwilioSetting _settings;

        public VoiceNotificationService(
            IOptions<TwilioSetting> settings)
        {
            _settings = settings.Value;
        }

        public async Task<string> SendAppointmentConfirmationCallAsync(
            string patientPhoneNumber,
            string doctorName,
            string appointmentDate,
            string appointmentTime)
        {
            TwilioClient.Init(
                _settings.AccountSid,
                _settings.AuthToken);

            var message =
                $"Hello. Your CuraDesk appointment with {doctorName} " +
                $"has been confirmed for {appointmentDate} at {appointmentTime}. " +
                $"Thank you for using CuraDesk.";

            var call = await CallResource.CreateAsync(
                to: new PhoneNumber(patientPhoneNumber),
                from: new PhoneNumber(_settings.PhoneNumber),
                twiml: new Twilio.TwiML.VoiceResponse()
                    .Say(message)
                    .ToString()
            );

            return call.Sid;
        }
    }
}