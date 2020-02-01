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
        /*
        System.Array values = System.Enum.GetValues(typeof(KeyCode));
        foreach (KeyCode code in values)
        {
            if (Input.GetKeyDown(code)) { Debug.Log("butts: " + System.Enum.GetName(typeof(KeyCode), code)); }
        }
        */
        if (player != null)
        {
            player.controlAbility();
            move(player.inputName());
        }
    }

    void move(string input)
    {
        float moveHorizontal = Input.GetAxis(input + "Horizontal");
        float moveVertical = -Input.GetAxis(input + "Vertical");

        if (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical) < .1)
        {
            return;
        }

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        Vector2 newpos = rb2d.position + (movement * speed);

        if(player.abilityActive > 0 && player.character == Character.mauzilla && laserLine != null)
        {
            laserLine.gameObject.SetActive(true);
            newpos = new Vector2(endPoint.position.x, endPoint.position.y) + (movement * speed);
            moveLaser(newpos);
            return;
        }

        if (laserLine != null)
        {
            laserLine.gameObject.SetActive(false);
        }
        movePlayer(newpos);

    }

    void movePlayer(Vector2 newpos)
    {
        // Flip(moveHorizontal);
        // FrontBack(moveVertical);
        rb2d.MovePosition(newpos);
    }

    void moveLaser(Vector2 newpos)
    {
        endPoint.transform.position = new Vector3(newpos.x, newpos.y, rb2d.transform.position.z);

        laserLine.SetPosition(0, rb2d.transform.position);
        laserLine.SetPosition(1, endPoint.position);
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