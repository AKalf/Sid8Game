using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    [SerializeField]
    Transform shotPoint;
    

    [SerializeField]
    float fireRate = 0.2f;
    float timeSinceLastShot = 0.0f;
    bool canShoot = true;

    [SerializeField]
    GameObject projectile;
    float projManaCost = 0;

    [SerializeField]
    Camera myCamera;

    PlayerStats pMana = null;
	// Use this for initialization
	void Start () {
        pMana = GetComponent<PlayerStats>();
        projManaCost = projectile.GetComponent<PlayerProjectiles>().GetManaCost();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && pMana.GetCurrentMana() >= projManaCost)
        {  
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.transform.position = shotPoint.transform.position;
            pMana.IncDecMana(-projManaCost);

            canShoot = false;
        }
        if (!canShoot) {
            timeSinceLastShot += Time.deltaTime;
           
             if (timeSinceLastShot >= fireRate * (pMana.GetMaxMana() - pMana.GetCurrentMana())) {
                canShoot = true;
                timeSinceLastShot = 0.0f;
            }
        }
        
	}
}
