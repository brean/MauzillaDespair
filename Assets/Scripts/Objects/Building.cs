﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    [HideInInspector]
    [Range(0, 2)]
    public int state; // 0 = normal 1 = destroyed 2 = repaired
    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public int health;

    // 0 = stone (Maurer) 1 = cloth (Schneider) 2 = wood (Tischeler)
    [HideInInspector]
    public bool[] materials = new bool[3];
    [HideInInspector]
    public int materialCount;

    //public Sprite[] sprites = new Sprite[3];
    [HideInInspector]
    public Sprite[] infoBubbleSprites = new Sprite[3];
    [HideInInspector]
    public Sprite[] materialSprites = new Sprite[3];
    [HideInInspector]
    public Vector3Int positionInTileMap;
    BuildingManager buildingManager;
    GameObject healthbar;
    CityHealthbar cityHealthbar;
    GameObject infoBubble;
    MauzillaPlayer mauzilla;

    Animator repairSmokeAnimator;
    public GameObject explosion;
    [HideInInspector]
    public int explosionTimer = 0;
    GameObject firstMaterial, secondMaterial, thirdMaterial;

    // Start is called before the first frame update
    void Start() {
        //sprites[0] = GetComponent<SpriteRenderer>().sprite; // Set normal sprite as normal
        if (GameObject.FindGameObjectsWithTag("mauzilla").Length > 0) {
            mauzilla = GameObject.FindWithTag("mauzilla").GetComponent<MauzillaPlayer>();
        }

        buildingManager = GameObject.Find("Managers").GetComponent<BuildingManager>();

        // Get Infobubble and Materials and hide them
        infoBubble = this.transform.GetChild(1).gameObject; 
        firstMaterial = infoBubble.transform.GetChild(0).gameObject;
        secondMaterial = infoBubble.transform.GetChild(1).gameObject;
        thirdMaterial = infoBubble.transform.GetChild(2).gameObject;
        infoBubble.SetActive(false);

        state = 0; // Building starts in state normal

        RandomizeMaterials(); // Randomly decide how many materials are required

        SetMaxHealth(); // set maxHealth to 10, 20 or 30 depending on how many materials are required
        health = maxHealth;
        healthbar = this.transform.GetChild(0).gameObject; // Get Healthbar
        SetXScale(healthbar, 1); //reset any editor changes to healthbar
        SetXScale(healthbar, healthbar.transform.localScale.x * maxHealth); //calculate size according to maxhealth
        SetXPos(healthbar, maxHealth * -0.01f);

        cityHealthbar = GameObject.Find("CityHealthbar").GetComponent<CityHealthbar>();

        // Find has advantages. A: you can search in code for the name of the GameObject
        // B: The order of the children in the Prefab is irrelevant.
        GameObject repairSmoke = transform.Find("RepairSmoke").gameObject;
        repairSmokeAnimator = repairSmoke.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        // nothing

        // CHEATCODES
        if (Input.GetKeyDown(KeyCode.Alpha1)) { // press 1 to revert to normal
            ChangeState(0);
            Debug.Log("CHEAT: Changing all Buildings state to normal.");        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { // press 2 to destroy
            ChangeState(1);
            health = 0;
            SetXScale(healthbar, 0); //reduce healthbar to zero
            Debug.Log("CHEAT: Changing all Buildings state to destroyed and health to zero.");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)) { // press 3 to repair
            ChangeState(2);
            Debug.Log("CHEAT: Changing all Buildings state to repaired.");
        }

        if (explosionTimer >= 0)
        {
            explosionTimer -= 1;
        }
        else
        {
            explosion.SetActive(false);
        }

    }

    void populateInfobubble() {
        switch (materialCount) {
            case 1:
                firstMaterial.name = "Material 1/1";
                
                firstMaterial.SetActive(true);
                secondMaterial.SetActive(false);
                thirdMaterial.SetActive(false);
                break;
            case 2:
                firstMaterial.name = "Material 1/2";
                secondMaterial.name = "Material 2/2";

                firstMaterial.SetActive(true);
                secondMaterial.SetActive(true);
                thirdMaterial.SetActive(false);

                SetXPos(firstMaterial, -0.15f);
                SetXPos(secondMaterial, 0.15f);
                break;
            case 3:
                firstMaterial.name = "Material 1/3";
                secondMaterial.name = "Material 2/3";
                thirdMaterial.name = "Material 3/3";

                firstMaterial.SetActive(true);
                secondMaterial.SetActive(true);
                thirdMaterial.SetActive(true);
                break;
            default:
                print("ERROR: Incorrect number of materialCount!");
                break;

        }

        List<GameObject> materialGameObjects = new List<GameObject>();
        materialGameObjects.Add(firstMaterial);
        materialGameObjects.Add(secondMaterial);
        materialGameObjects.Add(thirdMaterial);

        int spriteIndex = 0;
        for (int index = 0; index < materials.Length; index++)
        {
            if (materials[index] == true)
            {
                GameObject nextMaterialSprite = materialGameObjects[spriteIndex];

                nextMaterialSprite.GetComponent<SpriteRenderer>().sprite =
                    materialSprites[index];
                spriteIndex++;
            }
        }
    }

    void SetXPos (GameObject go, float newX) {
        Vector3 tempPos = go.transform.localPosition;
        tempPos.x = newX;
        go.transform.localPosition = tempPos;
    }
    void SetXScale(GameObject go, float newX) {
        Vector3 tempScale = go.transform.localScale;
        tempScale.x = newX;
        go.transform.localScale = tempScale;
    }

    public void ChangeState(int newState) {
        state = newState;
        UpdateSprite();

        switch (state) {
            case 0: // normal 
                infoBubble.SetActive(false);
                GetComponent<BoxCollider2D>().isTrigger = false;
                break;
            case 1: // destroyed
                gameObject.GetComponent<AudioSource>().Play(0);
                GetComponent<BoxCollider2D>().isTrigger = true;
                infoBubble.GetComponent<SpriteRenderer>().sprite = infoBubbleSprites[materialCount-1];
                infoBubble.SetActive(true);
                populateInfobubble();
                cityHealthbar.LoseBuilding();
                explosion.SetActive(true);
                explosionTimer = 30;
                break;
            case 2: // repaired
                GetComponent<BoxCollider2D>().isTrigger = false;
                infoBubble.SetActive(false);
                health = maxHealth;
                mauzilla.TakeDamage(maxHealth);
                break;
            default:
                print("ERROR: Incorrect state given to ChangeState()!");
                break;
        }
    }

    void RandomizeMaterials() {
        // randomly decide if material is required
        for (int i = 0; i < materials.Length; i++) {
            materials[i] = (Random.value > 0.5f);
        }

        // if no material was randomly selected, select at least one
        if (!materials[0] && !materials[1] && !materials[2]) {
            materials[Random.Range(0, 3)] = true;
        }

        // only show materials which are required in the infobubble
        // and increase the counter for every positive m´needed material
        for (int i = 0; i < materials.Length; i++) {
            if (materials[i]) {
                materialCount++;
            }
            infoBubble.transform.GetChild(i).gameObject.SetActive(materials[i]);
        }
    }

    // Set max health according to how many materials are required
    void SetMaxHealth() {
        for (int i = 0; i < materials.Length; i++) {
            maxHealth = materials[i] ? maxHealth + 10 : maxHealth;
        }
    }

    public void adjustHealth(int value) {
        health += value;

        if(value > 0) {
            repairSmokeAnimator.Play("RepairSmokeAnim");
        }

        // adjust healthbar
        Vector3 newHealthbar = healthbar.transform.localScale;
        newHealthbar.x = healthbar.transform.localScale.x + value;
        healthbar.transform.localScale = newHealthbar;

        // check if health reaches zero or max
        if(health >= maxHealth) {
            health = maxHealth;
            ChangeState(2);
        } else if (health <= 0) {
            health = 0;
            ChangeState(1);
        }
    }

    // Building sprite is updated according to state
    public void UpdateSprite() {
        // this.GetComponent<SpriteRenderer>().sprite = sprites[state];
        switch (state) {
            case 1:
                buildingManager.DestroyBuilding(positionInTileMap);
                break;
            case 2:
                buildingManager.RepairBuilding(positionInTileMap);
                break;
        }
    }

    // Check if all required Artisans are near the Building
    public bool RepairConditionsMet(Character character)
    {
        if (character == Character.maurer && materials[0] == true) {
            return true;
        }
        if (character == Character.schneider && materials[1] == true) {
            return true;
        }
        if (character == Character.tischler && materials[2] == true) {
            return true;
        }

        return false;
    }
}
