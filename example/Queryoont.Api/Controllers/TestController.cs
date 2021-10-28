using Microsoft.AspNetCore.Mvc;
using Queryoont.Api.Actions;
using Queryoont.Api.Models;
using Queryoont.Attributes;
using SqlKata;

namespace Queryoont.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [QueryoontFilter(FilterAction = typeof(GetQueryActions))]
        [HttpPost]
        public Query GetQuery()
        {
            return new Query("db.Customers");
        }
    }
}