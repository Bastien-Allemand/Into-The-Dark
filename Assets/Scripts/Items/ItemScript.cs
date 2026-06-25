using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public bool deployed = false;
    public bool needsToBePlaced = false;
    public bool usingTimer = false;
    
    public float placingDelay = 0f;
    public float timer = 0f;

    public void Update()
    {
        if (deployed && usingTimer)
        {
            if (timer < 0f)
            {
                Destroy(gameObject);
            }
            timer -= Time.deltaTime;
        }
    }
}
