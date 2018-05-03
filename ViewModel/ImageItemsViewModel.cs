using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Plate.Trainer.Data;
using Plate.Trainer.Model;

namespace Plate.Trainer.ViewModel
{
	class ImageItemsViewModel : DependencyObject
	{
		private static readonly string[] ImageExtensions = new[] { ".jpg", ".bmp", ".png" };
		private static readonly string NamesFile = "predifined_classes.txt";
		public DirectoryKeeper DirKeep { get; set; }
		public string NameBuffer { get; set; }


		public ImageItemsViewModel()
		{
			DirKeep = new DirectoryKeeper();
			DirKeep.Path = (string)Properties.Settings.Default["Path"];
			RefreshImages();
		}

		private int _lastIndex = -1;

		public bool IsRegAny()
		{
			if (_lastIndex != -1 && Images != null )
			{
				var imageItems = Images.Cast<ImageItem>().ToArray();
				if (!imageItems.Any())
					return false;
				
				return imageItems[_lastIndex].Regions.Any();
			}
			return false;
		}

		public string[] NamesList
		{
			get
			{
				return GetNamesArray();
			}
			private set
			{
				value = null;
			}
		}

		private string[] GetNamesArray()
		{
			try
			{
				var lines = File.ReadAllLines(NamesFile);
				return lines;
			}
			catch (Exception e)
			{
				return new string[0];
			}
		}

		public void UpdateNamesArray(string name)
		{
			List<string> existed = GetNamesArray().ToList();
			if (!existed.Contains(name))
			{
				existed.Add(name);
				try
				{
					File.WriteAllLines(NamesFile, existed.ToArray());
				}
				catch (Exception e)
				{
					throw;
				}
			}
		}

		public void RefreshImages()
		{
			if (!string.IsNullOrWhiteSpace(DirKeep.Path))
			{
				Images = CollectionViewSource.GetDefaultView(LoadImages(DirKeep.Path));
			}
		}

		public void RefreshRegions(int index)
		{
			if (index == -1)
			{
				CurrentItemRegions = null;
			}
			else
			{
				var imageItems = Images.Cast<ImageItem>().ToList();
				CurrentItemRegions = CollectionViewSource.GetDefaultView(imageItems[index].Regions);
				_lastIndex = index;
			}
		}

		public void CheckAndFixRegionValues(int index)
		{
			var imageItems = Images.Cast<ImageItem>().ToList();
			var regions = CollectionViewSource.GetDefaultView(imageItems[index].Regions).Cast<RegionObject>();
			foreach (var regionPosition in regions)
			{
				regionPosition.CheckAndFixRegionValues();
			}
		}

		public static ImageItem[] LoadImages(string folder)
		{
			var result = new List<ImageItem>();

			try
			{
				DirectoryInfo dir = new DirectoryInfo(folder);
				var files = dir.GetFiles();
				foreach (var fileInfo in files)
				{
					try
					{
						if (ImageExtensions.Contains(fileInfo.Extension))
							result.Add(new ImageItem(fileInfo));
					}
					catch (NotSupportedException e)
					{
					}
				}
			}
			catch (Exception e)
			{
				return null;
			}

			return result.ToArray();
		}

		public ICollectionView Images
		{
			get { return (ICollectionView)GetValue(ImagesProperty); }
			set { SetValue(ImagesProperty, value); }
		}

		public static readonly DependencyProperty ImagesProperty =
			DependencyProperty.Register("Images", typeof(ICollectionView), typeof(ImageItemsViewModel), new PropertyMetadata(null));

		public ICollectionView CurrentItemRegions
		{
			get { return (ICollectionView)GetValue(RegionsProperty); }
			set { SetValue(RegionsProperty, value); }
		}

		public static readonly DependencyProperty RegionsProperty =
			DependencyProperty.Register("CurrentItemRegions", typeof(ICollectionView), typeof(ImageItemsViewModel), new PropertyMetadata(null));
	}
}
