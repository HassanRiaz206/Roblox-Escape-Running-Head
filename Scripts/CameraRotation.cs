using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotation : MonoBehaviour
{
    public Transform[] players;
    public float rotationSpeed = 0.1f;
    public float maxUpwardRotation = 45f;
    public float maxDownwardRotation = 45f;
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;
    public float feetOffset = 1f;

    private Vector3 lastMousePosition;
    private float horizontalRotation;
    private float verticalRotation;
    private float currentVerticalRotation;

    private PlayerMovement activePlayerMovement;

    private static CameraRotation instance;

    private int joystickFingerId = -1;
    private List<int> swipePanelFingerIds = new List<int>();

    public static CameraRotation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraRotation>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("CameraRotation");
                    instance = obj.AddComponent<CameraRotation>();
                }
            }
            return instance;
        }
    }

    void Start()
    {
        FindActivePlayerMovement();
    }

    void Update()
    {
        if (activePlayerMovement == null || !activePlayerMovement.isActiveAndEnabled)
        {
            FindActivePlayerMovement();
        }

        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                // Track the finger ID that is touching the joystick
                if (IsPointerOverJoystick(touch.position) && joystickFingerId == -1)
                {
                    joystickFingerId = touch.fingerId;
                }

                // Track finger IDs that start on the swipe panel
                if (IsPointerOverSwipePanel(touch.position) && !swipePanelFingerIds.Contains(touch.fingerId))
                {
                    swipePanelFingerIds.Add(touch.fingerId);
                }

                // Handle camera rotation if the touch is over the swipe panel and not the joystick finger
                if (CanRotateWithFinger(touch.fingerId) && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began))
                {
                    Vector2 touchDeltaPosition = touch.deltaPosition;
                    horizontalRotation = touchDeltaPosition.x * rotationSpeed;
                    verticalRotation = -touchDeltaPosition.y * rotationSpeed;
                    ApplyCameraRotation();
                }

                // Reset joystick and swipe panel finger IDs when the touch ends
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (touch.fingerId == joystickFingerId)
                    {
                        joystickFingerId = -1;
                    }
                    if (swipePanelFingerIds.Contains(touch.fingerId))
                    {
                        swipePanelFingerIds.Remove(touch.fingerId);
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0) && IsPointerOverSwipePanel(Input.mousePosition))
        {
            // Only rotate if no joystick finger is currently on the swipe panel
            if (joystickFingerId == -1)
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                horizontalRotation = delta.x * rotationSpeed;
                verticalRotation = -delta.y * rotationSpeed;
                ApplyCameraRotation();

                lastMousePosition = Input.mousePosition;
            }
        }

        lastMousePosition = Input.mousePosition;
    }

    public bool CanRotateWithFinger(int id)
    {
        return swipePanelFingerIds.Contains(id) && id != joystickFingerId;
    }

    private bool CanRotateCamera()
    {
        if (activePlayerMovement == null) return true;

        bool touchingJoystickOrJumpButton = activePlayerMovement.IsTouchingJoystick() || activePlayerMovement.IsJumpButtonPressed();
        return !touchingJoystickOrJumpButton;
    }

    private void ApplyCameraRotation()
    {
        if (activePlayerMovement == null) return;

        Vector3 rotationPoint = activePlayerMovement.transform.position - Vector3.up * feetOffset;
        currentVerticalRotation += verticalRotation;
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, -maxDownwardRotation, maxUpwardRotation);

        transform.RotateAround(rotationPoint, Vector3.up, horizontalRotation);
        transform.rotation = Quaternion.Euler(currentVerticalRotation, transform.eulerAngles.y, 0);

        horizontalRotation = 0f;
        verticalRotation = 0f;
    }

    void LateUpdate()
    {
        if (activePlayerMovement == null || !activePlayerMovement.isActiveAndEnabled)
        {
            FindActivePlayerMovement();
        }

        if (activePlayerMovement == null)
        {
            Debug.LogWarning("No active player found.");
            return;
        }

        Vector3 desiredPosition = activePlayerMovement.transform.position - transform.forward * cameraDistance + Vector3.up * cameraHeight;
        transform.position = desiredPosition;
    }

    private void FindActivePlayerMovement()
    {
        foreach (Transform playerTransform in players)
        {
            if (playerTransform == null) continue;

            PlayerMovement pm = playerTransform.GetComponent<PlayerMovement>();
            if (pm != null && pm.isActiveAndEnabled)
            {
                activePlayerMovement = pm;
                break;
            }
        }

        if (activePlayerMovement == null)
        {
            Debug.LogError("No active PlayerMovement script found among the provided players.");
        }
    }

    private bool IsPointerOverSwipePanel(Vector2 touchPosition)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = touchPosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject.CompareTag("SwipePanel"))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPointerOverJoystick(Vector2 touchPosition)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = touchPosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject.CompareTag("Joystick"))
            {
                return true;
            }
        }

        return false;
    }
}
