using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mauzilla : MonoBehaviour {
    public bool colliding = false; // Is Mauzilla currently near a Building?
    public Building collidingBuilding; // The Building Mauzilla is near

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        // Mauzilla is near a normal/repaired Building and pressing Action Key
        if (colliding && Input.GetKeyDown("space") && collidingBuilding.health > 0) {
            // Reduce Building Health by 1 and adjust Healthbar
            collidingBuilding.health -= 1;
            Vector3 newHealthbar = collidingBuilding.healthbar.transform.localScale;
            newHealthbar.x = collidingBuilding.healthbar.transform.localScale.x - 1;
            collidingBuilding.healthbar.transform.localScale = newHealthbar;
            Debug.Log("Building lost 1 HP. Remaining: " + collidingBuilding.health);
            // If Building Health is zero, Building state changes to destroyed
            if (collidingBuilding.health == 0) {
                Debug.Log("Building was destroyed by Mauzilla!");
                collidingBuilding.state = 1;
            }
        }
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
