using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectiles : MonoBehaviour {

    [SerializeField]
    float speed = 10.0f;
    [SerializeField]
    int damage = 5;
    [SerializeField]
    float manaCostPerShot = 3.5f;
    Vector3 targetPosition;

	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
    
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * 2, Time.deltaTime * speed);
        
	}
   
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    public int GetDamage() {
        return damage;
    }
    public float GetManaCost() {
        return manaCostPerShot;
    }
}
