True.Kentico.RazorEmails
-----------------------------------

Library can be used to grab Razor content from Kentico email template bodies and send the template
by model-binding to a custom model. 

Based on RazorEngine
https://github.com/Antaris/RazorEngine

Usage 
------------------------------------
The razor in the email template has to be html, with @Model.PropertyName replacements. 
If using @Html... then look up documentation. @Html.Raw should be replaced in the view with @Raw

If using Layouts, currently the code assumes that the layout is another template in the email templates 
application of Kentico. 

@model ...EmailData
@{
    Layout = "layout";
}

private readonly IEmailSender _emailSender;
 _emailSender.SendEmailWithModel<ComplianceReminderEmailData>(templateName, model);
 
 Default implementation of IEmailSender is RazorEmailSender