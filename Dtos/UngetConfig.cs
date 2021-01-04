using System;
using System.ComponentModel.DataAnnotations.Schema;
using EFC4RESTAPI.Models;
using EFC4RESTAPI.Models.Super;

namespace EFC4RESTAPI.Dtos
{
    [NotMapped]
    public record UngetConfig : IUngetCommon
    {
        public override Config AsModel()
        => new Config
        {
            Id = Guid.NewGuid(),
            ParentId = ParentId,
            Name = Name,
            Valid = Valid,
            Description = Description
        };
    }
}