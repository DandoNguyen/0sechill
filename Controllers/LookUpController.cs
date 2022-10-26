﻿using _0sechill.Data;
using _0sechill.Models.LookUpData;
using _0sechill.Services;
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

        public LookUpController(
            ApiDbContext context,
            IExcelService excelService)
        {
            this.context = context;
            this.excelService = excelService;
        }

        /// <summary>
        /// this endpoints is to import look up data for the system
        /// </summary>
        /// <param name="file">excel file contains look up data</param>
        /// <returns>Http Response</returns>
        [HttpPost, Route("ImportLookUpData")]
        public async Task<IActionResult> ImportLookUpExcel(IFormFile file)
        {
            var listLookUp = await excelService.ImportLookUpFile(file);

            if (listLookUp.Any())
            {
                try
                {
                    var listToDelete = await context.lookUp.Select(x => new LookUpTable { lookUpID = x.lookUpID }).ToListAsync();
                    context.lookUp.RemoveRange(listToDelete);

                    context.lookUp.AddRange(listLookUp);
                    await context.SaveChangesAsync();
                    return Ok("Look Up Data Added");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Can't read file");
        }
    }
}
