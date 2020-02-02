using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mauzilla : MonoBehaviour {

    public bool colliding = false; // Is Mauzilla currently near a Building?
    public Building collidingBuilding; // The Building Mauzilla is near



    public int maxHealth;
    public int health;
    public Image healthbar;
    public Image laserbar;

    Player player;

    GameObject damageEffect;

    // Start is called before the first frame update
    void Start() {
        maxHealth = 100;
        health = maxHealth;
        healthbar = GameObject.Find("MauzillaHealthbar").transform.GetChild(1).gameObject.GetComponent<Image>();
        healthbar.fillAmount = 1.0f;

        laserbar = GameObject.Find("Laserbar").transform.GetChild(0).gameObject.GetComponent<Image>();
        laserbar.fillAmount = 0f;


        damageEffect = transform.GetChild(1).gameObject;
        damageEffect.SetActive(false);

        player = GetComponent<InputControl>().player;
    }

    // Update is called once per frame
    void Update() {
        // Mauzilla is near a normal/repaired Building and pressing Action Key
        if (colliding && player.PressedActionKey() && collidingBuilding.state != 1 && collidingBuilding.health > 0) {
            collidingBuilding.adjustHealth(-1);
            gameObject.GetComponent<AudioSource>().Play(0);
        }

        laserbar.fillAmount = (player.cooldownTime - player.abilityCooldown) / player.cooldownTime;
    }

    public void TakeDamage(int value) {
        if (health > 10) {
            health -= value;
            Debug.Log("Mauzilla took " + value + " damage!");
            float newHealthbarPercentage = (float)health / (float)(maxHealth - 0);
            healthbar.fillAmount = newHealthbarPercentage;

            StartCoroutine(MauzillaTakesDamageEffect());

        } else if (health <= 10) {
            Debug.Log("Mauzilla retreats!");
        }
    }

    IEnumerator MauzillaTakesDamageEffect() {
        damageEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        damageEffect.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("building")) {
            Debug.Log("Mauzilla collided with " + col.gameObject.name);
            colliding = true;
            collidingBuilding = col.gameObject.GetComponent<Building>();
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.CompareTag("building")) {
            Debug.Log("Mauzilla stopped colliding with " + col.gameObject.name);
            colliding = false;
            collidingBuilding = null;
        }
    }
}
