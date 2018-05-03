using System.IO;
using System.Windows;
using Plate.Trainer.DirectoryManager;
using Plate.Trainer.Model;

namespace Plate.Trainer
{
	/// <summary>
	/// Interaction logic for DirectoryDialogWindow.xaml
	/// </summary>
	public partial class DirectoryDialogWindow : Window
	{
		private string _lastPath;

		public DirectoryDialogWindow()
		{
			InitializeComponent();
			var drives = DriveInfo.GetDrives();
			foreach (var drive in drives)
			{
				this.DirectoryTree.Items.Add(new FileSystemObjectInfo(drive));
			}
			Loaded += DirectoryDialogWindow_OnLoaded;
		}

		private void DirectoryDialogWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			//_lastPath = ((ImageItemsViewModel) DataContext).Path;
		}

		private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
		{
			var selected = DirectoryTree.SelectedItem as FileSystemObjectInfo;
			if (selected != null)
			{
				((DirectoryKeeper) DataContext).Path = selected.FileSystemInfo.FullName;
				Close();
			}
		}

		private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
		{
			((DirectoryKeeper) DataContext).Path = _lastPath;
			Close();
		}

		private void DirectoryTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			var selected = DirectoryTree.SelectedItem as FileSystemObjectInfo;
			if (selected != null)
			{
				((DirectoryKeeper) DataContext).Path = selected.FileSystemInfo.FullName;
			}
		}
	}
}