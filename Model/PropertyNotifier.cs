using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Plate.Trainer.Model
{
	[Serializable]
	public abstract class PropertyNotifier : INotifyPropertyChanged
	{
		#region Events

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public PropertyNotifier()
			: base()
		{
			this.AllowRaiseEvent = true;
		}

		#region Properties

		[XmlIgnore]
		protected bool AllowRaiseEvent { get; set; }

		#endregion

		#region INotifyPropertyChanged Members

		protected void OnPropertyChanged(string propertyName)
		{
			if (this.AllowRaiseEvent)
			{
				if (!object.ReferenceEquals(this.PropertyChanged, null))
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				}
			}
		}

		#endregion
	}
}