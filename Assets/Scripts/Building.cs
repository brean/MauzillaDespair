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

    public Sprite[] sprites = new Sprite[3];
    public Sprite[] infoBubbleSprites = new Sprite[3];
    GameObject healthbar;
    GameObject infoBubble;
    Mauzilla mauzilla;


    // Start is called before the first frame update
    void Start() {
        sprites[0] = this.GetComponent<SpriteRenderer>().sprite; // Set normal sprite as normal
        mauzilla = GameObject.FindWithTag("mauzilla").GetComponent<Mauzilla>();

        infoBubble = this.transform.GetChild(1).gameObject; // Get Infobubble and hide
        infoBubble.SetActive(false);

        state = 0; // Building starts in state normal

        RandomizeMaterials(); // Randomly decide how many materials are required

        SetMaxHealth(); // set maxHealth to 10, 20 or 30 depending on how many materials are required
        health = maxHealth;
        healthbar = this.transform.GetChild(0).gameObject; // Get Healthbar and multiply width by maxHealth
        healthbar.transform.localScale = new Vector3(healthbar.transform.localScale.x * maxHealth, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
    }

    // Update is called once per frame
    void Update() {
        // nothing

        // CHEATCODES
        if (Input.GetKeyDown(KeyCode.Alpha1)) { // press 1 to revert to normal
            ChangeState(0);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) { // press 2 to destroy
            ChangeState(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)) { // press 3 to repair
            ChangeState(2);
        }
    }

    public void ChangeState(int newState) {
        state = newState;
        UpdateSprite();

        switch (state) {
            case 0: // normal 
                infoBubble.SetActive(false);
                break;
            case 1: // destroyed
                infoBubble.GetComponent<SpriteRenderer>().sprite = infoBubbleSprites[materialCount-1];
                infoBubble.SetActive(true);
                break;
            case 2: // repaired
                infoBubble.SetActive(false);
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
        Debug.Log(materialCount + " Materials arequired. Stone: " + materials[0] + ", Cloth: " + materials[1] + ", Wood: " + materials[2]);
    }

    // Set max health according to how many materials are required
    void SetMaxHealth() {
        for (int i = 0; i < materials.Length; i++) {
            maxHealth = materials[i] ? maxHealth + 10 : maxHealth;
            Debug.Log("Setting building's maxHealth as " + maxHealth);
        }
    }

    public void adjustHealth(int value) {
        health += value;
        Debug.Log("Building " + (value > 0 ? "gained" : "lost") + Mathf.Abs(value) + "HP. Current HP: " + health);
        
        // adjust healthbar
        Vector3 newHealthbar = healthbar.transform.localScale;
        newHealthbar.x = healthbar.transform.localScale.x + value;
        healthbar.transform.localScale = newHealthbar;

        // check if health reaches zero or max
        if(health == maxHealth) {
            ChangeState(2);
            Debug.Log("Building was repaired by Artisan!");
        } else if (health == 0) {
            ChangeState(1);
            Debug.Log("Building was destroyed by Mauzilla!");
        }
    }

    // Building sprite is updated according to state
    public void UpdateSprite() {
        this.GetComponent<SpriteRenderer>().sprite = sprites[state];
    }

    // Store all Artisans near the Building in Array
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.name == "Maurer") collidingArtisans[0] = true;
        if (col.gameObject.name == "Schneider") collidingArtisans[1] = true;
        if (col.gameObject.name == "Tischler") collidingArtisans[2] = true;
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.name == "Maurer") collidingArtisans[0] = false;
        if (col.gameObject.name == "Schneider") collidingArtisans[1] = false;
        if (col.gameObject.name == "Tischler") collidingArtisans[2] = false;
    }

    // Check if all required Artisans are near the Building
    public bool RepairConditionsMet() {
        Debug.Log("Executing repairconditions fucnt2");
        bool repairable = false;
        for (int i = 0; i < materials.Length; i++) {
            if (materials[i]) repairable = collidingArtisans[i];
            Debug.Log("Artisan " + i + " is required and present: " + repairable);
        }
        return repairable;
    }
}
