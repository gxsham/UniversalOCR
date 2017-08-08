using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace UniversalOCR.Controllers
{
	public class HomeController : Controller
    {
		private readonly IHostingEnvironment _environment;
		public HomeController(IHostingEnvironment env)
		{
			_environment = env;
		
		}
        public IActionResult Index()
        {
			ViewBag.Languages = new SelectList(new string[] { "afr", "amh", "ara", "ara", "asm", "aze",  "bel", "ben", "bod", "bos", "bul", "cat", "ceb", "ces", "chi",  "chr", "cym", "dan",  "deu",  "dzo", "ell",   "eng", "enm", "epo", "equ", "est", "eus", "fas", "fin", "fra", "frk", "frm", "gle", "glg", "grc", "guj", "hat", "heb",  "hin", "hrv", "hun", "iku", "ind", "isl", "ita",  "jav", "jpn", "kan", "kat",  "kaz", "khm", "kir", "kor", "kur", "lao", "lat", "lav", "lit", "mal", "mar", "mkd", "mlt", "msa", "mya", "nep", "nld", "nor", "ori", "osd", "pan", "pol", "por", "pus", "ron",  "rus", "san", "sin", "slk",  "slv",  "spa", "spa_old", "sqi", "srp",  "swa", "swe", "syr", "tam", "tel", "tgk", "tgl", "tha", "tir", "tur", "uig", "ukr", "urd", "uzb", "vie", "yid" });
			return View();
        }
		[HttpPost]
		public IActionResult Index(FileViewModel model)
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
						HttpContext.Session.SetString("result", JsonConvert.SerializeObject(result));
						return RedirectToAction("Result");
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
					HttpContext.Session.SetString("result", JsonConvert.SerializeObject(result));
					return RedirectToAction("Result");
				}
				System.IO.File.Delete(path);
				result.Success = false;
				result.Text = $"Supported Formats: {extractor.SupportedFormats()} {Environment.NewLine}{Environment.NewLine} Supported Languages: {extractor.SupportedLanguages()}";
				HttpContext.Session.SetString("result", JsonConvert.SerializeObject(result));
				return RedirectToAction("Result");
			}
			result.Success = false;
			result.Text = "File is null";
			HttpContext.Session.SetString("result", JsonConvert.SerializeObject(result));
			return RedirectToAction("Result");
		}
        public IActionResult Result()
        {
			var model = JsonConvert.DeserializeObject<Result>(HttpContext.Session.GetString("result"));
			if (model == null)
				return View(new Result());
			else
				return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
