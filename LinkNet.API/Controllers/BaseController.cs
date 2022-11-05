using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkNet.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
