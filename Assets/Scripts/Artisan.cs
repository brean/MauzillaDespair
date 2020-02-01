using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artisan : MonoBehaviour
{
    [Range(0, 2)]
    public int type; // 0 = maurer 1 = schneider 2 = tischler
    public bool colliding = false; // Is Artisan currently near a Building?
    public Building collidingBuilding; // The Artisan Mauzilla is near

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        // Artisan is near a destroyed Building and pressing Action Key
        if (colliding && Input.GetKeyDown("space") && collidingBuilding.state == 1) {

            // Check if all required Artisans are near the Building
            if (collidingBuilding.RepairConditionsMet()) {
                collidingBuilding.adjustHealth(1);

                // If Building Health is zero, Building state changes to destroyed
                if (collidingBuilding.health == collidingBuilding.maxHealth) {
                    Debug.Log("Building was repaired by Artisan!");
                    collidingBuilding.ChangeState(2);
                }
            } else {
                Debug.Log("You're missing the right skills to repair this building!");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("building")) {
            Debug.Log("Artisan collided with " + col.gameObject.name);
            colliding = true;
            collidingBuilding = col.gameObject.GetComponent<Building>();
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.CompareTag("building")) {
            Debug.Log("Artisan stopped colliding with " + col.gameObject.name);
            colliding = false;
            collidingBuilding = null;
        }
    }
}
