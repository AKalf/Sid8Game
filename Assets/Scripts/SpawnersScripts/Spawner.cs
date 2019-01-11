using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;
public class Spawner : MonoBehaviour
{

    /*
     * Handles many spawners
     * Gets its children 
     * spawns an enemy to each one's position. Loops until the number of gameobject that must spawn, have spawned
     * sets what GameObject should be spawned
     * sets how many GameObjects should spawn
     * sets the time between spawns
     * starts spawning when collider is triggered
     * if all units spawned, destroy itself
    */


    [SerializeField]
    GameObject unitToSpawn;
    [SerializeField]
    public int numberOfUnitsToSpawn = 10;
    [SerializeField]
    public int spawningFrequency = 10;
    //int unitsCounter = 0;   
    private bool startSpawing = false;
    public List<GameObject> spawnPoints = new List<GameObject>();
    private Timer timerForSpawning = null;
    private int unitsCounter = 0;
    private int indexOfSpawnpoints = 0;
    private Timer SpawnSoundTimer = null;
    [SerializeField][HideInInspector]
    public GameObject thisGO;

    
    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start()
    {
        
        SpawnSoundTimer = gameObject.AddComponent<Timer>();
        timerForSpawning = gameObject.AddComponent<Timer>();
        Transform[] childerntransforms = GetComponentsInChildren<Transform>(); // Get childer spawners (positions that enemies should spawn)
        foreach (Transform child in childerntransforms)
        {
            if (child.gameObject != this.gameObject)
            {
                spawnPoints.Add(child.gameObject); // Add number of child spawners 
            }
        }
    }

    void Update()
    {
        if (startSpawing == true)
        {
            StartSpawning();
        }
    }

    // if collider triggered start spawning
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && startSpawing == false)
        {
            Debug.Log(gameObject.name + " triggered");
            SpawnSoundTimer.StartTimer();
            timerForSpawning.StartTimer();
            startSpawing = true;
            // add this spawner to SpawnersManager's list: activeSpawners
            SpawnersManager.GetInstance().AddToList(this.gameObject);
            //MessageDispatch.GetInstance().SendAudioMessageForDispatch("SpawnerTriggered", this.gameObject.GetComponent<AudioSource>());
            //Debug.Log("spawner triggered");
        }
    }

    // spawn an enemy after X timing period  until units that spawned are equal to the max units this spawner can spawn. Then it destroys itself
    private void StartSpawning()
    {
        if (indexOfSpawnpoints >= spawnPoints.Count)
        {
            indexOfSpawnpoints = 0;
        }
        if (timerForSpawning.GetTime() >= spawningFrequency && unitsCounter <= numberOfUnitsToSpawn)
        {

            // add this enemy as alive to EnemyManager's list: aliveEnemies
            EnemyManager.GetInstance().AddToList(Instantiate(unitToSpawn, spawnPoints[indexOfSpawnpoints].transform.position, unitToSpawn.transform.rotation).gameObject);
            timerForSpawning.StopAndReset();
            timerForSpawning.StartTimer();
            //Debug.Log("Spawned unit " + unitsCounter + " from position with index " + indexOfSpawnpoints);
            unitsCounter++;
            indexOfSpawnpoints++;
            //MessageDispatch.GetInstance().SendAudioMessageForDispatch("EnemySpawned", this.gameObject.GetComponent<AudioSource>());
            //Debug.Log("unitsCounter " + unitsCounter + " and unitsToSpawn" + unitsToSpawn);
        }
        else if (unitsCounter > numberOfUnitsToSpawn)
        {
            // remove this spawner from SpawnersManager list: activeSpawners
            SpawnersManager.GetInstance().RemoveFromList(this.gameObject);
            //Debug.Log("spawner removed from list: " + this.gameObject.name);

            Destroy(this.gameObject, 5f);
        }


    }
    public GameObject GetUnitToSpawn() {
        return unitToSpawn;
    }
    public void SetUnitToSpawn (GameObject unit)
    {
        unitToSpawn = unit;

    }

}





