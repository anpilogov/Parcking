using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Plate.Trainer.Data
{
	[XmlRoot("annotation")]
	public class Annotation
	{
		public Annotation()
		{
		}

		public Annotation(FileInfo info)
		{
			var folder = string.Empty;
			if (info.Directory != null)
				folder = info.Directory.Name;

			FullName = info.FullName;
			FileName = info.Name;
			Folder = folder;
			Size = GetImageDimensions(info.FullName);
			ObjectsList = new List<RegionObject>();
		}

		[XmlIgnore]
		public string FullName { get; set; }

		[XmlElement("filename")]
		public string FileName { get; set; }
		[XmlElement("folder")]
		public string Folder { get; set; }
		[XmlElement("size")]
		public Size Size { get; set; }
		[XmlElement("object")]
		public RegionObject[] Objects
		{
			get { return ObjectsList.ToArray(); }
			set { ObjectsList = value.ToList(); }
		}

		[XmlIgnore]
		public List<RegionObject> ObjectsList;

		public RegionObject AddRegion(int sX, int sY, int eX, int eY, BitmapImage bitmap)
		{
			var newRegion = new RegionObject(sX, sY, eX, eY, bitmap);
			ObjectsList.Add(newRegion);
			return newRegion;
		}

		public RegionObject UpdateRegion(int index, int sX, int sY, int eX, int eY)
		{
			var region = ObjectsList[index];
			region.StartX = sX;
			region.StartY = sY;
			region.EndX = eX;
			region.EndY = eY;
			return region;
		}

		public bool RemoveRegion(object regionObj)
		{
			var region = regionObj as RegionObject;
			if (region == null)
				return false;

			ObjectsList.Remove(region);
			return true;
		}

		public void ClearRegions()
		{
			ObjectsList.Clear();
		}

		public Size GetImageDimensions(string path)
		{
			using (var imageStream = File.OpenRead(path))
			{
				var decoder = BitmapDecoder.Create(imageStream, BitmapCreateOptions.IgnoreColorProfile,
					BitmapCacheOption.Default);
				var size = new Size
				{
					Width = decoder.Frames[0].PixelWidth,
					Height = decoder.Frames[0].PixelHeight
				};
				return size;
			}
		}

		public void FillMissingValues(FileInfo info, BitmapImage bitmapImage)
		{
			FullName = info.FullName;
			Size = GetImageDimensions(info.FullName);
			foreach (var regionObject in ObjectsList)
			{
				regionObject.OwnerImage = bitmapImage;
			}
		}
	}

	[XmlRoot("size")]
	public class Size
	{
		[XmlElement("width")]
		public int Width { get; set; }
		[XmlElement("height")]
		public int Height { get; set; }
	}
}
