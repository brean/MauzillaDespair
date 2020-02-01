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
        if (colliding && Input.GetKeyDown("space") && collidingBuilding.state != 1 && collidingBuilding.health > 0) {
            collidingBuilding.adjustHealth(-1);
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
