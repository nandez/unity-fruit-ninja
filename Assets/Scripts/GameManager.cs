using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI scoreText;
    [SerializeField] protected SwordController swordController;
    [SerializeField] protected SpawnController spawnController;
    [SerializeField] protected Image fadeOverlay;
    [SerializeField] protected float fadeDuration = 0.5f;
    [SerializeField] protected Color fadeColor = Color.white;
    [SerializeField] protected CameraController cameraController;
    [SerializeField] protected GameObject menuUI;
    [SerializeField] protected TextMeshProUGUI menuTitle;

    private int score = 0;
    private bool isPaused = false;
    private bool isGameOver = false;

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            menuTitle.text = "MENU";
            isPaused = !isPaused;
            menuUI.SetActive(isPaused);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void NewGame()
    {
        score = 0;
        scoreText.text = score.ToString();

        // Habilitamos los controles y el spawner
        spawnController.enabled = true;
        swordController.enabled = true;

        // Buscamos todas las frutas y las destruimos.
        var fruits = FindObjectsOfType<FruitController>();
        if (fruits?.Length > 0)
        {
            foreach (var fruit in fruits)
                Destroy(fruit.gameObject);
        }

        // Buscamos todas las frutas y las destruimos.
        var bombs = FindObjectsOfType<BombController>();
        if (bombs?.Length > 0)
        {
            foreach (var bomb in bombs)
                Destroy(bomb.gameObject);
        }

        Time.timeScale = 1f;
    }

    public void OnBombExplosion()
    {
        isGameOver = true;

        spawnController.enabled = false;
        swordController.enabled = false;

        StartCoroutine(GameOverSequence());
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void OnPlay()
    {
        isPaused = false;
        menuUI.SetActive(isPaused);

        if (isGameOver)
        {
            isGameOver = false;
            NewGame();
        }
    }

    private IEnumerator GameOverSequence()
    {
        StartCoroutine(cameraController.Shake(.1f, .3f));

        // Obtenemos todas las bombas en escena y detenemos el sonido de la mecha.
        var bombs = FindObjectsOfType<BombController>();
        foreach (var b in bombs)
            b.StopWickSound();

        // Ejecutamos la animación de FadeIn para el overlay.
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            var t = Mathf.Clamp01(elapsed / fadeDuration);
            fadeOverlay.color = Color.Lerp(Color.clear, fadeColor, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.25f);
        menuTitle.text = "GAME OVER";
        menuUI.SetActive(true);

        // Ejecutamos la animación de FadeOut para el overlay.
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            var t = Mathf.Clamp01(elapsed / fadeDuration);
            fadeOverlay.color = Color.Lerp(fadeColor, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        Time.timeScale = 0f;
    }
}
