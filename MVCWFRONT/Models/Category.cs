using MVCWFRONT.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MVCWFRONT.Models
{
    public class Category:BaseEntity
    {
        [MaxLength(30, ErrorMessage ="Name 30 dan cox yazmaq olmaz")]
        public string Name { get; set; }

        public List<Product>? Products { get; set; }
    }
}
