using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    

    [SerializeField]
    [Range(1, 4)]
    public int number; // number of input (1-4)

    [SerializeField]
    public Character character; //mauzilla, schneider, maurer or tischler

    [SerializeField]
    public Color color;

    [SerializeField]
    public int team = 0;  // Team 0 or 1

    public bool ready = false; // user pressed the input Button to start the game.
    public bool active = false; // user pressed any key to activate himself

    public float cooldownTime = 5;

    public float abilityCooldown = -1;
    public float abilityActiveDuration = -1;
    public bool usesLaser = false;

    InputControl inputControl;

    private void Start() {
        
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
}