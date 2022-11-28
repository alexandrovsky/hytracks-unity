using System.Collections;
using System.Collections.Generic;
using TouchScript.Editor.EditorUI;
using UnityEditor;
using UnityEngine;
using System;
using static Unity.VisualScripting.Member;

namespace HyTracks
{
	[CustomEditor(typeof(HyTracksSteepParameters))]
	public class HyTracksSteepParametersEditor : UnityEditor.Editor
	{
		public static readonly GUIContent TEXT_SOCIAL_HEADER = new GUIContent("Social");
		public static readonly GUIContent TEXT_TECHNOLOGY_HEADER = new GUIContent("Technology");
		public static readonly GUIContent TEXT_ECONOMIC_HEADER = new GUIContent("Economics");
		public static readonly GUIContent TEXT_ENVIRONMENT_HEADER = new GUIContent("Environment");
		public static readonly GUIContent TEXT_POLITICS_HEADER = new GUIContent("Politics");


		public static readonly GUIContent TEXT_HEADER_INPUT = new GUIContent("INPUT");
		public static readonly GUIContent TEXT_HEADER_OUTPUT = new GUIContent("OUTPUT");



		HyTracksSteepParameters steepParameters;
		private SerializedProperty parametersProps;
		private void OnEnable()
		{
			steepParameters = serializedObject.targetObject as HyTracksSteepParameters;
			parametersProps = serializedObject.FindProperty("parametersProps");
		}


		void drawByReflection(object obj)
		{
			Type te = obj.GetType();
			System.Reflection.PropertyInfo[] pi = te.GetProperties();
			foreach (System.Reflection.PropertyInfo p in pi)
			{
				try
				{
					EditorGUILayout.BeginHorizontal();

					Debug.LogWarning(p.Name + " : " + p.GetValue(obj));
					EditorGUILayout.LabelField(p.Name);
					EditorGUILayout.LabelField(p.GetValue(obj).ToString());

					EditorGUILayout.EndHorizontal();
				}
				catch
				{

				}
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			base.OnInspectorGUI();

			if (GUILayout.Button("Load JSON Data"))
			{
				steepParameters.LoadDataFromJSONFiles();
			}

			if (GUILayout.Button("Generate Dummy JSON"))
			{
				steepParameters.GenerateDummyJSONParamters();				
			}

			if (GUILayout.Button("Generate Dummy JSON List"))
			{
				steepParameters.GenerateDummyJSONParamtersList();
			}
		}
	}
}
