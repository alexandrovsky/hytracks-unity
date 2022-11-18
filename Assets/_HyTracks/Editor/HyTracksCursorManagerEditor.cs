/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using TouchScript.Behaviors.Cursors;
using UnityEditor;
using UnityEngine;
using TouchScript.Editor.EditorUI;

namespace HyTracks.Editor {

    [CustomEditor(typeof(HyTracksCursorManager))]
    internal sealed class HyTracksCursorManagerEditor:UnityEditor.Editor {

        public static readonly GUIContent TEXT_DPI_HEADER = new GUIContent("Use DPI","Scale touch pointer based on DPI.");
        public static readonly GUIContent TEXT_CURSORS_HEADER = new GUIContent("Cursors","Cursor prefabs used for different pointer types.");
        public static readonly GUIContent TEXT_POINTER_SIZE = new GUIContent("Pointer size (cm)","Pointer size in cm based on current DPI.");
        public static readonly GUIContent TEXT_POINTER_PIXEL_SIZE = new GUIContent("Pointer size (px)","Pointer size in pixels.");

        public static readonly GUIContent MODEL_OBJECT_CURSORS_HEADER = new GUIContent("Model Objects","Tangibles that represent the model assumptions.");
        public static readonly GUIContent AGENT_OBJECT_CURSORS_HEADER = new GUIContent("Agent Objects","Tangibles that represent the agents.");


        private SerializedProperty mousePointerProxy, touchPointerProxy, penPointerProxy, objectPointerProxy, modelObjectProxies, agentObjectProxies;
        private SerializedProperty useDPI, cursorSize, cursorPixelSize;
        private SerializedProperty cursorsProps;

        private void OnEnable()
        {
            mousePointerProxy = serializedObject.FindProperty("mouseCursor");
            touchPointerProxy = serializedObject.FindProperty("touchCursor");
            penPointerProxy = serializedObject.FindProperty("penCursor");
            objectPointerProxy = serializedObject.FindProperty("objectCursor");

            useDPI = serializedObject.FindProperty("useDPI");
            cursorSize = serializedObject.FindProperty("cursorSize");
            cursorPixelSize = serializedObject.FindProperty("cursorPixelSize");

            modelObjectProxies = serializedObject.FindProperty("modelObjectCursors");
            agentObjectProxies = serializedObject.FindProperty("agentObjectCursors");

            cursorsProps = serializedObject.FindProperty("cursorsProps");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Space(5);

            EditorGUILayout.PropertyField(useDPI,TEXT_DPI_HEADER);
            if(useDPI.boolValue) {
                EditorGUILayout.PropertyField(cursorSize,TEXT_POINTER_SIZE);
            } else {
                EditorGUILayout.PropertyField(cursorPixelSize,TEXT_POINTER_PIXEL_SIZE);
            }

            var display = GUIElements.Header(TEXT_CURSORS_HEADER,cursorsProps);
            if(display) {
                EditorGUI.indentLevel++;                
                EditorGUILayout.PropertyField(mousePointerProxy,new GUIContent("Mouse Pointer Proxy"));
                EditorGUILayout.PropertyField(touchPointerProxy,new GUIContent("Touch Pointer Proxy"));
                EditorGUILayout.PropertyField(penPointerProxy,new GUIContent("Pen Pointer Proxy"));
                EditorGUILayout.PropertyField(objectPointerProxy,new GUIContent("Object Pointer Proxy"));
                EditorGUI.indentLevel--;
            }

            display = GUIElements.Header(MODEL_OBJECT_CURSORS_HEADER,cursorsProps);
			if(display) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(modelObjectProxies,new GUIContent("Model Objects Proxy"));                
                EditorGUI.indentLevel--;
            }

            display = GUIElements.Header(AGENT_OBJECT_CURSORS_HEADER,cursorsProps);
            if(display) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(agentObjectProxies,new GUIContent("Agent Objects Proxy"));
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
