using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFC4RESTAPI.Models.Super
{
    [NotMapped]
    public record IUngetBase : IUngetSuper
    {
        [Required]
        [Display(Name = "名称")]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; init; }
        [Display(Name = "描述信息")]
        public string Description { get; init;}
    }
}