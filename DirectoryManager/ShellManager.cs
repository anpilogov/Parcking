using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Plate.Trainer.DirectoryManager
{
	public class ShellManager
	{
		public static Icon GetIcon(string path, ItemType type, IconSize size, ItemState state)
		{
			var flags = (uint)(Interop.SHGFI_ICON | Interop.SHGFI_USEFILEATTRIBUTES);
			var attribute = (uint)(object.Equals(type, ItemType.Folder) ? Interop.FILE_ATTRIBUTE_DIRECTORY : Interop.FILE_ATTRIBUTE_FILE);
			if (object.Equals(type, ItemType.Folder) && object.Equals(state, ItemState.Open))
			{
				flags += Interop.SHGFI_OPENICON;
			}
			if (object.Equals(size, IconSize.Small))
			{
				flags += Interop.SHGFI_SMALLICON;
			}
			else
			{
				flags += Interop.SHGFI_LARGEICON;
			}
			var shfi = new Interop.SHFileInfo();
			var res = Interop.SHGetFileInfo(path, attribute, out shfi, (uint)Marshal.SizeOf(shfi), flags);
			if (object.Equals(res, IntPtr.Zero)) throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
			try
			{
				Icon.FromHandle(shfi.hIcon);
				return (Icon)Icon.FromHandle(shfi.hIcon).Clone();
			}
			catch
			{
				throw;
			}
			finally
			{
				Interop.DestroyIcon(shfi.hIcon);
			}
		}
	}

	public enum IconSize : short
	{
		Small,
		Large
	}

	public enum ItemState : short
	{
		Undefined,
		Open,
		Close
	}

	public enum ItemType : short
	{
		Folder,
		File
	}
}
