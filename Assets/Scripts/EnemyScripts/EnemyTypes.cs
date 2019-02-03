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
    protected float minOnHitLayerWeight = 0.3f;
    protected float maxOnHitLayerWeight = 0.7f;
    /// <summary>
    /// the basic constructor for all enemies
    /// </summary>
    /// <param name="tempDamage"></param>
    /// <param name="tempHealth"></param>
    /// <param name="tempSpeed"></param>
    /// <param name="tempRange"></param>
    /// <param name="tempAttackSpeed"></param>
    /// <param name="thisGameobject">the gameobject that created this class object</param>
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
        minOnHitLayerWeight = thisAnimator.GetFloat("minOnHitWeight");
        maxOnHitLayerWeight = thisAnimator.GetFloat("maxOnHitWeight");
    }

    /// <summary>
    /// Check if enemy should attack
    /// </summary>
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
    /// <summary>
    /// Remove rdyToAttck and Moving states from current states
    /// and start attack animation
    /// </summary>
    protected virtual void Attack()
    {
        states.Remove(States.readyToAttack);
        states.Remove(States.Moving);
        if (thisAnimator != null) {
            thisAnimator.SetBool("Attacking", true);
        }
    }
    /// <summary>
    /// called by other objects to inflict damage and check if this unit should die
    /// </summary>
    /// <param name="dmgTaken"></param>
    public virtual void LooseHealth(int dmgTaken)
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
            
            thisAnimator.SetLayerWeight(1, UnityEngine.Random.Range(minOnHitLayerWeight, maxOnHitLayerWeight));
            thisAnimator.SetFloat("Speed", 2 - thisAnimator.GetLayerWeight(1)); // decrease speed for as much damage as animation weight
            thisAnimator.SetTrigger("OnHit");
        }

    }
    /// <summary>
    /// called by other objects to increase health 
    /// </summary>
    /// <param name="amount">the amount to increase </param>
    public void IncreaseHealth(int amount)
    {
        m_health = m_health + amount;

    }
    /// <summary>
    /// returns this enemy's current health
    /// </summary>
    /// <returns></returns>
    public float GetHealth()
    {
        return m_health;

    }
    /// <summary>
    /// How to die
    /// </summary>
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
    /// <summary>
    /// Movement mechanism
    /// </summary>
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
    /// <summary>
    /// Synchronize functionality with animations. Base functionality is null
    /// </summary>
    public virtual void OnAttackAnimEvent()
    {


    }
    /// <summary>
    ///  Set functionality when footstep is triggered
    /// </summary>
    public virtual void OnFootStepEvent() {

    }
    /// <summary>
    ///  Set functionality when "getting hit" is triggered. Basic is null
    /// </summary>
    public virtual void OnHitEvent() {

    }
    /// <summary>
    /// Set cooldown state and enable movement
    /// </summary>
    public virtual void OnEndAttackAnimEvent() {
        m_thisGameObject.GetComponent<Timer>().StopAndReset(); // set cooldown timer to 0
        m_thisGameObject.GetComponent<Timer>().StartTimer(); // start counting time to cooldown      
        states.Add(States.OnCooldown); // set this enemy state to "being on cooldown"
        states.Add(States.Moving);
        thisAnimator.SetBool("Attacking", false); // set animator state to "walk"  

    }
    /// <summary>
    /// Basic functionality: Drop items, Destroy after 5 sec, set animation speed to 1
    /// </summary>
    public virtual void OnDeathAnimEvent() {
        m_thisGameObject.GetComponent<EnemyDrops>().SpawnDrops();
        UnityEngine.Object.Destroy(m_thisGameObject, 5.0f); // Destroy this gameObjec
        thisAnimator.SetFloat("Speed", 1);
    }
    /// <summary>
    /// Get the list with this enemies current states
    /// </summary>
    /// <returns>list with current states</returns>
    public List<States> GetStates() {
        return states;
    }
    /// <summary>
    /// Put any code you want to run on Unity's Update()
    /// </summary>
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
    // Synchronize functionality with animations
    public override void OnAttackAnimEvent()
    {
        // if player still in reach when animation attack event occurs 
        if (m_distanceFromPlayer <= m_attackRange)
        {
            m_player.GetComponentInParent<PlayerStats>().IncDecHealth(-m_damage); // inflict damage to player
        }
        MessageDispatch.GetInstance().SendAudioMessageForDispatch("AtSkel", m_thisGameObject.GetComponent<AudioSource>());
    }
    public override void OnFootStepEvent()
    {
        MessageDispatch.GetInstance().SendAudioMessageForDispatch("FstepSkel", m_thisGameObject.GetComponent<AudioSource>());
    }
    public override void OnDeathAnimEvent()
    {
        base.OnDeathAnimEvent() ;
        MessageDispatch.GetInstance().SendAudioMessageForDispatch("DeathSkel", m_thisGameObject.GetComponent<AudioSource>());
    }
    public override void OnHitEvent()
    {
        MessageDispatch.GetInstance().SendAudioMessageForDispatch("OnHitSkel", m_thisGameObject.GetComponent<AudioSource>());
    }

}

