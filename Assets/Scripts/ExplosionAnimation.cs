using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimation : MonoBehaviour
{
    private float timer = 0f;
    private float maxTime = 1f;
    
    // Start is called before the first frame update
    void Start(){
        updateTimer();
    }

    // Update is called once per frame
    void Update(){
        updateTimer();
    }

    private void updateTimer(){
        timer += Time.deltaTime;
        if (timer >= maxTime)
            Destroy(gameObject);
    }
}
