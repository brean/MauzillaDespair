using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[ExecuteInEditMode]
public class Cars : MonoBehaviour
{
    ParticleSystem ps;
    int timer = 0;

    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();
    public GameObject explosion;

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(timer >= 0)
        {
            timer -= 1;
        }
        else
        {
            explosion.SetActive(false);
        }
    }

    void OnParticleTrigger()
    {
        // get the particles which matched the trigger conditions this frame
        if(ps == null)
        {
            return;
        }
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);


        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            p.velocity = new Vector3(0, 0, 0);
            explosion.transform.position = p.position;
            explosion.SetActive(true);
            timer = 30;
            enter[i] = p;
        }

        // re-assign the modified particles back into the particle system
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }
}
