using UnityEditor;
using UnityEngine;

namespace ExternalAssets.PBS_Materials_Variety_Pack.zzz.Editor
{
    [CustomEditor(typeof(MaterialRandomizerScript))]
    public class MaterialRandomizer : UnityEditor.Editor 
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MaterialRandomizerScript myScript = (MaterialRandomizerScript)target;
            if(GUILayout.Button("Find Materials"))
            {
                myScript.findMaterials();
            }
            if(GUILayout.Button("Find Spheres"))
            {
                myScript.findMaterialSpheres();
            }
            if(GUILayout.Button("Randomize Materials"))
            {
                myScript.randomizeMaterials();
            }
        }
    }
}
