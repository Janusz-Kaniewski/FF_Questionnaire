namespace FF_Questionnaire.Services.Interface
{
    public interface IEmailService
    {
        void SendEmail(string subject, string email);
    }
}
