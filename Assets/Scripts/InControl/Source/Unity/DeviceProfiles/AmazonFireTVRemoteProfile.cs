﻿using System;


namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class AmazonFireTVRemote : UnityInputDeviceProfile
	{
		public AmazonFireTVRemote()
		{
			Name = "Amazon Fire TV Remote";
			Meta = "Amazon Fire TV Remote on Amazon Fire TV";

			SupportedPlatforms = new[] {
				"Amazon AFTB",
				"Amazon AFTM"
			};

			JoystickNames = new[] {
				"",
				"Amazon Fire TV Remote"
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "Back",
					Target = InputControlType.Select,
					Source = EscapeKey
				}
			};

			AnalogMappings = new[] {
				DPadLeftMapping( Analog4 ),
				DPadRightMapping( Analog4 ),
				DPadUpMapping( Analog5 ),
				DPadDownMapping( Analog5 ),
			};
		}
	}
	// @endcond
}

