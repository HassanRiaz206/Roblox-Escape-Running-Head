using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Button jumpButton;
    public Animator animator;
    public Transform cameraTransform;
    public LayerMask groundMask;
    public List<string> pushBackTags; // Tags that should trigger push back
    public float pushBackForce = 2f; // Adjust this value as needed

    private Rigidbody rb;
    private bool isGrounded = false;
    private float jumpCooldown = 1f;
    private float lastJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.None; // Disable interpolation
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Use continuous collision detection
        jumpButton.onClick.AddListener(Jump);
        lastJumpTime = -jumpCooldown;
    }

    void Update()
    {
        Animate();
        CheckGroundStatus();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        direction = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * direction;
        direction.y = 0;

        // Invert the direction for opposite movement
        direction = -direction;

        if (direction.magnitude >= 0.1f)
        {
            rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
            transform.forward = direction;
        }
    }

    void Jump()
    {
        if (isGrounded && Time.time - lastJumpTime >= jumpCooldown)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time;
            isGrounded = false; // Reset grounded status
        }
    }

    void Animate()
    {
        bool isMoving = IsMovingWithJoystick();
        animator.SetBool("isRun", isMoving);
        animator.SetBool("isJump", !isGrounded && (Time.time - lastJumpTime < jumpCooldown));
        if (!isMoving && isGrounded)
        {
            animator.SetBool("isRun", false);
            animator.SetBool("isJump", false);
        }
    }

    void CheckGroundStatus()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        isGrounded = Physics.Raycast(ray, 0.3f, groundMask);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsGroundOrJumpable(collision))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            HandleWallCollision(collision);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (IsGroundOrJumpable(collision))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (IsGroundOrJumpable(collision))
        {
            isGrounded = false;
        }
    }

    bool IsGroundOrJumpable(Collision collision)
    {
        return collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Jumpable");
    }

    void HandleWallCollision(Collision collision)
    {
        // Logic to handle wall collision
        // Push the player back slightly to prevent passing through the wall
        Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
        rb.MovePosition(transform.position + pushDirection * 0.1f); // Adjust the push back distance as needed
    }

    void OnTriggerEnter(Collider other)
    {
        if (pushBackTags.Contains(other.gameObject.tag))
        {
            Vector3 pushDirection = (transform.position - other.transform.position).normalized;
            rb.AddForce(pushDirection * pushBackForce, ForceMode.Impulse);
        }
    }

    public bool IsMovingWithJoystick()
    {
        return joystick.Horizontal != 0 || joystick.Vertical != 0;
    }

    public bool IsJumpButtonPressed()
    {
        return jumpButton != null;
    }

    public bool IsTouchingJoystick()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Joystick"))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsSecondTouchDetected()
    {
        return Input.touchCount > 1;
    }

    // Method to check if touch is over the swipe panel
    public bool IsPointerOverSwipePanel(Vector2 touchPosition)
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
}
