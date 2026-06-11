using UnityEngine;

public class RadioScript : MonoBehaviour
{
    public string targetTag = "Ghost";
    public bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Ghost entered detection zone");
            isActivated = true;
        }
    }
    //maybe start audio when enter and stop audio when exit
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Ghost left detection zone");
            isActivated = false;
        }
    }
    void Update()
    {
        if (isActivated)
        {
            Debug.Log("Radio is activated");
        }
    }
}
