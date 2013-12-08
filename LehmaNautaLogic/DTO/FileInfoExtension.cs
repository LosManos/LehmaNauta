using System;

namespace LehmaNautaLogic.DTO
{
	public static class FileInfoExtension
	{
		/// <summary>This method ensures we have an ID.
		/// If there already is an ID we let it be.
		/// </summary>
		/// <param name="me"></param>
		/// <returns></returns>
		public static Guid EnsureID(this FileInfo me)
		{
			if (me.Id == Guid.Empty)
			{
				me.Id = Guid.NewGuid();
			}
			return me.Id;
		}

		/// <summary>This method sets all properties
		/// for a fully populated object.
		/// </summary>
		/// <param name="me"></param>
		/// <param name="filename"></param>
		/// <param name="owner"></param>
		/// <returns></returns>
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