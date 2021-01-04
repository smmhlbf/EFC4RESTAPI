using System.ComponentModel.DataAnnotations.Schema;

namespace EFC4RESTAPI.Models.Super
{
    [NotMapped]
    public abstract record IUngetGeneric
    {
        public abstract ISuper AsModel();
    }
}