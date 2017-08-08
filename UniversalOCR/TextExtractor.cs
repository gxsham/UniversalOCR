using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesseract;

namespace UniversalOCR
{
    public class TextExtractor
    {
		public List<string> DocumentFormats = new List<string> { ".txt", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", "pptx", ".rtf" };
		public List<string> ImageFormats = new List<string> { ".bmp", ".png", ".png", ".jfif", ".jpeg", ".tiff" };
		public List<string> AcceptedLanguages = new List<string> { "afr", "amh", "ara", "ara", "asm", "aze", "bel", "ben", "bod", "bos", "bul", "cat", "ceb", "ces", "chi", "chr", "cym", "dan", "deu", "dzo", "ell", "eng", "enm", "epo", "equ", "est", "eus", "fas", "fin", "fra", "frk", "frm", "gle", "glg", "grc", "guj", "hat", "heb", "hin", "hrv", "hun", "iku", "ind", "isl", "ita", "jav", "jpn", "kan", "kat", "kaz", "khm", "kir", "kor", "kur", "lao", "lat", "lav", "lit", "mal", "mar", "mkd", "mlt", "msa", "mya", "nep", "nld", "nor", "ori", "osd", "pan", "pol", "por", "pus", "ron", "rus", "san", "sin", "slk", "slv", "spa", "spa_old", "sqi", "srp", "swa", "swe", "syr", "tam", "tel", "tgk", "tgl", "tha", "tir", "tur", "uig", "ukr", "urd", "uzb", "vie", "yid" };

		public string SelectStrategy(string filePath, string language = "ron")
		{
			var format = System.IO.Path.GetExtension(filePath);
			if (DocumentFormats.Contains(format))
			{
				return ExtractFromDocument(filePath);
			}
			else if (ImageFormats.Contains(format))
			{
				return ExtractFromImage(filePath, language);
			}
			else
			{
				return null;
			}
		}
		public string ExtractFromDocument(string filePath)
		{
			try
			{
				var extractor = new TikaOnDotNet.TextExtraction.TextExtractor();
				var text = extractor.Extract(filePath);
				return text.Text;
			}
			catch (Exception) { return null; }
		}

		public string ExtractFromImage(string filePath, string language = "ron")
		{
			try
			{
				if(language == null)
				{
					language = "ron";
				}
				using (var engine = new TesseractEngine(@"./tessdata", language, EngineMode.Default))
				{
					using (var img = Pix.LoadFromFile(filePath))
					{
						using (var page = engine.Process(img))
						{
							var text = page.GetText();
							return text;
						}
					}
				}
			}
			catch (Exception ex) { return null; }
		}

		public string SupportedFormats()
		{
			return DocumentFormats.Aggregate((a, b) => a + " , " + b) + ImageFormats.Aggregate((a, b) => a + " , " + b);
		}

		public string SupportedLanguages()
		{
			return AcceptedLanguages.Aggregate((a, b) => a + " , " + b);
		}
	}
}
