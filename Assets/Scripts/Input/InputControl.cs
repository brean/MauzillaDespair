using UnityEngine;

public class InputControl : MonoBehaviour
{
    [Tooltip("Player information")]
    public Player player;

    [Tooltip("speed of the player")]
    public float speed = .08f;

    public float attackAnim = 0;

    public Rigidbody2D rb2d;
    public Animator animator;

    bool facingRight;
    Vector3 initialScale;

    public Vector2 currentMovement;
    bool isAction1InputPressed;
    bool isAction2InputPressed;

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameManager.instance.playerForCharacter(GetComponent<CharacterSetting>().character);
      //  spriteSettings = GetComponent<CharacterSpriteManager>().SpritesForCharacter(player.character);

       // animator = GetComponent<Animator>();
       // animator.speed = 1;
        facingRight = true;
        rb2d = GetComponent<Rigidbody2D>();

        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }

        currentMovement = getMovementFromAxis(player.inputName());

        this.updateInputActionKeys();
        player.controlAbility(this.isAction2InputPressed);

        movePlayer();
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

    void updateInputActionKeys()
    {
        if (Input.GetKeyDown(this.ActionKey(player.inputType, player.number)))
        {
            this.isAction1InputPressed = true;
        }
        if (Input.GetKeyUp(this.ActionKey(player.inputType, player.number)))
        {
            this.isAction1InputPressed = false;
        }
        
        if (Input.GetKeyDown(this.AbilityKey(player.inputType, player.number)))
        {
            this.isAction2InputPressed = true;
        }
        if (Input.GetKeyUp(this.AbilityKey(player.inputType, player.number)))
        {
            this.isAction2InputPressed = false;
        }
    }

    public bool PressedActionKey()
    {
        return Input.GetKeyDown(this.ActionKey(player.inputType, player.number));
    }

    public KeyCode ActionKey(InputType inputType, int number)
    {
        // Debug.Log("InputType: " + inputType);
        if (inputType == InputType.Key)
        {
            switch (number)
            {
                case 1:
                    return KeyCode.Space;
                case 2:
                    return KeyCode.Q;
                case 3:
                    return KeyCode.R;
                case 4:
                    return KeyCode.U;
            }
        }
        else
        {
            switch (number)
            {
                case 1:
                    return KeyCode.Joystick1Button0;
                case 2:
                    return KeyCode.Joystick2Button0;
                case 3:
                    return KeyCode.Joystick3Button0;
                case 4:
                    return KeyCode.Joystick4Button0;
            }
        }
        return KeyCode.Return;
    }

        public KeyCode AbilityKey(InputType inputType, int number)
    {
        if (inputType == InputType.Key)
        {
            switch (number)
            {
                case 1:
                    return KeyCode.M;
                case 2:
                    return KeyCode.E;
                case 3:
                    return KeyCode.Z;
                case 4:
                    return KeyCode.O;
            }
        }
        else
        {
            switch (number)
            {
                case 1:
                    return KeyCode.Joystick1Button1;
                case 2:
                    return KeyCode.Joystick2Button1;
                case 3:
                    return KeyCode.Joystick3Button1;
                case 4:
                    return KeyCode.Joystick4Button1;
            }
        }
        return KeyCode.Return;
    }

    public virtual void movePlayer()
    {
        Vector2 newPlayerPosition = rb2d.position + (currentMovement * speed);
        if (animator != null && attackAnim <= 0) { 
            Flip(currentMovement.x);
            FrontBack(currentMovement.y);
        }
        rb2d.MovePosition(newPlayerPosition);
    }

    public virtual void Flip(float moveHorizontal)
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

    public virtual void FrontBack(float moveVertical)
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