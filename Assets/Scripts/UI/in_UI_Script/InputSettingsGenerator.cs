using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InputSettingsGenerator : MonoBehaviour
{
    [Header("Input")]
    public InputActionAsset inputAsset;

    [Header("UI")]
    public Transform content;
    public GameObject headerPrefab;
    public RebindRow rowPrefab;

    [Header("Font & Styling Override")]
    public TMP_FontAsset customFontAsset;
    [Tooltip("Taille du texte pour le titre")]
    public float headerFontSize = 50f;
    [Tooltip("Taille du texte pour les touches et descriptions")]
    public float rowFontSize = 35f;

    private const string SaveKey = "InputBindings";

    private void Awake()
    {
        if (inputAsset != null)
        {
            inputAsset = Instantiate(inputAsset);
        }
        else
        {
            Debug.LogError("[InputSettingsGenerator] 'inputAsset' is NULL in the Inspector!", this);
        }
    }

    private void Start()
    {
        if (inputAsset == null || content == null || headerPrefab == null || rowPrefab == null)
        {
            Debug.LogError("[InputSettingsGenerator] Missing UI or Input references in Inspector!", this);
            return;
        }

        inputAsset.Disable();

        if (PlayerPrefs.HasKey(SaveKey))
        {
            string overrides = PlayerPrefs.GetString(SaveKey);
            if (!string.IsNullOrEmpty(overrides))
            {
                inputAsset.LoadBindingOverridesFromJson(overrides);
            }
        }

        inputAsset.Enable();

        Generate();
    }

    public void Generate()
    {
        if (content == null) return;

        for (int i = content.childCount - 1; i >= 0; i--)
        {
            GameObject child = content.GetChild(i).gameObject;
            child.transform.SetParent(null);
            Destroy(child);
        }

        foreach (var map in inputAsset.actionMaps)
        {
            var header = Instantiate(headerPrefab, content, false);
            ResetZCoordinate(header);

            var tmpText = header.GetComponent<TMP_Text>() ?? header.GetComponentInChildren<TMP_Text>();
            if (tmpText != null)
            {
                tmpText.text = map.name;
                ApplyTextStyle(tmpText, headerFontSize);
            }

            foreach (var action in map.actions)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];

                    if (binding.isComposite)
                        continue;

                    var row = Instantiate(rowPrefab, content, false);
                    ResetZCoordinate(row.gameObject);

                    row.Initialize(action, i, SaveKey);

                    TMP_Text[] rowTexts = row.GetComponentsInChildren<TMP_Text>(true);
                    foreach (var txt in rowTexts)
                    {
                        ApplyTextStyle(txt, rowFontSize);
                    }
                }
            }
        }

        Canvas.ForceUpdateCanvases();
        if (content.TryGetComponent<RectTransform>(out var contentRect))
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        }
    }

    private void ApplyTextStyle(TMP_Text textComponent, float fontSize)
    {
        if (textComponent == null) return;

        if (customFontAsset != null)
        {
            textComponent.font = customFontAsset;
        }

        if (fontSize > 0)
        {
            textComponent.fontSize = fontSize;
        }
    }

    public void ResetAllBindings()
    {
        if (inputAsset == null) return;
        inputAsset.Disable();
        foreach (var map in inputAsset.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
        PlayerPrefs.DeleteKey(SaveKey); PlayerPrefs.Save();
        inputAsset.Enable();
        Generate();
        Debug.Log("All bindings reset to default");
    }

    private void ResetZCoordinate(GameObject obj)
    {
        if (obj.TryGetComponent<RectTransform>(out var rt))
        {
            Vector3 localPos = rt.localPosition;
            rt.localPosition = new Vector3(localPos.x, localPos.y, 0f);
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;
        }
    }
}