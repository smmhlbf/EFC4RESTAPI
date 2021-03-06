using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFC4RESTAPI.Models.Super
{
    [NotMapped]
    public record ISuper : IGeneric
    {
        [Column(Order = 3)]
        [Display(Name = "有效性")]
        public bool Valid { get; init; }
        public override IUngetSuper AsUnget() => null;
    }
}