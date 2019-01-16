using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager {

    static MyGameManager inst = null;
    private GameObject player = null;

    private MyGameManager() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public static MyGameManager GetInstance() {
        if (inst == null)
        {
            inst = new MyGameManager();
            return inst;
        }
        else {
            return inst;
        }
    }
    public GameObject GetPlayer() {
        return player;
    }
}
