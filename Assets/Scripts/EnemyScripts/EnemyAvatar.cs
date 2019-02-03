﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public enum EnemyTypes { EnemyMellee, EnemyRange, EnemyGolem }
/*
 * Creates a new enemy
 * Sets enemy's type and stats, based on the given parameters
*/
public class EnemyAvatar : MonoBehaviour
{
    public int m_dmg;
    public float m_health;
    public float m_speed;
    public float m_range;
    public float m_attackSpeed;
    public EnemyTypes m_enemyType;
   
    [HideInInspector]
    public float projectileSpeed = 10f;
    [HideInInspector]
    public GameObject projectile = null;
    [HideInInspector]
    public Transform shootingPoint = null;

    [HideInInspector]
    public float torusScaleSpeed = 15.0f;
    [HideInInspector]
    public float torusMaxScaleTimes = 5;
    [HideInInspector]
    public float specialSkillRange = 10.0f;
    [HideInInspector]
    public float specialSkillCooldown = 10.0f;


    private Enemy m_enemy;
    //private bool spawnSound = false;


    //[Range(0, 50)]
    //private int segments = 50;
    //[Range(0, 5)]
    //private float xradius = 5;
    //[Range(0, 5)]
    //private float yradius = 5;
    //LineRenderer line;


    // Use this for initialization
    void Awake()
    {


    }
    private void Start()
    {
        SelectEnemy();
      
    }
    private void SelectEnemy()
    {
        if (m_enemyType == EnemyTypes.EnemyMellee)
        {
            m_enemy = new EnemyMellee(m_dmg, m_health, m_speed, m_range, m_attackSpeed, this.gameObject);
            
        }
        else if (m_enemyType == EnemyTypes.EnemyRange)
        {
            m_enemy = new EnemyRange(m_dmg, m_health, m_speed, m_range, m_attackSpeed, this.gameObject, projectile, projectileSpeed, shootingPoint.gameObject);
            MessageDispatch.GetInstance().SendAudioMessageForDispatch("SpawnFDeam", GetComponent<AudioSource>());

        }
        else if (m_enemyType == EnemyTypes.EnemyGolem)
        {
            m_enemy = new EnemyGolem(specialSkillCooldown, specialSkillRange, torusMaxScaleTimes, torusScaleSpeed, m_dmg, m_health, m_speed, m_range, m_attackSpeed, this.gameObject);
        }
        if (GetComponent<AudioSource>() == null)
        {
            this.gameObject.AddComponent<AudioSource>().volume = 1f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //    if (spawnSound == false && engageTimer.GetTime() > 0.2f)
        //    {

        //        spawnSound = true;
        //    }
        m_enemy.OnUpdate();
        

    }
    void OnCollisionEnter(Collision coll)
    {
        // Check if collided with player's spell
        if (coll.gameObject.tag == "Projectile")
        {
            m_enemy.LooseHealth(coll.gameObject.GetComponent<PlayerProjectiles>().GetDamage());
        }

    }


    public Enemy GetThisEnemy()
    {
        return m_enemy;
    }

    // creates the circle ui element around the enemy
    //void CreatePoints()
    //{
    //    line = gameObject.GetComponent<LineRenderer>();
    //    yradius = 0.05f;
    //    xradius = 0.05f;
    //    line.positionCount = segments + 1;
    //    line.useWorldSpace = false;

    //    float x;
    //    float z;

    //    float angle = 20f;

    //    for (int i = 0; i < (segments + 1); i++)
    //    {
    //        x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
    //        z = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

    //        line.SetPosition(i, new Vector3(x, 0, z));

    //        angle += (360f / segments);
    //    }
    //}

    public void OnAttackAnimEvent()
    {
        m_enemy.OnAttackAnimEvent();
    }
    public void DeathAnimEvent() {
        m_enemy.OnDeathAnimEvent();
    }
    public void OnEndAttackAnimEvent() {
        m_enemy.OnEndAttackAnimEvent();
    }
    public void OnFootStepEvent() {
        if (m_enemyType == EnemyTypes.EnemyMellee)
        {
            if (!AudioManager.GetInstance().GetIfLoopingFsteps())
            {
                m_enemy.OnFootStepEvent();
            }
        }
        else {
            m_enemy.OnFootStepEvent();
        }
    }
    public void OnHitEvent() {
        m_enemy.OnHitEvent();
    }

    public void OnSpecialAttackEvent() {
        ((EnemyGolem)m_enemy).OnSpecialAttackAnimEvent();
    }
    public void OnSpecialAttackEndAnimEvent() {
        ((EnemyGolem)m_enemy).OnSpecialAttackEndAnimEvent();
    }
}
