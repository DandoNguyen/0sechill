using _0sechill.Data;
using _0sechill.Models;
using _0sechill.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly IExcelService excelService;
        private readonly ILogger<BlockController> logger;
        private readonly ApiDbContext context;
        private readonly IMapper mapper;

        public BlockController(
            IExcelService excelService,
            ILogger<BlockController> logger,
            ApiDbContext context,
            IMapper mapper)
        {
            this.excelService = excelService;
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("ImportExcel")]
        public async Task<IActionResult> ImportExcelAsync(IFormFile formFile)
        {
            if (formFile is null)
            {
                return BadRequest("formFile is null");
            }
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("formFile format invalid");

            try
            {
                var listFaultApartment = new List<string>();
                //Call excel Service
                var listBlock = await excelService.ImportBlock(formFile);

                //Import read data into Database
                foreach (var block in listBlock)
                {
                    var listApartment = await excelService.ReadApartmentInBlock(formFile, block.blockName);
                    foreach (var apartment in listApartment)
                    {
                        if (apartment.bedroomAmount.Equals(0))
                        {
                            listFaultApartment.Add(apartment.apartmentName);
                        }
                    }

                    //Try add to database
                    try
                    {
                        block.blockId = Guid.NewGuid();

                        //Update or add new block and apartment
                        var existingBlock = await context.blocks
                            .Where(x => x.blockName.Equals(block.blockName))
                            .FirstOrDefaultAsync();
                        if (existingBlock is null)
                        {
                            context.blocks.Add(block);
                            await AddApartmentAsync(listApartment, block.blockId);
                        }
                        else
                        {
                            existingBlock.flourAmount = block.flourAmount;
                            context.blocks.Update(existingBlock);
                            await AddApartmentAsync(listApartment, existingBlock.blockId);
                        }
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"File read success but error while import to database\nError Message: {ex.Message}");
                    }
                }

                if (listFaultApartment.Count.Equals(0))
                {
                    return Ok("File imported with no fault");
                }
                var result = "File imported with fault apartment(s)";
                return new JsonResult(new { result, listFaultApartment }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private async Task AddApartmentAsync(List<Apartment> listApartment, Guid blockId)
        {
            var listExistingApartment = await context.apartments
                .Where(x => x.blockId.Equals(blockId))
                .ToListAsync();
            if (listExistingApartment.Count != 0)
            {
                switch (listApartment.Count <= listExistingApartment.Count)
                {
                    case true:
                        for (int i = 0; i < listExistingApartment.Count; i++)
                        {
                            for (int j = 0; j < listApartment.Count; j++)
                            {
                                //If existingApartment has the same name as one object in listApartment
                                if (listExistingApartment[i].apartmentName.Equals(listApartment[j].apartmentName))
                                {
                                    //Update each existingApartment when condition met
                                    mapper.Map(listApartment[j], listExistingApartment[i]);
                                    context.apartments.Update(listExistingApartment[i]);
                                }
                                else //For each object doesnt met condition
                                {
                                    context.apartments.Remove(listExistingApartment[i]);
                                }
                            }
                        }
                        break;

                    case false:
                        for (int j = 0; j < listApartment.Count; j++)
                        {
                            for (int i = 0; i < listExistingApartment.Count; i++)
                            {
                                if (listApartment[j].apartmentName.Equals(listExistingApartment[i].apartmentName))
                                {
                                    mapper.Map(listApartment[j], listExistingApartment[i]);
                                    context.apartments.Update(listExistingApartment[i]);
                                }
                                else
                                {
                                    listApartment[j].blockId = blockId;
                                    context.apartments.Add(listApartment[j]);
                                }
                            }
                        }
                        break;
                }                
            }
            else
            {
                foreach (var apartment in listApartment)
                {
                    apartment.blockId = blockId;
                    context.apartments.Add(apartment);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
