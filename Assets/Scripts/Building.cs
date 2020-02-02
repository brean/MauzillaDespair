using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    [Range(0, 2)]
    public int state; // 0 = normal 1 = destroyed 2 = repaired
    public int maxHealth;
    public int health;
    public bool[] materials = new bool[3]; // 0 = stone 1 = cloth 2 = wood
    public int materialCount;
    public bool[] collidingArtisans = new bool[3]; // 0 = maurer 1 = schneider 2 = tischler

    //public Sprite[] sprites = new Sprite[3];
    public Sprite[] infoBubbleSprites = new Sprite[3];
    public Sprite[] materialSprites = new Sprite[3];
    public StoreyManager storeyManager;
    GameObject healthbar;
    CityHealthbar cityHealthbar;
    GameObject infoBubble;
    Mauzilla mauzilla;

    GameObject firstMaterial, secondMaterial, thirdMaterial;

    // Start is called before the first frame update
    void Start() {
        //sprites[0] = GetComponent<SpriteRenderer>().sprite; // Set normal sprite as normal
        if (GameObject.FindGameObjectsWithTag("mauzilla").Length > 0) {
            mauzilla = GameObject.FindWithTag("mauzilla").GetComponent<Mauzilla>();
        }
        storeyManager = GetComponent<StoreyManager>();

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
        healthbar = this.transform.GetChild(0).gameObject; // Get Healthbar and multiply width by maxHealth
        //reset any editor changes to healthbar, before calculating size according to maxhealth
        healthbar.transform.localScale = new Vector3(1, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
        healthbar.transform.localScale = new Vector3(healthbar.transform.localScale.x * maxHealth, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
        SetXPos(healthbar, maxHealth * -0.01f);

        cityHealthbar = GameObject.Find("CityHealthbar").GetComponent<CityHealthbar>();
   }

    // Update is called once per frame
    void Update() {
        // nothing

        // CHEATCODES
        if (Input.GetKeyDown(KeyCode.Alpha1)) { // press 1 to revert to normal
            ChangeState(0);
            Debug.Log("Changing Building state to normal.");        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { // press 2 to destroy
            ChangeState(1);
            Debug.Log("Changing Building state to destroyed.");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)) { // press 3 to repair
            ChangeState(2);
            Debug.Log("Changing Building state to repaired.");
        }
    }

    void populateInfobubble() {
        switch (materialCount) {
            case 1:
                firstMaterial.name = "Material 1/1";

                firstMaterial.SetActive(true);
                secondMaterial.SetActive(false);
                thirdMaterial.SetActive(false);

                firstMaterial.GetComponent<SpriteRenderer>().sprite =
                    materials[0] ? materialSprites[0] : (materials[1] ? materialSprites[1] : materialSprites[2]);
                Debug.Log("Selected Material 1/1: " + firstMaterial.GetComponent<SpriteRenderer>().sprite);
                break;
            case 2:
                firstMaterial.name = "Material 1/2";
                secondMaterial.name = "Material 2/2";

                firstMaterial.SetActive(true);
                secondMaterial.SetActive(true);
                thirdMaterial.SetActive(false);

                SetXPos(firstMaterial, -0.15f);
                SetXPos(secondMaterial, 0.15f);

                int taken = 9; //random
                for (int i = 0; i < materials.Length; i++) {
                    Debug.Log("MC = " + materialCount + ". Selecting Material 1/2");
                    if (materials[i]) {
                        firstMaterial.GetComponent<SpriteRenderer>().sprite = materialSprites[i];
                        taken = i;
                        Debug.Log("Selected Material 1/2: " + i);
                        break;
                    }
                }
                for (int i = 0; i < materials.Length; i++) {
                    Debug.Log("MC = " + materialCount + ". Selecting Material 2/2");
                    if (materials[i] && taken != i) {
                        secondMaterial.GetComponent<SpriteRenderer>().sprite = materialSprites[i];
                        Debug.Log("Selected Material 2/2: " + i);
                    }
                }
                break;
            case 3:
                firstMaterial.name = "Material 1/3";
                secondMaterial.name = "Material 2/3";
                thirdMaterial.name = "Material 3/3";

                firstMaterial.SetActive(true);
                secondMaterial.SetActive(true);
                thirdMaterial.SetActive(true);

                firstMaterial.GetComponent<SpriteRenderer>().sprite = materialSprites[0];
                secondMaterial.GetComponent<SpriteRenderer>().sprite = materialSprites[1];
                thirdMaterial.GetComponent<SpriteRenderer>().sprite = materialSprites[2];
                Debug.Log("Selected all Materias 1+2+3");

                break;
            default:
                print("ERROR: Incorrect number of materialCount!");
                break;

        }
    }

    void SetXPos (GameObject go, float newX) {
        Vector3 tempPos = go.transform.localPosition;
        tempPos.x = newX;
        go.transform.localPosition = tempPos;
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
                GetComponent<BoxCollider2D>().isTrigger = true;
                infoBubble.GetComponent<SpriteRenderer>().sprite = infoBubbleSprites[materialCount-1];
                infoBubble.SetActive(true);
                populateInfobubble();
                cityHealthbar.LoseBuilding();
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
        for (int i = 0; i < materials.Length; i++) {
            if (materials[i]) materialCount++;
            infoBubble.transform.GetChild(i).gameObject.SetActive(materials[i]);
        }
        Debug.Log(materialCount + " Materials are required. " + 
                  "Stone: " + materials[0] + ", Cloth: " + materials[1] + 
                  ", Wood: " + materials[2]);
    }

    // Set max health according to how many materials are required
    void SetMaxHealth() {
        for (int i = 0; i < materials.Length; i++) {
            maxHealth = materials[i] ? maxHealth + 10 : maxHealth;
        }
        Debug.Log("Setting building's maxHealth as " + maxHealth);
    }

    public void adjustHealth(int value) {
        health += value;
        Debug.Log("Building " + (value > 0 ? "gained" : "lost") + Mathf.Abs(value) + 
                  "HP. Current HP: " + health + " HP. max: " + maxHealth);
        
        // adjust healthbar
        Vector3 newHealthbar = healthbar.transform.localScale;
        newHealthbar.x = healthbar.transform.localScale.x + value;
        healthbar.transform.localScale = newHealthbar;

        // check if health reaches zero or max
        if(health >= maxHealth) {
            health = maxHealth;
            ChangeState(2);
            Debug.Log("Building was repaired by Artisan!");
        } else if (health <= 0) {
            health = 0;
            ChangeState(1);
            Debug.Log("Building was destroyed by Mauzilla!");
        }
    }

    // Building sprite is updated according to state
    public void UpdateSprite() {
        // this.GetComponent<SpriteRenderer>().sprite = sprites[state];
        switch (state) {
            case 1:
                storeyManager.DestroyBuilding();
                break;
            case 2:
                storeyManager.RepairBuilding();
                break;
        }
    }

    // Store all Artisans near the Building in Array
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name == "Maurer") collidingArtisans[0] = true;
        if (col.gameObject.name == "Schneider") collidingArtisans[1] = true;
        if (col.gameObject.name == "Tischler") collidingArtisans[2] = true;
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name == "Maurer") collidingArtisans[0] = false;
        if (col.gameObject.name == "Schneider") collidingArtisans[1] = false;
        if (col.gameObject.name == "Tischler") collidingArtisans[2] = false;
    }

    // Check if all required Artisans are near the Building
    public bool RepairConditionsMet() {
        bool repairable = false;
        for (int i = 0; i < materials.Length; i++) {
            if (materials[i]) repairable = collidingArtisans[i];
            Debug.Log("Artisan " + i + " is required and present: " + repairable);
        }
        return repairable;
    }
}
