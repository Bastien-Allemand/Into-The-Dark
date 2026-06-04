    using UnityEngine;
    using UnityEngine.InputSystem;
    public class SoundScript : MonoBehaviour
    {
        [SerializeField] private int soundIntensity;
    [SerializeField] private int soundRadius;
    public bool isMakingSound = false ;
        private SphereCollider soundSphere; 
        private GameObject soundDebug;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
        {

        soundSphere = new SphereCollider();
  
        soundSphere = gameObject.AddComponent<SphereCollider>();
        soundSphere.isTrigger = true;

 
        soundSphere.center = transform.position ;
       
        soundSphere.radius = soundIntensity * soundRadius;

        soundDebug = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        Destroy(soundDebug.GetComponent<Collider>());

        MeshRenderer meshRend = soundDebug.GetComponent<MeshRenderer>();
            meshRend.material.color = Color.red;

            soundDebug.GetComponent<Transform>().transform.localPosition = soundSphere.center ;
            soundDebug.transform.SetParent(this.transform);

        soundDebug.transform.localScale = new Vector3(soundIntensity * soundRadius, soundIntensity * soundRadius, soundIntensity * soundRadius);
    }

        // Update is called once per frame
        void Update()
        {
        Vector3 position = soundDebug.GetComponent<Transform>().position;

        Debug.Log("Position globale : " + soundSphere.center);
        Debug.Log("Position globale : " + position);
        isMakingSound = Keyboard.current.spaceKey.isPressed;

            soundSphere.enabled = isMakingSound;
        soundDebug.SetActive(true);
        soundDebug.transform.localScale = new Vector3(soundIntensity * soundRadius, soundIntensity * soundRadius, soundIntensity * soundRadius);
        soundDebug.SetActive(isMakingSound);

        }
    }
