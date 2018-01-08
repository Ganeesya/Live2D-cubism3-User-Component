using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using Live2D.Cubism.Core;

namespace Ganeesyan.Cubism3Compornets
{
	public class StandAloneLeapControll : MonoBehaviour
	{
		public Controller controller;
		public bool isLeft;

		public Camera leapCam;

		[SerializeField]
		public List<PosePoint> palmPositionPosePoints;

		[SerializeField]
		private Vector3 palmPos;

		[SerializeField]
		private float lanceDot;

		[SerializeField]
		private float chopDot;

		[SerializeField]
		private float palmDot;

		[SerializeField, Multiline(10)]
		private string debugField;
		
		[SerializeField]
		private List<float> fingerLevels;

		private CubismModel model;
		private int targetPose = 0;
		private bool settingNow = false;
		private List<List<FingerExtendParameter>> fingerParams;

		#region testers
		public List<GameObject> posdatas;
		public GameObject palmtester;
		public GameObject targetertester;

		public GameObject testprehub;
		#endregion

		// Use this for initialization
		void Start()
		{
			createController();
			palmPos = new Vector3();
			model = GetComponentInParent<CubismModel>();


			posdatas = new List<GameObject>();
			foreach(var e in palmPositionPosePoints)
			{
				posdatas.Add(Instantiate(testprehub, e.position, Quaternion.identity));
			}
			palmtester = Instantiate(testprehub, palmPos, Quaternion.identity);
			targetertester = Instantiate(testprehub, new Vector3(0,0,0), Quaternion.identity);


			fingerLevels = new List<float>();

			fingerParams = new List<List<FingerExtendParameter>>();
			for(int i = 0; i <= (int)Finger.FingerType.TYPE_PINKY; i++)
			{
				fingerLevels.Add(0f);
				fingerParams.Add( new List<FingerExtendParameter>() );
				foreach (var e in model.GetComponentsInChildren<FingerExtendParameter>())
				{
					if( (e.type == (Finger.FingerType)i) && e.isLeft == isLeft )
					{
						fingerParams[i].Add(e);
					}
				}
			}

			Debug.Log("StandAlone : Start()");
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

		// Update is called once per frame
		void Update()
		{
			if (controller != null)
			{
				var handdata = controller.Frame().Hands.Find(x => x.IsLeft == isLeft);
				if(leapCam != null)
				{
					handdata = controller.GetTransformedFrame(new LeapTransform(leapCam.transform.position.ToVector(), leapCam.transform.rotation.ToLeapQuaternion())).Hands.Find(x => x.IsLeft == isLeft);
				}

				if (handdata != null)
				{
					palmPos = handdata.PalmPosition.ToVector3();
					Vector3 chop = Vector3.Cross(handdata.Direction.ToVector3(), handdata.PalmNormal.ToVector3());

					lanceDot = Vector3.Dot(handdata.Direction.ToVector3(), Vector3.back);
					chopDot = Vector3.Dot(chop, Vector3.back);
					palmDot = Vector3.Dot(handdata.PalmNormal.ToVector3(), Vector3.back);
					
					for (int i = 0; i < (int)Finger.FingerType.TYPE_PINKY; i++)
					{
						Finger finger = handdata.Fingers.Find(x => x.Type == (Finger.FingerType)i);
						if(finger != null)
						{
							fingerLevels[i] = getFingerGrab(finger);
							fingerParams[i].ForEach(x => x.setGrab(fingerLevels[i]));
						}
					}
				}
			}

			if (Input.GetKeyDown(KeyCode.R))
			{
				RollStartMode();
			}

			if (settingNow)
			{
				if (palmPositionPosePoints.Count > 0)
				{
					palmPositionPosePoints[targetPose].Pose.SetPosing(model);
					debugField = "setmode:" + targetPose.ToString();
				}
			}
			else
			{
				//setMagnitudePose();
				//setMagnitudeTyankoPose();
				//trianglerPose();
				Nicoiti();
			}

			palmtester.transform.position = palmPos / 1000;
		}

		#region Mix functions
		private void setMagnitudePose()
		{
			if (palmPositionPosePoints.Count == 0) return;
			debugField = "";
			float[] levels = new float[palmPositionPosePoints.Count];
			CubismPose[] poselist = new CubismPose[palmPositionPosePoints.Count];
			float maxMagnitude = 0f;
			for (int i = 0; i < palmPositionPosePoints.Count; i++)
			{
				levels[i] = (palmPositionPosePoints[i].position - palmPos).magnitude;
				maxMagnitude = Mathf.Max(maxMagnitude, levels[i]);
				poselist[i] = palmPositionPosePoints[i].Pose;
			}
			for (int i = 0; i < palmPositionPosePoints.Count; i++)
			{
				debugField += i.ToString() + ":" + levels[i].ToString() + "/" + palmPositionPosePoints[i].position.ToString() + "\n";
				levels[i] = maxMagnitude - levels[i];
			}
			CubismPose.CombinePoses(poselist, levels).SetPosing(model);
		}

		private void setMagnitudeTyankoPose()
		{
			if (palmPositionPosePoints.Count == 0) return;
			debugField = "";
			float sumMagnitude = 0f;
			List<PosePoint> copylist = new List<PosePoint>(palmPositionPosePoints.ToArray());
			for (int i = 0; i < copylist.Count; i++)
			{
				sumMagnitude += (copylist[i].position - palmPos).magnitude;
			}
			if(sumMagnitude ==0)
			{
				return;
			}

			copylist.Sort(delegate(PosePoint x,PosePoint y)
			{
				return ((x.position - palmPos).magnitude < (y.position - palmPos).magnitude)?-1:1;
			});

			float[] levels = new float[copylist.Count];
			float remain = 1f;
			for (int i = 0; i < copylist.Count; i++)
			{ 
				float mag = (copylist[i].position - palmPos).magnitude;
				levels[i] = (sumMagnitude - mag) / sumMagnitude * remain;
				remain *= (1f - levels[i]);
				debugField += i.ToString() + ":" + levels[i].ToString() + "/" + copylist[i].position.ToString() + "\n";
			}
			CubismPose.CombinePoses(copylist.ConvertAll<CubismPose>(delegate(PosePoint x) { return x.Pose; }).ToArray(), levels).SetPosing(model);
		}

		private void trianglerPose()
		{
			if (palmPositionPosePoints.Count <= 2) return;
			debugField = "";

			List<PosePoint> copylist = new List<PosePoint>(palmPositionPosePoints.ToArray());			

			//最近点選ぶ　＞　KeyPoint
			PosePoint keyP = getMaxPosePoint(copylist,x => (x.position - palmPos).magnitude);
			copylist.Remove(keyP);
			debugField += "key:" + keyP.position.ToString() + "\n";

			//最もベクターが反対の点とのKeyMag距離点の中和ポーズの設定
			Vector3 PalmToKeyVect;
			while (copylist.Count > 0)
			{
				PalmToKeyVect = keyP.position - palmPos;
				PosePoint Apoint = getMinPosePoint(copylist, x => Vector3.Dot(PalmToKeyVect, (x.position - palmPos)));
				copylist.Remove(Apoint);

				keyP = PosePoint.MixPointNearst(keyP, palmPos, Apoint,true);
				debugField += "koo:" + (keyP.position - palmPos).magnitude.ToString() + "%" + Apoint.position.ToString() + "\n";
			}

			debugField += "pos:" + palmPos.ToString() + "\n";
			keyP.Pose.SetPosing(model);
			targetertester.transform.position = keyP.position / 1000;
		}

		private void Nicoiti()
		{
			if (palmPositionPosePoints.Count == 0) return;
			debugField = "";
			PosePoint ans = RecallNearst(0, palmPositionPosePoints.Count - 1);
			debugField += "lng:" + (ans.position - palmPos).magnitude.ToString() + "\n";
			debugField += "ans:" + ans.position.ToString() + "\n";
			debugField += "pal:" + palmPos.ToString() + "\n";
			ans.Pose.SetPosing(model);
			targetertester.transform.position = ans.position / 1000;
		}
		#endregion

		private PosePoint RecallNearst(int A, int B)
		{
			if(A==B)
			{
				return palmPositionPosePoints[A];
			}
			else
			{
				PosePoint PoseA = RecallNearst(A, Mathf.CeilToInt((A + B) / 2));
				PosePoint PoseB = RecallNearst(Mathf.CeilToInt((A + B) / 2)+1,B);

				return PosePoint.MixPointNearst(PoseA, palmPos, PoseB,true);
			}
		}

		private delegate float ramd(PosePoint x);
		private PosePoint getMaxPosePoint(List<PosePoint> listdata, ramd calc )
		{
			float levels = float.MinValue;
			PosePoint r = null ;
			foreach(var e in listdata)
			{
				float calcResult = calc(e);
				if( calcResult > levels)
				{
					r = e;
					levels = calcResult;
				}
			}
			return r;
		}
		private PosePoint getMinPosePoint(List<PosePoint> listdata, ramd calc)
		{
			float levels = float.MaxValue;
			PosePoint r = null;
			foreach (var e in listdata)
			{
				float calcResult = calc(e);
				if (calcResult < levels)
				{
					r = e;
					levels = calcResult;
				}
			}
			return r;
		}

		public void RollStartMode()
		{
			if (settingNow && (palmPositionPosePoints.Count > 0))
			{
				palmPositionPosePoints[targetPose].position = palmPos;
				posdatas[targetPose].transform.position = palmPos / 1000;
				targetPose++;
				if (targetPose >= palmPositionPosePoints.Count)
				{
					targetPose = 0;
				}
			}
			settingNow = !settingNow;
		}

		private static float getFingerGrab(Finger finger)
		{
			float dot = 1f;
			Vector3 beforeDirection = finger.bones[1].Direction.ToVector3();
			for(int i = 2; i < finger.bones.Length; i++)
			{
				dot *= Mathf.Abs(Vector3.Dot(beforeDirection, finger.bones[i].Direction.ToVector3()));
				beforeDirection = finger.bones[i].Direction.ToVector3();
			}
			return dot;
		}
	}
}