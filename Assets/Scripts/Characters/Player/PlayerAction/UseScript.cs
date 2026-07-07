using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlaceScript : MonoBehaviour
{
    [Header("Fill In")]
    [SerializeField] public Camera playerCamera;
    [SerializeField] public PlayerView playerView;
    [SerializeField] public PlayerAction controls;

    public HandContent leftHandContent;
    public HandContent rightHandContent;

    [SerializeField] public GameObject preview;
    Mesh previewMesh;

    [Header("Auto Finding")]

    private bool isPlacingLeft = false;
    private bool isPlacingRight = false;

    private bool editMode = false;
    private float currentRotation;
    private Vector3 placePosition;



    [SerializeField] private float rotationSpeed = 100f;

    private  bool CheckSurface(out Vector3 _hitPoint)
    {
        Raycast.RaycastResult ray = Raycast.CheckRaycast(playerCamera, 4);
        bool result = false;    
        _hitPoint = Vector3.zero;
        if (ray != null )
        {
            if(!preview.activeSelf)
            {
                preview.SetActive(true);
            }
            if (ray.hitObject.layer == 16)
            {
                _hitPoint = ray.hitPoint;
                result = true;
            }
        }
        else
        {
            if (preview.activeSelf)
            {
                preview.SetActive(false);
            }
        }
            return result;
    }
    private void OnInteract(HandContent hand)
    {
        if (hand.filled)
        {
            if(hand.itemScript.needsToBePlaced)
            {
                if(editMode && preview.activeSelf)
                {
                    isPlacingLeft = true;
                    playerView.canLook = false;
                    preview.GetComponent<MeshFilter>().mesh = hand.inHand.GetComponent<MeshFilter>().mesh;
                }
            }
            else
            {
                hand.itemScript.deployed = true;
            }
        }
    }
    private void PlaceObject(HandContent hand)
    {
        if (hand.filled)
        {
            if(hand.itemScript.needsToBePlaced)
            {
                if(editMode && isPlacingLeft && preview.activeSelf)
                {
                    isPlacingLeft = false;
                    hand.itemScript.deployed = true;
                    GameObject obj = hand.TakeOutObject(false);

                    if (obj != null && preview != null)
                    {
                        obj.transform.position = preview.transform.position;
                        obj.transform.rotation = preview.transform.rotation;
                        preview.GetComponent<MeshFilter>().mesh = previewMesh;
                    }

                    if (!rightHandContent.filled && !hand.filled)
                    {
                        editMode = false;
                        preview.SetActive(editMode);
                    }

                    playerView.canLook = true;
                }
            }
        }
    }
    private void SwitchEditMode()
    {
        if (rightHandContent.filled || leftHandContent.filled)
        {
            editMode = !editMode;
            preview.SetActive(editMode);
        }

        Debug.Log($"Edit Mode: {editMode}");
    }

    private void OnEnable()
    {
        controls.PlayerInteraction.TakeUseLeftobject.started += _ => OnInteract(leftHandContent);
        controls.PlayerInteraction.TakeUseRightobject.started += _ => OnInteract(rightHandContent);

        controls.PlayerInteraction.TakeUseLeftobject.canceled += _ => PlaceObject(leftHandContent);
        controls.PlayerInteraction.TakeUseRightobject.canceled += _ => PlaceObject(rightHandContent);

        controls.PlayerInteraction.EnterLeaveEditMode.performed += _ => SwitchEditMode();
    }
    private void OnDisable()
    {
        controls.PlayerInteraction.TakeUseLeftobject.started -= _ => OnInteract(leftHandContent);
        controls.PlayerInteraction.TakeUseRightobject.started -= _ => OnInteract(rightHandContent);

        controls.PlayerInteraction.TakeUseLeftobject.canceled -= _ => PlaceObject(leftHandContent);
        controls.PlayerInteraction.TakeUseRightobject.canceled -= _ => PlaceObject(rightHandContent);

        controls.PlayerInteraction.EnterLeaveEditMode.performed -= _ => SwitchEditMode();
    }

    private void Awake()
    {
        controls = InputManager.controls;
        previewMesh = preview.GetComponent<MeshFilter>().mesh;
    }

    public void Update()
    {
        if (editMode && preview != null)
        {
            if (CheckSurface(out Vector3 hitPoint))
            {
                preview.transform.position = hitPoint;
            }
            if (isPlacingLeft || isPlacingRight)
            {
                Vector2 lookInput = controls.GeneriqueMove.Look.ReadValue<Vector2>();

                currentRotation += lookInput.x * rotationSpeed * Time.deltaTime;

                preview.transform.localRotation = Quaternion.Euler(0, currentRotation, 0);
            }
            Debug.Log(preview);
            Debug.Log(playerView);
            Debug.Log(controls);
        }
    }

}
