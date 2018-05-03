using System;
using System.Collections.Generic;
using Plate.Trainer.DirectoryManager;

namespace Plate.Trainer.Model
{
	public abstract class BaseObject : PropertyNotifier
	{
		#region Privates

		private IDictionary<string, object> __values = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

		#endregion

		#region Methods

		public T GetValue<T>(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return default(T);
			}
			var value = this.GetValue(key);
			if (value is T)
			{
				return (T)value;
			}
			return default(T);
		}

		private object GetValue(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			if (this.__values.ContainsKey(key))
			{
				return this.__values[key];
			}
			return null;
		}

		public void SetValue(string key, object value)
		{
			if (!this.__values.ContainsKey(key))
			{
				this.__values.Add(key, value);
			}
			else
			{
				this.__values[key] = value;
			}
			base.OnPropertyChanged(key);
		}

		#endregion
	}
}
