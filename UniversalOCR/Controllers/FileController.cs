using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Diagnostics;

namespace UniversalOCR.Controllers
{
	[Route("api/[controller]")]
    public class FileController : Controller
    {
		private readonly IHostingEnvironment _environment;
		public FileController(IHostingEnvironment environment)
		{
			_environment = environment;
		}
		// GET api/values
		[HttpPost("process")]
		public JsonResult Process(FileViewModel model)
		{
			var result = new Result();
			if (model.File != null)
			{

				var path = Path.Combine(_environment.ContentRootPath, model.File.FileName);
				using (var stream = new FileStream(path, FileMode.Create))
				{
					try
					{
						model.File.CopyTo(stream);
					}
					catch (Exception ex)
					{
						result.Text = ex.Message;
						result.Success = false;
						return new JsonResult(result);
					}
				}
				var extractor = new TextExtractor();
				var watch = Stopwatch.StartNew();
				var text = extractor.SelectStrategy(path, model.Language);
				watch.Stop();
				if (text != null)
				{
					System.IO.File.Delete(path);
					result.Text = text.Trim();
					result.Success = true;
					result.ExecutionTime = watch.ElapsedMilliseconds;
					return new JsonResult(result);
				}
				System.IO.File.Delete(path);
				result.Success = false;
				result.Text = $"Supported Formats: {extractor.SupportedFormats()} {Environment.NewLine} Supported Languages: {extractor.SupportedLanguages()}";
				return new JsonResult(result);
			}

			result.Success = false;
			result.Text = "File is null";
			return new JsonResult(result);
		}
	}
}
