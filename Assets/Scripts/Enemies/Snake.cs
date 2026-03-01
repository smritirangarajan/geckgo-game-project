using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] [Range(0.2f, 2f)] private float animationSpeed = 0.6f;
    [SerializeField] private float despawnMargin = 2f;

    private bool moveRight;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Camera cam;

    public void Initialize(bool movingRight)
    {
        moveRight = movingRight;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (animator != null)
            animator.speed = animationSpeed;
        if (spriteRenderer != null && animator == null)
        {
            spriteRenderer.flipX = !moveRight;
        }
        cam = Camera.main;
    }

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (animator != null)
            animator.speed = animationSpeed;
        if (cam == null)
            cam = Camera.main;
    }

    private void Update()
    {
        float direction = moveRight ? 1f : -1f;
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        if (animator != null)
        {
            animator.SetBool("MoveRight", moveRight);
        }

        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Vector3 screenLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 screenRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        if (moveRight && transform.position.x > screenRight.x + despawnMargin)
            Destroy(gameObject);
        else if (!moveRight && transform.position.x < screenLeft.x - despawnMargin)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GeckoController gecko = collision.GetComponentInParent<GeckoController>();

        if (gecko != null)
        {
            FindFirstObjectByType<GameManager>()?.GameOver();
        }
    }
}
