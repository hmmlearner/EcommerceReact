using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EcommerceReact.Server.DTO.Category
{
    public class CategoryCreateDto
    {

        [Required]
        public string Name { get; set; } = "Kitchen";
        [DisplayName("Display Name")]
        [Range(1, 100, ErrorMessage = "Display order must be between 1 and 100")]
        public int DisplayOrder { get; set; } = 1;
    }
}
