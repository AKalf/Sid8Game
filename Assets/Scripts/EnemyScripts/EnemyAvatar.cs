﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public enum EnemyTypes { EnemyMellee, EnemyRange, EnemyLeader }
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
    public int m_buffAmount;
    [HideInInspector]
    public float m_buffRange;
    [HideInInspector]
    public float projectileSpeed = 10f;
    [HideInInspector]
    public GameObject projectile = null;
    [HideInInspector]
    public float m_buffCooldown = 10f;
    private Enemy m_enemy;
    private Timer engageTimer;
    private bool spawnSound = false;

    [Range(0, 50)]
    private int segments = 50;
    [Range(0, 5)]
    private float xradius = 5;
    [Range(0, 5)]
    private float yradius = 5;
    LineRenderer line;


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
            CreatePoints();
        }
        else if (m_enemyType == EnemyTypes.EnemyRange)
        {
           // m_enemy = new EnemyRange(m_dmg, m_health, m_speed, m_range, m_attackSpeed, this.gameObject, projectile, projectileSpeed);

        }
        else if (m_enemyType == EnemyTypes.EnemyLeader)
        {
          //  m_enemy = new EnemyLeader(m_dmg, m_health, m_speed, m_range, m_attackSpeed, this.gameObject, m_buffRange, m_buffAmount, m_buffCooldown);

        }

        engageTimer = this.gameObject.AddComponent<Timer>();
        this.gameObject.AddComponent<AudioSource>().volume = 1f;

        engageTimer.StartTimer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //    if (spawnSound == false && engageTimer.GetTime() > 0.2f)
        //    {

        //        spawnSound = true;
        //    }
        Debug.DrawLine(transform.position, transform.position + transform.forward * m_range);
        if (engageTimer.GetTime() > 0.5f)
        {

            m_enemy.Move();

            Destroy(engageTimer);
        }

        // evaluate if enemy can/should attack
        m_enemy.EvaluateAttackConditions();
        //if (m_enemyType == EnemyTypes.EnemyLeader)
        //{
        //    ((EnemyLeader)m_enemy).DoBuff();

        //}

    }
    void OnCollisionEnter(Collision coll)
    {
        Debug.Log("e");
        // Check if collided with player's spell
        if (coll.gameObject.tag == "Projectile")
        {
            m_enemy.LooseHealth(coll.gameObject.GetComponent<Projectile>().GetDamage());
            Debug.Log("enemy hitted projectile");
        }

    }


    public Enemy GetThisEnemy()
    {
        return m_enemy;
    }

    // creates the circle ui element around the enemy
    void CreatePoints()
    {
        line = gameObject.GetComponent<LineRenderer>();
        yradius = m_range / transform.lossyScale.x;
        xradius = m_range / transform.lossyScale.x;
        line.positionCount = segments + 1;
        line.useWorldSpace = false;

        float x;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / segments);
        }
    }


}
