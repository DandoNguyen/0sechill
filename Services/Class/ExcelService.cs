using _0sechill.Models;
using OfficeOpenXml;
using System.Linq;

namespace _0sechill.Services.Class
{
    public class ExcelService : IExcelService
    {
        public async Task<List<Block>> ImportBlock(IFormFile formFile)
        {
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                var package = new ExcelPackage(stream);

                //Count Worksheet (each worksheet is a name of a Block)
                var blockList = ReadBlock(package);
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
                            var entryCell = GetCellAddress(package, worksheet.Name, "Floor/Room");
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

        private List<Block> ReadBlock(ExcelPackage package)
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
                        block.blockName = worksheet.Name;
                        //find key word in cell
                        var cellAddress = GetCellAddress(package, worksheet.Name, "Floor/Room");

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
                    if (worksheet.Name.Equals(worksheetName))
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
    }
}
