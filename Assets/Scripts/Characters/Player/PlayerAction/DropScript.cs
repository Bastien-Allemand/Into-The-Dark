using UnityEngine;
using UnityEngine.InputSystem;

public class DropScript : MonoBehaviour
{
    [SerializeField] public Camera playerCamera;

    [Header("Hands")]
    [SerializeField] public GameObject leftHand;
    [SerializeField] public GameObject rightHand;

    private HandContent leftHandContent;
    private HandContent rightHandContent;

    private PlayerAction controls;

    [Header("Throw settings")]
    [SerializeField] private float minForce = 5f;
    [SerializeField] public float maxForce = 20f;
    [SerializeField] private float chargeSpeed = 10f;

    public float leftCharge;
    public float rightCharge;

    public bool chargingLeft;
    public bool chargingRight;

    private void Awake()
    {
        controls = InputManager.controls;

        leftHandContent = leftHand.GetComponent<HandContent>();
        rightHandContent = rightHand.GetComponent<HandContent>();
    }
    private void OnEnable()
    {
        controls.GamePlay.ThrowLeftObject.started += OnLeftStart;
        controls.GamePlay.ThrowLeftObject.canceled += OnLeftRelease;

        controls.GamePlay.ThrowRightObject.started += OnRightStart;
        controls.GamePlay.ThrowRightObject.canceled += OnRightRelease;
    }

    private void OnDisable()
    {
        controls.GamePlay.ThrowLeftObject.started -= OnLeftStart;
        controls.GamePlay.ThrowLeftObject.canceled -= OnLeftRelease;

        controls.GamePlay.ThrowRightObject.started -= OnRightStart;
        controls.GamePlay.ThrowRightObject.canceled -= OnRightRelease;
    }

    private void OnLeftStart(InputAction.CallbackContext ctx)
    {
        if (!leftHandContent.filled) return;

        chargingLeft = true;
        leftCharge = 0f;
    }

    private void OnLeftRelease(InputAction.CallbackContext ctx)
    {
        if (!chargingLeft) return;

        chargingLeft = false;
        Throw(leftHandContent, leftCharge);
    }

    private void OnRightStart(InputAction.CallbackContext ctx)
    {
        if (!rightHandContent.filled) return;

        chargingRight = true;
        rightCharge = 0f;
    }

    private void OnRightRelease(InputAction.CallbackContext ctx)
    {
        if (!chargingRight) return;

        chargingRight = false;
        Throw(rightHandContent, rightCharge);
    }

    private void Update()
    {
        if (chargingLeft)
            leftCharge += Time.deltaTime * chargeSpeed;

        if (chargingRight)
            rightCharge += Time.deltaTime * chargeSpeed;
    }

    private void Throw(HandContent hand, float charge)
    {
        GameObject obj = hand.TakeOutObject(true);

        if (obj == null)
            return;

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError($"{obj.name} has no Rigidbody!");
            return;
        }

        float force = Mathf.Clamp(charge, minForce, maxForce);

        rb.AddForce(
            playerCamera.transform.forward * force,
            ForceMode.Impulse
        );
    }

}
