using _0sechill.Models;
using _0sechill.Models.LookUpData;
using OfficeOpenXml;

namespace _0sechill.Services
{
    public interface IExcelService
    {
        Task<List<Block>> ImportBlock(IFormFile formFile);
        Task<List<Apartment>> ReadApartmentInBlock(IFormFile formFile, string blockName);
        Task<List<LookUpTable>> ImportLookUpFile(IFormFile formFile);
    }
}
