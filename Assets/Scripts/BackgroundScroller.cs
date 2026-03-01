using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 2f;

    private float spriteHeight;
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteHeight = sr.bounds.size.y;
    }

    private void Update()
    {
        // Move downward
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // If completely below camera view, move above
        if (transform.position.y < cam.position.y - spriteHeight)
        {
            transform.position += Vector3.up * spriteHeight * 2f;
        }
    }
}