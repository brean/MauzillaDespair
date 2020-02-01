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

        laserLine = laser.GetComponent<LineRenderer>();
        laserLine.startWidth = .1f;
        laserLine.endWidth = .1f;
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

        move(player.inputName());
        player.controlAbility();
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

        movePlayer(newpos);
        moveLaser(newpos);
    }

    void movePlayer(Vector2 newpos)
    {
        // Flip(moveHorizontal);
        // FrontBack(moveVertical);
        rb2d.MovePosition(newpos);
    }

    void moveLaser(Vector2 newpos)
    {
        startPoint.transform.position = newpos;
        startPoint.transform.position = new Vector3(newpos.x, newpos.y, 100);

        endPoint.transform.position = new Vector3(newpos.x +5, newpos.y +5, 100);

        laserLine.SetPosition(0, startPoint.position);
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