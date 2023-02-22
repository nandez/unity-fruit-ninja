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

    private int score = 0;

    private void Start()
    {
        NewGame();
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = $"SCORE: {score}";
    }

    public void NewGame()
    {
        score = 0;
        scoreText.text = $"SCORE: {score}";

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
        spawnController.enabled = false;
        swordController.enabled = false;

        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
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

        yield return new WaitForSecondsRealtime(0.5f);
        NewGame();

        // Ejecutamos la animación de FadeOut para el overlay.
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            var t = Mathf.Clamp01(elapsed / fadeDuration);
            fadeOverlay.color = Color.Lerp(fadeColor, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
    }
}
