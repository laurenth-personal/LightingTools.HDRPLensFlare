using System.Collections.Generic;
using UnityEngine;

namespace HDRPLensFlare
{
    [CreateAssetMenu(fileName = "LensFlareAsset", menuName = "Rendering/Lens Flare", order = 1)]
    public class LensFlareAsset : ScriptableObject
    {
        public FlareData[] Flares = new FlareData[1];
        public Mesh mesh;

        public void CreateMesh()
        {
            Debug.Log("Creating LensFlareAsset mesh");

            var flareMesh = new Mesh();
            flareMesh.MarkDynamic();
            flareMesh.name = this.name + "_lensFlareMesh";

            if (Flares.Length > 0)
            {
                // Positions
                List<Vector3> vertices = new List<Vector3>();
                foreach (FlareData s in Flares)
                {
                    vertices.Add(new Vector3(-1, -1, 0));
                    vertices.Add(new Vector3(1, -1, 0));
                    vertices.Add(new Vector3(1, 1, 0));
                    vertices.Add(new Vector3(-1, 1, 0));
                }
                flareMesh.SetVertices(vertices);

                // UVs
                List<Vector2> uvs = new List<Vector2>();
                foreach (FlareData s in Flares)
                {
                    uvs.Add(new Vector2(0, 1));
                    uvs.Add(new Vector2(1, 1));
                    uvs.Add(new Vector2(1, 0));
                    uvs.Add(new Vector2(0, 0));
                }
                flareMesh.SetUVs(0, uvs);

                flareMesh.subMeshCount = Flares.Length;

                // Tris
                for (int i = 0; i < Flares.Length; i++)
                {
                    int[] tris = new int[6];
                    tris[0] = (i * 4) + 0;
                    tris[1] = (i * 4) + 1;
                    tris[2] = (i * 4) + 2;
                    tris[3] = (i * 4) + 2;
                    tris[4] = (i * 4) + 3;
                    tris[5] = (i * 4) + 0;
                    flareMesh.SetTriangles(tris, i);
                }
                flareMesh.SetColors(GetLensFlareColor());
                flareMesh.SetUVs(1, GetLensFlareData());

                flareMesh.UploadMeshData(false);
            }


#if UNITY_EDITOR
            var objects = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(UnityEditor.AssetDatabase.GetAssetPath(this));
            if (objects.Length > 0)
            {
                foreach (var obj in objects)
                {
                    if (UnityEditor.AssetDatabase.IsSubAsset(obj))
                        DestroyImmediate(obj, true);
                }
            }
            if (Flares.Length > 0)
            {
                UnityEditor.AssetDatabase.AddObjectToAsset(flareMesh, this);
                mesh = flareMesh;
            }

            UnityEditor.AssetDatabase.ImportAsset(UnityEditor.AssetDatabase.GetAssetPath(flareMesh));
            UnityEditor.AssetDatabase.ImportAsset(UnityEditor.AssetDatabase.GetAssetPath(this));

            //Update lens flares component referring this scriptable object.
            var sceneFlares = FindObjectsOfType<HDRPLensFlareRenderer>();
            foreach (var flare in sceneFlares)
            {
                if (flare.flare == this)
                    flare.Refresh();
            }
#endif
        }

        List<Color> GetLensFlareColor()
        {
            List<Color> colors = new List<Color>();
            foreach (FlareData s in Flares)
            {
                Color c = s.Color;

                colors.Add(c);
                colors.Add(c);
                colors.Add(c);
                colors.Add(c);
            }
            return colors;
        }


        List<Vector4> GetLensFlareData()
        {
            List<Vector4> lfData = new List<Vector4>();

            foreach (FlareData f in Flares)
            {
                Vector4 data = new Vector4(f.position, f.AutoRotate ? -1 : Mathf.Abs(f.Rotation), f.Size.x, f.Size.y);
                lfData.Add(data); lfData.Add(data); lfData.Add(data); lfData.Add(data);
            }
            return lfData;
        }
    }
}
