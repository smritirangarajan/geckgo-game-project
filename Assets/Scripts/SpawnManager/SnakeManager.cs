using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] private GameObject snake;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnMargin = 2f;

    private float timer;
    private bool spawnFromLeftNext = true;
    private Vector3 screenLeft;
    private Vector3 screenRight;
    private float screenTop;
    private float screenBottom;

    private void Start()
    {
        UpdateScreenBounds();
    }

    private void UpdateScreenBounds()
    {
        if (Camera.main == null) return;

        screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
    }

    private void Update()
    {
        if (snake == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnSnake();
            timer = 0f;
        }
    }

    private void SpawnSnake()
    {
        UpdateScreenBounds();

        bool moveRight = spawnFromLeftNext;
        spawnFromLeftNext = !spawnFromLeftNext;

        float spawnX = moveRight ? screenLeft.x - spawnMargin : screenRight.x + spawnMargin;
        float spawnY = Random.Range(screenTop - 3f, screenTop + 1f);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        GameObject spawned = Instantiate(snake, spawnPosition, Quaternion.identity);

        Snake snakeScript = spawned.GetComponent<Snake>();
        if (snakeScript != null)
        {
            snakeScript.Initialize(moveRight);
        }
    }
}
