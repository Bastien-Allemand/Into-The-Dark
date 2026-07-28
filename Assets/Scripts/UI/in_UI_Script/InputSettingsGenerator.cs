using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InputSettingsGenerator : MonoBehaviour
{
    [Header("Input")]
    public InputActionAsset inputAsset;

    [Header("UI")]
    public Transform content;
    public GameObject headerPrefab;
    public RebindRow rowPrefab;

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
            // Spawn Header
            var header = Instantiate(headerPrefab, content, false);
            ResetZCoordinate(header);

            var tmpText = header.GetComponent<TMPro.TMP_Text>() ?? header.GetComponentInChildren<TMPro.TMP_Text>();
            if (tmpText != null)
            {
                tmpText.text = map.name;
            }

            foreach (var action in map.actions)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];

                    if (binding.isComposite)
                        continue;

                    // Spawn Row
                    var row = Instantiate(rowPrefab, content, false);
                    ResetZCoordinate(row.gameObject);

                    // Initialize Row
                    row.Initialize(action, i, SaveKey);
                }
            }
        }

        // 2. Force Unity UI Layout Group to refresh positions immediately
        Canvas.ForceUpdateCanvases();
        if (content.TryGetComponent<RectTransform>(out var contentRect))
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
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