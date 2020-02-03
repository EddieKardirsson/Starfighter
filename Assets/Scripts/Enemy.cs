using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100f;
    private float shotCounter;
    [SerializeField] float cooldownMin = 0.2f;
    [SerializeField] float cooldownMax = 3f;
    [SerializeField] int scoreValue = 100;
    
    [Header("FX")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] AudioClip destructionSFX;
    [SerializeField] [Range(0f, 0.5f)] float destructVolume = 0.3f;
    [SerializeField] AudioClip fireLaserSFX;
    [SerializeField] [Range(0f, 1f)] float fireVolume = 0.25f;

    [Header("Laser Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    
    

    // Start is called before the first frame update
    void Start(){
        ResetShotCounter();
    }

    // Update is called once per frame
    void Update(){
        FireCountdown();
    }

    private void OnTriggerEnter2D(Collider2D other){
        HandleCollision(other);
    }

    private void ResetShotCounter() { shotCounter = Random.Range(cooldownMin, cooldownMax); }

    private void FireCountdown(){
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f){
            Fire();
            ResetShotCounter();
        }
    }    

    private void Fire(){
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(fireLaserSFX, Camera.main.transform.position, fireVolume);
    }

    private void HandleCollision(Collider2D other){
        DamageHandler damageHandler = other.gameObject.GetComponent<DamageHandler>();
        GameObject gameObject = other.gameObject;
        if (gameObject.tag != "Enemy"){
            HitToDamage(damageHandler);            
            Destroy(other.gameObject);
        }
    }

    private void HitToDamage(DamageHandler damageHandler){
        health -= damageHandler.GetPlayerDamage();
        if (health < 1)
            Destruction();
    }

    private void Destruction(){
        FindObjectOfType<GameSession>().SetScore(scoreValue);
        Destroy(gameObject);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(destructionSFX, Camera.main.transform.position, destructVolume);
    }
}
