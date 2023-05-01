using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public Button play, quit;
    public void PlayGame() {
        play.enabled = false;
        StartCoroutine(GameManager.instance.TriggerAnimationAndWait("Close", StartGame));
    }

    public void QuitGame() {
        quit.enabled = false;
        Application.Quit();
    }

    public void StartGame() {
        SceneManager.LoadScene("GameScene");
    }
}
