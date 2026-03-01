using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Despawn")]
    [SerializeField] private float despawnDistance = 10f;

    [Header("Animation")]
    [SerializeField] private float pulseSpeed = 4f;
    [SerializeField] private float pulseAmount = 0.15f;

    private Transform player;
    private Vector3 originalScale;

    private void Start()
    {
        GeckoController gecko = FindFirstObjectByType<GeckoController>();
        if (gecko != null)
        {
            player = gecko.transform;
        }

        originalScale = transform.localScale;
    }

    private void Update()
    {
        AnimatePulse();
        CheckDespawn();
    }

    private void AnimatePulse()
    {
        float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale * scale;
    }

    private void CheckDespawn()
    {
        if (player == null) return;

        if (transform.position.y < player.position.y - despawnDistance)
        {
            Destroy(gameObject);
        }
    }

    public void Collect()
    {
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm.AddScore(1);
        }

        Destroy(gameObject);
    }
}