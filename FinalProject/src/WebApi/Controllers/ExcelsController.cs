using Application.Features.Excel.Commands.WriteProducts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class ExcelsController : ApiControllerBase
    {

        [HttpPost("WriteProducts")]
        public async Task<IActionResult> ReadCitiesAsync(ExcelWriteProductCommand command)
        {
          
            var byteArray = await Mediator.Send(command);
            string saveAsFileName = string.Format("ProductList-{0:d}.xls", DateTime.Now).Replace("/", "-");
           return File(byteArray, "application/octet-stream", saveAsFileName);
        
        }
    }
}
