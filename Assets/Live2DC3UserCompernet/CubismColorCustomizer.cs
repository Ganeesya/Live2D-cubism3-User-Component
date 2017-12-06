using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Framework.UserData;
using Live2D.Cubism.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CubismColorCustomizer : MonoBehaviour
{
	public Color color = Color.white;
	public string targetTag;
	
	private List<CubismRenderer> targetRenderers;

	// Use this for initialization
	public void Start()
	{
		var drawables = gameObject.GetComponentsInChildren<CubismUserDataTag>();
		if (targetRenderers == null)
		{
			targetRenderers = new List<CubismRenderer>();
		}
		else
		{
			targetRenderers.Clear();
		}

		foreach (var e in drawables)
		{
			if (e.Value == targetTag)
			{
				targetRenderers.Add(e.gameObject.GetComponent<CubismRenderer>());
			}
		}
	}

	// Update is called once per frame
	public void Update()
	{
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

		if(tags == null )
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
			if(!tags.Exists(x => x == e.Value))
			{
				tags.Add(e.Value);
			}
		}
	}

	public override void OnInspectorGUI()
	{
		CubismColorCustomizer mp = (CubismColorCustomizer)target;

		var newColor = EditorGUILayout.ColorField("color",mp.color);

		var tagIndex = EditorGUILayout.Popup("targetTag", tags.FindIndex(x => x == mp.targetTag), tags.ToArray());
		//if (tagIndex == -1) return;
		//if(mp.targetTag != tags[tagIndex])
		//{
		//	mp.color = Color.white;
		//	mp.Update();
		//}
		mp.targetTag = tags[tagIndex];
		//mp.color = newColor;
		//mp.targetTag = EditorGUILayout.TextField("targetTag", mp.targetTag);

		mp.Start();
		mp.Update();
	}
}
