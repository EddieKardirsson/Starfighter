using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    private float minTimer = 12f;
    private float maxTimer = 25f;
    private float currentMaxTimer = 0f;
    private float timer = 0f;
    
    private float positionXmin;
    private float positionXmax;
    
    [SerializeField] GameObject[] powerups;
    // Start is called before the first frame update
    void Start(){
        SetBounds();
    }

    // Update is called once per frame
    void Update(){
        timer += Time.deltaTime;
        //Debug.Log(timer);
        if (currentMaxTimer < minTimer)
            currentMaxTimer = Random.Range(minTimer, maxTimer);
        if (timer >= currentMaxTimer)
            SpawnPowerup();        
    }

    private void SetBounds(){
        positionXmin = gameObject.transform.position.x - 5;
        positionXmax = gameObject.transform.position.x + 5;
    }

    private void SpawnPowerup()
    {
        int i = Random.Range(0, powerups.Length);
        Vector2 position = new Vector2(Random.Range(positionXmin, positionXmax), gameObject.transform.position.y);
        Instantiate(powerups[i], position, Quaternion.identity);
        timer = 0f;
        currentMaxTimer = 0f;
    }
}
