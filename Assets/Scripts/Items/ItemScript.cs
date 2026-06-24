using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public bool Deployed = false;
    public float timer = 0f;
    public bool UsingTimer = false;

    public void Update()
    {
        if (Deployed && UsingTimer)
        {
            if (timer < 0f)
            {
                Destroy(gameObject);
            }
            timer -= Time.deltaTime;
        }
    }
}
