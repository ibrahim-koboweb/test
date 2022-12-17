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

namespace kobowebmvp_backend_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldAgentsController : ControllerBase 
    {
        private readonly KoboWebDbContext _context;
        private IConfiguration _configuration;
        private Authentication authentication;
        public static IWebHostEnvironment? _environment;
      

        public FieldAgentsController(KoboWebDbContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
        }

        // GET: api/FieldAgents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FieldAgent>>> GetFieldAgents()
        { 
            return await _context.FieldAgents.ToListAsync();
        }

        // GET:
        [HttpGet("{FieldAgentID}")]
        public async Task<ActionResult<FieldAgent>> GetFieldAgent(Guid FieldAgentID)
        {
            var fieldAgent = await _context.FieldAgents.FindAsync( FieldAgentID);

            if (fieldAgent == null)
            {
                return NotFound();
            }

            return fieldAgent;
        }

        // PUT: api/FieldAgents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{FieldAgentID}")]
        public async Task<IActionResult> UpdateFieldAgent(Guid FieldAgentID, [FromForm] AddFieldAgentModel addfieldAgent)
        {
            var fieldAgent = await _context.FieldAgents.FindAsync(FieldAgentID);
            if (fieldAgent == null)
            {
                return NotFound();
            }


            if (addfieldAgent.ProfilePicture.Length > 0)
            {

                FileInfo pfileInfo = new FileInfo(addfieldAgent.ProfilePicture.FileName);


                if (!Directory.Exists(_environment.WebRootPath + "\\upload"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\upload\\");
                }
                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\upload\\pic_" + FieldAgentID.ToString() + pfileInfo.Extension.ToString()))
                {
                    addfieldAgent.ProfilePicture.CopyTo(filestream);
                    filestream.Flush();
                    //  return "\\Upload\\" + objFile.files.FileName;
                }
            }

            if (addfieldAgent.GovtIssuedIdentity.Length > 0)
            {

                FileInfo govfileInfo = new FileInfo(addfieldAgent.ProfilePicture.FileName);

                if (!Directory.Exists(_environment.WebRootPath + "\\uploads"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                }
                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\gov_id_" + FieldAgentID.ToString() + govfileInfo.Extension.ToString()))
                {
                    addfieldAgent.GovtIssuedIdentity.CopyTo(filestream);
                    filestream.Flush();
                    //  return "\\Upload\\" + objFile.files.FileName;
                }
            }

                fieldAgent.FirstName = addfieldAgent.FirstName;
                fieldAgent.SecondName = addfieldAgent.SecondName;
                fieldAgent.LastName = addfieldAgent.LastName;
                fieldAgent.Email = addfieldAgent.Email;
                fieldAgent.Phone1 = addfieldAgent.Phone1;
                fieldAgent.Phone2 = addfieldAgent.Phone2;
                fieldAgent.StreetAddress = addfieldAgent.StreetAddress;
                fieldAgent.TownAddress = addfieldAgent.TownAddress;
                fieldAgent.StateAddress = addfieldAgent.StreetAddress;
                fieldAgent.ImageUrl = "\\upload\\pic_" + FieldAgentID.ToString();
                fieldAgent.GovtIssuedId = "\\upload\\gov_id_" + FieldAgentID.ToString();
                fieldAgent.GuarantorFirstName = addfieldAgent.GuarantorFirstName;
                fieldAgent.GuarantorSecondName = addfieldAgent.GuarantorSecondName;
                fieldAgent.GuarantorLastName = addfieldAgent.GuarantorLastName;
                fieldAgent.GuarantorPhone1 = addfieldAgent.GuarantorPhone1;
                fieldAgent.GuarantorPhone2 = addfieldAgent.GuarantorPhone2;
                fieldAgent.GuarantorEmail = addfieldAgent.GuarantorEmail;
                fieldAgent.GuarantorStreetAddress = addfieldAgent.GuarantorStreetAddress;
                fieldAgent.GuarantorTownAddress = addfieldAgent.GuarantorTownAddress;
                fieldAgent.GuarantorStateAddress = addfieldAgent.GuarantorStateAddress;
                _context.Entry(fieldAgent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FieldAgentExists(FieldAgentID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(fieldAgent);
        }


        // POST: api/FieldAgents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{registeredby}")]
        public async  Task<ActionResult<AddFieldAgentModel>> Register(string registeredby, [FromForm] AddFieldAgentModel addfieldAgent)
        {

            var duser = await _context.Users.FirstOrDefaultAsync(x => x.Email == addfieldAgent.Email);
            if (duser != null)
            {
                return BadRequest("Email already exist.");
            }

            Guid dFieldAgentID = Guid.NewGuid();



      
            

            if (addfieldAgent.ProfilePicture != null)
            {

                FileInfo pfileInfo = new FileInfo(addfieldAgent.ProfilePicture.FileName);

                if (!Directory.Exists(_environment.WebRootPath + "\\upload"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\upload\\");
                }
                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\upload\\pic_" + dFieldAgentID.ToString() + pfileInfo.Extension.ToString()))
                {
                    addfieldAgent.ProfilePicture.CopyTo(filestream);
                    filestream.Flush();
                    //  return "\\Upload\\" + objFile.files.FileName;
                }
            }

            if (addfieldAgent.GovtIssuedIdentity != null)
            {

                FileInfo govfileInfo = new FileInfo(addfieldAgent.ProfilePicture.FileName);
                if (!Directory.Exists(_environment.WebRootPath + "\\uploads"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                }
                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\gov_id_" + dFieldAgentID.ToString() + govfileInfo.Extension.ToString()))
                {
                    addfieldAgent.GovtIssuedIdentity.CopyTo(filestream);
                    filestream.Flush();
                    //  return "\\Upload\\" + objFile.files.FileName;
                }
            }

            var fieldAgent = new FieldAgent()
            {
                FieldAgentID = dFieldAgentID,
                DateOfRegistration = DateTime.Now.Date,
                Status = "pending",
                FirstName = addfieldAgent.FirstName,
                SecondName = addfieldAgent.SecondName,
                LastName = addfieldAgent.LastName,
                Email = addfieldAgent.Email,
                Phone1 = addfieldAgent.Phone1,
                Phone2 = addfieldAgent.Phone2,
                StreetAddress = addfieldAgent.StreetAddress,
                TownAddress = addfieldAgent.TownAddress,
                StateAddress = addfieldAgent.StreetAddress,
                ImageUrl =   "\\upload\\pic_" + dFieldAgentID.ToString(),
                GovtIssuedId = "\\upload\\gov_id_" + dFieldAgentID.ToString(),
                GuarantorFirstName = addfieldAgent.GuarantorFirstName,
                GuarantorSecondName = addfieldAgent.GuarantorSecondName,
                GuarantorLastName = addfieldAgent.GuarantorLastName,
                GuarantorPhone1 = addfieldAgent.GuarantorPhone1,
                GuarantorPhone2 = addfieldAgent.GuarantorPhone2,
                GuarantorEmail = addfieldAgent.GuarantorEmail,
                GuarantorStreetAddress = addfieldAgent.GuarantorStreetAddress,
                GuarantorTownAddress = addfieldAgent.GuarantorTownAddress,
                GuarantorStateAddress = addfieldAgent.GuarantorStateAddress,
                RegisteredBy = registeredby
            };

            var user = new User() ;
            user.UserID = fieldAgent.FieldAgentID;
            user.FirstName = fieldAgent.FirstName;
            user.LastName = fieldAgent.LastName;
            user.Email = fieldAgent.Email;
            user.Role = "User";
            user.UserType = "Field Agent";
            user.Verified = false;
            authentication = new Authentication(addfieldAgent.Email, addfieldAgent.Password,_configuration);
            user.PasswordHash = authentication.passwordHash;
            user.PasswordSalt = authentication.passwordSalt;
            user.TokenCreated = authentication.TokenCreated;
            user.TokenExpires = authentication.TokenExpires;
            _context.Users.Add(user);

            _context.FieldAgents.Add(fieldAgent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFieldAgent", new { FieldAgentID = fieldAgent.FieldAgentID }, fieldAgent);
        }

        


        // DELETE: api/FieldAgents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFieldAgent(Guid FieldAgentID)
        {
            var fieldAgent = await _context.FieldAgents.FindAsync(FieldAgentID);
            if (fieldAgent == null)
            {
                return NotFound();
            }

            _context.FieldAgents.Remove(fieldAgent);
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


        private bool FieldAgentExists(Guid FieldAgentID)
        {
            return _context.FieldAgents.Any(e => e.FieldAgentID == FieldAgentID);
        }

    }
}
