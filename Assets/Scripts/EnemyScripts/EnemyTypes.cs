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
    readyToAttack, OnCooldown, OnDeath, Moving
}


public abstract class Enemy
{

    protected int m_damage;
    protected float m_health;
    protected float m_speed;
    protected float m_acceleration = 5.0f;
    protected float m_attackRange;
    protected float m_attackSpeed;
    protected GameObject m_thisGameObject;
    protected GameObject m_player;
    protected List<States> states = new List<States>();
    protected float m_distanceFromPlayer;
    protected NavMeshAgent thisAgent = null;
    protected Animator thisAnimator = null;
    protected Timer damageAnimTimer ;

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
        thisAnimator = m_thisGameObject.GetComponent<Animator>();
        // m_thisGameObject.GetComponent<NavMeshAgent>().stoppingDistance = m_attackRange - 2; // stop moving when player is in attack range 
        m_thisGameObject.AddComponent<Timer>(); // get a timer
        thisAgent.speed = 1;
        states.Add(States.readyToAttack);
        states.Add(States.Moving);
        damageAnimTimer = m_thisGameObject.AddComponent<Timer>();
        damageAnimTimer.SetTime(4.0f);
    }

    // Check if enemy should attack
    public void EvaluateAttackConditions()
    {
        // find distance between player and this enemy
        m_distanceFromPlayer = Vector3.Distance(m_thisGameObject.transform.position, m_player.transform.position);

        // evaluate if cooldown is done
        if (m_thisGameObject.GetComponent<Timer>().GetTime() >= m_attackSpeed)
        {
            states.Add(States.readyToAttack);
        }
        // evaluate if player in range
        if (m_distanceFromPlayer <= m_attackRange && states.Contains(States.readyToAttack))
        {
            Attack();
        }

    }
    // Start attack animation
    protected virtual void Attack()
    {
        states.Remove(States.readyToAttack);
        states.Remove(States.Moving);
        if (thisAnimator != null) {
            thisAnimator.SetBool("Attacking", true);
        }
    }
   
    // called by other objects to inflict damage and check if this unit should die
    public void LooseHealth(int dmgTaken)
    {
        m_health = m_health - dmgTaken; // change health
        if (m_health <= 0) // check if health > 0
        {
            Die();
        }
        else if (damageAnimTimer.GetTime() >= 1.5f){
            damageAnimTimer.StopAndReset();
            damageAnimTimer.StartTimer();
            thisAgent.speed = 0;
            
            thisAnimator.SetLayerWeight(1, UnityEngine.Random.Range(0.2f, 0.6f));
            thisAnimator.SetFloat("Speed", 2 - thisAnimator.GetLayerWeight(1));
            thisAnimator.SetTrigger("OnHit");
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
        thisAgent.destination = m_thisGameObject.transform.position;
        thisAgent.isStopped = true;
        states.Clear();
        states.Add(States.OnDeath);
        thisAnimator.SetLayerWeight(1, 0);
        thisAnimator.SetFloat("Speed", 1.0f);
        thisAnimator.SetTrigger("OnDeath");
        UnityEngine.Object.Destroy(m_thisGameObject.GetComponent<Collider>());
        //MessageDispatch.GetInstance().SendAudioMessageForDispatch("EnemyDied", this.m_thisGameObject.GetComponent<AudioSource>());
        EnemyManager.GetInstance().RemoveFromList(m_thisGameObject); // remove this gameobject from enemiesAlive list

        

    }
    // Movement mechanism
    protected virtual void Move()
    {
        if (states.Contains(States.Moving))
        {
            if (thisAgent.speed > m_speed)
            {
                m_thisGameObject.transform.LookAt(m_player.transform); // always look player        
                thisAgent.destination = m_player.transform.position; // set destination of navAgent to player's transform
               
            }
            else {
                thisAgent.speed += m_acceleration * Time.deltaTime;
                thisAnimator.SetFloat("Speed", thisAgent.speed / m_speed);
                m_thisGameObject.transform.LookAt(m_player.transform); // always look player        
                thisAgent.destination = m_player.transform.position; // set destination of navAgent to player's transform
            }
        }
        else {
            thisAgent.speed = 0;
           
        }
    }
    // Synchronize functionality with animation
    public virtual void OnAttackAnimEvent() {
        // if player still in reach when animation attack event occurs 
        if (m_distanceFromPlayer <= m_attackRange) {
            m_player.GetComponentInParent<PlayerStats>().IncDecHealth(-m_damage); // inflict damage to player
        }
        
    }
    public virtual void OnEndAttackAnimEvent() {
        m_thisGameObject.GetComponent<Timer>().StopAndReset(); // set cooldown timer to 0
        m_thisGameObject.GetComponent<Timer>().StartTimer(); // start counting time to cooldown      
        states.Add(States.OnCooldown); // set this enemy state to "being on cooldown"
        states.Add(States.Moving);
        thisAnimator.SetBool("Attacking", false); // set animator state to "walk"  

    }
    public virtual void OnDeathAnimEvent() {
        m_thisGameObject.GetComponent<EnemyDrops>().SpawnDrops();
        UnityEngine.Object.Destroy(m_thisGameObject, 5.0f); // Destroy this gameObjec
        thisAnimator.SetFloat("Speed", 0);
    }
    public List<States> GetStates() {
        return states;
    }
    public virtual void OnUpdate() {
        Move();
        EvaluateAttackConditions();
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

