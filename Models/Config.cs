using System.ComponentModel.DataAnnotations.Schema;
using EFC4RESTAPI.Dtos;
using EFC4RESTAPI.Models.Super;

namespace EFC4RESTAPI.Models
{
    [Table("Configs")]
    public record Config : ICommon
    {
        public override UngetConfig AsUnget()
        => new UngetConfig
        {
            ParentId = ParentId,
            Name = Name,
            Valid = Valid,
            Description = Description
        };
    }
}