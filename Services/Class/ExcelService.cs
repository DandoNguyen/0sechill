using _0sechill.Data;
using _0sechill.Models;
using _0sechill.Models.Infrastructure;
using _0sechill.Models.LookUpData;
using OfficeOpenXml;
using System.Linq;

namespace _0sechill.Services.Class
{
    public class ExcelService : IExcelService
    {
        private readonly string FACIL_KEYWORD = "FACIL";
        private readonly string APARTMENT = "Floor/Room";
        private readonly IConfiguration config;
        private readonly ApiDbContext context;

        public ExcelService(IConfiguration config, ApiDbContext context)
        {
            this.config = config;
            this.context = context;
        }
        public async Task<List<Block>> ImportBlock(IFormFile formFile)
        {
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                var package = new ExcelPackage(stream);

                //Count Worksheet (each worksheet is a name of a Block)
                var blockList = await ReadBlock(package);
                return blockList;
            }
        }

        public async Task<List<Apartment>> ReadApartmentInBlock(IFormFile formFile, string blockName)
        {
            var listApartment = new List<Apartment>();
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);
                var package = new ExcelPackage(stream);
                if (package is not null)
                {
                    foreach (var worksheet in package.Workbook.Worksheets)
                    {
                        if (worksheet.Name.ToLower().Trim().Equals(blockName.ToLower().Trim()))
                        {
                            var entryCell = GetCellAddress(package, worksheet.Name, APARTMENT);
                            for (int row = entryCell.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
                            {
                                for (int col = entryCell.Start.Column + 1; col <= worksheet.Dimension.End.Column; col++)
                                {
                                    var apartment = new Apartment();
                                    var rowValue = int.TryParse(worksheet.Cells[row, entryCell.Start.Column].Value.ToString(), out int floorNumber);
                                    var colValue = int.TryParse(worksheet.Cells[entryCell.Start.Row, col].Value.ToString(), out int apartmentNumber);
                                    if (rowValue && colValue)
                                    {
                                        apartment.apartmentName = $"{worksheet.Name[0].ToString().ToUpper()}.{floorNumber:00}{apartmentNumber:00}";
                                    }
                                    else
                                    {
                                        apartment.apartmentName = $"{worksheet.Name[0].ToString().ToUpper()}.{rowValue}{colValue}";
                                    }

                                    var apartmentDetail = "Null";
                                    if (worksheet.Cells[row, col].Value is not null)
                                    {
                                        apartmentDetail = worksheet.Cells[row, col].Value.ToString();
                                    }

                                    var newApartment = GetApartmentDetail(apartment, apartmentDetail.ToLower().Trim(), package);
                                    listApartment.Add(newApartment);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return listApartment;
        }

        private async Task getPublicFacil(ExcelWorksheet worksheet)
        {
            var listNewFacil = new List<PublicFacility>();
            for (int row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
            {
                for (int col = worksheet.Dimension.Start.Column + 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    var newFacil = new PublicFacility();
                    newFacil.ID = Guid.NewGuid();
                    newFacil.typeFacil = worksheet.Cells[row, worksheet.Dimension.Start.Column + 1].Value.ToString().Trim().ToLower();
                    var cellValue = worksheet.Cells[row, col].Value;
                    if (cellValue is null)
                    {
                        break;
                    }
                    newFacil.facilCode = cellValue.ToString();
                    listNewFacil.Add(newFacil);
                }
            }
            await context.AddRangeAsync(listNewFacil);
            await context.SaveChangesAsync();
        }
        private Apartment GetApartmentDetail(Apartment apartment, string apartmentDetail, ExcelPackage package)
        {
            foreach (var worksheet in package.Workbook.Worksheets)
            {
                if (worksheet.Name.Equals("RoomDetails"))
                {
                    var entryPoint = GetCellAddress(package, worksheet.Name, "Type");
                    for (int row = entryPoint.Start.Row; row <= worksheet.Dimension.End.Row; row++)
                    {
                        for (int col = entryPoint.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                        {
                            if (apartmentDetail.Equals(worksheet.Cells[row, entryPoint.Start.Column].Value.ToString().ToLower().Trim()))
                            {
                                switch (worksheet.Cells[entryPoint.Start.Row, col].Value.ToString())
                                {
                                    case nameof(apartment.bedroomAmount): 
                                        apartment.bedroomAmount = int.Parse(worksheet.Cells[row, col].Value.ToString());
                                        break;
                                    case nameof(apartment.heartWallArea):
                                        apartment.heartWallArea = int.Parse(worksheet.Cells[row, col].Value.ToString());
                                        break;
                                    case nameof(apartment.clearanceArea):
                                        apartment.clearanceArea = int.Parse(worksheet.Cells[row, col].Value.ToString());
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            return apartment;
        }

        private async Task<List<Block>> ReadBlock(ExcelPackage package)
        {
            //Create a list of Block object 
            List<Block> blockList = new();
            if (package is not null)
            {
                if (!package.Workbook.Worksheets.Count.Equals(0))
                {
                    foreach (var worksheet in package.Workbook.Worksheets)
                    {
                        var block = new Block();
                        if (worksheet.Name.Equals("RoomDetails"))
                            continue;
                        if (worksheet.Name.ToUpper().Trim().Equals(FACIL_KEYWORD))
                        {
                            await getPublicFacil(worksheet);
                            continue;
                        }
                        block.blockName = worksheet.Name;
                        //find key word in cell
                        var cellAddress = GetCellAddress(package, worksheet.Name, APARTMENT);

                        if (cellAddress is null)
                        {
                            block.flourAmount = 0;
                        }
                        else
                        {
                            block.flourAmount = worksheet.Dimension.End.Row - cellAddress.Start.Row;
                        }
                        blockList.Add(block);
                    }
                }
                return blockList;
            }
            return blockList;
        }

        private ExcelAddressBase GetCellAddress(ExcelPackage package, string worksheetName, string keyword)
        {
            if (package is not null)
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    if (worksheet.Name.Equals(worksheetName.Trim()))
                    {
                        //create an excel query
                        var query = (from cell in worksheet.Cells[1,1,4,4]
                                     where cell.Value.ToString().ToLower().Trim().Equals(keyword.ToLower().Trim())
                                     select cell).FirstOrDefault();
                        return query;
                    }
                }
            }
            return null;
        }


        public async Task<List<LookUpTable>> ImportLookUpFile(IFormFile formFile)
        {
            List<LookUpTable> lookUpsList = new List<LookUpTable>();

            var package = new ExcelPackage();
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                package = new ExcelPackage(stream);
                if (package is not null)
                {
                    var ws = package.Workbook.Worksheets.FirstOrDefault();

                    for (int col = ws.Dimension.Start.Column + 1; col <= ws.Dimension.Start.Column + 10; col++)
                    {
                        for (int row = ws.Dimension.Start.Row + 1; row <= ws.Dimension.End.Row; row++)
                        {
                            var cellValue = ws.Cells[row, col].Value;
                            if (cellValue is null)
                            {
                                break;
                            } 

                            else
                            {
                                if (row == 2)
                                {
                                    continue;
                                }

                                var newLookUpItem = new LookUpTable();
                                newLookUpItem.lookUpID = Guid.NewGuid();
                                newLookUpItem.lookUpTypeName = ws.Cells[2, col].Value.ToString();

                                newLookUpItem.lookUpTypeCode = ws.Cells[1, col].Value.ToString();
                                newLookUpItem.index = (row - 2).ToString("00");
                                newLookUpItem.valueString = cellValue.ToString();

                                lookUpsList.Add(newLookUpItem);
                            }
                        }
                    }
                }
            }

            return lookUpsList;
        }
    }
}
