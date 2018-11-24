using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

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
    
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
        if (transform.position == targetPosition) {
            targetPosition += new Vector3 (targetPosition.x * 10, 0, targetPosition.z * 10); 
        }
	}
    public void SetTarget(Vector3 target) {
        targetPosition = target;
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
