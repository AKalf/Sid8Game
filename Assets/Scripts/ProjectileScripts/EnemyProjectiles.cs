using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyProjectiles : MonoBehaviour {
    
    public enum ProjectileTypes { FireDeamon }
   
    EnemyRange enemyWhoShotProjectile;
    int damage = 1;
	// Use this for initialization
	void Start () {
        transform.tag = "EnemyProjectile";  
        
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up * -2, Time.deltaTime * 8);
    }
   
    public void SetEnemyWhoShot(EnemyRange enemy) {
        enemyWhoShotProjectile = enemy;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Collider>().tag == "Player")
        {
            collision.transform.GetComponent<PlayerStats>().IncDecHealth(-enemyWhoShotProjectile.GetDamage());
            Destroy(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }
}
