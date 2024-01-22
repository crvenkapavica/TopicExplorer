using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : AudioPlayerController
{
    public GameObject playerPrefab;
    public GameObject obstaclePrefab;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;

    private Button startButton;
    private GameObject player;
    private Coroutine nextLevelCoroutine;

    private float spawnRate = 1.5f; 
    private float nextSpawnTime;
    private int level = 1;
    private int score = 0;
    private bool bIsGamePlaying = false;


    private void Start()
    {
        gameOverText.gameObject.SetActive(false);
        startButton = GetComponentInChildren<Button>();
        startButton.onClick.AddListener(() => OnStartGame());
        bIsAudioPaused = true;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime && bIsGamePlaying)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + spawnRate;
        }

        UpdateProgressBar();
    }

    private void OnStartGame()
    {
        bIsGamePlaying = true;
        gameOverText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        levelText.text = "LEVEL: 1";
        scoreText.text = "SCORE: 0";

        Camera mainCamera = Camera.main;
        Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, mainCamera.nearClipPlane));
        spawnPosition.z = 0;
        player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        nextLevelCoroutine = StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);

            if (spawnRate >= 1f)
            {
                spawnRate -= 0.2f;
            }
            else if (spawnRate >= 0.2f)
            {
                spawnRate -= 0.1f;
            }
            else
            {
                spawnRate -= 0.025f;
            }

            IncrementLevel();

            if (spawnRate <= 0.0755f)
            {
                StopCoroutine(nextLevelCoroutine);
            }
        }
    }

    private void SpawnObstacle()
    {
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float randomX = Random.Range(-screenWidth, screenWidth);
        Vector3 spawnPosition = new Vector3(randomX, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y + 1, 0);
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }

    private void IncrementLevel()
    {
        ++level;
        levelText.text = "LEVEL: " + level.ToString();
    }

    public void IncrementScore()
    {
        ++score;
        scoreText.text = "SCORE: " + score.ToString();
    }

    public void EndGame()
    {
        bIsGamePlaying = false;

        Obstacle[] obstacles = GameObject.FindObjectsOfType<Obstacle>();
        foreach (Obstacle obstacle in obstacles)
        {
            GameObject.Destroy(obstacle.gameObject);
        }

        GameObject.Destroy(player.gameObject);

        StopCoroutine(nextLevelCoroutine);

        gameOverText.text = "GAME OVER\nSCORE: " + score.ToString();
        level = 1;
        score = 0;
        spawnRate = 1.5f;
        gameOverText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
    }
}
