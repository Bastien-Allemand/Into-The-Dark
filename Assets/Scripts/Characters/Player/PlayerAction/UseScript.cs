using UnityEngine;

public class PlaceScript : MonoBehaviour
{
    [Header("Fill In")]
    [SerializeField] public Camera playerCamera;

    [SerializeField] public PlayerAction controls;

    [SerializeField] public GameObject leftHand;
    [SerializeField] public GameObject rightHand;


    [Header("Auto Finding")]
    private HandContent leftHandContent;
    private HandContent rightHandContent;

    private LayerMask placeableAreas;
    private bool EditMode = false;
    private void CheckSurface()
    {
       GameObject surface = Raycast.CheckRaycast(playerCamera, 4);
        if (surface != null )
        {
            if (surface.CompareTag("Placeable"))
            {

            }
        }

    }

    private void OnEnable()
    {
        controls.GamePlay.EnterLeaveEditMode.performed += OnInteract;
        controls.GamePlay.TakePlaceobject.performed += OnInteract;
    }
    private void OnDisable()
    {
        controls.GamePlay.EnterLeaveEditMode.performed -= OnInteract;
        controls.GamePlay.TakePlaceobject.performed += OnInteract;
    }


    private void Awake()
    {
        controls = InputManager.controls;
        leftHandContent = leftHand.GetComponent<HandContent>();
        rightHandContent = rightHand.GetComponent<HandContent>();
    }

    public void Update()
    {
        if(EditMode)
        {
            CheckSurface();
        }
    }

}
