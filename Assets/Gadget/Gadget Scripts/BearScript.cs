using UnityEngine;

public class BearScript : MonoBehaviour
{
    [SerializeField] public GameObject soundObject;
    private bool MakingSound = true;
    private bool JustPlaced = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MakingSound)
        {
            soundObject.GetComponent<SoundScript>().SoundTimer = 100;
        }
        if (JustPlaced)
        {
            JustPlaced = false;
            GameObject sound = Instantiate(soundObject, transform.position, Quaternion.identity);
            sound.GetComponent<SoundScript>().Resize(6.0f);
        }
    }
}
