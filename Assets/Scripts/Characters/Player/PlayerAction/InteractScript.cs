using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    private PlayerAction controls;

    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [Header("Interaction")]
    [SerializeField] private float interactDistance = 5f;

    [SerializeField] private Image leftHandImageComponent;
    [SerializeField] private Image rightHandImageComponent;

    [Header("UI Sprites")]
    [SerializeField] private Sprite rihtHandOpened;
    [SerializeField] private Sprite leftHandOpened;
    [SerializeField] private Sprite rihtHandClosed;
    [SerializeField] private Sprite leftHandClosed;

    [Header("HandContent References")]
    [SerializeField] private HandContent leftHandContent;
    [SerializeField] private HandContent rightHandContent;

    private Readable currentReadObject;
    private bool isHoveringInteractive = false;

    private bool isHoldingLeft = false;
    private bool isHoldingRight = false;

    private void Awake()
    {
        controls = InputManager.controls;
    }

    private void Start()
    {
        HideUIHands();
    }

    private void OnEnable()
    {
        controls.PlayerInteraction.Interact.performed += OnInteractPressed;
        controls.PlayerInteraction.Interact.canceled += OnInteractReleased;
    }

    private void OnDisable()
    {
        controls.PlayerInteraction.Interact.performed -= OnInteractPressed;
        controls.PlayerInteraction.Interact.canceled -= OnInteractReleased;

        isHoldingLeft = false;
        isHoldingRight = false;
        HideUIHands();
    }

    private void Update()
    {
        CheckHoverObject();
    }

    private void OnInteractPressed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        bool isLeft = ctx.control.name == "leftButton";
        bool isRight = ctx.control.name == "rightButton";

        if (isLeft)
        {
            isHoldingLeft = true;
        }
        else if (isRight)
        {
            isHoldingRight = true;
        }
        else
        {
            isHoldingLeft = true;
            isHoldingRight = true;
        }

        if (isHoveringInteractive)
        {
            SetHandsUIState(isHoldingLeft, isHoldingRight);
        }

        InteractWithObject(isLeft, isRight);
    }

    private void OnInteractReleased(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if (ctx.control.name == "leftButton")
        {
            isHoldingLeft = false;
        }
        else if (ctx.control.name == "rightButton")
        {
            isHoldingRight = false;
        }
        else
        {
            isHoldingLeft = false;
            isHoldingRight = false;
        }

        if (isHoveringInteractive)
        {
            SetHandsUIState(isHoldingLeft, isHoldingRight);
        }
        else
        {
            HideUIHands();
        }
    }

    private bool LeftHandHasObject()
    {
        return leftHandContent != null && leftHandContent.filled;
    }

    private bool RightHandHasObject()
    {
        return rightHandContent != null && rightHandContent.filled;
    }

    private void CheckHoverObject()
    {
        if (currentReadObject != null)
        {
            if (isHoveringInteractive || (leftHandImageComponent != null && leftHandImageComponent.enabled) || (rightHandImageComponent != null && rightHandImageComponent.enabled))
            {
                isHoveringInteractive = false;
                HideUIHands();
            }
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        bool lookingAtTarget = false;

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            bool isReadable = hit.collider.GetComponentInParent<Readable>() != null || hit.collider.CompareTag("Readable");
            bool isLootable = hit.collider.GetComponentInParent<BasicAnimationScript>() != null || hit.collider.CompareTag("Lootable");
            bool isGadjet = hit.collider.CompareTag("Item") || (hit.collider.transform.parent != null && hit.collider.transform.parent.CompareTag("Item"));

            if (isReadable || isLootable || isGadjet)
            {
                lookingAtTarget = true;
            }
        }

        if (lookingAtTarget != isHoveringInteractive)
        {
            isHoveringInteractive = lookingAtTarget;

            if (isHoveringInteractive)
            {
                SetHandsUIState(isHoldingLeft, isHoldingRight);
            }
            else
            {
                HideUIHands();
            }
        }
    }

    private void SetHandsUIState(bool leftClosed, bool rightClosed)
    {
        if (leftHandImageComponent != null)
        {
            if (LeftHandHasObject())
            {
                leftHandImageComponent.enabled = false;
            }
            else
            {
                leftHandImageComponent.sprite = leftClosed ? leftHandClosed : leftHandOpened;
                leftHandImageComponent.enabled = (leftHandImageComponent.sprite != null);
            }
        }

        if (rightHandImageComponent != null)
        {
            if (RightHandHasObject())
            {
                rightHandImageComponent.enabled = false;
            }
            else
            {
                rightHandImageComponent.sprite = rightClosed ? rihtHandClosed : rihtHandOpened;
                rightHandImageComponent.enabled = (rightHandImageComponent.sprite != null);
            }
        }
    }

    private void HideUIHands()
    {
        if (leftHandImageComponent != null) leftHandImageComponent.enabled = false;
        if (rightHandImageComponent != null) rightHandImageComponent.enabled = false;
    }

    private void InteractWithObject(bool isLeftClick, bool isRightClick)
    {
        if (isLeftClick && LeftHandHasObject())
            return;

        if (isRightClick && RightHandHasObject())
            return;

        if (currentReadObject != null)
        {
            if (currentReadObject.IsInAnimation)
                return;

            currentReadObject.PlaceObjectInFOV();

            if (!currentReadObject.IsReading)
                currentReadObject = null;

            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            return;

        ComplexAnimatorScript animatorObject = hit.collider.GetComponentInParent<ComplexAnimatorScript>();

        if (animatorObject != null)
        {
            animatorObject.Interact();
            return;
        }

        BasicAnimationScript lootItem = hit.collider.GetComponentInParent<BasicAnimationScript>();

        if (lootItem != null)
        {
            lootItem.Interact();
            return;
        }

        Readable readable = hit.collider.GetComponentInParent<Readable>();

        if (readable != null)
        {
            readable.PlaceObjectInFOV();
            currentReadObject = readable;
            return;
        }
    }
}