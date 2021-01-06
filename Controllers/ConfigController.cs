using EFC4RESTAPI.Controllers.Super;
using EFC4RESTAPI.Dtos;
using EFC4RESTAPI.Models;
using EFC4RESTAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFC4RESTAPI.Controllers
{
    [ApiController]
    [Route("/[Controller]")]
    public class ConfigController : SuperController<Config, Config, UngetConfig>
    {
        public ConfigController(IDBContext dBContext) : base(dBContext) { }
    }
}