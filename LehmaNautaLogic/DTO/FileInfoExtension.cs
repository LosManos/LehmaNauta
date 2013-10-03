using System;

namespace LehmaNautaLogic.DTO
{
	public static class FileInfoExtension
	{
		public static Guid EnsureID(this FileInfo me)
		{
			if (me.Id == Guid.Empty)
			{
				me.Id = Guid.NewGuid();
			}
			return me.Id;
		}

		public static FileInfo Set(
			this FileInfo me, 
			string filename,
			string owner)
		{
			me.Filename = filename;
			me.Created = me.Created > DateTime.Now ? me.Created : DateTime.Now;
			me.Owner = owner;
			return me;
		}
	}
}