using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC4RESTAPI.Models;
using EFC4RESTAPI.Models.Super;

namespace EFC4RESTAPI.Dtos
{
    [NotMapped]
    public record UngetPermission : IUngetBase
    {
        [Required]
        [Display(Name = "权重评分")]
        [Range(0.0, 10.0)]
        public double Score { get; init; }
        [Display(Name = "失效时间戳")]
        public DateTimeOffset InvalidTimestamp { get; init; }
        public override Permission AsModel()
        => new Permission
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Valid = Valid,
            Score = Score,
            InvalidTimestamp = InvalidTimestamp,
            Description = Description
        };
    }
}