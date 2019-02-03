using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torusOnColl : MonoBehaviour {

    [SerializeField]
    int torusDamage = 1;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == ("Player")) {
            MyGameManager.GetInstance().GetPlayer().GetComponent<PlayerStats>().IncDecHealth(-torusDamage);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == ("Player"))
        {
            MyGameManager.GetInstance().GetPlayer().GetComponent<PlayerStats>().IncDecHealth(-torusDamage);
        }
    }


}
