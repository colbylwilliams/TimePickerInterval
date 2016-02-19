using System;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using CoreGraphics;
using UIKit;
using Foundation;

using TimePickerInterval;
using TimePickerInterval.iOS;

[assembly: ExportRenderer (typeof (IntervalTimePicker), typeof (IntervalTimePickerRenderer))]
namespace TimePickerInterval.iOS
{
	class HideCaretTextField : UITextField
	{
		public HideCaretTextField (CGRect rect) : base (rect)
		{

		}

		public override CGRect GetCaretRectForPosition (UITextPosition position)
		{
			return new CGRect ();
		}
	}

	public class IntervalTimePickerRenderer : ViewRenderer<IntervalTimePicker, UITextField>
	{

		UIDatePicker datePicker;

		protected override void OnElementChanged (ElementChangedEventArgs<IntervalTimePicker> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement != null) {

				if (Control == null) {

					var picker = e.NewElement as IntervalTimePicker;

					if (picker != null) {

						datePicker = new UIDatePicker {
							Mode = UIDatePickerMode.Time,
							MinuteInterval = picker.Interval
						};

						var textField = new HideCaretTextField (picker.Bounds.ToRectangleF ());
						textField.BorderStyle = UITextBorderStyle.RoundedRect;
						textField.InputView = datePicker;

						datePicker.ValueChanged += DatePicker_ValueChanged;

						SetNativeControl (textField);
					}

					setTime ();
				}
			}
		}


		void DatePicker_ValueChanged (object sender, EventArgs e)
		{
			setTime ();
		}


		void setTime ()
		{
			nint hour, minute, second, nanosecond;

			NSCalendar.CurrentCalendar.GetHourComponentsFromDate (out hour, out minute, out second, out nanosecond, datePicker.Date);

			var roundedMinutes = Math.Round ((minute + (hour * 60.0)) / Element.Interval) * Element.Interval;

			var timeSpan = TimeSpan.FromMinutes (roundedMinutes);

			Control.Text = DateTime.Today.Add (timeSpan).ToString ("t");
		}
	}
}