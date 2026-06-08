using Unity.VisualScripting;
using UnityEngine;

public class VentolineScript : MonoBehaviour
{
    public GameObject ventoline;
    public GameObject player;

    void Start()
    {

    }

    void Update()
    {
        if (ventoline.transform.parent != null && Input.GetKeyDown(KeyCode.Space))
        {
            OnAction();
        }
    }

    private void OnAction()
    {
        Debug.Log("Stamina Regen");

        player.GetComponent<PlayerController>().RegenAllStamina();

        Destroy(ventoline);
    }
}