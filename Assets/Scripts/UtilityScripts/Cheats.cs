using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour {

    GameObject player;
    PlayerStats pStats;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        pStats = player.GetComponent<PlayerStats>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            pStats.IncDecExp(pStats.GetMaxExp() - pStats.GetCurrentExp());
        }
	}
}
