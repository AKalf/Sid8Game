using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersManager : MonoBehaviour
{

    /*
     * Singleton
     * Checks which spawners are currently active
     * Spawners add themselfs to activeSpawners list when they start spawning
     * Spawners remove themselfs from activeSpawners list when they have spawened all the units that they must spawn
     * Sends a "SpawnerFinished" event message when active spawners finish spawning
    */
    private static SpawnersManager instance = null;
    private List<GameObject> activeSpawners = new List<GameObject>();

    public delegate void SpawnersMessageHandler(string message);
    private List<SpawnersMessageHandler> spanwnersMessageHandlers = new List<SpawnersMessageHandler>();
    private Queue<SpawnerMessage> spawnerMessages = new Queue<SpawnerMessage>();

    // Use this for initialization
    private SpawnersManager() { }
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // if messages queue has message(s) for dispatch
        while (spawnerMessages.Count != 0)
        {

            // take the first message added to the queue 
            SpawnerMessage msg = spawnerMessages.Dequeue();
            try
            {
                // send the message to the handlers that are interested
                foreach (SpawnersMessageHandler messageHandler in spanwnersMessageHandlers)
                {
                    messageHandler(msg.message);
                }
            }
            catch
            {
                spawnerMessages.Enqueue(msg);
            }
        }
    }

    // sends a message
    // method is used from other instances to tell that an audio event has occured
    public void SendSpawnerMessageForDispatch(string message)
    {
        SpawnerMessage msg = new SpawnerMessage(); // creates a new spanwer message
        msg.message = message; // the message
        spawnerMessages.Enqueue(msg); // add this message to queue for dispatch
    }

    // used from instances of other classes to tell that they are interested for messages of type audio
    public void AddMessageHandler(SpawnersMessageHandler messageHandler)
    {
        spanwnersMessageHandlers.Add(messageHandler);
    }

    // used from instances of other classes to tell that they are no longer interested for messages of type audio
    public void RemoveMessageHandler(SpawnersMessageHandler messageHandler)
    {
        spanwnersMessageHandlers.Remove(messageHandler);
    }


    // get this instance by reference
    public static SpawnersManager GetInstance()
    {
        return instance;

    }
    // get all currently active spawners
    public List<GameObject> GetList()
    {
        return activeSpawners;
    }
    // add a spawner as currently active
    public void AddToList(GameObject spawner)
    {
        activeSpawners.Add(spawner);
    }
    // remove a spawner as currently active
    public void RemoveFromList(GameObject spawner)
    {
        if (activeSpawners.Contains(spawner))
        {
            activeSpawners.Remove(spawner);
        }
        if (activeSpawners.Count == 0)
        {
            SendSpawnerMessageForDispatch("SpClear");
        }

    }
    // get an active spawner from the list
    public GameObject GetItemFromList(int index)
    {
        return activeSpawners[index];
    }


}

class SpawnerMessage
{
    public string message;
}
