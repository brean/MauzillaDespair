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

    public Rigidbody2D rb2d;

    Animator animator;

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
        } else {
            movePlayer(movement);
        }
    }

    void movePlayer(Vector2 movement)
    {
        toggleLaserVisibility(false);
        Vector2 newPlayerPosition = rb2d.position + (movement * speed);
        // Flip(moveHorizontal);
        // FrontBack(moveVertical);
        rb2d.MovePosition(newPlayerPosition);
    }

    void moveLaser(Vector2 movement)
    {
        toggleLaserVisibility(true);

        // Startpoint is always Player Position
        startPoint = rb2d.transform;
        laserLine.SetPosition(0, new Vector3(startPoint.position.x, startPoint.position.y, 100));

        Vector2 newpos = new Vector2(endPoint.position.x, endPoint.position.y) + (movement * speed);
        endPoint.transform.position = new Vector3(newpos.x, newpos.y, 100);
        laserLine.SetPosition(1, endPoint.position);
    }

    void toggleLaserVisibility(bool visible)
    {
        if (laserLine != null) {
            laserLine.gameObject.SetActive(visible);
        }
    }

        private void Flip(float moveHorizontal)
    {
        if (moveHorizontal > 0.1 || moveHorizontal < -0.1)
        {
            GetComponent<SpriteRenderer>().sprite = spriteSettings.left;
            facingRight = moveHorizontal < -0.1;
            animator.SetInteger("Direction", 2);
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
            GetComponent<SpriteRenderer>().sprite = spriteSettings.back;
            transform.localScale = initialScale;
            animator.SetInteger("Direction", 1);
        }

        if (moveVertical < -0.1)
        {
            GetComponent<SpriteRenderer>().sprite = spriteSettings.front;
            transform.localScale = initialScale;
            animator.SetInteger("Direction", 3);
        }
    }
}