using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ControleurShader : MonoBehaviour
{
    [System.Serializable]
    public class FloatData
    {
        public string referenceName;
        public string displayName;
        public float value;
    }

    [System.Serializable]
    public class ColorData
    {
        public string referenceName;
        public string displayName;
        public Color value = Color.white;
    }

    [System.Serializable]
    public class MaterialConfig
    {
        public string nomAffiche;
        public Material material;

        [Header("Rendu URP (Optionnel)")]
        [Tooltip("Glisse la Renderer Feature associée à ce matériau")]
        public ScriptableRendererFeature rendererFeature;
        [Tooltip("Active ou désactive complètement la Renderer Feature sur le GPU")]
        public bool featureActive = true;

        [Header("Variables du Shader")]
        public List<FloatData> floatProperties = new List<FloatData>();
        public List<ColorData> colorProperties = new List<ColorData>();
    }

    [Header("Liste des Matériaux & Effets")]
    [SerializeField] private List<MaterialConfig> materiaux = new List<MaterialConfig>();
    [ContextMenu("1. Scanner les Matériaux de la liste")]
    public void ScanMaterialsAndShaders()
    {
        if (materiaux == null || materiaux.Count == 0)
        {
            Debug.LogWarning("Ajoute d'abord des éléments dans la liste 'Materiaux' !");
            return;
        }

        foreach (var config in materiaux)
        {
            if (config.material == null || config.material.shader == null) continue;
         
            config.nomAffiche = config.material.name;
            Shader shader = config.material.shader;

            var oldFloats = new Dictionary<string, float>();
            foreach (var f in config.floatProperties) oldFloats[f.referenceName] = f.value;

            var oldColors = new Dictionary<string, Color>();
            foreach (var c in config.colorProperties) oldColors[c.referenceName] = c.value;

            config.floatProperties.Clear();
            config.colorProperties.Clear();

            int propertyCount = shader.GetPropertyCount();
            for (int i = 0; i < propertyCount; i++)
            {
                string propName = shader.GetPropertyName(i);
                string description = shader.GetPropertyDescription(i);
                ShaderPropertyType type = shader.GetPropertyType(i);

                if (type == ShaderPropertyType.Float || type == ShaderPropertyType.Range)
                {
                    float val = oldFloats.ContainsKey(propName)
                        ? oldFloats[propName]
                        : (config.material.HasProperty(propName) ? config.material.GetFloat(propName) : 0f);

                    config.floatProperties.Add(new FloatData
                    {
                        referenceName = propName,
                        displayName = description,
                        value = val
                    });
                }
                else if (type == ShaderPropertyType.Color)
                {
                    Color val = oldColors.ContainsKey(propName)
                        ? oldColors[propName]
                        : (config.material.HasProperty(propName) ? config.material.GetColor(propName) : Color.white);

                    config.colorProperties.Add(new ColorData
                    {
                        referenceName = propName,
                        displayName = description,
                        value = val
                    });
                }
            }
        }

        Debug.Log($"<color=cyan><b>Scan terminé !</b> {materiaux.Count} bloc(s) de matériaux structuré(s).</color>");
    }

    private void Start()
    {
        ApplyAll();
    }

    private void OnValidate()
    {
        ApplyAll();
    }

    public void ApplyAll()
    {
        if (materiaux == null || materiaux.Count == 0) return;

        foreach (var config in materiaux)
        {
            // 1. Gestion ON/OFF de la Renderer Feature (Si assignée)
            if (config.rendererFeature != null)
            {
                config.rendererFeature.SetActive(config.featureActive);
            }

            // 2. Application des Floats et Couleurs sur le Matériau
            if (config.material != null)
            {
                foreach (var f in config.floatProperties)
                {
                    if (config.material.HasProperty(f.referenceName))
                        config.material.SetFloat(f.referenceName, f.value);
                }

                foreach (var c in config.colorProperties)
                {
                    if (config.material.HasProperty(c.referenceName))
                        config.material.SetColor(c.referenceName, c.value);
                }
            }
        }
    }
}