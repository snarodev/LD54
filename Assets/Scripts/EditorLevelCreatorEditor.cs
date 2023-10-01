using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(EditorLevelCreator))]
public class EditorLevelCreatorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorLevelCreator myScript = (EditorLevelCreator)target;

		if (GUILayout.Button("Generate Configuration"))
		{
			myScript.CreateLevelConfiguration();
		}


		if (GUILayout.Button("Load Configuration"))
		{
			myScript.LoadLevelConfiguration();
		}
	}
}
#endif