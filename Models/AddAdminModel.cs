﻿namespace kobowebmvp_backend_dotnet.Models
{
    public class AddAdminModel
    {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; }
            public IFormFile? ProfilePicture { get; set; }


    }
  
}
