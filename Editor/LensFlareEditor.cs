using UnityEngine;
using UnityEditor;

namespace HDRPLensFlare
{
    [CustomEditor(typeof(LensFlareAsset))]
    public class LensFlareEditor : Editor
    {
        SerializedProperty lookAtPoint;
        LensFlareAsset lensFlare;

        void OnEnable()
        {
            lookAtPoint = serializedObject.FindProperty("lookAtPoint");
            lensFlare = target as LensFlareAsset;
        }

        //Only use for this is to refresh the mesh when the asset gets edited.
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            DrawDefaultInspector();
            if(EditorGUI.EndChangeCheck())
                lensFlare.CreateMesh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}

