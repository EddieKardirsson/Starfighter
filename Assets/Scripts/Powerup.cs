using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] AudioClip powerupSFX;
    [SerializeField] [Range(0f, 1f)] float sFXVolume = 0.6f;
    private SpriteRenderer powerUpSprite;    

    private float spinTimer = 0f;
    private float maxSpinTimer = 1f / 6f;
    int spriteIndex = 0;

    // Start is called before the first frame update
    void Start(){
        powerUpSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update(){
        UpdateAnimation();
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Player"){
            AudioSource.PlayClipAtPoint(powerupSFX, Camera.main.transform.position, sFXVolume);
        }
        Destroy(gameObject);
    }

    private void UpdateAnimation(){
        spinTimer += Time.deltaTime;
        if(spinTimer > maxSpinTimer){
            spriteIndex = SelectNextSprite();
            powerUpSprite.sprite = sprites[spriteIndex];
            spinTimer = 0;
        }
    }

    private int SelectNextSprite(){
        if (spriteIndex < sprites.Length - 1)
            return spriteIndex + 1;
        else
            return 0;
    }
}
