using UnityEngine;

public class InputControl : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    [HideInInspector]
    public Rigidbody2D rb2d;
    [HideInInspector]
    public Animator animator;

    [Tooltip("speed of the player")]
    public float speed = .08f;

    [HideInInspector]
    public float attackAnim = 0;

    [HideInInspector]
    public Vector3 initialScale;

    public Vector2 currentMovement;
    bool isAction1InputPressed;
    bool isAction2InputPressed;
    InputType inputType;

    public virtual void Start()
    {
        inputType = GameManager.instance.inputType;

        player = GetComponent<Player>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        initialScale = transform.localScale;
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        currentMovement = getMovementFromAxis();

        this.updateInputActionKeys();
        player.controlAbility(this.isAction2InputPressed);

        movePlayer();
    }

    Vector2 getMovementFromAxis()
    {
        string inputManagerName = this.inputManagerNameAxis();
        float moveHorizontal = Input.GetAxis(inputManagerName + "Horizontal");
        float moveVertical = -Input.GetAxis(inputManagerName + "Vertical");

        if (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical) < .1)
        {
            return new Vector2(0,0);
        }

        return new Vector2(moveHorizontal, moveVertical);
    }

    void updateInputActionKeys()
    {
        this.isAction1InputPressed = Input.GetButton(inputManagerNameAction(1));
        this.isAction2InputPressed = Input.GetButton(inputManagerNameAction(2));
    }

    public bool PressedActionKey()
    {
        return Input.GetButtonDown(inputManagerNameAction(1));
    }

    public bool PressedAbilityKey()
    {
        return Input.GetButtonDown(inputManagerNameAction(2));
    }

    public virtual void movePlayer()
    {
        Vector2 newPlayerPosition = rb2d.position + (currentMovement * speed);
        if (animator != null && attackAnim <= 0) { 
            player.Flip(currentMovement.x);
            player.FrontBack(currentMovement.y);
        }
        rb2d.MovePosition(newPlayerPosition);
    }
    
    string inputManagerNameAxis()
    {
        // z.B. Player1Keyboard oder Player2Joy
        return inputNamePlayer() + inputType.ToString();
    }
    string inputManagerNameAction(int action)
    {
        // z.B. Player1Action1 oder Player4Action2
        return inputNamePlayer() + "Action" + action;
    }

    string inputNamePlayer()
    {
        return "Player" + player.number;
    }






}