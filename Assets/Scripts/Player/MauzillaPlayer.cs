using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MauzillaPlayer : Player
{
    public GameObject laser;
    public Transform endPoint;
    LineRenderer laserLine;

    public int maxHealth;
    public int health;
    public Image healthbar;
    public Image laserbar;

    Player player;

    GameObject damageEffect;

    public override void Start()
    {
        base.Start();
        
        maxHealth = 300;
        health = maxHealth;
        healthbar = GameObject.Find("MauzillaHealthbar").transform.GetChild(1).gameObject.GetComponent<Image>();
        healthbar.fillAmount = 1.0f;

        laserbar = GameObject.Find("laserpower").GetComponent<Image>();
        laserbar.fillAmount = 1f;


        damageEffect = transform.GetChild(1).gameObject;
        damageEffect.SetActive(false);

        player = GetComponent<Player>();

        initLaser();
    }

    public override void Update()
    {
        // Mauzilla is near a normal/repaired Building and pressing Action Key
        if (colliding && GetComponent<InputControl>().isActionKeyPressedInFrame() && collidingBuilding.state != 1 && collidingBuilding.health > 0) {
            collidingBuilding.adjustHealth(-1);
            gameObject.GetComponent<AudioSource>().Play(0);
        }
        laserbar = GameObject.Find("laserpower").GetComponent<Image>();
        player = GetComponent<Player>();

        laserbar.fillAmount = (player.cooldownTime - player.abilityCooldown) / player.cooldownTime;
        if(laserbar.fillAmount == 1)
        {
            GameObject.Find("Laserbar").transform.GetChild(1).gameObject.SetActive(true);
        } else
        {
            GameObject.Find("Laserbar").transform.GetChild(1).gameObject.SetActive(false);
        }

        base.Update();
    }

    void initLaser()
    {
        if (laser != null)
        {
            laserLine = laser.GetComponent<LineRenderer>();
            laserLine.startWidth = .1f;
            laserLine.endWidth = .1f;

            //set lasers to discharged on start
            abilityCooldown = cooldownTime;
        }
    }

    public override void movePlayer()
    {
        if (inputControl.isActionKeyPressedInFrame())
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
        if (isUsingAbility())
        {
            moveLaser();
            abilityCooldown = cooldownTime;
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

        Vector2 newpos = new Vector2(endPoint.position.x, endPoint.position.y) + (inputControl.getCurrentMovement() * speed);
        endPoint.transform.position = new Vector3(newpos.x, newpos.y, 100);

        RaycastHit2D[] hits;

        Vector3 v1 = new Vector3(endPoint.position.x, endPoint.position.y, 0);
        Vector3 v2 = new Vector3(rb2d.transform.position.x, rb2d.transform.position.y, 0);
        var heading = v1 - v2;
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

    public void TakeDamage(int value) {
        if (health > 10) {
            health -= value;
            Debug.Log("Mauzilla took " + value + " damage!");
            float newHealthbarPercentage = (float)health / (float)(maxHealth - 0);
            healthbar.fillAmount = newHealthbarPercentage;

            StartCoroutine(MauzillaTakesDamageEffect());

        } else if (health <= 10) {
            Debug.Log("Mauzilla retreats!");
        }
    }

    IEnumerator MauzillaTakesDamageEffect() {
        damageEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        damageEffect.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("Mauzilla collide Enter");

        if (col.gameObject.CompareTag("building")) {
            Debug.Log("Mauzilla collided with " + col.gameObject.name);
            colliding = true;
            collidingBuilding = col.gameObject.GetComponent<Building>();
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        Debug.Log("Mauzilla collide Exit");

        if (col.gameObject.CompareTag("building")) {
            Debug.Log("Mauzilla stopped colliding with " + col.gameObject.name);
            colliding = false;
            collidingBuilding = null;
        }
    }
}
