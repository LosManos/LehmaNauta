using System;

namespace LehmaNautaLogic.DTO
{
	public class FileInformation
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public string Filename { get; set; }
		public string Owner { get; set; }
	}
}