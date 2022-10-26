using _0sechill.Dto.FE001.Model;
using _0sechill.Models;

namespace _0sechill.Dto.FE001.Response
{
    public class searchFilterResultDto
    {
        public bool isSucceed { get; set; }
        public string message { get; set; }
        public string error { get; set; }
        public List<FE001UserModel> result { get; set; }

        public searchFilterResultDto()
        {
            isSucceed = false;
            message = "";
            error = "";
            result = null;
        }
    }
}
