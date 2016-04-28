namespace True.Kentico.RazorEmails
{
    public interface IEmailSender
    {
        bool SendEmailWithModel<T>(string templateName, T viewModel) where T : BaseEmailData;
    }
}