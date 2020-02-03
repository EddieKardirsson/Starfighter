using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private readonly float delayTimer = 2f;

    void Awake(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadStartMenu() { SceneManager.LoadScene(0); }

    public void StartNewGame() { 
        SceneManager.LoadScene(1);
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void SetGameOver() { StartCoroutine(DelayLoad()); }

    public void QuitGame() { Application.Quit(); }

    private IEnumerator DelayLoad(){        
        yield return new WaitForSeconds(delayTimer);
        SceneManager.LoadScene("Game Over");
    }
}
