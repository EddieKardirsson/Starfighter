using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    [SerializeField] int playerDamage = 100;
    [SerializeField] int enemyDamage = 20;

    public int GetPlayerDamage(){ return playerDamage; }

    public int GetEnemyDamage(){ return enemyDamage; }

    public void Hit(){ Destroy(gameObject); }
}
