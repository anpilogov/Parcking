using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Plate.Trainer.Model
{
	/// <summary>
	/// Своя реализация автокомплита с подпиской на событие Enter и фокусом
	/// </summary>
	public class CustomizedAutoCompleteBox :AutoCompleteBox
	{
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			var textbox = Template.FindName("Text", this) as TextBox;
			if (textbox != null) textbox.Focus();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Key == Key.Enter) RaiseEnterKeyDownEvent();
		}

		public event Action<object> EnterKeyDown;
		private void RaiseEnterKeyDownEvent()
		{
			var handler = EnterKeyDown;
			if (handler != null) handler(this);
		}
	}
}
