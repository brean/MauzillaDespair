using UnityEngine;

public class MauzillaControl : InputControl
{
    public GameObject laser;
    public Transform endPoint;
    LineRenderer laserLine;

    public override void Start()
    {
        base.Start();

        initLaser();
    }

    void initLaser()
    {
        if (laser != null)
        {
            laserLine = laser.GetComponent<LineRenderer>();
            laserLine.startWidth = .1f;
            laserLine.endWidth = .1f;

            //set lasers to discharged on start
            player.abilityCooldown = player.cooldownTime;
        }
    }

    public override void movePlayer()
    {
        if (this.PressedActionKey())
        {
            animator.SetBool("AttackActive", true);
            attackAnim = 30;
        }
        else
        {
            if (attackAnim <= 0)
            {
                animator.SetBool("AttackActive", false);
            }
            else
            {
                attackAnim -= 1;
            }
        }
        if (player.isUsingAbility())
        {
            moveLaser();
            player.abilityCooldown = player.cooldownTime;
        }
        else
        {
            toggleLaserVisibility(false);
            base.movePlayer();
        }

        updateLaserSound();
    }

    void moveLaser()
    {
        toggleLaserVisibility(true);

        Vector2 newpos = new Vector2(endPoint.position.x, endPoint.position.y) + (currentMovement * speed);
        endPoint.transform.position = new Vector3(newpos.x, newpos.y, 100);

        RaycastHit2D[] hits;

        var heading = new Vector3(endPoint.position.x, endPoint.position.y, 0) - new Vector3(rb2d.transform.position.x, rb2d.transform.position.y, 0);
        var distance = heading.magnitude;
        var direction = heading / distance;

        hits = Physics2D.RaycastAll(rb2d.transform.position, direction, distance);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];

            if (hit.collider.gameObject.tag == "building")
            {
                Building building = hit.collider.gameObject.GetComponent<Building>();

                if (building.state != 1 && building.health >= 1)
                {
                    //building is not destroyed
                    building.adjustHealth(-1);
                }
            }
        }

        laserLine.SetPosition(0, new Vector3(rb2d.transform.position.x, rb2d.transform.position.y, 100));
        laserLine.SetPosition(1, endPoint.position);
    }

    void toggleLaserVisibility(bool visible)
    {
        if (laserLine != null)
        {
            laserLine.gameObject.SetActive(visible);
        }
    }

    void updateLaserSound()
    {
        if (laser == null)
        {
            return;
        }

        AudioSource audio = laser.GetComponent<AudioSource>();
        if (!audio.isPlaying)
        {
            audio.Play(0);
        }
    }
}
