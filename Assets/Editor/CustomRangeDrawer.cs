using Assets.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomPropertyDrawer(typeof(RangeDouble))]
    public class CustomRangeDrawer : PropertyDrawer
    {
 
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
 
            // First get the attribute since it contains the range for the slider
            RangeDouble range = attribute as RangeDouble;
 
            // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
            if (property.propertyType == SerializedPropertyType.Float)
                EditorGUI.Slider(position, property, (float) range.Min, (float) range.Max, label);
            else if (property.propertyType == SerializedPropertyType.Integer)
         
                EditorGUI.IntSlider(position, property, (int)range.Min, (int)range.Max, label);
            else
                EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
        }
    }
}