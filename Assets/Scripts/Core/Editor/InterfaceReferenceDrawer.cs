using Core.Utilities;
using UnityEditor;
using UnityEngine;

// Замените на namespace, где лежит ваш InterfaceReference

// Говорим Unity, что этот скрипт отвечает за отрисовку InterfaceReference
namespace Core.Editor
{
    [CustomPropertyDrawer(typeof(InterfaceReference<>))]
    public class InterfaceReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Находим то самое внутреннее поле _value
            SerializedProperty valueProp = property.FindPropertyRelative("_value");

            // Рисуем поле _value, но даем ему имя самого элемента списка (label)
            EditorGUI.PropertyField(position, valueProp, label);
        }

        // Сообщаем Unity правильную высоту поля (чтобы списки не наезжали друг на друга)
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProp = property.FindPropertyRelative("_value");
            return EditorGUI.GetPropertyHeight(valueProp, label);
        }
    }
}