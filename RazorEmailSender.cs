
using System;
using CMS.EmailEngine;
using CMS.MacroEngine;
using CMS.SiteProvider;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace True.Kentico.RazorEmails
{
    public class RazorEmailSender : IEmailSender
    {
        private readonly string _layoutTemplateName;
        private EmailTemplateInfo _layoutTemplate;
        private EmailTemplateInfo LayoutTemplate
        {
            get
            {
                if ((_layoutTemplate == null && !String.IsNullOrEmpty(_layoutTemplateName))
                    || _layoutTemplate!=null)
                {
                    var layoutTemplate = EmailTemplateProvider.GetEmailTemplate(_layoutTemplateName,
                        SiteContext.CurrentSiteID);
                    _layoutTemplate = layoutTemplate;
                }
                else _layoutTemplate = null;

                return _layoutTemplate;
            }
        }

        public RazorEmailSender()
        {
            
        }
        public RazorEmailSender(string layoutTemplateName)
        {
            _layoutTemplateName = layoutTemplateName;
        }

        public bool SendEmailWithModel<T>(string templateName, T viewModel) where T : BaseEmailData
        {
            if (String.IsNullOrEmpty(viewModel.ContactEmails))
                return false;

            var emailTemplate = EmailTemplateProvider.GetEmailTemplate(templateName, SiteContext.CurrentSiteID);

            EmailMessage email = new EmailMessage();
            email.EmailFormat = EmailFormatEnum.Both;
            email.From = emailTemplate.TemplateFrom;
            email.Recipients = viewModel.ContactEmails;
            emailTemplate.TemplateSubject = email.Subject = viewModel.Subject;

            var emailBody = BindEmailBody(emailTemplate, viewModel);
            if (String.IsNullOrEmpty(emailBody))
                return false;
            email.Body = emailBody;

            EmailSender.SendEmailWithTemplateText(SiteContext.CurrentSiteName, email, emailTemplate, MacroContext.CurrentResolver, true);
            return true;
        }

        private string BindEmailBody<T>(EmailTemplateInfo emailTemplate, T viewModel)
        {
        
            var service = Engine.Razor;

            if (!String.IsNullOrEmpty(_layoutTemplateName))
                service.AddTemplate("layout", LayoutTemplate.TemplateText);
            service.AddTemplate("template", emailTemplate.TemplateText);
            service.Compile("template");
            var emailHtmlBody = service.Run("template", typeof(T), viewModel);

            return emailHtmlBody;
        }
    }
}
