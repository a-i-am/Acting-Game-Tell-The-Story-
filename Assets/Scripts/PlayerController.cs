using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float walkSpeed = 6;
    public float jumpForce = 220;
    public LayerMask groundedMask;

    bool grounded;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    Vector3 moveDir;
    Vector3 lastMoveDir;
    float verticalLookRotation;
    Transform cameraTransform;

    Animator anim;
    Rigidbody rb;


    void Awake()
    {
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Calculate movement:
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(inputX, 0, inputY).normalized;

        // ī�޶��� ������ ����Ͽ� �̵� ������ ���
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f; // ī�޶��� y ������ �����Ͽ� ���� ���⸸ ���
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0f; // ���������� ���� ���⸸ ���
        right.Normalize();

        // �̵� ������ ī�޶��� ȸ�� ������ �������� ���
        Vector3 targetMoveAmount = (forward * moveDir.z + right * moveDir.x) * walkSpeed;

        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        // Jump
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }

        // Grounded check
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 1 + .1f, groundedMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void FixedUpdate()
    {
            MoveRb();
    }

    void MoveRb()
    {
        // Apply movement to rb
        Vector3 localMove = new Vector3(moveAmount.x, 0, moveAmount.z) * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + localMove);
    }
}
