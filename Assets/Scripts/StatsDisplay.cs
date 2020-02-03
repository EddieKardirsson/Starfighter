using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] GameObject health;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject power;

    TextMeshProUGUI healthText;
    TextMeshProUGUI shieldText;
    TextMeshProUGUI powerText;
    Player player;

    // Start is called before the first frame update
    void Start(){
        healthText = health.GetComponent<TextMeshProUGUI>();
        shieldText = shield.GetComponent<TextMeshProUGUI>();
        powerText = power.GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update(){
        healthText.text = "HP: " + player.GetHealth().ToString();
        shieldText.text = "SH: " + player.GetShield().ToString();
        powerText.text = "PW " + player.GetPower().ToString();
    }
}
