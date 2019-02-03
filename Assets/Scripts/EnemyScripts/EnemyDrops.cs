using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrops : MonoBehaviour {
    [SerializeField]
    GameObject expDropPrefab = null;
    [SerializeField]
    GameObject manaDropPrefab = null;
    [SerializeField]
    GameObject healthDropPrefab = null;

    [SerializeField]
    static int chancesPercentForHealthDrop = 20;
    [SerializeField]
    static int chancesPercentForManaDrop = 45;

    public void SpawnDrops() {

        GameObject objSpawned = Instantiate(expDropPrefab);
        objSpawned.transform.position = transform.position + new Vector3(3, 1.5f, 2);
        if (Random.Range(0, 100) >= chancesPercentForHealthDrop) {
            objSpawned = Instantiate(healthDropPrefab);
            objSpawned.transform.position = transform.position + new Vector3(-3, 1.5f, 0);
        }
        if (Random.Range(0, 100) >= chancesPercentForManaDrop)
        {
            objSpawned = Instantiate(manaDropPrefab);
            objSpawned.transform.position = transform.position + new Vector3(0, 1.5f, -2);
        }
    }
}
