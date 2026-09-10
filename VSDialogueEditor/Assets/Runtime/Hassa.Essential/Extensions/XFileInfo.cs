using System.IO;

namespace Hassa.Essentials
{
	public static class XFileInfo
	{

		public static string[] Split(this FileInfo info)
		{
			return info.FullName.Replace("\\", "/").Split('/');
		}

		public static bool RelativeTo(this FileInfo info, DirectoryInfo dir, out string[] relative)
		{
			if (info.FullName.StartsWith(dir.FullName)) {
				relative = info.FullName.Substring(dir.FullName.Length+1).Split('\\');
				return true;
			}

			relative = null;
			return false;
		}

		public static DirectoryInfo Parent(this FileInfo info)
		{
			var parts = info.Split();
			return new DirectoryInfo(string.Join("/", parts, 0, parts.Length - 1));
		}

		public static string Stem(this FileInfo info)
		{
			return (!string.IsNullOrEmpty(info.Extension)) ? info.Name.Substring(0, info.Name.Length - info.Extension.Length) : info.Name;
		}

		public static FileInfo Combine(this FileInfo info, DirectoryInfo dir, string next)
		{
			return new FileInfo(Path.Combine(dir.FullName, next));
		}

		public static FileInfo Combine(this FileInfo info, DirectoryInfo dir, FileInfo next)
		{
			return new FileInfo(Path.Combine(dir.FullName, next.FullName));
		}

		public static FileInfo Combine(this FileInfo info, string next)
		{
			return new FileInfo(Path.Combine(info.FullName, next));
		}

		public static FileInfo Combine(this FileInfo info, string[] next)
		{
			string result = info.FullName;
			foreach (var item in next) {
				result = Path.Combine(result, item);
			}

			return new FileInfo(result);
		}

		public static FileInfo Combine(this FileInfo info, FileInfo next)
		{
			return Combine(info, next.FullName);
		}

		public static FileInfo Combine(this FileInfo info, FileInfo[] next)
		{
			string result = info.FullName;
			foreach (var item in next) {
				result = Path.Combine(result, item.FullName);
			}

			return new FileInfo(result);
		}
	}
}