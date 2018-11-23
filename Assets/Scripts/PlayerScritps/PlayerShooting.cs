using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    [SerializeField]
    Transform shotPoint;
    [SerializeField]
    Transform target;

    [SerializeField]
    float fireRate = 0.2f;
    float timeSinceLastShot = 0.0f;
    bool canShoot = true;

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    Camera myCamera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot)
        {  
          
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.position = shotPoint.transform.position;
          
            newProjectile.GetComponent<Projectile>().SetTarget(target.position);
            canShoot = false;
        }
        if (!canShoot) {
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot > fireRate) {
                canShoot = true;
                timeSinceLastShot = 0.0f;
            }
        }
        
	}
}
