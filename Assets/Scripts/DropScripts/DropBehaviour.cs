using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBehaviour : MonoBehaviour {

    public enum DropType {HealthPotion, ManaPotion, EXP }

    [SerializeField]
    DropType dropType = DropType.EXP;

    [SerializeField]
    int amount = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(transform.position, 1.0f);
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("Pl")) {
            switch (dropType) {
                case DropType.HealthPotion:
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().IncDecHealth(amount);
                    Destroy(this.gameObject);
                    break;
                case DropType.ManaPotion:
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().IncDecMana(amount);
                    Destroy(this.gameObject);
                    break;
                case DropType.EXP:
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().IncDecExp(amount);
                    Destroy(this.gameObject);
                    break;
            }
        }
    }
}
