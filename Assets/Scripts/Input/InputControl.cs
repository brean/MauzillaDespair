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

        player.controlAbility();

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