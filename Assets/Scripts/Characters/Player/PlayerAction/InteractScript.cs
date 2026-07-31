using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private PlayerAction controls;

    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [SerializeField] private GameObject displayHands;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    [Header("Sprites")]
    [SerializeField] private Image openedLeft;
    [SerializeField] private Image closedLeft;
    [SerializeField] private Image openedRight;
    [SerializeField] private Image closedRight;

    [Header("Interaction")]
    [SerializeField] private float interactDistance = 5f;
    [SerializeField] private LayerMask interactableLayers = ~0;

    private Readable currentReadObject;

    private bool isHoveringInteractive;
    private bool isHoldingLeft;
    private bool isHoldingRight;

    private void Awake()
    {
        controls = InputManager.controls;

        if (playerCamera == null)
            playerCamera = Camera.main;

        HideHandsUI();
    }

    private void OnEnable()
    {
        controls.PlayerMoves.Interact.performed += OnInteractPressed;
        controls.PlayerMoves.Interact.canceled += OnInteractReleased;
    }

    private void OnDisable()
    {
        controls.PlayerMoves.Interact.performed -= OnInteractPressed;
        controls.PlayerMoves.Interact.canceled -= OnInteractReleased;

        HideHandsUI();
    }

    private void Update()
    {
        CheckHoverObject();
    }

    private bool LeftHandOccupied()
    {
        return leftHand != null && leftHand.transform.childCount > 0;
    }

    private bool RightHandOccupied()
    {
        return rightHand != null && rightHand.transform.childCount > 0;
    }

    private void UpdateHandsUI(bool leftClosed, bool rightClosed)
    {
        displayHands.SetActive(true);

        bool leftFree = !LeftHandOccupied();
        bool rightFree = !RightHandOccupied();

        openedLeft.enabled = leftFree && !leftClosed;
        closedLeft.enabled = leftFree && leftClosed;

        openedRight.enabled = rightFree && !rightClosed;
        closedRight.enabled = rightFree && rightClosed;
    }

    private void HideHandsUI()
    {
        displayHands.SetActive(false);
    }

    private void OnInteractPressed(InputAction.CallbackContext ctx)
    {
        if (ctx.control.name == "leftButton")
            isHoldingLeft = true;

        if (ctx.control.name == "rightButton")
            isHoldingRight = true;

        if (isHoveringInteractive)
            UpdateHandsUI(isHoldingLeft, isHoldingRight);

        InteractWithObject(ctx.control.name == "leftButton",
                           ctx.control.name == "rightButton");
    }

    private void OnInteractReleased(InputAction.CallbackContext ctx)
    {
        if (ctx.control.name == "leftButton")
            isHoldingLeft = false;

        if (ctx.control.name == "rightButton")
            isHoldingRight = false;

        if (isHoveringInteractive)
            UpdateHandsUI(isHoldingLeft, isHoldingRight);
        else
            HideHandsUI();
    }

    private void CheckHoverObject()
    {
        if (playerCamera == null)
            return;

        if (currentReadObject != null)
        {
            if (isHoveringInteractive)
            {
                isHoveringInteractive = false;
                HideHandsUI();
            }
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        bool lookingAtTarget = false;

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayers, QueryTriggerInteraction.Ignore))
        {
            lookingAtTarget =
                hit.collider.GetComponentInParent<ComplexAnimatorScript>() != null ||
                hit.collider.GetComponentInParent<BasicAnimationScript>() != null ||
                hit.collider.GetComponentInParent<Readable>() != null ||
                hit.collider.CompareTag("Readable") ||
                hit.collider.CompareTag("Lootable") ||
                hit.collider.CompareTag("Item") ||
                (hit.collider.transform.parent != null &&
                 hit.collider.transform.parent.CompareTag("Item"));
        }

        if (lookingAtTarget != isHoveringInteractive)
        {
            isHoveringInteractive = lookingAtTarget;

            if (lookingAtTarget)
                UpdateHandsUI(isHoldingLeft, isHoldingRight);
            else
                HideHandsUI();
        }
    }

    private void InteractWithObject(bool leftClick, bool rightClick)
    {
        if (leftClick && LeftHandOccupied())
            return;

        if (rightClick && RightHandOccupied())
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

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayers, QueryTriggerInteraction.Ignore))
            return;

        ComplexAnimatorScript animator = hit.collider.GetComponentInParent<ComplexAnimatorScript>();

        if (animator != null)
        {
            animator.Interact();
            return;
        }

        BasicAnimationScript loot = hit.collider.GetComponentInParent<BasicAnimationScript>();

        if (loot != null)
        {
            loot.Interact();
            return;
        }

        Readable readable = hit.collider.GetComponentInParent<Readable>();

        if (readable != null)
        {
            readable.PlaceObjectInFOV();
            currentReadObject = readable;
        }
    }
}