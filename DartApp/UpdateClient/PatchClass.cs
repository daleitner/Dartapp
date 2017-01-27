using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateClient
{
	public class PatchClass
	{
		private Dictionary<string, List<string>> patches;

		public PatchClass()
		{
			this.patches = new Dictionary<string, List<string>>();
			var list1 = new List<string> {Patch1, Patch2};
			this.patches.Add("1.0", list1);
		}

		public List<string> GetPatches(string version)
		{
			return this.patches[version];
		} 

		#region patches
		//Verson 1.0
		private const string Patch1 = "CREATE TABLE VersionTable(Id INTEGER PRIMARY KEY, Version VARCHAR(255));";
		private const string Patch2 = "INSERT INTO VersionTable VALUES (\"1\",\"1.0\");";

		#endregion

		#region VersionList
		private readonly List<string> versionList = new List<string> {"0.9", "1.0"};

		public List<string> GetVersionsForUpdate(string actVersion)
		{
			var pos = this.versionList.IndexOf(actVersion);
			if (pos >= 0)
				return this.versionList.GetRange(pos + 1, this.versionList.Count - (pos + 1));
			return null;
		}

		public string GetLatestVersion()
		{
			return this.versionList.Last();
		}
		#endregion
	}
}
