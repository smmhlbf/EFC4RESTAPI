using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFC4RESTAPI.Models.Super
{
    [NotMapped]
    public abstract record IGeneric
    {
        [Key]
        [Column(Order = 0)]
        [Display(Name = "唯一编码")]
        public Guid Id { get; init; }
        public abstract IUngetSuper AsUnget();
    }
}