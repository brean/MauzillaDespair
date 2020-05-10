using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MauzillaPlayer : Player
{
    public GameObject laser;
    public Transform endPoint;
    LineRenderer laserLine;

    int maxHealth = 300;
    public int currentHealth;
    public Image healthbar;
    public Image laserbar;

    GameObject damageEffect;

    public override void Start()
    {
        base.Start();
        
        currentHealth = maxHealth;
        healthbar = GameObject.Find("MauzillaHealthbar").transform.GetChild(1).gameObject.GetComponent<Image>();
        healthbar.fillAmount = 1f;

        laserbar = GameObject.Find("laserpower").GetComponent<Image>();
        laserbar.fillAmount = 1f;

        damageEffect = transform.GetChild(1).gameObject;
        damageEffect.SetActive(false);

        laserLine = laser.GetComponent<LineRenderer>();
        laserLine.startWidth = .1f;
        laserLine.endWidth = .1f;

        //set lasers to discharged on start
        currentAbilityCooldown = cooldownTime;
    }

    public override void Update()
    {
        base.Update();
        
        updateLaserbar();
        controlAbility();
    }

    void updateLaserbar()
    {
        laserbar = GameObject.Find("laserpower").GetComponent<Image>();

        laserbar.fillAmount = (cooldownTime - currentAbilityCooldown) / cooldownTime;
        if(laserbar.fillAmount == 1)
        {
            GameObject.Find("Laserbar").transform.GetChild(1).gameObject.SetActive(true);
        } else
        {
            GameObject.Find("Laserbar").transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void controlAbility()
    {
        bool isKeyPressed = inputControl.isAbilityKeyPressed();
        if (currentAbilityCooldown <= 0 && isKeyPressed)
        {
            currentAbilityCooldown = cooldownTime;
            currentAbilityActiveDuration = 3;
            return;
        }

        if (currentAbilityCooldown > -1)
        {
            currentAbilityCooldown -= Time.deltaTime;
        }
        if (currentAbilityActiveDuration >= 0)
        {
            currentAbilityActiveDuration -= Time.deltaTime;
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
        if (currentAbilityActiveDuration > 0)
        {
            moveLaser();
            currentAbilityCooldown = cooldownTime;
        }
        else
        {
            toggleLaserVisibility(false);
            base.movePlayer();
        }

        updateLaserSound();
    }

    public override void animatePlayer()
    {
        if (currentlyNotUsingAbility()) {
            base.animatePlayer();
        }
    }

    bool currentlyNotUsingAbility()
    {
        return currentAbilityActiveDuration <= 0;
    }

    public override void updateAction()
    {
        if(inputControl.isActionKeyPressedInFrame())
        {
            // Mauzilla is near a normal/repaired Building and pressing Action Key
            if (collidingBuilding && GetComponent<InputControl>().isActionKeyPressedInFrame() && collidingBuilding.state != 1 && collidingBuilding.health > 0) {
                collidingBuilding.adjustHealth(-1);
                gameObject.GetComponent<AudioSource>().Play(0);
            }
        }
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
        if (currentHealth > 10) {
            currentHealth -= value;
            float newHealthbarPercentage = (float)currentHealth / (float)(maxHealth - 0);
            healthbar.fillAmount = newHealthbarPercentage;

            StartCoroutine(MauzillaTakesDamageEffect());

        } else if (currentHealth <= 10) {
            Debug.Log("Mauzilla retreats!");
        }
    }

    IEnumerator MauzillaTakesDamageEffect() {
        damageEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        damageEffect.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("building")) {
            collidingBuilding = col.gameObject.GetComponent<Building>();
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.CompareTag("building")) {
            collidingBuilding = null;
        }
    }
}
