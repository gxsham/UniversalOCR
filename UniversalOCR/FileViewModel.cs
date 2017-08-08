using Microsoft.AspNetCore.Http;

namespace UniversalOCR
{
	public class FileViewModel
    {
		public IFormFile File { get; set; }
		public string Language { get; set; }
		public string Text { get; set; }
		public FileViewModel()
		{
			Language = "ron";
		}
	}
}
