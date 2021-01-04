using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC4RESTAPI.Dtos;
using EFC4RESTAPI.Models.Super;

namespace EFC4RESTAPI.Models
{
    [Table("Permissions")]
    public record Permission : IBase
    {
        [Required]
        [Column(Order = 5)]
        [Display(Name = "权重评分")]
        [Range(0.0, 10.0)]
        public double Score { get; init; }
        [Column(Order = 6)]
        [Display(Name = "失效时间戳")]
        public DateTimeOffset InvalidTimestamp { get; init; }
        public override UngetPermission AsUnget()
        => new UngetPermission
        {
            Name = Name,
            Valid = Valid,
            Score = Score,
            InvalidTimestamp = InvalidTimestamp,
            Description = Description
        };
    }
}