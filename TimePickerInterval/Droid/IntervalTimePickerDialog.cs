﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Widget;

namespace TimePickerInterval.Droid
{
	public class IntervalTimePickerDialog : TimePickerDialog
	{
		int interval;
		TimePicker timePicker;
		IOnTimeSetListener listener;

		int hourOfDay, minute;

		public IntervalTimePickerDialog (Context context, IOnTimeSetListener listener, int hourOfDay, int minute, bool is24HourView, int interval)
			: base (context, Resource.Style.IntervalPickerTheme, listener, hourOfDay, minute/interval, is24HourView)
		{
			this.interval = interval;
			this.listener = listener;
			this.hourOfDay = hourOfDay;
			this.minute = minute;
		}

		public override void OnTimeChanged (TimePicker view, int hourOfDay, int minute)
		{
			base.OnTimeChanged (view, hourOfDay, minute * interval);
		}

		public override void OnClick (IDialogInterface dialog, int which)
		{
			//base.OnClick (dialog, which);

			timePicker?.ClearFocus ();
			var hour = timePicker.CurrentHour.IntValue ();
			var minute = timePicker.CurrentMinute.IntValue();
			listener?.OnTimeSet (timePicker, hour, minute * interval);
		}

		protected override void OnStop ()
		{
			//base.OnStop ();
		}

		public override void OnAttachedToWindow ()
		{
			base.OnAttachedToWindow ();

			Java.Lang.Class classForId = Java.Lang.Class.ForName ("com.android.internal.R$id");
			Java.Lang.Reflect.Field timePickerField = classForId.GetField ("timePicker");

			timePicker = FindViewById<TimePicker> (timePickerField.GetInt (null));

			Java.Lang.Reflect.Field field = classForId.GetField ("minute");

			NumberPicker mMinuteSpinner = timePicker.FindViewById<NumberPicker> (field.GetInt (null));
			if (mMinuteSpinner != null) {

				mMinuteSpinner.MinValue = 0;
				mMinuteSpinner.MaxValue = (60 / interval) - 1;

				List<string> displayedValues = new List<string> ();

				for (int i = 0; i < 60; i += interval) {
					displayedValues.Add (i.ToString ("00"));
				}

				mMinuteSpinner.SetDisplayedValues (displayedValues.ToArray ());
			}
			OnTimeChanged (timePicker, hourOfDay, minute/interval);
		}
	}
}