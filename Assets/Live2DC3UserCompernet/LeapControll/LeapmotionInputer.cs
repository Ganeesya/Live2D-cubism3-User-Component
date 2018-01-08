using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;


namespace Ganeesyan.Cubism3Compornets
{
	public class LeapmotionInputer : MonoBehaviour
	{
		protected Controller controller;
		public Camera leapCam;

		public Controller Controller { get { return controller; } }

		// Use this for initialization
		void Start()
		{
			createController();
		}

		protected virtual void OnDestroy()
		{
			destroyController();
		}

		protected virtual void OnApplicationPause(bool isPaused)
		{
			if (controller != null)
			{
				if (isPaused)
				{
					controller.StopConnection();
				}
				else
				{
					controller.StartConnection();
				}
			}
		}

		/*
		 * Initializes the Leap Motion policy flags.
		 * The POLICY_OPTIMIZE_HMD flag improves tracking for head-mounted devices.
		 */
		protected void initializeFlags()
		{
			if (controller == null)
			{
				return;
			}
			//Optimize for top-down tracking if on head mounted display.
			controller.ClearPolicy(Controller.PolicyFlag.POLICY_OPTIMIZE_HMD);
		}

		/** Create an instance of a Controller, initialize its policy flags
		 * and subscribe to connection event */
		protected void createController()
		{
			if (controller != null)
			{
				destroyController();
			}

			controller = new Controller();
			if (controller.IsConnected)
			{
				initializeFlags();
			}
			else
			{
				controller.Device += onHandControllerConnect;
			}
		}

		protected void onHandControllerConnect(object sender, LeapEventArgs args)
		{
			initializeFlags();
			controller.Device -= onHandControllerConnect;
		}

		/** Calling this method stop the connection for the existing instance of a Controller, 
		 * clears old policy flags and resets to null */
		protected void destroyController()
		{
			if (controller != null)
			{
				if (controller.IsConnected)
				{
					controller.ClearPolicy(Controller.PolicyFlag.POLICY_OPTIMIZE_HMD);
				}
				controller.StopConnection();
				controller = null;
			}
		}

		public Hand getHanddata(bool isLeft)
		{
			if (controller != null)
			{
				if (leapCam == null)
				{
					return controller.Frame().Hands.Find(x => x.IsLeft == isLeft);
				}
				else
				{
					return controller.GetTransformedFrame(new LeapTransform(leapCam.transform.position.ToVector(), leapCam.transform.rotation.ToLeapQuaternion())).Hands.Find(x => x.IsLeft == isLeft);
				}
			}

			return null;
		}
	}
}