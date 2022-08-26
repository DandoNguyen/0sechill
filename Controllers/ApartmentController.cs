using _0sechill.Data;
using _0sechill.Dto.Apartment.Request;
using _0sechill.Dto.Apartment.Response;
using _0sechill.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApartmentController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<ApartmentController> logger;

        public ApartmentController(
            ApiDbContext context,
            IMapper mapper,
            ILogger<ApartmentController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet, Route("GetAllApartmentInBlock")]
        public async Task<IActionResult> GetAllApartmentBlockAsync(string blockId)
        {
            if (blockId is null)
                return BadRequest("Block ID is null");

            var existingBlock = await context.blocks
                .FirstOrDefaultAsync(x => x.blockId.Equals(Guid.Parse(blockId)));
            if (existingBlock is null)
                return BadRequest("No Block is found");

            var listApartment = await context.apartments
                .Where(x => x.blockId.Equals(Guid.Parse(blockId)))
                .ToListAsync();

            if (listApartment.Count.Equals(0))
                return NoContent();

            var listApartmentDto = new List<ApartmentDto>();
            foreach (var apartment in listApartment)
            {
                var newApartmentDto = mapper.Map<ApartmentDto>(apartment);
                listApartmentDto.Add(newApartmentDto);
            }
            return Ok(listApartmentDto);
        }

        [HttpPut, Route("EditApartment")]
        public async Task<IActionResult> EditApartmentAsync(string apartmentId, EditApartmentDto dto)
        {
            if (apartmentId is null)
                return BadRequest("Apartment Id is null");

            var existingApartment = await context.apartments
                .FirstOrDefaultAsync(x => x.apartmentId.Equals(Guid.Parse(apartmentId)));
            if (existingApartment is null)
                return NoContent();
            existingApartment = mapper.Map<Apartment>(dto);
            try
            {
                context.apartments.Update(existingApartment);
                await context.SaveChangesAsync();
                return Ok($"Apartment {existingApartment.apartmentName} updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest($"Error in updating Apartment {existingApartment.apartmentName}: {ex.Message}");
            }
        }
    }
}
