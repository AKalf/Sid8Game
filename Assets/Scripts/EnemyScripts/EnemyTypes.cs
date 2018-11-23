using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


/*
 * Holds the basic constructor for all enemies
 * Holds the basic functionality for all enemies
 * This is evaluate if enemy should attack
 * The default attack mechanism (can be overrided)
 * Decrease/increase/get health mechanism
 * Death (can be overrided)
 * Movement mechanism (can be overrided)
*/
public enum States
{
    readyToAttack, InCooldown, OnDeath
}


public abstract class Enemy
{

    protected int m_damage;
    protected float m_health;
    protected float m_speed;
    protected float m_attackRange;
    protected float m_attackSpeed;
    protected GameObject m_thisGameObject;
    protected GameObject m_player;
    protected States m_attackState = States.readyToAttack;
    protected float m_distanceFromPlayer;
    protected NavMeshAgent thisAgent = null;


    // the basic constructor for all enemies
    protected Enemy(int tempDamage, float tempHealth, float tempSpeed, float tempRange, float tempAttackSpeed, GameObject thisGameobject)
    {
        m_damage = tempDamage;
        m_health = tempHealth;
        m_speed = tempSpeed;
        m_attackRange = tempRange;
        m_attackSpeed = tempAttackSpeed;
        m_thisGameObject = thisGameobject;
        m_player = GameObject.FindGameObjectWithTag("Player");
        thisAgent = m_thisGameObject.GetComponent<NavMeshAgent>();
        // m_thisGameObject.GetComponent<NavMeshAgent>().stoppingDistance = m_attackRange - 2; // stop moving when player is in attack range 
        m_thisGameObject.AddComponent<Timer>(); // get a timer
        thisAgent.speed = m_speed;
        thisAgent.acceleration = 5000f;
        thisAgent.autoBraking = false;
    }

    // Check if enemy should attack
    public void EvaluateAttackConditions()
    {
        // find distance between player and this enemy
        m_distanceFromPlayer = Vector3.Distance(m_thisGameObject.transform.position, m_player.transform.position);

        // evaluate if cooldown is done
        if (m_thisGameObject.GetComponent<Timer>().GetTime() >= m_attackSpeed)
        {
            m_attackState = States.readyToAttack;
        }
        // evaluate if player in range
        if (m_distanceFromPlayer <= m_attackRange && m_attackState == States.readyToAttack)
        {
            Attack();
        }

    }
    // the default attack of enemies
    protected virtual void Attack()
    {
        m_player.GetComponentInParent<PlayerHealth>().LooseHP(m_damage); // inflict damage to player
        m_thisGameObject.GetComponent<Timer>().StopAndReset(); // set cooldown timer to 0
        m_thisGameObject.GetComponent<Timer>().StartTimer(); // start counting time to cooldown      
        m_attackState = States.InCooldown; // set this enemy state to "being on cooldown"
        Debug.Log("Attacked");
    }
    // called by other objects to inflict damage and check if this unit should die
    public void LooseHealth(int dmgTaken)
    {
        m_health = m_health - dmgTaken; // change health
        if (m_health <= 0) // check if health > 0
        {
            Die();
        }

    }
    // called by other objects to increase health 
    public void IncreaseHealth(int amount)
    {
        m_health = m_health + amount;

    }
    // returns this enemy's current health
    public float GetHealth()
    {
        return m_health;

    }
    // How to die
    protected virtual void Die()
    {
        Debug.Log("message sent" + this.m_thisGameObject.GetComponent<AudioSource>());
        //MessageDispatch.GetInstance().SendAudioMessageForDispatch("EnemyDied", this.m_thisGameObject.GetComponent<AudioSource>());
        EnemyManager.GetInstance().RemoveFromList(m_thisGameObject); // remove this gameobject from enemiesAlive list

        UnityEngine.Object.Destroy(m_thisGameObject, 0.1f); // Destroy this gameObjec

    }
    // Movement mechanism
    public virtual void Move()
    {
        m_thisGameObject.transform.LookAt(m_player.transform); // always look player        
        thisAgent.destination = m_player.transform.position; // set destination of navAgent to player's transform

    }
}


/*
 * The basic enemy. He is mostly identical to base class.
 * Mellee 
*/
public class EnemyMellee : Enemy
{

    public EnemyMellee(int tempDamage, float tempHealth, float tempSpeed, float tempRange, float tempAttackSpeed, GameObject thisGameobject) : base(tempDamage, tempHealth, tempSpeed, tempRange, tempAttackSpeed, thisGameobject)
    {

    }


}

