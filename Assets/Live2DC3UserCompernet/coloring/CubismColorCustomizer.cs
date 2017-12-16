using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Framework.UserData;
using Live2D.Cubism.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ganeesyan.Cubism3Compornets
{
	public class CubismColorCustomizer : MonoBehaviour
	{
		public Color color = Color.white;
		public string targetTag;
		private string oldTag;

		private List<CubismRenderer> targetRenderers;

		// Use this for initialization
		void Start()
		{
			var drawables = gameObject.GetComponentsInChildren<CubismUserDataTag>();
			if (targetRenderers == null)
			{
				targetRenderers = new List<CubismRenderer>();
			}
			else
			{
#if UNITY_EDITOR
				foreach (var e in targetRenderers)
				{
					e.Color = Color.white;
				}
#endif
				targetRenderers.Clear();
			}

			foreach (var e in drawables)
			{
				if (e.Value == targetTag)
				{
					targetRenderers.Add(e.gameObject.GetComponent<CubismRenderer>());
				}
			}
#if UNITY_EDITOR
			oldTag = targetTag;
#endif
		}

		// Update is called once per frame
		public void Update()
		{
#if UNITY_EDITOR
			if (oldTag != targetTag)
			{
				Start();
			}
#endif
			foreach (var e in targetRenderers)
			{
				e.Color = color;
			}
		}
	}

	[CustomEditor(typeof(CubismColorCustomizer))]
	public class CubismClrCstmEditer : Editor
	{
		SerializedProperty colorProp;
		SerializedProperty targetTagProp;
		SerializedProperty targetRenderersProp;

		private List<string> tags;

		void OnEnable()
		{
			// Setup the SerializedProperties.
			colorProp = serializedObject.FindProperty("color");
			targetTagProp = serializedObject.FindProperty("targetTag");
			targetRenderersProp = serializedObject.FindProperty("targetRenderers");

			if (tags == null)
			{
				tags = new List<string>();
			}
			else
			{
				tags.Clear();
			}
			CubismColorCustomizer mp = (CubismColorCustomizer)target;

			foreach (var e in mp.gameObject.GetComponentsInChildren<CubismUserDataTag>())
			{
				if (!tags.Exists(x => x == e.Value))
				{
					tags.Add(e.Value);
				}
			}

			EditorApplication.update += () =>
			{
				((CubismColorCustomizer)target).Update();
			};
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			CubismColorCustomizer mp = (CubismColorCustomizer)target;

			mp.color = EditorGUILayout.ColorField("color", mp.color);

			var tagIndex = EditorGUILayout.Popup("targetTag", tags.FindIndex(x => x == mp.targetTag), tags.ToArray());
			if (tagIndex == -1)
			{
				return;
			}
			mp.targetTag = tags[tagIndex];

			serializedObject.ApplyModifiedProperties();
		}
	}
}