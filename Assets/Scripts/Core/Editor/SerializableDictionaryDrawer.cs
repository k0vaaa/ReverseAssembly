using Core.UI.Types;
using UnityEditor;
using UnityEngine;


namespace Core.Editor
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<>))]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Находим внутренний список _values
            SerializedProperty valuesProp = property.FindPropertyRelative("_values");

            // Рисуем список, передавая true (чтобы нарисовались все вложенные элементы списка),
            // и передаем label (чтобы список назывался "Views", а не "Values")
            EditorGUI.PropertyField(position, valuesProp, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty valuesProp = property.FindPropertyRelative("_values");
            // Возвращаем высоту всего списка вместе с его элементами
            return EditorGUI.GetPropertyHeight(valuesProp, label, true);
        }
    }
}