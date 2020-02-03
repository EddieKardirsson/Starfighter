using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config
    [Header("Player")]
    [Tooltip("Movement speed times deltatime")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.5f;
    [Tooltip("Set player max health")]
    [SerializeField] float health = 100f;
    private float maxHealth = 0f;
    [SerializeField] float power = 100f;
    private float maxPower = 0f;
    private float shield = 0f;
    [SerializeField] float maxShield = 100f;
    [Tooltip("Shield rating in percent")] 
    [SerializeField] [Range(0,100)] float shieldRating = 25;
    [SerializeField] int playerLives = 3;

    [Header("FX")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] [Range(0f, 1f)] float hitVolume = 0.75f;
    [SerializeField] AudioClip destructionSFX;    
    [SerializeField] [Range(0f, 0.5f)] float destructVolume = 0.3f;
    [SerializeField] AudioClip fireLaserSFX;
    [SerializeField] [Range(0f, 1f)] float fireVolume = 0.25f;
    [SerializeField] AudioClip loseSFX;
    [SerializeField] [Range(0f, 1f)] float loseVolume = 1f;
    

    [Header("Laser Projectile")]
    [Tooltip("Laser projectile prefab selection")]
    [SerializeField] GameObject laserPrefab;
    [Tooltip("Projectile speed times deltatime")]
    [SerializeField] float projectileSpeed = 10f;
    [Tooltip("Rounds per minutes, affects weapon cooldown. The higher the value, the more rapid shots")]
    [SerializeField] float RPM = 420f;

    private Coroutine fireCoroutine;
    private const float MINUTE_IN_SECONDS = 60f;

    private float laserTimer;
    private float laserCooldown;
    private float shieldTimer;
    private float shieldCooldown = 2f;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    

    // Start is called before the first frame update
    void Start(){
        SetUpBoundaries();
        InitCooldown();
        InitShipStats();
    }

    // Update is called once per frame
    void Update(){
        Move();
        FireLaser();
        RaiseShield();
        updateCooldown();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        HandleCollision(other);
    }

    private void OnTriggerEnter2D(Collider2D other){
        HandleCollision(other);
    }

    private void SetUpBoundaries(){
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void InitCooldown(){
        laserCooldown = MINUTE_IN_SECONDS / RPM;
        laserTimer = laserCooldown;
        shieldTimer = shieldCooldown;
    }

    private void InitShipStats(){
        maxPower = power;
        maxHealth = health;
        shieldRating /= 100;
    }

    private void Move(){
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, /*transform.position.y*/ newYPos);
    }

    private void FireLaser() {        
        if (Input.GetButtonDown("Fire1")){
            if (laserTimer >= laserCooldown){
                fireCoroutine = StartCoroutine(ContinuousFire());
                laserTimer = 0f;
            }
        }
        if (Input.GetButtonUp("Fire1"))
            StopCoroutine(fireCoroutine);
        if(Input.GetAxis("RT") > 0 && laserTimer >= laserCooldown){
            CreateProjectile();
            laserTimer = 0f;
        }
    }

    private IEnumerator ContinuousFire() {
        while (true){
            CreateProjectile();
            yield return new WaitForSeconds(laserCooldown);
        }
    }

    private void CreateProjectile(){
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        AudioSource.PlayClipAtPoint(fireLaserSFX, Camera.main.transform.position, fireVolume);
    }

    private void RaiseShield(){
        shieldTimer += Time.deltaTime;
        if((shieldTimer >= shieldCooldown) && (Input.GetKey(KeyCode.C) || Input.GetAxis("Button X") > 0) && (shield < 80 || power >= 20))
            ShieldUp();
    }

    private void ShieldUp(){
        power -= 20;
        shield = maxShield;
        Debug.Log("HP: " + health + "\n" + "PW: " + power + " SH: " + shield);
        shieldTimer = 0f;
    }

    private void updateCooldown(){
        laserTimer += Time.deltaTime;
        //Debug.Log(cooldownTimer);
    }

    private void HandleCollision(Collider2D other){
        DamageHandler damageHandler = other.gameObject.GetComponent<DamageHandler>();
        GameObject gameObject = other.gameObject;
        if (gameObject.tag != "Player"){
            HitToDamage(damageHandler);
            Destroy(other.gameObject);
        }
    }
    private void HandleCollision(Collision2D other){
        GameObject powerUp = other.gameObject;
        if (powerUp.tag == "Powerup"){
            if (powerUp.name == "MaxHealth(Clone)")
                MaxHealthPowerUp();
            if (powerUp.name == "MaxShield(Clone)")
                MaxShieldPowerUp();
            if (powerUp.name == "MaxPower(Clone)")
                MaxPowerPowerUp();
            Debug.Log("HP: " + health + "\n" + "PW: " + power + " SH: " + shield);
        }
    }

    private void MaxHealthPowerUp() { health = maxHealth; }
    private void MaxShieldPowerUp() { shield = maxShield; }
    private void MaxPowerPowerUp() { power = maxPower; }

    private void HitToDamage(DamageHandler damageHandler){
        if(shield > 0){
            shield -= damageHandler.GetEnemyDamage();
            health -= (1 - shieldRating) * damageHandler.GetEnemyDamage();
        }
        else
            health -= damageHandler.GetEnemyDamage();
        if (health < 1)
            Destruction();
        else
            AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position, hitVolume);
        Debug.Log("HP: " + health + "\n" + "PW: " + power + " SH: " + shield);
    }

    private void Destruction(){
        if (playerLives < 1)
            GameOver();
        else
            NewMethod();
    }
       
    private void GameOver(){
        Destroy(gameObject);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(loseSFX, Camera.main.transform.position, loseVolume);
        FindObjectOfType<LevelLoader>().SetGameOver();
    }

    private void NewMethod(){
        playerLives--;
        health = 100;
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(destructionSFX, Camera.main.transform.position, destructVolume);
    }

    public float GetHealth() { return health; }
    public float GetShield() { return shield; }
    public float GetPower() { return power; }
}
