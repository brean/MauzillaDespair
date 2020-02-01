using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    [SerializeField]
    public InputType inputType; // all, key or joy

    [SerializeField]
    [Range(1, 5)]
    public int number; // number of input (1-4)

    [SerializeField]
    public Character character; //mauzilla, schneider, maurer or tischler

    [SerializeField]
    public Color color;

    [SerializeField]
    public int team = 0;  // Team 0 or 1

    public bool ready = false; // user pressed the input Button to start the game.
    public bool active = false; // user pressed any key to activate himself

    public float abilityCooldown = 5;
    public float abilityActiveDuration = -1;

    public KeyCode ActionKey()
    {
        return ActionKey(inputType, number);
    }

    public bool PressedActionKey()
    {
        return Input.GetKeyDown(this.ActionKey());
    }

    public static KeyCode ActionKey(InputType inputType, int number)
    {
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

    public static KeyCode AbilityKey(InputType inputType, int number)
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


    private KeyCode AbilityKey()
    {
        return AbilityKey(inputType, number);
    }

    internal float HorizontalAxis()
    {
        return Input.GetAxisRaw(this.inputName() + "Horizontal");
    }

    public float VerticalAxis()
    {
        return Input.GetAxisRaw(this.inputName() + "Vertical");
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

    public string inputName()
    {
        if (inputType == InputType.All)
        {
            string[] controllers = Input.GetJoystickNames();
            if (number <= controllers.Length)
            {
                inputType = InputType.Joy;
            }
            else
            {
                inputType = InputType.Key;
            }
        }
        return "Player" + number + inputType.ToString();
    }

    public bool isUsingAbility()
    {
        return abilityActiveDuration > 0;
    }

    internal void controlAbility()
    {
        
        if (abilityCooldown <= 0 && Input.GetKeyDown(this.AbilityKey()))
        {
            //TODO: do something  
            Debug.Log("abilities");
            abilityCooldown = 5;
            abilityActiveDuration = 3;
        }
        else
        {
            if(abilityCooldown > -1)
            {
                abilityCooldown -= Time.deltaTime;
            }
            if (abilityActiveDuration > -1)
            {
                abilityActiveDuration -= Time.deltaTime;
            }

            if (Input.GetKeyUp(this.AbilityKey()))
            {
                abilityActiveDuration = -1;
            }
        }
    }
}