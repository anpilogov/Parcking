using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Plate.Trainer.Data
{
	public class ImageItem
	{
		public static readonly string AnnotationsFolder = "Annotations";
		public Annotation Annotation { get; set; }

		public IReadOnlyCollection<RegionObject> Regions => Annotation.ObjectsList.AsReadOnly();
		public BitmapImage BitmapImage { get; private set; }

		public ImageItem(FileInfo info)
		{
			BitmapImage = new BitmapImage(new Uri(info.FullName));
			Annotation = GetAnnotation(info, BitmapImage);
		}

		public RegionObject AddRegion(int sX, int sY, int eX, int eY)
		{
			return Annotation.AddRegion(sX, sY, eX, eY, BitmapImage);
		}

		public RegionObject UpdateRegion(int index, int sX, int sY, int eX, int eY)
		{
			return Annotation.UpdateRegion(index, sX, sY, eX, eY);
		}

		public bool RemoveRegion(object region)
		{
			return Annotation.RemoveRegion(region);
		}

		public void ClearRegions()
		{
			Annotation.ObjectsList.Clear();
		}

		public Annotation GetAnnotation(FileInfo info, BitmapImage bitmapImage)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Annotation));
			//если понадобятся события
			//serializer.UnknownNode += new	XmlNodeEventHandler(serializer_UnknownNode);
			//serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);
			try
			{
				using (FileStream fs = new FileStream(GetXmlPath(info.FullName), FileMode.Open))
				{
					Annotation result;
					result = (Annotation)serializer.Deserialize(fs);
					result.FillMissingValues(info, bitmapImage);
					return result;
				}
			}
			catch (Exception e)
			{
				return new Annotation(info);
			}
		}

		public void SaveAnnotation()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Annotation));

			using (TextWriter writer = new StreamWriter(GetXmlPath(Annotation.FullName)))
			{
				serializer.Serialize(writer, Annotation);
				writer.Close();
			}
		}

		private void CheckAndCreateDirForFile(string filePath)
		{
			var dividedDir = filePath.Split('\\').ToList();
			dividedDir.Remove(dividedDir.Last());
			var path = String.Join("\\", dividedDir);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}

		private string GetXmlPath(string imagePath)
		{
			//TODO оптимизировать построение пути
			var dividedPath = imagePath.Split('\\').ToList();
			var fileName = dividedPath.Last();
			dividedPath.Remove(fileName);
			dividedPath.Add(AnnotationsFolder);
			dividedPath.Add(fileName);
			string wFolder = String.Join("\\", dividedPath);

			var pathParts = wFolder.Split('.').ToList();
			if (!pathParts.Any())
				return null;
			pathParts.Remove(pathParts.Last());
			pathParts.Add("xml");
			var path = String.Join(".", pathParts);
			CheckAndCreateDirForFile(path);
			return path;
		}
	}
}