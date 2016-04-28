
using System;
using System.Collections.Generic;
using System.Linq;

namespace True.Kentico.RazorEmails
{
    public class BaseEmailData
    {
        private readonly List<string> _contactEmails;

        public BaseEmailData(string subject, string contactEmail)
        {
            _contactEmails = new List<string>();
            Subject = subject;
            AddRecipient(contactEmail); 
        }

        internal string ContactEmails
        {
            get { return _contactEmails.Aggregate("", (accumulator, piece) => accumulator + ";" + piece); }
        }

        public string Subject { get; internal set; }

        public void AddRecipient(string contactEmail)
        {
            if (!String.IsNullOrWhiteSpace(contactEmail))
                _contactEmails.Add(contactEmail);
        }
    }
}
