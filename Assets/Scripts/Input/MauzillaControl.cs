using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MauzillaControl : InputControl
{
    public override void Start()
    {
        base.Start();
    }

    public override void initLaser()
    {
        base.initLaser();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override Vector2 getMovementFromAxis(string playerName)
    {
        return base.getMovementFromAxis(playerName);
    }

    public override void handleMauzillaMovement(Vector2 movement)
    {
        base.handleMauzillaMovement(movement);
    }

    public override void updateLaserSound() {
        base.updateLaserSound();
    }

    public override bool laserActive() {
        return base.laserActive();
    }

    public override void movePlayer(Vector2 movement)
    {
        base.movePlayer(movement);
    }

    public override void moveLaser(Vector2 movement)
    {
        base.moveLaser(movement);
    }

    public override void toggleLaserVisibility(bool visible)
    {
        base.toggleLaserVisibility(visible);
    }

    public override void Flip(float moveHorizontal)
    {
        base.Flip(moveHorizontal);
    }

    public override void FrontBack(float moveVertical)
    {
        base.FrontBack(moveVertical);
    }
}