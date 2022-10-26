using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.FE001.Request
{
    public class SearchFilterDto
    {
        /// <summary>
        /// this is mandatory, must not be null
        /// </summary>
        [Required]
        public string nameString { get; set; }

        /// <summary>
        /// this is age check flag, used to determine which filter to include into the search, must not be null, either false or true
        /// </summary>
        [Required]
        public bool hasAgeCheck { get; set; }
        public int ageFrom { get; set; }
        public int ageTo { get; set; }

        /// <summary>
        /// gender check flag
        /// </summary>
        [Required]
        public bool hasGenderCheck { get; set; }
        public bool isMale { get; set; }

        /// <summary>
        /// role check flag
        /// </summary>
        [Required]
        public bool hasRoleCheck { get; set; }
        public string roleID { get; set; }
        public string roleName { get; set; }
    }
}
