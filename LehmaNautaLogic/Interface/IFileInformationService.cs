using System;

namespace LehmaNautaLogic.Inferface
{
	public interface IFileInformationService
	{
		Guid Create(DTO.FileInformation fileInfo);
		void Delete(Guid id);
		void DeleteOld();
		void EnsureDatabaseExists();
		System.Collections.Generic.IList<DTO.FileInformation> IT_GetAll();
		DTO.FileInformation Load(Guid id);
	}
}
