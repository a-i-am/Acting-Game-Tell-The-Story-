using EightDirectionalSpriteSystem;
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
    float inputX;
    float inputY;


    Animator anim;
    Rigidbody rb;
    ActorBillboard actorBillboard;

    void Awake()
    {
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        actorBillboard = GetComponent<ActorBillboard>();
    }

    void Update()
    {
        if (DialogueManager.Instance.isDialogueActive)
        {
            // ��ȭ ���� �� �̵��� ��޴ϴ�.
            jumpForce = 0;
            inputX = 0;
            inputY = 0;

            if(grounded) rb.constraints |= RigidbodyConstraints.FreezePosition;

            //gameObject.GetComponent<ActorBillboard>().enabled = false;
        }
        else
        {
            //gameObject.GetComponent<ActorBillboard>().enabled = true;
            // ��ȭ�� ������ �� �̵� ������ �����մϴ�.
            jumpForce = 220;
            rb.constraints &= ~RigidbodyConstraints.FreezePosition;
        }



        // Calculate movement:
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

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

        // ����ĳ��Ʈ�� �ð������� ǥ��
        Debug.DrawRay(ray.origin, ray.direction * 0.6f, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, 0.6f, groundedMask))
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
