using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
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
            var response = new VoiceResponse();

            response.Say(
                "Hello. Your appointment with the doctor has been successfully booked. Thank you for using CuraDesk.",
                language: "en-US"
            );

            var call = await CallResource.CreateAsync(
                to: new PhoneNumber(patientPhoneNumber),
                from: new PhoneNumber(_settings.PhoneNumber),
                url: new Uri(
                    "https://webhooks.twilio.com/v1/Voice/Template/voice_text_to_speech")
            );

            return call.Sid;
        }
    }
}