    using UnityEngine;
    using UnityEngine.InputSystem;
public class SoundScript : MonoBehaviour
{
    [SerializeField] private bool debug = false;
    private bool MakingSound = true;
    public float SoundTimer = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    void Update()
    {
        if (!MakingSound)
            return;

        if (SoundTimer < 0)
            Destroy(this.gameObject);

        if (debug)
            Debug.Log("Sound at: " + transform.position);

        SoundTimer -= Time.deltaTime;
    }

    public void Resize(float _scale )
    {
        transform.localScale = new Vector3(_scale, _scale, _scale);
    }
}
