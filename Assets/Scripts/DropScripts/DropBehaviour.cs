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
                    MyGameManager.GetInstance().GetPlayer().GetComponent<PlayerStats>().IncDecHealth(amount);
                    MessageDispatch.GetInstance().SendAudioMessageForDispatch("HpPickUp", MyGameManager.GetInstance().GetPlayer().GetComponent<AudioSource>());
                    Destroy(this.gameObject);
                    break;
                case DropType.ManaPotion:
                    MyGameManager.GetInstance().GetPlayer().GetComponent<PlayerStats>().IncDecMana(amount);
                    MessageDispatch.GetInstance().SendAudioMessageForDispatch("ManaPickUp", MyGameManager.GetInstance().GetPlayer().GetComponent<AudioSource>());
                    Destroy(this.gameObject);
                    break;
                case DropType.EXP:
                    MyGameManager.GetInstance().GetPlayer().GetComponent<PlayerStats>().IncDecExp(amount);
                    MessageDispatch.GetInstance().SendAudioMessageForDispatch("ExpPickUp", MyGameManager.GetInstance().GetPlayer().GetComponent<AudioSource>());
                    Destroy(this.gameObject);
                    break;
            }
        }
    }
}
