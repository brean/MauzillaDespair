using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputControl : MonoBehaviour
{
    [Tooltip("Player information")]
    public Player player;

    [Tooltip("speed of the player")]
    public float speed = .01f;

    public float attackAnim = 0;

    public Rigidbody2D rb2d;

    public Animator animator;

    private bool facingRight;
    private Vector3 initialScale;

    CharacterSpiteSettings spriteSettings;

    public GameObject laser;
    LineRenderer laserLine;
    public Transform startPoint;
    public Transform endPoint;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.playerForCharacter(GetComponent<CharacterSetting>().character);
      //  spriteSettings = GetComponent<CharacterSpriteManager>().SpritesForCharacter(player.character);

       // animator = GetComponent<Animator>();
       // animator.speed = 1;
        facingRight = true;
        rb2d = GetComponent<Rigidbody2D>();

        initialScale = transform.localScale;

        initLaser();
    }

    void initLaser()
    {
        if (laser != null)
        {
            laserLine = laser.GetComponent<LineRenderer>();
            laserLine.startWidth = .1f;
            laserLine.endWidth = .1f;

            //set lasers to discharged on start
            player.abilityCooldown = player.cooldownTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = getMovementFromAxis(player.inputName());
        if (player == null)
        {
            return;
        }

        player.controlAbility();
        if (player.PressedActionKey()){
            animator.SetBool("AttackActive", true);
            attackAnim = 30;
        } else
        {
            if (attackAnim <= 0)
            {
                animator.SetBool("AttackActive", false);
            }else
            {
                attackAnim -= 1;
            }
            }
        //animator.SetBool("AttackActive", true);

        if (player.character == Character.mauzilla) {
            handleMauzillaMovement(movement);
            return;
        }

        // all other Players, move always. Erstmal!
        movePlayer(movement);
    }

    Vector2 getMovementFromAxis(string playerName)
    {
        float moveHorizontal = Input.GetAxis(playerName + "Horizontal");
        float moveVertical = -Input.GetAxis(playerName + "Vertical");

        if (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical) < .1)
        {
            return new Vector2(0,0);
        }

        return new Vector2(moveHorizontal, moveVertical);
    }

    void handleMauzillaMovement(Vector2 movement)
    {
        if (player.isUsingAbility()) {
            moveLaser(movement);
            player.abilityCooldown = player.cooldownTime;
        } else {
            movePlayer(movement);
        }
        updateLaserSound();
    }

    void updateLaserSound() {
        if (GameObject.Find("laser") == null) {
            return;
        }
        AudioSource audio = GameObject.Find("laser").GetComponent<AudioSource>();
        if (!audio.isPlaying) {
            audio.Play(0);
        }
    }

    bool laserActive() {
        return laserLine.gameObject.active == true;
    }

    void movePlayer(Vector2 movement)
    {
        toggleLaserVisibility(false);
        Vector2 newPlayerPosition = rb2d.position + (movement * speed);
        if (animator != null && attackAnim <= 0) { 
            Flip(movement.x);
            FrontBack(movement.y);
        }
        rb2d.MovePosition(newPlayerPosition);
    }

    void moveLaser(Vector2 movement)
    {
        toggleLaserVisibility(true);
        Vector2 newpos = new Vector2(endPoint.position.x, endPoint.position.y) + (movement * speed);
        endPoint.transform.position = new Vector3(newpos.x, newpos.y, 100);

        RaycastHit2D[] hits;

        var heading = new Vector3(endPoint.position.x, endPoint.position.y, 0) - new Vector3(rb2d.transform.position.x, rb2d.transform.position.y, 0);
        var distance = heading.magnitude;
        var direction = heading / distance;

        hits = Physics2D.RaycastAll(rb2d.transform.position, direction, distance);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];

            if (hit.collider.gameObject.tag == "building")
            {
                Building building = hit.collider.gameObject.GetComponent<Building>();

                if (building.state != 1 && building.health >= 1)
                {
                    //building is not destroyed
                    building.adjustHealth(-1);
                }
            }
        }

        laserLine.SetPosition(0, new Vector3(rb2d.transform.position.x, rb2d.transform.position.y, 100));
        laserLine.SetPosition(1, endPoint.position);
    }

    void toggleLaserVisibility(bool visible)
    {
        if (laserLine != null) {
            laserLine.gameObject.SetActive(visible);
        }
    }

    public void Flip(float moveHorizontal)
    {
        if (moveHorizontal > 0.1 || moveHorizontal < -0.1)
        {
            //GetComponent<SpriteRenderer>().sprite = spriteSettings.left;
            facingRight = moveHorizontal < -0.1;
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