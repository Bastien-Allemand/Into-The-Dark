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
    private void OnInteract(InputAction.CallbackContext _context)
    {
        if (_context.control.name == "leftButton" && leftHandContent.filled)
        {
            if(leftHandContent.itemScript.needsToBePlaced)
            {
                if(editMode && preview.activeSelf)
                {
                    isPlacingLeft = true;
                    playerView.canLook = false;
                    preview.GetComponent<MeshFilter>().mesh = leftHandContent.inHand.GetComponent<MeshFilter>().mesh;
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
                if (editMode && preview.activeSelf)
                {
                    isPlacingRight = true;
                    playerView.canLook = false;
                    preview.GetComponent<MeshFilter>().mesh = rightHandContent.inHand.GetComponent<MeshFilter>().mesh;
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
                if(editMode && isPlacingLeft && preview.activeSelf)
                {
                    isPlacingLeft = false;
                    leftHandContent.itemScript.deployed = true;
                    GameObject obj = leftHandContent.TakeOutObject(false);

                    if (obj != null && preview != null)
                    {
                        obj.transform.position = preview.transform.position;
                        obj.transform.rotation = preview.transform.rotation;
                        preview.GetComponent<MeshFilter>().mesh = previewMesh;
                    }

                    if (!rightHandContent.filled && !leftHandContent.filled)
                    {
                        editMode = false;
                        preview.SetActive(editMode);
                    }

                    playerView.canLook = true;
                }
            }
        }
        else if (_context.control.name == "rightButton" && rightHandContent.filled)
        {
            if (rightHandContent.itemScript.needsToBePlaced)
            {
                if (editMode && isPlacingRight && preview.activeSelf)
                {
                    isPlacingRight = false;
                    rightHandContent.itemScript.deployed = true;
                    GameObject obj = rightHandContent.TakeOutObject(false);

                    if (obj != null && preview != null)
                    {
                        obj.transform.position = preview.transform.position;
                        obj.transform.rotation = preview.transform.rotation;
                        preview.GetComponent<MeshFilter>().mesh = previewMesh;
                    }

                    if (!rightHandContent.filled && !leftHandContent.filled)
                    {
                        editMode = false;
                        preview.SetActive(editMode);
                    }
                    playerView.canLook = true;
                }
            }
        }
    }
    private void SwitchEditMode(InputAction.CallbackContext _context)
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
                Vector2 lookInput = controls.GamePlay.Look.ReadValue<Vector2>();

                currentRotation += lookInput.x * rotationSpeed * Time.deltaTime;

                preview.transform.localRotation = Quaternion.Euler(0, currentRotation, 0);
            }
            Debug.Log(preview);
            Debug.Log(playerView);
            Debug.Log(controls);
        }
    }

}
