using IronPdf;
using Microsoft.AspNetCore.Mvc;

namespace ApiExercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        [HttpGet]
        public ActionResult GeneratePdfFromHtml([FromBody]string html)
        {
            var Renderer = new HtmlToPdf();
            var pdf = Renderer.RenderHtmlAsPdf(html);
            return new FileStreamResult(pdf.Stream, "application/pdf")
            {
                FileDownloadName = "file.pdf"
            };
        }
    }
}
