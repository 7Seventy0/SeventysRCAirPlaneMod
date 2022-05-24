using UnityEngine;
using System.Collections;

public class PlayerClass : MonoBehaviour
{
    public float health = 20;
    bool alive = true;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);
    }
    public void Heal(float amount)
    {
        health += amount;
    }
    float timeSpentDead;
    

    void Revive()
    {
        alive = true;
        GorillaLocomotion.Player.Instance.jumpMultiplier = 1.1f;
        health = 7;
        return;
    }

    void Update()
    {
        if (health <= 0 && alive)
        {
            Debug.Log("YOU HAVE DIED DUE TO DEATH!");
            if (alive)
            {
                alive = false;
            }
            
            timeSpentDead  = Time.time + 12;


        }
        if (!alive)
        {
            GorillaLocomotion.Player.Instance.jumpMultiplier = 0f;
        }

        if(Time.time > timeSpentDead)
        {
            if (!alive)
            {
                Revive();
            }
        }

    }
}