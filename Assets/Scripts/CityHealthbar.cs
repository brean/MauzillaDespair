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
        maxHealth = buildings.Length;
        healthbar = transform.GetChild(1).gameObject.GetComponent<Image>();
        // Start is called before the first frame update
    }

}
