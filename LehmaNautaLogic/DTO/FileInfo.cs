using System;

namespace LehmaNautaLogic.DTO
{
	public class FileInfo
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public string Filename { get; set; }
		public string Owner { get; set; }
	}
}