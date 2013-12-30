using System.Configuration;
namespace AnonymousWeb.Common
{
	internal static class Settings
	{
		/// <summary>This struct contains the keys we use in web.config.
		/// </summary>
		private struct WebConfigKeys
		{
			internal const string DownloadPathKey = "DownloadPath";

			internal const string RepositoryPathKey = "RepositoryPath";

			internal const string UploadPathKey = "UploadPath";
		}

		/// <summary>This getter returns the Download path.
		/// That is where we temporarly store the files before streaming them to the user.
		/// We don't want to burden the repository with streaming stuff.
		/// </summary>
		internal static string DownloadPath
		{
			get
			{
				return ConfigurationManager.AppSettings[WebConfigKeys.DownloadPathKey];
			}
		}

		/// <summary>This getter returns the path for the Repository.
		/// </summary>
		internal static string RepositoryPath
		{
			get
			{
				return ConfigurationManager.AppSettings[WebConfigKeys.RepositoryPathKey];
			}
		}

		/// <summary>This getter is where uploaded files first land
		/// before being transferred to the repository.
		/// </summary>
		internal static string UploadPath
		{
			get
			{
				return ConfigurationManager.AppSettings[WebConfigKeys.UploadPathKey];
			}
		}

	}
}