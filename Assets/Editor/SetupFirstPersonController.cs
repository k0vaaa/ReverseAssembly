using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SetupFirstPersonController : MonoBehaviour
    {
        [ContextMenu("Run")]
        public void Run()
        {
            string[] prefabPaths =
            {
                "Assets/Mini First Person Controller/First Person Controller.prefab",
                "Assets/Mini First Person Controller/First Person Controller Minimal.prefab"
            };

            foreach (var path in prefabPaths)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null) continue;

                // Remove old scripts
                var allScripts = prefab.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var script in allScripts)
                {
                    if (script == null) continue;
                    string scriptName = script.GetType().Name;
                    if (scriptName == "FirstPersonAudio" || scriptName == "FirstPersonLook" ||
                        scriptName == "FirstPersonMovement" || scriptName == "Crouch" ||
                        scriptName == "GroundCheck" || scriptName == "Jump" || scriptName == "Zoom")
                    {
                        Object.DestroyImmediate(script, true);
                    }
                }

                var rb = prefab.GetComponent<Rigidbody>();
                if (rb != null) Object.DestroyImmediate(rb, true);

                var cap = prefab.GetComponent<CapsuleCollider>();
                if (cap != null) Object.DestroyImmediate(cap, true);

                // Add new components on root
                var charCtrl = prefab.GetComponent<CharacterController>();
                if (charCtrl == null) charCtrl = prefab.AddComponent<CharacterController>();

                var anim = prefab.GetComponent<PlayerAnimator>();
                if (anim == null) anim = prefab.AddComponent<PlayerAnimator>();

                var moveCtrl = prefab.GetComponent<MovementController>();
                if (moveCtrl == null) moveCtrl = prefab.AddComponent<MovementController>();

                if (prefab.GetComponent<FightController>() == null) prefab.AddComponent<FightController>();
                if (prefab.GetComponent<PlayerGlitchController>() == null) prefab.AddComponent<PlayerGlitchController>();
                if (prefab.GetComponent<AbilitiesController>() == null) prefab.AddComponent<AbilitiesController>();
                if (prefab.GetComponent<WristTerminalController>() == null) prefab.AddComponent<WristTerminalController>();

                // Try to assign references for MovementController
                // Fields: _foot, _head, _root, _spine
                // Using SerializedObject to set private serialized fields
                SerializedObject so = new SerializedObject(moveCtrl);

                Transform head = prefab.transform.Find("First Person Camera");
                if (head == null) head = prefab.transform; // fallback

                Transform root = prefab.transform;
                Transform spine = head; // usually camera is the spine/head fallback
                Transform foot = prefab.transform; // fallback

                // If they have specific names in the FPC prefab:
                var camChild = prefab.GetComponentInChildren<Camera>(true);
                if (camChild != null)
                {
                    head = camChild.transform;
                    spine = camChild.transform;
                }

                so.FindProperty("_head").objectReferenceValue = head;
                so.FindProperty("_spine").objectReferenceValue = spine;
                so.FindProperty("_foot").objectReferenceValue = foot;
                so.FindProperty("_root").objectReferenceValue = root;
                so.ApplyModifiedProperties();

                EditorUtility.SetDirty(prefab);
            }

            AssetDatabase.SaveAssets();
            Debug.Log("FIRST_PERSON_CONTROLLER_SETUP_COMPLETE");
        }
    }
}