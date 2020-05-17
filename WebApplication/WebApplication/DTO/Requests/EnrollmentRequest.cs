using System;

namespace WebApplication.DTO.Requests
{
    public class EnrollmentRequest
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Studies { get; set; }

        public DateTime ExtractBirthDate()
        {
            var dateInput = BirthDate.Split(".");
            return new DateTime(Int16.Parse(dateInput[2]), Int16.Parse(dateInput[1]), Int16.Parse(dateInput[0]));
        }

        public bool CorrectRequest()
        {
            var correct = true;
            foreach (var prop in typeof(EnrollmentRequest).GetProperties())
            {
                if (!string.IsNullOrEmpty(prop.GetValue(this, null)?.ToString())) continue;
                correct = false;
            }
            return correct;
        }
    }
}