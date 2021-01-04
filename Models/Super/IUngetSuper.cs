using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFC4RESTAPI.Models.Super
{
    [NotMapped]
    public record IUngetSuper : IUngetGeneric
    {
        [Required]
        [Display(Name = "有效性")]
        public bool Valid { get; init; }

        public override ISuper AsModel() => null;
    }
}