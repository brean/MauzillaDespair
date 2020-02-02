using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityHealthbar : MonoBehaviour
{
    public GameObject[] buildings;
    public int maxHealth;
    public int health;
    public Image healthbar;

    // Start is called before the first frame update
    void Start() {
        buildings = GameObject.FindGameObjectsWithTag("building");
        maxHealth = buildings.Length * 2;
        healthbar = transform.GetChild(1).gameObject.GetComponent<Image>();
        healthbar.fillAmount = 1.0f;
        health = maxHealth;
    }

   public void LoseBuilding() {
        health = health - 1;
        Debug.Log("City lost a building to Mauzilla's Wrath!");
        float newHealthbarPercentage = (float)health / (float)(maxHealth);
        healthbar.fillAmount = newHealthbarPercentage;
    }

}
