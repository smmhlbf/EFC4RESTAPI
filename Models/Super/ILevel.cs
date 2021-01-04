using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFC4RESTAPI.Models.Super
{
    [NotMapped]
    public record ILevel : ICommon
    {
        [Column(Order = 2)]
        [Display(Name = "主管唯一编码")]
        public int ManagerId { get; init; }
    }
}