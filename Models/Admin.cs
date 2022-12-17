using System;

namespace kobowebmvp_backend_dotnet.Models
{
    public class Admin
    {
        public Guid AdminID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }


    }
}
