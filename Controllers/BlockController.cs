using _0sechill.Data;
using _0sechill.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly IExcelService excelService;
        private readonly ILogger<BlockController> logger;
        private readonly ApiDbContext context;

        public BlockController(
            IExcelService excelService,
            ILogger<BlockController> logger,
            ApiDbContext context)
        {
            this.excelService = excelService;
            this.logger = logger;
            this.context = context;
        }

        [HttpPost]
        [Route("ImportExcel")]
        public async Task<IActionResult> ImportExcelAsync(IFormFile formFile)
        {
            if(formFile is null)
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
                        context.blocks.Add(block);
                        foreach (var apartment in listApartment)
                        {
                            apartment.blockId = block.blockId;
                            context.apartments.Add(apartment);
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
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
