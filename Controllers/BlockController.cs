using _0sechill.Data;
using _0sechill.Dto.Block.Response;
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

        [HttpGet, Route("GetAllBlock")]
        public async Task<IActionResult> GetAllBlockAsync()
        {
            var listBlock = await context.blocks.ToListAsync();
            if (listBlock.Count.Equals(0))
            {
                return BadRequest("No Block Available");
            }
            var listBlockDto = new List<BlockDto>();
            foreach (var block in listBlock)
            {
                var blockDto = mapper.Map<BlockDto>(block);
                listBlockDto.Add(blockDto);
            }
            return Ok(listBlockDto);
        }

        [HttpPost, Route("AddBlock")]
        public async Task<IActionResult> AddBlockAsync(string blockName)
        {
            if (blockName is null)
            {
                return BadRequest("Block Name is required");
            }
            var newBlock = new Block();
            newBlock.blockName = blockName;
            if (ModelState.IsValid)
            {
                try
                {
                    await context.blocks.AddAsync(newBlock);
                    await context.SaveChangesAsync();
                    return Ok($"Block {blockName} is created");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return BadRequest($"Error in creating Block {blockName}: {ex.Message}");
                }
            }
            return BadRequest("Model State is not valid");
        }

        [HttpPost, Route("ImportApartment")]
        public async Task<IActionResult> ImportApartmentAsync([FromForm] string blockId, IFormFile formFile)
        {
            if (blockId is null)
            {
                return BadRequest("Block ID is null");
            }
            else if (formFile is null)
            {
                return BadRequest("Form file is null");
            }
            else if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("formFile format invalid");

            var existingBlock = await context.blocks
                .FirstOrDefaultAsync(x => x.blockId.Equals(Guid.Parse(blockId)));
            if (existingBlock is null)
                return BadRequest("Selected Block not Found");
            var listApartment = await excelService.ReadApartmentInBlock(formFile, existingBlock.blockName);
            if (listApartment is null)
                return BadRequest("Coundn't read file content");
            try
            {
                await AddApartmentAsync(listApartment, existingBlock.blockId);
                await context.SaveChangesAsync();
                return Ok("Apartments Imported");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest($"Error in Importing Apartments {ex.Message}");
            }
        }

        [HttpGet, Route("GetBlock")]
        public async Task<IActionResult> GetBlockAsync(string blockId)
        {
            if (blockId is null)
            {
                return BadRequest("Block Id is null");
            }
            var block = await context.blocks.FirstOrDefaultAsync(x => x.blockId.Equals(Guid.Parse(blockId)));
            if (block == null)
                return BadRequest("No Block is foung");
            var blockDto = mapper.Map<BlockDto>(block);
            return Ok(blockDto);
        }

        [HttpPut, Route("EditBlock")]
        public async Task<IActionResult> EditBlockAsync(string blockId, string blockName)
        {
            if (blockId is null)
            {
                return BadRequest("Block ID is null");
            }

            var existingBlock = await context.blocks
                .Where(x => x.blockId.Equals(Guid.Parse(blockId)))
                .FirstOrDefaultAsync();
            if (existingBlock is null)
            {
                return BadRequest("Selected Block is not found!");
            }
            existingBlock.blockName = blockName;
            context.blocks.Update(existingBlock);
            await context.SaveChangesAsync();
            return Ok("Block Updated Success!");
        }

        [HttpDelete, Route("DeleteBlock")]
        public async Task<IActionResult> DeleteBlockAsync(string blockId)
        {
            if (blockId is null)
            {
                return BadRequest("Block Id is null");
            }
            if (await IsApartmentKeyConstraintAsync(blockId))
            {
                return BadRequest("There are Apartments assigned to this Block");
            }

            try
            {
                var existingBlock = await context.blocks
                    .FirstOrDefaultAsync(x => x.blockId.Equals(Guid.Parse(blockId)));
                context.blocks.Remove(existingBlock);
                await context.SaveChangesAsync();
                return Ok($"Block {existingBlock.blockName} is deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest($"Error in Deleting Block: {ex.Message}");
            }
        }

        private async Task<bool> IsApartmentKeyConstraintAsync(string blockId)
        {
            var listExistingApartment = await context.apartments
                .Where(x => x.blockId.Equals(Guid.Parse(blockId)))
                .ToListAsync();
            if (!listExistingApartment.Count.Equals(0))
            {
                return true;
            }
            return false;
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
