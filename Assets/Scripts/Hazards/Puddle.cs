using UnityEngine;

public class Puddle : MonoBehaviour
{
    [Header("Sprite Variations")]
    [SerializeField] private Sprite[] puddleSprites;

    [Header("Despawn")]
    [SerializeField] private float despawnDistance = 12f;

    private Transform player;
    private SpriteRenderer sr;

    private void Start()
    {
        GeckoController gecko = FindFirstObjectByType<GeckoController>();
        if (gecko != null)
        {
            player = gecko.transform;
        }

        sr = GetComponent<SpriteRenderer>();

        if (puddleSprites != null && puddleSprites.Length > 0 && sr != null)
        {
            int randomIndex = Random.Range(0, puddleSprites.Length);
            sr.sprite = puddleSprites[randomIndex];
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (transform.position.y < player.position.y - despawnDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GeckoController gecko = collision.collider.GetComponentInParent<GeckoController>();
        if (gecko != null)
        {
            gecko.StartSliding();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GeckoController gecko = collision.collider.GetComponentInParent<GeckoController>();
        if (gecko != null)
        {
            gecko.StopSliding();
            gecko.TriggerSlip(); // Keep falling for a bit after leaving puddle, can't climb
        }
    }
}