public class EnemyRange : Enemy {
    GameObject m_projectile;
    GameObject m_shootingPoint;
    float m_projectileSpeed;
    public EnemyRange(int tempDamage, float tempHealth, float tempSpeed, float tempRange, float tempAttackSpeed, GameObject thisGameobject, GameObject projectile, float projectileSpeed, GameObject shootingPoint) : base(tempDamage, tempHealth, tempSpeed, tempRange, tempAttackSpeed, thisGameobject)
    {
        m_projectile = projectile;
        m_projectileSpeed = projectileSpeed;
        m_shootingPoint = shootingPoint;
    }
    /// <summary>
    /// Creates a new projectile at the shootingPoint's position
    /// </summary>
    public override void OnAttackAnimEvent()
    {
        GameObject projectile = MonoBehaviour.Instantiate(m_projectile, m_shootingPoint.transform.position, m_shootingPoint.transform.rotation);
        EnemyProjectiles projComp = projectile.AddComponent<EnemyProjectiles>();
        MessageDispatch.GetInstance().SendAudioMessageForDispatch("AtFDeam", m_thisGameObject.GetComponent<AudioSource>());
        projComp.SetEnemyWhoShot(this);
    }
    public override void OnHitEvent()
    {
        MessageDispatch.GetInstance().SendAudioMessageForDispatch("OnHitFDeam", m_thisGameObject.GetComponent<AudioSource>());
    }
    protected override void Die()
    {
        base.Die();
        MessageDispatch.GetInstance().SendAudioMessageForDispatch("DeathFDeam", m_thisGameObject.GetComponent<AudioSource>());
    }
    public int GetDamage() {
        return m_damage;
    }

}

public class EnemyGolem : Enemy
{

    GameObject torus = null;
    float m_torusScaleSpeed = 15.0f;
    float m_torusMaxScaleTimes = 5;
    float m_specialSkillRange = 10.0f;
    float m_specialSkillCd = 10.0f;

    Timer spSkillTimer;
    Vector3 startingScale = Vector3.zero;
    bool isTorusScaling = false;
    bool isSpSkillOnCd = false;

    public EnemyGolem(float specialSkillCooldown, float specialSkillRange, float torusMaxScaleTimes, float torusScaleSpeed, int tempDamage, float tempHealth, float tempSpeed, float tempRange, float tempAttackSpeed, GameObject thisGameobject) : base(tempDamage, tempHealth, tempSpeed, tempRange, tempAttackSpeed, thisGameobject)
    {
        torus = m_thisGameObject.transform.GetChild(0).gameObject;
        m_torusScaleSpeed = torusScaleSpeed;
        m_torusMaxScaleTimes = torusMaxScaleTimes;
        m_specialSkillRange = specialSkillRange;
        startingScale = torus.transform.localScale;
        spSkillTimer = m_thisGameObject.AddComponent<Timer>();
        
    }

    public void OnSpecialAttackAnimEvent()
    {
        MessageDispatch.GetInstance().SendAudioMessageForDispatch("SpSkillGolem", m_thisGameObject.GetComponent<AudioSource>());
        torus.SetActive(true);
        isTorusScaling = true;
    }
    public void OnSpecialAttackEndAnimEvent()
    {
        states.Add(States.Moving);
        states.Add(States.readyToAttack);
    }
    public override void OnFootStepEvent()
    {
        MessageDispatch.GetInstance().SendAudioMessageForDispatch("FtSpGolem", m_thisGameObject.GetComponent<AudioSource>());
    }
    public override void OnAttackAnimEvent()
    {
        
        if (m_distanceFromPlayer <= m_attackRange)
        {
            m_player.GetComponentInParent<PlayerStats>().IncDecHealth(-m_damage); // inflict damage to player
        }
        MessageDispatch.GetInstance().SendAudioMessageForDispatch("AtGolem", m_thisGameObject.GetComponent<AudioSource>());
    }
    public override void OnDeathAnimEvent()
    {
        base.OnDeathAnimEvent();
        MessageDispatch.GetInstance().SendAudioMessageForDispatch("DeathGolem", m_thisGameObject.GetComponent<AudioSource>());
    }

    private void ScaleTorus() {
        torus.transform.localScale += new Vector3(m_torusScaleSpeed * Time.deltaTime, 0, m_torusScaleSpeed * Time.deltaTime);
        if (torus.transform.localScale.x >  m_torusMaxScaleTimes)
        {
            torus.transform.localScale = startingScale;
            isTorusScaling = false;
            torus.SetActive(false);
        }
       
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isTorusScaling) {
            ScaleTorus();
        }
        else
        {
           
            if (!isSpSkillOnCd)
            {
                if (Vector3.Distance(MyGameManager.GetInstance().GetPlayer().transform.position, m_thisGameObject.transform.position) <= m_specialSkillRange)
                {
                    states.Remove(States.Moving);
                    states.Remove(States.readyToAttack);
                    thisAnimator.SetTrigger("SpecialSkill");
                    isSpSkillOnCd = true;
                    spSkillTimer.StartTimer();
                }
            }
            else
            {
                if (spSkillTimer.GetTime() >= m_specialSkillCd)
                {
                    isSpSkillOnCd = false;
                    spSkillTimer.StopAndReset();
                }
            }
        }
        
        

    }
}

