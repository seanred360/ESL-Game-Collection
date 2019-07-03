using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
#endif

namespace UnluckSoftware {
	public class SortingLayerAttribute : PropertyAttribute {
	}
#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(SortingLayerAttribute))]
	public class SortingLayerDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			if (property.propertyType != SerializedPropertyType.Integer) {
				return;
			}
			EditorGUI.LabelField(position, label);
			position.x += EditorGUIUtility.labelWidth;
			position.width -= EditorGUIUtility.labelWidth;
			string[] sortingLayerNames = GetSortingLayerNames();
			int[] sortingLayerIDs = GetSortingLayerIDs();
			int sortingLayerIndex0 = Mathf.Max(0, System.Array.IndexOf < int > (sortingLayerIDs, property.intValue));
			int sortingLayerIndex = EditorGUI.Popup(position, sortingLayerIndex0, sortingLayerNames);
			if(sortingLayerIndex != sortingLayerIndex0) {
				property.intValue = sortingLayerIDs[sortingLayerIndex];
			}
		}

		private string[] GetSortingLayerNames() {
			System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
			PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty(
				"sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
			return (string[])sortingLayersProperty.GetValue(null, new object[0]);
		}

		private int[] GetSortingLayerIDs() {
			System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
			PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty(
				"sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
			return (int[])sortingLayersProperty.GetValue(null, new object[0]);
		}
	}
#endif
}