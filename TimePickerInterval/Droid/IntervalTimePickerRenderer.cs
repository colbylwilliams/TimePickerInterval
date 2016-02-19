using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Android.App;
using Android.Widget;


using TimePickerInterval;
using TimePickerInterval.Droid;

[assembly: ExportRenderer (typeof (IntervalTimePicker), typeof (IntervalTimePickerRenderer))]
namespace TimePickerInterval.Droid
{
	public class IntervalTimePickerRenderer : ViewRenderer<IntervalTimePicker, EditText>, TimePickerDialog.IOnTimeSetListener
	{

		protected override void OnElementChanged (ElementChangedEventArgs<IntervalTimePicker> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement == null) {

				var editText = new EditText (Context) {
					Clickable = true,
					Focusable = false
				};

				editText.Click += EditText_Click;

				SetNativeControl (editText);
			}

			setTime (TimeSpan.FromSeconds (0));
		}

		IntervalTimePickerDialog dialog;

		void EditText_Click (object sender, EventArgs e)
		{
			dialog = new IntervalTimePickerDialog (Context, this, 0, 0, true, Element.Interval);

			dialog.Show ();
		}


		public void OnTimeSet (Android.Widget.TimePicker view, int hourOfDay, int minute)
		{
			var timeSpan = new TimeSpan (hourOfDay, minute, 0);
			setTime (timeSpan);
		}


		void setTime (TimeSpan timeSpan)
		{
			Control.Text = DateTime.Today.Add (timeSpan).ToString ("t");
		}
	}
}

