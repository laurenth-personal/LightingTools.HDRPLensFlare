using System.Collections.Generic;
using UnityEngine;

namespace HDRPLensFlare
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class HDRPLensFlareRenderer : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        MeshRenderer m_MeshRenderer;
        [SerializeField, HideInInspector]
        MeshFilter m_MeshFilter;

        [SerializeField]
        public LensFlareAsset flare;

        [Header("Global Settings")]
        public float OcclusionRadius = 1.0f;
        public float NearFadeStartDistance = 1.0f;
        public float NearFadeEndDistance = 3.0f;
        public float FarFadeStartDistance = 10.0f;
        public float FarFadeEndDistance = 50.0f;

        void Awake()
        {
            CacheComponents();
            ConfigureMeshRenderer();

            if (flare == null)
            {
                if (Application.isEditor)
                    Debug.Log("empty lens flare on" + gameObject.name.ToString());
            }
        }

        void CacheComponents()
        {
            m_MeshFilter = GetComponent<MeshFilter>();
            m_MeshRenderer = GetComponent<MeshRenderer>();
        }

        //Maybe optimizes meshrenderer ?
        void ConfigureMeshRenderer()
        {
            m_MeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            m_MeshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            m_MeshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            m_MeshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        }

        void OnEnable()
        {
            CacheComponents();
            ConfigureMeshRenderer();
            OnValidate();
        }

        void OnDisable()
        {
            Delete();
        }

        void OnValidate()
        {
            if (flare == null)
            {                
                Debug.Log("empty lens flare on" + gameObject.name.ToString());
                Delete();
            }
            else
            {
                m_MeshFilter.sharedMesh = flare.mesh;
                UpdateMaterials();
                UpdateVaryingAttributes();
            }

        }

        //Called by the lens flare scriptable object when settings change.
        public void Refresh()
        {
            m_MeshRenderer.additionalVertexStreams.Clear();
            m_MeshRenderer.additionalVertexStreams = null;
            OnValidate();
        }

        void UpdateMaterials()
        {
            if (flare == null)
                return;

            Material[] mats = new Material[flare.Flares.Length];

            int i = 0;
            foreach(FlareData f in flare.Flares)
            {
                mats[i] = Instantiate(f.Material);
                if(flare.Flares[i].texture != null)
                    mats[i].SetTexture("_MainTex", flare.Flares[i].texture);
                i++;
            }
            m_MeshRenderer.sharedMaterials = mats;
        }

        void UpdateVaryingAttributes()
        {
            if (m_MeshFilter.sharedMesh == null)
                return;

            if(m_MeshRenderer.additionalVertexStreams == null)
                m_MeshRenderer.additionalVertexStreams = Instantiate(flare.mesh);
            m_MeshRenderer.additionalVertexStreams.SetUVs(2, GetWorldPositionAndRadius());
            m_MeshRenderer.additionalVertexStreams.SetUVs(3, GetDistanceFadeData());
        }

        List<Vector4> GetDistanceFadeData()
        {
            List<Vector4> fadeData = new List<Vector4>();

            foreach (FlareData f in flare.Flares)
            {
                Vector4 data = new Vector4(NearFadeStartDistance,NearFadeEndDistance, FarFadeStartDistance, FarFadeEndDistance);
                fadeData.Add(data); fadeData.Add(data); fadeData.Add(data); fadeData.Add(data);
            }
            return fadeData;
        }
    

        List<Vector4> GetWorldPositionAndRadius()
        {
            List<Vector4> worldPos = new List<Vector4>();
            Vector3 pos = transform.position;
            Vector4 value = new Vector4(pos.x,pos.y,pos.z, OcclusionRadius);
            foreach (FlareData s in flare.Flares)
            {
                worldPos.Add(value); worldPos.Add(value); worldPos.Add(value); worldPos.Add(value);
            }

            return worldPos;
        }

        void Delete()
        {
            m_MeshFilter.sharedMesh = null;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawSphere(transform.position, OcclusionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, OcclusionRadius);
        }
    }

    [System.Serializable]
    public class FlareData
    {
        public float position;
        public Texture texture;
        public Material Material;
        [ColorUsage(true, true)]
        public Color Color = Color.white;
        public Vector2 Size = new Vector2(0.5f,0.5f);
        public bool AutoRotate;
        public float Rotation;

    }
}
