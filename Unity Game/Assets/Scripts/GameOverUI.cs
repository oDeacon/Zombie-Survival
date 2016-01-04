using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverUI : MonoBehaviour
{

    SpawnManager spawner;

    public Image fadePlane;
    public GameObject gameOverUI;
    public Text roundsSurvived;

    void Start() {
        spawner = FindObjectOfType<SpawnManager>();
        FindObjectOfType<Player>().OnDeath += OnGameOver;
    }

    void OnGameOver() {
        StartCoroutine(Fade(Color.clear, Color.black, 1));
        roundsSurvived.text = SetRoundsSurvived();
        gameOverUI.SetActive(true);
    }

    public void RestartGame() {
        Application.LoadLevel("game");
    }

    string SetRoundsSurvived() {
        if (spawner.GetRoundNum() == 1) {
            return ("You survived " + spawner.GetRoundNum() + " round");
        }
        else {
            return ("You survived " + spawner.GetRoundNum() + " rounds");
        }
    }

    IEnumerator Fade(Color from, Color to, float time) {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1) {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }
}
