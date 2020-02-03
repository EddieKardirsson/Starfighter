using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundDynamic : MonoBehaviour
{
    [SerializeField] float backgroundSpeed = 0.01f;
    Material material;
    Vector2 minOffset;
    Vector2 maxOffset;
    Vector2 offset;
    float timer;
    [SerializeField] float maxTime = 26f;
    bool direction;


    void Start(){
        InitComponent();
        UpdateTimer();
        direction = true;
    }

    void Update(){
        UpdateTimer();
        if (direction)
            OffsetPositive();
        else if (!direction)
            OffsetNegative();
        //Debug.Log(timer);
    }

    private void UpdateTimer(){ timer += Time.deltaTime; }

    private void InitComponent(){
        material = GetComponent<Renderer>().material;
        offset = new Vector2(0f, backgroundSpeed);
        minOffset = new Vector2(0f, -0.13f);
        maxOffset = new Vector2(0f, 0.129f);
        material.mainTextureOffset = minOffset;
    }

    private void OffsetPositive(){
        material.mainTextureOffset += offset * Time.deltaTime;
        if (timer >= maxTime){
            timer = 0f;
            direction = false;
        }
    }

    private void OffsetNegative(){
        material.mainTextureOffset -= offset * Time.deltaTime;
        if (timer >= maxTime){
            timer = 0f;
            direction = true;
        }
    }
}
