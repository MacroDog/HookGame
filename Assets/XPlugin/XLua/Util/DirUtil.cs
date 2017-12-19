using System.IO;

public static class DirUtil
{
	public static void CreateDir(string path)
	{
		string[] pathes = path.Split('/');
		if (pathes.Length > 0) {
			string curPath = pathes[0];
			for (int i = 1; i < pathes.Length; i++) {
				curPath += "/" + pathes[i];
				if (!Directory.Exists(curPath)) {
					Directory.CreateDirectory(curPath);
				}
			}
		}
	}

	public static void CreateDirForFile(string file)
	{
		string path = file.Substring(0, file.LastIndexOf("/"));
		CreateDir(path);
	}
}
