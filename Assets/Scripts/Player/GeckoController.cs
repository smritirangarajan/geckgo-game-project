using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GeckoController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float climbSpeed = 6f;
    [SerializeField] private float horizontalSpeed = 5f;
    [SerializeField] private float gravityScale = 1.5f;

    [Header("Slip Settings")]
    [SerializeField] private float slipForce = 10f;
    [SerializeField] private float slipDuration = 0.7f;

    [Header("Tongue")]
    [SerializeField] private GameObject tonguePrefab;
    [SerializeField] private Transform tongueSpawnPoint;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isSlipping = false;
    private float slipTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleSlipTimer();
        HandleTongue();
        CheckFallDeath();
        UpdateAnimations();
        WrapHorizontal();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        bool climbing = Input.GetKey(KeyCode.UpArrow);
        animator.SetBool("isClimbing", climbing);
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool climbing = Input.GetKey(KeyCode.UpArrow);

        float verticalVelocity = rb.linearVelocity.y;

        if (isSliding || isSlipping)
        {
            verticalVelocity = -slipForce;
        }
        else if (climbing)
        {
            verticalVelocity = climbSpeed;
        }

        rb.linearVelocity = new Vector2(horizontalInput * horizontalSpeed, verticalVelocity);
    }

    private void HandleSlipTimer()
    {
        if (!isSlipping) return;

        slipTimer -= Time.deltaTime;

        if (slipTimer <= 0f)
        {
            isSlipping = false;
        }
    }

    public void TriggerSlip()
    {
        isSlipping = true;
        slipTimer = slipDuration;
    }

    private void HandleTongue()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootTongue();
        }
    }

    private void ShootTongue()
    {
        if (tonguePrefab == null || tongueSpawnPoint == null) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector2 direction = mouseWorld - tongueSpawnPoint.position;

        // Only allow upward shots
        if (direction.y <= 0f)
            return;

        direction.Normalize();

        GameObject tongue = Instantiate(
            tonguePrefab,
            tongueSpawnPoint.position,
            Quaternion.identity
        );

        tongue.GetComponent<Tongue>().Initialize(direction);
    }

    private void WrapHorizontal()
    {
        if (Camera.main == null) return;

        Vector3 leftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 rightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        float margin = 0.5f;
        if (transform.position.x < leftEdge.x - margin)
        {
            Vector3 pos = transform.position;
            pos.x = rightEdge.x + margin;
            transform.position = pos;
        }
        else if (transform.position.x > rightEdge.x + margin)
        {
            Vector3 pos = transform.position;
            pos.x = leftEdge.x - margin;
            transform.position = pos;
        }
    }

    private void CheckFallDeath()
    {
        if (transform.position.y < -6f)
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null)
            {
                gm.GameOver();
            }
        }
    }

    private bool isSliding = false;

    public void StartSliding()
    {
        isSliding = true;
    }

    public void StopSliding()
    {
        isSliding = false;
    }
}