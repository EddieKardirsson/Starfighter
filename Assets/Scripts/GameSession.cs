using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score = 0;

    void Awake(){
        SetUpSingleton();
    }
    private void SetUpSingleton(){
        int numberOfSessions = FindObjectsOfType<GameSession>().Length;
        if (numberOfSessions > 1){
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
            DontDestroyOnLoad(gameObject);
    }    
    public int GetScore(){ return score; }
    public void SetScore(int score) { this.score += score; 
    }    
    public void ResetGame() { Destroy(gameObject); }
}
