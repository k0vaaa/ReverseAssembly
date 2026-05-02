using UnityEditor;
using UnityEngine;

namespace Invector_3rdPersonController_LITE.Scripts.CharacterController.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(vThirdPersonInput), true)]
    public class vThirdPersonInputEditor : UnityEditor.Editor
    {
        GUISkin skin;

        public override void OnInspectorGUI()
        {
            if (!skin) skin = UnityEngine.Resources.Load("vSkin") as GUISkin;
            GUI.skin = skin;

            GUILayout.BeginVertical("INPUT MANAGER", "window");

            GUILayout.Space(30);

            EditorGUILayout.BeginVertical();

            base.OnInspectorGUI();

            GUILayout.Space(10);

            GUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.Space(2);
        }
    }
}