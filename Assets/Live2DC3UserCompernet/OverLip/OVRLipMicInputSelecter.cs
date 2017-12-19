using UnityEngine;
using System.Collections;


namespace Ganeesyan.Cubism3Compornets
{
	public class OVRLipMicInputSelecter : OVRLipSyncMicInput
	{

		private bool micPickuped = false;

		// Use this for initialization
		void Start()
		{
			audioSource.loop = true;    // Set the AudioClip to loop
			audioSource.mute = false;

			if (Microphone.devices.Length != 0)
			{
				selectedDevice = Microphone.devices[0].ToString();
				micPickuped = true;
				GetMicCaps();
			}
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}