using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFC4RESTAPI.Models.Super
{
    [NotMapped]
    public record ICommon : IBase
    {
        [Column(Order = 1)]
        [Display(Name = "父级唯一编码")]
        public int ParentId { get; init; }
    }
}