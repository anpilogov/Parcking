using System.ComponentModel;

namespace Plate.Trainer.Model
{
	public class DirectoryKeeper : BaseObject
	{
		public DirectoryKeeper()
		{
			this.PropertyChanged += DirectoryKeeper_PropertyChanged;
		}
		
		public string Path
		{
			get { return base.GetValue<string>("Path"); }
			set { base.SetValue("Path", value); }
		}

		public string LastPath
		{
			get { return base.GetValue<string>("LastPath"); }
			set { base.SetValue("LastPath", value); }
		}

		private void DirectoryKeeper_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
		}
	}
}
