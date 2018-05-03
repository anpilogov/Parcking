using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using Plate.Trainer.Annotations;

namespace Plate.Trainer.Data
{
	[XmlRoot("object")]
	public class RegionObject : INotifyPropertyChanged
	{
		public RegionObject()
		{
		}

		public RegionObject(int startX, int startY, int endX, int endY, BitmapImage ownerImage)
		{
			BndBox = new BoundBox();
			OwnerImage = ownerImage;
			BndBox.Xmin = startX;
			BndBox.Ymin = startY;
			BndBox.Xmax = endX;
			BndBox.Ymax = endY;
			_name = string.Empty;
			_truncated = false;
			_difficult = false;
			PropertyChanged = null;
		}

		[XmlIgnore]
		public BitmapImage OwnerImage;

		[XmlElement("bndbox")]
		public BoundBox BndBox;

		private string _name;
		private bool _truncated;
		private bool _difficult;

		[XmlIgnore]
		public double Height => EndY - StartY;
		[XmlIgnore]
		public double Width => EndX - StartX;

		[XmlIgnore]
		public bool Truncated
		{
			get { return _truncated; }
			set
			{
				_truncated = value;
				OnPropertyChanged(nameof(Truncated));
			}
		}
		[XmlElement("truncated")]
		public string TruncatedSerialize
		{
			get { return this.Truncated ? "1" : "0"; }
			set { this.Truncated = XmlConvert.ToBoolean(value); }
		}

		[XmlIgnore]
		public bool Difficult
		{
			get { return _difficult; }
			set
			{
				_difficult = value;
				OnPropertyChanged(nameof(Difficult));
			}
		}
		[XmlElement("difficult")]
		public string DifficultSerialize
		{
			get { return this.Difficult ? "1" : "0"; }
			set { this.Difficult = XmlConvert.ToBoolean(value); }
		}

		[XmlElement("name")]
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		[XmlIgnore]
		public int StartX
		{
			get { return BndBox.Xmin; }
			set
			{
				BndBox.Xmin = value;
				OnPropertyChanged(nameof(StartX));
			}
		}

		[XmlIgnore]
		public int StartY
		{
			get { return BndBox.Ymin; }
			set
			{
				BndBox.Ymin = value;
				OnPropertyChanged(nameof(StartY));
			}
		}

		[XmlIgnore]
		public int EndX
		{
			get { return BndBox.Xmax; }
			set
			{
				BndBox.Xmax = value;
				OnPropertyChanged(nameof(EndX));
			}
		}

		[XmlIgnore]
		public int EndY
		{
			get { return BndBox.Ymax; }
			set
			{
				BndBox.Ymax = value;
				OnPropertyChanged(nameof(EndY));
			}
		}

		public void CheckAndFixRegionValues()
		{
			if (StartX < 0)
				StartX = 0;
			if (StartY < 0)
				StartY = 0;
			if (EndX > OwnerImage.Width)
				EndX = (int)OwnerImage.Width;
			if (EndY > OwnerImage.Height)
				EndY = (int)OwnerImage.Height;
			if (StartX > EndX)
			{
				var temp = StartX;
				StartX = EndX;
				EndX = temp;
			}
			if (StartY > EndY)
			{
				var temp = StartY;
				StartY = EndY;
				EndY = temp;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class BoundBox
	{
		[XmlElement("xmin")]
		public int Xmin { get; set; }
		[XmlElement("ymin")]
		public int Ymin { get; set; }
		[XmlElement("xmax")]
		public int Xmax { get; set; }
		[XmlElement("ymax")]
		public int Ymax { get; set; }
	}
}