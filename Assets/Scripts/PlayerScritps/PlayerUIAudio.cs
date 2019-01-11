using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIAudio : MonoBehaviour {

    [SerializeField]
    int priority = 0;
    AudioSource UIsoundsSource = null;
	// Use this for initialization
	void Start () {
        UIsoundsSource = GetComponent<AudioSource>();
        UIsoundsSource.priority = priority;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
