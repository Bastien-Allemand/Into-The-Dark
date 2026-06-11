using UnityEngine;
using TMPro;

public class DisplayID : MonoBehaviour
{
    public float interactDistance = 3f;
    public TextMeshProUGUI itemText;

    // Update is called once per frame
    void Update()
    {
        itemText.gameObject.SetActive(false);

        Ray ray = Camera.main.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.CompareTag("Item"))
            {
                itemText.gameObject.SetActive(true);
                itemText.text = hit.collider.gameObject.name;
            }
        }
    }
}