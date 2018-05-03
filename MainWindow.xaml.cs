using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Plate.Trainer.Data;
using Plate.Trainer.ViewModel;

namespace Plate.Trainer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static RoutedCommand PrevCommand = new RoutedCommand();
		public static RoutedCommand NextCommand = new RoutedCommand();

		public MainWindow()
		{
			InitializeComponent();
			Canvas.MouseMove += canvas_MouseMove;
			Canvas.MouseUp += canvas_MouseUp;
			Canvas.MouseDown += canvas_MouseDown;
			Loaded += MainWindow_Loaded;
			ThumbnailList.SelectionChanged += thumb_SelectionChanged;
			RegionsList.SelectionChanged += reg_SelectionChanged;
			_zoomFactor = 1;

			PrevCommand.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Alt));
			NextCommand.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Alt));
		}

		private double _multiplier;
		private double _zoomFactor;
		private double _initialWidth;

		private void RecalcSizes(bool noRects = false)
		{
			Canvas.Width = _initialWidth * _zoomFactor; //TODO проблема с пересчётами размеров, выезжающих элементов
			SourceImage.Width = _initialWidth * _zoomFactor;

			if (ThumbnailList.Items.Count != 0 && _thumbIndex != -1)
			{
				var curItem = (ImageItem)ThumbnailList.Items[_thumbIndex];
				_multiplier = curItem.Annotation.Size.Width / _initialWidth* 1/_zoomFactor; //Что-то сделать с этим пересчётом, чтобы он зависел только от размеров картинки
				Canvas.Height = curItem.Annotation.Size.Height / _multiplier;
				if (!Canvas.IsMouseCaptured)
				{
					if (!noRects)
					{
						RedrawRects();
					}
				}
			}
		}

		private void RedrawRects()
		{
			Canvas.Children.Clear();
			if (((ImageItem)ThumbnailList.Items[_thumbIndex]).Regions.Any())
			{
				var regTemp = ((ImageItem)ThumbnailList.Items[_thumbIndex]).Regions.ToArray();
				for (int i = 0; i < regTemp.Length; i++)
				{
					if (i == _regIndex)
					{
						DrawRect(regTemp[i], Brushes.OrangeRed);
						continue;
					}
					DrawRect(regTemp[i]);
				}
			}
		}

		private void DrawRect(RegionObject position, Brush brush = null, int thickness = 1)
		{
			if (brush == null) //cuz of time compilation issue
				brush = Brushes.Lime;

			Rectangle result = new Rectangle()
			{
				Stroke = brush,
				StrokeThickness = thickness
			};
			Canvas.Children.Add(result);

			Canvas.SetTop(result, position.StartY / (_multiplier));
			Canvas.SetLeft(result, position.StartX / (_multiplier));

			result.Height = Math.Abs(position.Height / (_multiplier));
			result.Width = Math.Abs(position.Width / (_multiplier));
		}

		private void ThumbPrevious()
		{
			if (_thumbIndex != -1)
				((ImageItem)ThumbnailList.Items[_thumbIndex]).SaveAnnotation();
			if (ThumbnailList.SelectedIndex != 0 && ThumbnailList.SelectedIndex != -1)
				ThumbnailList.SelectedIndex--;
		}

		private void ThumbNext()
		{
			if (_thumbIndex != -1)
				((ImageItem)ThumbnailList.Items[_thumbIndex]).SaveAnnotation();
			ThumbnailList.SelectedIndex++;
		}

		#region Events

		#region MainWindow

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			DataContext = new ImageItemsViewModel();
		}

		private void MainWindow_OnClosing(object sender, CancelEventArgs e)
		{
			if (_thumbIndex != -1 && ThumbnailList.Items.Count > 0)
				((ImageItem)ThumbnailList.Items[_thumbIndex]).SaveAnnotation();

			Properties.Settings.Default["Path"] = ((ImageItemsViewModel)DataContext).DirKeep.Path;
			Properties.Settings.Default.Save();
		}

		private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			_zoomFactor = 1;
			_initialWidth = GridImage.ActualWidth;
			RecalcSizes();
		}

		private void MainWindow_OnContentRendered(object sender, EventArgs e)
		{
			RecalcSizes();
		}

		#endregion

		private int _thumbIndex;


		private void thumb_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_initialWidth = GridImage.ActualWidth;
			_zoomFactor = 1;
			if (_thumbIndex != -1 && ThumbnailList.Items.Count!=0)
				((ImageItem)ThumbnailList.Items[_thumbIndex]).SaveAnnotation();
			Canvas.Children.Clear();
			_thumbIndex = ThumbnailList.SelectedIndex;
			if (_thumbIndex != -1 && ThumbnailList.Items.Count != 0)
			{
				((ImageItemsViewModel)DataContext).RefreshRegions(_thumbIndex);
				SourceImage.Source = ((ImageItem)ThumbnailList.Items[_thumbIndex]).BitmapImage;
			}
			else
			{
				SourceImage.Source = null;
			}
			_regIndex = -1;
			RecalcSizes();
		}

		private int _regIndex;

		private void reg_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_regIndex = RegionsList.SelectedIndex;
			if (_regIndex != -1)
			{
				RedrawRects();
			}
		}

		private void PrevCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			ThumbPrevious();
		}

		private void NextCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			ThumbNext();
		}

		#region Mouse

		private Rectangle _currentRect;
		private Point _anchorPoint;

		private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (_regIndex != -1 || !((ImageItemsViewModel)DataContext).IsRegAny())
			{
				Canvas.Children.Clear();
				Canvas.CaptureMouse();

				_anchorPoint = e.MouseDevice.GetPosition(Canvas);
				_currentRect = new Rectangle()
				{
					Stroke = Brushes.Goldenrod,
					StrokeThickness = 1
				};
				Canvas.Children.Add(_currentRect);
			}
		}

		private int _tempSx;
		private int _tempSy;
		private int _tempEx;
		private int _tempEy;

		private void canvas_MouseMove(object sender, MouseEventArgs e)
		{
			if (_regIndex != -1 || !((ImageItemsViewModel)DataContext).IsRegAny())
			{
				if (!Canvas.IsMouseCaptured)
					return;

				Point location = e.MouseDevice.GetPosition(Canvas);

				double minX = Math.Min(location.X, _anchorPoint.X);
				double minY = Math.Min(location.Y, _anchorPoint.Y);
				double maxX = Math.Max(location.X, _anchorPoint.X);
				double maxY = Math.Max(location.Y, _anchorPoint.Y);

				Canvas.SetTop(_currentRect, minY);
				Canvas.SetLeft(_currentRect, minX);

				double height = maxY - minY;
				double width = maxX - minX;

				_tempSx = (int)(minX * _multiplier);
				_tempSy = (int)(minY * _multiplier);
				_tempEx = (int)(maxX * _multiplier);
				_tempEy = (int)(maxY * _multiplier);

				_currentRect.Height = Math.Abs(height);
				_currentRect.Width = Math.Abs(width);
			}
		}

		private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
		{
			RegionObject regObj = null;
			if (_regIndex != -1)
			{
				Canvas.ReleaseMouseCapture();
				regObj = ((ImageItem)ThumbnailList.Items[_thumbIndex]).UpdateRegion(_regIndex, _tempSx, _tempSy, _tempEx, _tempEy);
				RecalcSizes();
			}
			else if (!((ImageItemsViewModel)DataContext).IsRegAny())
			{
				Canvas.ReleaseMouseCapture();
				regObj = ((ImageItem)ThumbnailList.Items[_thumbIndex]).AddRegion(_tempSx, _tempSy, _tempEx, _tempEy);
				RecalcSizes();
			}
			if (regObj != null && string.IsNullOrWhiteSpace(regObj.Name))
			{
				((ImageItemsViewModel)DataContext).NameBuffer = String.Empty;
				NameSelectorWindow nsw = new NameSelectorWindow
				{
					Owner = Application.Current.MainWindow,
					WindowStartupLocation = WindowStartupLocation.CenterOwner,
					DataContext = DataContext
				};
				nsw.ShowDialog();
				regObj.Name = ((ImageItemsViewModel)DataContext).NameBuffer;
			}
			((ImageItemsViewModel)DataContext).CurrentItemRegions.Refresh();
		}

		#endregion

		private void ButtonFolder_OnClick(object sender, RoutedEventArgs e)
		{
			DirectoryDialogWindow ddw = new DirectoryDialogWindow
			{
				Owner = Application.Current.MainWindow,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
				DataContext = ((ImageItemsViewModel)DataContext).DirKeep
			};
			ddw.ShowDialog();
			((ImageItemsViewModel)DataContext).RefreshImages();
		}

		private void ButtonPrevious_OnClick(object sender, RoutedEventArgs e)
		{
			ThumbPrevious();
		}

		private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
		{
			ThumbNext();
		}

		private void Region_OnLostFocus(object sender, RoutedEventArgs e)
		{
			((ImageItemsViewModel)DataContext).CheckAndFixRegionValues(_thumbIndex);
			((ImageItemsViewModel)DataContext).CurrentItemRegions.Refresh();
			RedrawRects();
		}

		private void AddRegion_OnClick(object sender, RoutedEventArgs e)
		{
			_thumbIndex = ThumbnailList.SelectedIndex;
			if (_thumbIndex != -1)
			{
				var newRegion = ((ImageItem)ThumbnailList.Items[_thumbIndex]).AddRegion(0, 0, 0, 0);
				((ImageItemsViewModel)DataContext).RefreshRegions(_thumbIndex);
				RegionsList.SelectedItem = newRegion;
			}
		}

		private void RegionItemRemove_OnClick(object sender, RoutedEventArgs e)
		{
			var containerFromElement = (ListBoxItem)RegionsList.ContainerFromElement((Button)sender);
			if (containerFromElement != null)
			{
				var curItem = containerFromElement.Content;
				if (((ImageItem)ThumbnailList.Items[_thumbIndex]).RemoveRegion(curItem))
				{
					_regIndex = -1;
					_thumbIndex = ThumbnailList.SelectedIndex;
					if (_thumbIndex != -1)
					{
						((ImageItemsViewModel)DataContext).RefreshRegions(_thumbIndex);
						RedrawRects();
					}
				}
			}
		}
		#endregion

		private void UpdateZoomFactor(bool zoomIn)
		{
			if (zoomIn)
			{
				if (_zoomFactor < 3)
				{
					_zoomFactor = _zoomFactor * 1.1;
					RecalcSizes();
				}
			}
			else
			{
				if (_zoomFactor > 0.15)
				{
					_zoomFactor = _zoomFactor * 0.9;
					RecalcSizes();
				}
			}
			ZoomText.Text = _zoomFactor.ToString();
		}

		private void ScrollViewer_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Delta > 0)
				UpdateZoomFactor(true);
			else if (e.Delta < 0)
				UpdateZoomFactor(false);

			e.Handled = true;
		}
	}
}