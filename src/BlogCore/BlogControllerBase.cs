using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BlogCore
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogControllerBase : ControllerBase
    {
        public ILogger logger { get; set; }
    }
}
