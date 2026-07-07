using UnityEngine;

public class ItemCameraUI : MonoBehaviour
{
    ItemScript itemScript;
    [SerializeField] private GameObject blackScreen;

    private void Start()
    {
        itemScript = GetComponent<ItemScript>();
        blackScreen.SetActive(false);
    }

    private void Update()
    {
        if (itemScript != null && itemScript.energy <= 0)
        {
            blackScreen.SetActive(true);
        }
        if (itemScript != null && itemScript.energy > 0)
        {
            if (blackScreen.activeSelf == true)
            {
                blackScreen.SetActive(false);
            }
        }
    }
}
