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
            if (ray.hitObject.layer == 16)
            {
                _hitPoint = ray.hitPoint;
                result = true;
            }
        }
        return result;
    }
    private void OnInteract(InputAction.CallbackContext _context)
    {
        if (_context.control.name == "leftButton" && leftHandContent.filled)
        {
            if(leftHandContent.itemScript.needsToBePlaced)
            {
                if(editMode)
                {
                    isPlacingLeft = true;
                }
            }
            else
            {
                leftHandContent.itemScript.deployed = true;
            }
        }
        else if (_context.control.name == "rightButton" && rightHandContent.filled)
        {
            if (rightHandContent.itemScript.needsToBePlaced)
            {
                if (editMode)
                {
                    isPlacingRight = true;
                }
            }
            else
            {
                rightHandContent.itemScript.deployed = true;
            }
        }
    }
    private void PlaceObject(InputAction.CallbackContext _context)
    {
        if (_context.control.name == "leftButton" && leftHandContent.filled)
        {
            if(leftHandContent.itemScript.needsToBePlaced)
            {
                if(editMode && isPlacingLeft)
                {
                    isPlacingLeft = false;
                    leftHandContent.itemScript.deployed = true;
                    GameObject obj = leftHandContent.TakeOutObject();

                    if (obj != null && preview != null)
                    {
                        obj.transform.position = preview.transform.position;
                    }

                    preview.SetActive(false);
                    playerView.canLook = true;
                }
            }
        }
        else if (_context.control.name == "rightButton" && rightHandContent.filled)
        {
            if (rightHandContent.itemScript.needsToBePlaced)
            {
                if (editMode && isPlacingLeft)
                {
                    isPlacingLeft = false;
                    rightHandContent.itemScript.deployed = true;
                    GameObject obj = rightHandContent.TakeOutObject();

                    if (obj != null && preview != null)
                    {
                        obj.transform.position = preview.transform.position;
                    }

                    preview.SetActive(false);
                    playerView.canLook = true;
                }
            }
        }
    }
    private void SwitchEditMode(InputAction.CallbackContext _context)
    {
        editMode = !editMode;
        Debug.Log($"Edit Mode: {editMode}");
    }

    private void OnEnable()
    {
        controls.GamePlay.TakePlaceobject.started += OnInteract;
        controls.GamePlay.TakePlaceobject.canceled += PlaceObject;
        controls.GamePlay.EnterLeaveEditMode.performed += SwitchEditMode;
    }
    private void OnDisable()
    {
        controls.GamePlay.TakePlaceobject.performed -= OnInteract;
        controls.GamePlay.TakePlaceobject.canceled -= PlaceObject;
        controls.GamePlay.EnterLeaveEditMode.performed -= SwitchEditMode;
    }

    private void Awake()
    {
        controls = InputManager.controls;
    }

    public void Update()
    {
        if (editMode && preview != null)
        {
            if (CheckSurface(out Vector3 hitPoint))
            {
                preview.SetActive(true);
                preview.transform.position = hitPoint;

            }
            else
            {
                preview.SetActive(false);
            }
            if (isPlacingLeft || isPlacingRight)
            {
                playerView.canLook = false;

                Vector2 lookInput = controls.GamePlay.Look.ReadValue<Vector2>();

                currentRotation += lookInput.x * rotationSpeed * Time.deltaTime;

                preview.transform.rotation =
                    Quaternion.Euler(0, currentRotation, 0);
            }
            else
            {

            }
        }
    }

}
