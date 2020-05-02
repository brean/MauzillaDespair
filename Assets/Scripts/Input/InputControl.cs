using UnityEngine;

public class InputControl : MonoBehaviour
{
    [SerializeField]
    [Range(1, 4)]
    int controllerNumber; // number of input (1-4)
    Vector2 currentMovement;
    InputType inputType;

    void Start()
    {
        inputType = GameManager.instance.inputType;
    }

    void Update()
    {
        currentMovement = getMovementFromAxis();
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

    public bool isActionKeyPressed()
    {
        return Input.GetButton(inputManagerNameAction(1));
    }
    public bool isActionKeyPressedInFrame()
    {
        return Input.GetButtonDown(inputManagerNameAction(1));
    }
    public bool isAbilityKeyPressed()
    {
        return Input.GetButton(inputManagerNameAction(2));
    }
    public bool isAbilityKeyPressedInFrame()
    {
        return Input.GetButtonDown(inputManagerNameAction(2));
    }

    public Vector2 getCurrentMovement()
    {
        return currentMovement;
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
        return "Player" + controllerNumber;
    }
}