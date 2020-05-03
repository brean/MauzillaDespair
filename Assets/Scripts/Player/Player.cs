using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    
    [HideInInspector]
    public Rigidbody2D rb2d;
    [HideInInspector]
    public Animator animator;



    [SerializeField]
    public Character character; //mauzilla, schneider, maurer or tischler

    [SerializeField]
    public Color color;

    [SerializeField]
    public int team = 0;  // Team 0 or 1

    [Tooltip("speed of the player")]
    public float speed = .08f;
    [HideInInspector]
    public Vector3 initialScale;

    [HideInInspector]
    public float attackAnim = 0;

    public bool ready = false; // user pressed the input Button to start the game.
    public bool active = false; // user pressed any key to activate himself

    public float cooldownTime = 5;

    public float abilityCooldown = -1;
    public float abilityActiveDuration = -1;
    public bool usesLaser = false;

    [HideInInspector]
    public InputControl inputControl;

    // colliding / triggering
    public bool colliding = false; // Is Artisan currently near a Building?
    public Building collidingBuilding; // The Artisan Mauzilla is near

    public virtual void Start() {
        inputControl = GetComponent<InputControl>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        initialScale = transform.localScale;
    }

    public virtual void Update() {
        movePlayer();
        
        controlAbility(inputControl.isAbilityKeyPressed());

        if (character != Character.mauzilla)
        {
            // Artisan is near a destroyed Building and pressing Action Key
            if (colliding && inputControl.isActionKeyPressedInFrame() && collidingBuilding.state == 1) {

                // Check if all required Artisans are near the Building
                if (collidingBuilding.RepairConditionsMet()) {
                    collidingBuilding.adjustHealth(1);
                    gameObject.GetComponent<AudioSource>().Play(0);
                } else {
                    Debug.Log("You're missing the right skills to repair this building!");
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("building")) {
            Debug.Log("Artisan collided with " + col.gameObject.name);
            colliding = true;
            collidingBuilding = col.gameObject.GetComponent<Building>();
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("building")) {
            Debug.Log("Artisan stopped colliding with " + col.gameObject.name);
            colliding = false;
            collidingBuilding = null;
        }
    }
    public virtual void movePlayer()
    {
        Vector2 newPlayerPosition = rb2d.position + (inputControl.getCurrentMovement() * speed);
        if (animator != null && attackAnim <= 0) { 
            Flip(inputControl.getCurrentMovement().x);
            FrontBack(inputControl.getCurrentMovement().y);
        }
        rb2d.MovePosition(newPlayerPosition);
    }

    



    public static Character nextCharacter(Character lastCharacter)
    {
        return (Character)(((int)lastCharacter + 1) % 4);
    }

    public static Character prevCharacter(Character lastCharacter)
    {
        if (lastCharacter == 0)
        {
            return (Character)3;
        }
        return (Character)(((int)lastCharacter - 1) % 4);
    }

    public int characterNumber()
    {
        return (int)character;
    }


    public bool isUsingAbility()
    {
        return abilityActiveDuration > 0;
    }

    public void controlAbility(bool isKeyPressed)
    {
        if (abilityCooldown <= 0 && isKeyPressed)
        {
            abilityCooldown = cooldownTime;
            abilityActiveDuration = 3;
        }
        else
        {
            if (abilityCooldown > -1)
            {
                abilityCooldown -= Time.deltaTime;
            }
            if (abilityActiveDuration > -1)
            {
                abilityActiveDuration -= Time.deltaTime;
            }

            if (!isKeyPressed)
            {
                abilityActiveDuration = -1;
            }
        }
    }

    public void Flip(float moveHorizontal)
    {
        if (moveHorizontal > 0.1 || moveHorizontal < -0.1)
        {
            if(animator != null)
            {
                animator.SetInteger("Direction", 2);
            }
            Vector3 theScale = transform.localScale;

            if (moveHorizontal > 0.1)
            {
                theScale.x = -initialScale.x;
            }
            else
            {
                theScale.x = initialScale.x;
            }


            transform.localScale = theScale;
        }
    }

    public void FrontBack(float moveVertical)
    {
        if (moveVertical > 0.1)
        {
            //GetComponent<SpriteRenderer>().sprite = spriteSettings.back;
           //transform.localScale = initialScale;
            animator.SetInteger("Direction", 1);
        }

        if (moveVertical < -0.1)
        {
            //GetComponent<SpriteRenderer>().sprite = spriteSettings.front;
            //transform.localScale = initialScale;
            animator.SetInteger("Direction", 3);
        }
    }
}