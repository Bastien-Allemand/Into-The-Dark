using UnityEngine;
using TMPro;

public class DisplayID : MonoBehaviour
{
    GameManager.CharacterData? character;

    public float interactDistance = 3f;
    public TextMeshProUGUI itemText;
    public Camera playerCamera;

    bool Ingame = false;

    private void Awake()
    {
     if (!character.HasValue)
        {
            Ingame = false;
            return;
        }
        playerCamera = character.Value.cam.GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!Ingame)
            return;
        if (itemText != null)
            itemText.gameObject.SetActive(false);

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.CompareTag("Item") || hit.collider.CompareTag("Consumable"))
            {
                itemText.gameObject.SetActive(true);
                itemText.text = hit.collider.gameObject.name.Replace("(Clone)", "").Replace("item_", "");
            }
        }
    }
    void ActivateUI(GameManager.CharacterData characterSelection)
    {
        character = characterSelection;
        Awake();
    }

}