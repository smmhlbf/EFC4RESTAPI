using System.ComponentModel.DataAnnotations;

namespace EFC4RESTAPI.Models.Super
{
    public record ISuper
    {
        [Key]
        public int Id { get; init; }
    }
}