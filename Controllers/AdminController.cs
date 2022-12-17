using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kobowebmvp_backend_dotnet.Models;
using kobowebmvp_backend_dotnet.Controllers;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using NuGet.Packaging.Signing;

namespace kobowebmvp_backend_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminControler : ControllerBase 
    {
        private readonly KoboWebDbContext _context;
        private IConfiguration _configuration;
        private Authentication authentication;
        public static IWebHostEnvironment? _environment;
      

        public AdminControler(KoboWebDbContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        { 
            return await _context.Admins.ToListAsync();
        }

        // GET:
        [HttpGet("{AdminID}")]
        public async Task<ActionResult<Admin>> GetAdmin(Guid AdminID)
        {
            var Admin = await _context.Admins.FindAsync( AdminID);

            if (Admin == null)
            {
                return NotFound();
            }

            return Admin;
        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{AdminID}")]
        public async Task<IActionResult> UpdateAdmin(Guid AdminID, [FromForm] AddAdminModel addAdmin)
        {
            var Admin = await _context.Admins.FindAsync(AdminID);
            if (Admin == null)
            {
                return NotFound();
            }


   


            if (addAdmin.ProfilePicture.Length > 0)
            {
                FileInfo pfileInfo = new FileInfo(addAdmin.ProfilePicture.FileName);

                if (!Directory.Exists(_environment.WebRootPath + "\\upload"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\upload\\");
                }
                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\upload\\pic_" + AdminID.ToString() + pfileInfo.Extension.ToString()))
                {
                    addAdmin.ProfilePicture.CopyTo(filestream);
                    filestream.Flush();
                    //  return "\\Upload\\" + objFile.files.FileName;
                }
            }


                Admin.FirstName = addAdmin.FirstName;
                Admin.LastName = addAdmin.LastName;
                Admin.Email = addAdmin.Email;
                Admin.ImageUrl = "\\upload\\pic_" + AdminID.ToString();
               
                _context.Entry(Admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(AdminID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(Admin);
        }


        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async  Task<ActionResult<AddAdminModel>> Register([FromForm] AddAdminModel addAdmin)
        {

            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Email == addAdmin.Email);
            if (admin != null)
            {
                return BadRequest("Email already exist.");
            }

            Guid dAdminID = Guid.NewGuid();


            FileInfo pfileInfo = new FileInfo(addAdmin.ProfilePicture.FileName);
            

            if (addAdmin.ProfilePicture.Length > 0)
            {
                if (!Directory.Exists(_environment.WebRootPath + "\\upload"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\upload\\");
                }
                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\upload\\pic_" + dAdminID.ToString() + pfileInfo.Extension.ToString()))
                {
                    addAdmin.ProfilePicture.CopyTo(filestream);
                    filestream.Flush();
                    //  return "\\Upload\\" + objFile.files.FileName;
                }
            }

        

            var Admin = new Admin()
            {
                AdminID = dAdminID,
                FirstName = addAdmin.FirstName,
                LastName = addAdmin.LastName,
                Email = addAdmin.Email,
                ImageUrl =   "\\upload\\pic_" + dAdminID.ToString(),
            };

            var user = new User() ;
            user.UserID = Admin.AdminID;
            user.FirstName = Admin.FirstName;
            user.LastName = Admin.LastName;
            user.Email = Admin.Email;
            user.Role = "Super Admin";
            user.UserType = "Admin";
            user.Verified = false;
            authentication = new Authentication(addAdmin.Email, addAdmin.Password,_configuration);
            user.PasswordHash = authentication.passwordHash;
            user.PasswordSalt = authentication.passwordSalt;
            user.TokenCreated = authentication.TokenCreated;
            user.TokenExpires = authentication.TokenExpires;
            _context.Users.Add(user);

            _context.Admins.Add(Admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdmin", new { AdminID = Admin.AdminID }, Admin);
        }

        


        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(Guid AdminID)
        {
            var Admin = await _context.Admins.FindAsync(AdminID);
            if (Admin == null)
            {
                return NotFound();
            }

            _context.Admins.Remove(Admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Login login)
        {

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == login.Email);

            if (user == null)
            {
                return BadRequest("User not found.");
            }
            authentication = new Authentication(login.Email, login.Password, _configuration);

            if (!authentication.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }
            string token = authentication.CreateToken(user);

            return Ok(token);


        }


        private bool AdminExists(Guid AdminID)
        {
            return _context.Admins.Any(e => e.AdminID == AdminID);
        }

    }
}
