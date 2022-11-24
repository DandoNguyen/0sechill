using _0sechill.Data;
using _0sechill.Models.LookUpData;
using _0sechill.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    /// <summary>
    /// this is for init lookup data
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LookUpController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly IExcelService excelService;
        private readonly IMapper mapper;

        public LookUpController(
            ApiDbContext context,
            IExcelService excelService,
            IMapper mapper)
        {
            this.context = context;
            this.excelService = excelService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get All Category string
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet, Route("GetAllLookUp")]
        public async Task<IActionResult> getAllLookUpAsync()
        {
            var listPhoneCode = await context.lookUp.Where(x => x.lookUpTypeCode == "02").ToListAsync();
            var listCate = await context.lookUp.Where(x => x.lookUpTypeCode == "01").ToListAsync();
            var listNationality = await context.lookUp.Where(x => x.lookUpTypeCode == "04").ToListAsync();
            var listGender = await context.lookUp.Where(x => x.lookUpTypeCode == "03").ToListAsync();
            return Ok(new
            {
                listPhoneCode,
                listCate,
                listNationality,
                listGender
            });
        }

        /// <summary>
        /// this endpoints is to import look up data for the system
        /// </summary>
        /// <param name="file">excel file contains look up data</param>
        /// <returns>Http Response</returns>
        [HttpPost, Route("ImportLookUpData")]
        public async Task<IActionResult> ImportLookUpExcel(IFormFile file)
        {
            var listNewLookUp = await excelService.ImportLookUpFile(file);
            var listExistLookUp = await context.lookUp.ToListAsync();
            var listUpdate = new List<LookUpTable>();
            var listAdd = new List<LookUpTable>();

            if (listNewLookUp.Any())
            {
                if (!listExistLookUp.Any())
                {
                    context.lookUp.AddRange(listNewLookUp);
                    await context.SaveChangesAsync();
                    return Ok("Look Up Data Added");
                }

                else
                {
                    try
                    {
                        foreach (var newLookUp in listNewLookUp)
                        {
                            foreach (var existLookUp in listExistLookUp)
                            {
                                if (newLookUp.lookUpTypeCode.Equals(existLookUp.lookUpTypeCode) && newLookUp.index.Equals(existLookUp.index))
                                {
                                    var updateLookUp = new LookUpTable();

                                    updateLookUp = mapper.Map<LookUpTable>(existLookUp);
                                    updateLookUp.valueString = existLookUp.valueString;

                                    listUpdate.Add(updateLookUp);
                                    listNewLookUp.Remove(newLookUp);
                                    break;
                                }
                            }
                            
                        }

                        if (listNewLookUp.Any())
                        {
                            listAdd = listNewLookUp;
                            context.lookUp.AddRange(listAdd);
                        }

                        context.lookUp.UpdateRange(listUpdate);
                       
                        await context.SaveChangesAsync();
                        return Ok("Look Up Data Added");
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                }
                
            }
            return BadRequest("Can't read file");
        }
    }
}
