namespace kobowebmvp_backend_dotnet.Models
{
    public class AddFieldAgentModel
    {
            public string? FirstName { get; set; }
            public string? SecondName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; }
            public string? Phone1 { get; set; }
            public string? Phone2 { get; set; } = string.Empty;
            public string? StreetAddress { get; set; } = string.Empty;
            public string? TownAddress { get; set; } = string.Empty;
            public string? StateAddress { get; set; } = string.Empty;
            public IFormFile? ProfilePicture { get; set; }
            public IFormFile? GovtIssuedIdentity { get; set; }
            public string? GuarantorFirstName { get; set; }
            public string? GuarantorSecondName { get; set; }
            public string? GuarantorLastName { get; set; }
            public string? GuarantorPhone1 { get; set; }
            public string? GuarantorPhone2 { get; set; }
            public string? GuarantorEmail { get; set; }
            public string? GuarantorStreetAddress { get; set; }
            public string? GuarantorTownAddress { get; set; }
            public string? GuarantorStateAddress { get; set; }
     }
  
}
