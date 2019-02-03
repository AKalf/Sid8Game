using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelFStepLoop : MonoBehaviour {

    int numberOfSkel = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SkelEnemy") {
            numberOfSkel++;
            if (numberOfSkel >= 5) {
                AudioManager.GetInstance().SetIfLoopingFsteps(true);
                Debug.Log("more than or 7");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SkelEnemy" && numberOfSkel > 0) {
            numberOfSkel--;
            if (numberOfSkel < 5)
            {
                Debug.Log("less than 7");
                AudioManager.GetInstance().SetIfLoopingFsteps(false);
            }
        }
    }
}
