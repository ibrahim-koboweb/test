using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace kobowebmvp_backend_dotnet.Models
{
    public class FieldAgent
    {
        public Guid FieldAgentID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string TownAddress { get; set; } = string.Empty;
        public string StateAddress { get; set; } = string.Empty;
        public string ImageUrl { get; set; }
        public string GovtIssuedId { get; set; }
        public string GuarantorFirstName { get; set; }
        public string GuarantorSecondName { get; set; }
        public string GuarantorLastName { get; set; }
        public string GuarantorPhone1 { get; set; }
        public string GuarantorPhone2 { get; set; }
        public string GuarantorEmail{ get; set; }
        public string GuarantorStreetAddress { get; set; }
        public string GuarantorTownAddress { get; set; }
        public string GuarantorStateAddress { get; set; }
        public string RegisteredBy { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public string Status { get; set; }

    }
}
