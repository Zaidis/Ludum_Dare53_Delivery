using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndMenu : MonoBehaviour
{

    public Button play, quit;
    public void PlayGame() {
        play.enabled = false;
        GameManager.instance.levelIndex = 1;
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
