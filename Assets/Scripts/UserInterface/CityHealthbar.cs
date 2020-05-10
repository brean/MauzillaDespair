using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityHealthbar : MonoBehaviour
{
    GameObject[] buildings;
    int maxHealth;
    [HideInInspector]
    public int health;
    Image healthbar;

    void Start() {
        buildings = GameObject.FindGameObjectsWithTag("building");
        maxHealth = buildings.Length * 2;
        healthbar = transform.GetChild(1).gameObject.GetComponent<Image>();
        healthbar.fillAmount = 1.0f;
        health = maxHealth;
    }

   public void LoseBuilding() {
        health = health - 1;
        float newHealthbarPercentage = (float)health / (float)(maxHealth);
        healthbar.fillAmount = newHealthbarPercentage;
    }

}
