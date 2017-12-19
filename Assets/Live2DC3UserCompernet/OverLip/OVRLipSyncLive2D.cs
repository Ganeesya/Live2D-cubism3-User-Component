using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ganeesyan.Cubism3Compornets
{
	public class OVRLipSyncLive2D : MonoBehaviour
	{
		//public Animation animation = null;
		//public AnimationClip[] blendMotions = new AnimationClip[OVRLipSync.VisemeCount];
		[PathAttribute]
		public string[] motion3Jsons = new string[OVRLipSync.VisemeCount];
		public int SmoothAmount = 100;		

		// Look for a lip-sync Context (should be set at the same level as this component)
		private OVRLipSyncContextBase lipsyncContext = null;

		private CubismPose[] visemePoses;

		private CubismModel target = null;

		// Use this for initialization
		void Start()
		{
			// morph target needs to be set manually; possibly other components will need the same
			//if (animation == null)
			//{
			//	Debug.Log("OVRLipSyncLive2D.Start WARNING: Please set required public components!");
			//	return;
			//}


			// make sure there is a phoneme context assigned to this object

			lipsyncContext = GetComponent<OVRLipSyncContextBase>();

			if (lipsyncContext == null)

			{

				Debug.Log("OVRLipSyncLive2D.Start WARNING: No phoneme context component set to object");

			}

			visemePoses = new CubismPose[motion3Jsons.Length];
			for( int i = 0; i < motion3Jsons.Length; i++)
			{
				visemePoses[i] = CubismPose.LoadCubismPoseFromAssetPath(motion3Jsons[i]);
				if(visemePoses[i] == null)
				{
					Debug.Log("OVRLipSyncLive2D.Start no load pose");
				}
			}

			target = gameObject.GetComponent<CubismModel>();
			if(target == null)
			{
				Debug.Log("OVRLipSyncLive2D.Start no find target cubism model");
			}

			// Send smoothing amount to context

			lipsyncContext.Smoothing = SmoothAmount;

		}

		// Update is called once per frame
		void Update()
		{
			if (lipsyncContext != null)

			{

				// trap inputs and send signals to phoneme engine for testing purposes



				// get the current viseme frame

				OVRLipSync.Frame frame = lipsyncContext.GetCurrentPhonemeFrame();

				if (frame != null)

				{

					SetVisemeToMorphTarget(frame);

				}
				//if (Input.GetKeyDown(KeyCode.A))
				//{
				//	animation.Blend("ovrlip_0", 0.5f);
				//}
				//if (Input.GetKeyDown(KeyCode.S))
				//{
				//	animation.Blend("ovrlip_2", 0.5f);
				//}

			}

		}

		/// <summary>

		/// Sets the viseme to morph target.

		/// </summary>

		void SetVisemeToMorphTarget(OVRLipSync.Frame frame)

		{
			var combinepose = CubismPose.CombinePoses(visemePoses, frame.Visemes);
			if(combinepose != null)
			{
				combinepose.SetPosing(target);
			}
			//animation.Stop();
			//for (int i = 0; i < blendMotions.Length; i++)

			//{
			//	animation.Blend("ovrlip_" + i.ToString(), frame.Visemes[i]);

			//}
		}
	}
}


//  PathAttribute.cs
//  http://kan-kikuchi.hatenablog.com/entry/PathAttribute
//
//  Created by kan.kikuchi on 2016.08.05.

/// <summary>
/// ドラック&ドロップでパスを設定するための属性
/// </summary>
public class PathAttribute : PropertyAttribute
{


}

//  PathAttributeDrawer.cs
//  http://kan-kikuchi.hatenablog.com/entry/PathAttribute
//
//  Created by kan.kikuchi on 2016.08.05.

/// <summary>
/// PathAttributeがInspectorでどう表示されるかを設定するクラス
/// </summary>
[CustomPropertyDrawer(typeof(PathAttribute))]
public class PathAttributeDrawer : PropertyDrawer
{

	//=================================================================================
	//更新
	//=================================================================================

	//GUIを更新する
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//string以外に設定されている場合はスルー
		if (property.propertyType != SerializedPropertyType.String)
		{
			return;
		}

		//D&D出来るGUIを作成、 ドロップされたオブジェクトのリストを取得
		List<Object> dropObjectList = CreateDragAndDropGUI(position);

		//オブジェクトがドロップされたらパスを設定
		if (dropObjectList.Count > 0)
		{
			property.stringValue = AssetDatabase.GetAssetPath(dropObjectList[0]);
		}

		//現在設定されているパスを表示
		GUI.Label(position, property.displayName + " : " + property.stringValue);
	}

	//D&DのGUIを作成
	private List<Object> CreateDragAndDropGUI(Rect rect)
	{
		List<Object> list = new List<Object>();

		//D&D出来る場所を描画
		GUI.Box(rect, "");

		//マウスの位置がD&Dの範囲になければスルー
		if (!rect.Contains(Event.current.mousePosition))
		{
			return list;
		}

		//現在のイベントを取得
		EventType eventType = Event.current.type;

		//ドラッグ＆ドロップで操作が 更新されたとき or 実行したとき
		if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
		{
			//カーソルに+のアイコンを表示
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

			//ドロップされたオブジェクトをリストに登録
			if (eventType == EventType.DragPerform)
			{
				list = new List<Object>(DragAndDrop.objectReferences);

				//ドラッグを受け付ける(ドラッグしてカーソルにくっ付いてたオブジェクトが戻らなくなる)
				DragAndDrop.AcceptDrag();
			}

			//イベントを使用済みにする
			Event.current.Use();
		}

		return list;
	}

}