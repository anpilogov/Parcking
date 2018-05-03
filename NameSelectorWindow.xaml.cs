using System.Windows;
using Plate.Trainer.ViewModel;

namespace Plate.Trainer
{
	public partial class NameSelectorWindow : Window
	{
		public NameSelectorWindow()
		{
			InitializeComponent();
			Loaded += NameSelectorWindow_OnLoaded;
			RegionName.EnterKeyDown += SaveAndClose;
			//EnterCommand.InputGestures.Add(new KeyGesture(Key.Enter));
		}

		private void NameSelectorWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			RegionName.OnApplyTemplate();
		}

		private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
		{
			SaveAndClose();
		}

		private void SaveAndClose(object obj = null)
		{
			((ImageItemsViewModel)DataContext).NameBuffer = RegionName.Text;
			((ImageItemsViewModel)DataContext).UpdateNamesArray(RegionName.Text);
			Close();
		}

		#region Hotkeys example

		//public static RoutedCommand EnterCommand = new RoutedCommand();

		//private void EnterCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		//{
		//	SaveAndClose();
		//}

		#endregion
	}
}