using _0sechill.Data;
using _0sechill.Dto.Contract.Request;
using _0sechill.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, blockManager")]
    public class ContractController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public ContractController(
            ApiDbContext context,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// this is the endpoint that add new contract
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, Route("AddNewContract")]
        public async Task<IActionResult> addContract(AddNewContractDto dto)
        {
            var newContract = new UserHistory();
            newContract = mapper.Map<UserHistory>(dto);
            newContract.applicationUser = await userManager.FindByIdAsync(dto.residentID);

            var listApartment = new List<Apartment>();
            foreach (var ID in dto.listApartmentID)
            {
                var existApartment = new Apartment();
                existApartment = await context.apartments
                    .Where(x => x.apartmentId.Equals(ID))
                    .FirstOrDefaultAsync();
                if (existApartment is not null)
                {
                    listApartment.Add(existApartment);
                }
            }

            if (listApartment.Any())
            {
                newContract.apartment = listApartment;
            }

            if (ModelState.IsValid)
            {
                await context.userHistories.AddAsync(newContract);
                await context.SaveChangesAsync();
                return Ok("Contract Added");
            }
            return Ok("Cannot add contract");
        }

        /// <summary>
        /// this is the endpoint that get all contract of a resident
        /// </summary>
        /// <param name="residentID"></param>
        /// <returns></returns>
        [HttpGet, Route("GetAllContractOfResident")]
        public async Task<IActionResult> GetAllContractOfResident(string residentID)
        {
            var listContract = await context.userHistories
                .Where(x => x.userId.Equals(residentID))
                .ToListAsync();
            return Ok(listContract);
        }
    }
}
