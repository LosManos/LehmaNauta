using System;

namespace LehmaNautaLogic.DTO
{
	public class FileInformation
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public string Filename { get; set; }
		public string Owner { get; set; }

		public FileInformation(){		}

		public FileInformation(
			Guid id, 
			DateTime created, 
			string filename, 
			string owner ) 
		{
			Set(id, created, filename, owner);
		}

		private void Set(Guid id, DateTime created, string filename, string owner)
		{
			this.Id = id;
			this.Created = created;
			this.Filename = filename;
			this.Owner = owner;
		}
	}
}