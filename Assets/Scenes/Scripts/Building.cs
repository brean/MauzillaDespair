using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [Range(0, 2)]
    public int state; // 0 = normal 1 = destroyed 2 = repaired
    public int health;
    public int materials;

    public Sprite[] sprites = new Sprite[3];
    public GameObject healthbar;
    
    // Start is called before the first frame update
    void Start()
    {
        sprites[0] = this.GetComponent<SpriteRenderer>().sprite;
        healthbar = this.transform.GetChild(0).gameObject;
        state = 0;
        health = 10;
        materials = 1;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<SpriteRenderer>().sprite = sprites[state];
    }
}
