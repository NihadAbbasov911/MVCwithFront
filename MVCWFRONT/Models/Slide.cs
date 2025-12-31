using MVCWFRONT.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCWFRONT.Models
{
    public class Slide : BaseEntity
    {

        public string Title { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public int Order { get; set; }
        [NotMapped]
        public IFormFile Photo
        {
            get; set;
        }
    }
}
