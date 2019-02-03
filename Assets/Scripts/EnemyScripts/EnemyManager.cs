using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /*
    * Singleton
    * Checks which enemies are currently alive
    * Spawners add the enemy they just spawned to aliveEnemies list
    * Enemies remove themselfs from aliveEnemies list when they die
    * Sends an "EnemyClear" event message when all enemies in the room have died
    * WARNING: EnemyManager does NOT contain enemies that have been manualy added to the scene. It contains ONLY enemies that have been spawned by a spawner
   */
    private static EnemyManager instance = null;
    private List<GameObject> activeEnemies = new List<GameObject>();

    public delegate void EnemyMessageHandler(string message);
    private List<EnemyMessageHandler> enemiesMessageHandlers = new List<EnemyMessageHandler>();
    private Queue<EnemyMessage> enemiesMessages = new Queue<EnemyMessage>();

    int numberOfSkeletons = 0;
    // Use this for initialization
    private EnemyManager() { }
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {  // if messages queue has message(s) for dispatch
        while (enemiesMessages.Count != 0)
        {

            // take the first message added to the queue 
            EnemyMessage msg = enemiesMessages.Dequeue();
            try
            {
                if (msg.message == "FstepSkel" && AudioManager.GetInstance().GetIfLoopingFsteps())
                {

                }
                else {
                    // send the message to the handlers that are interested
                    foreach (EnemyMessageHandler messageHandler in enemiesMessageHandlers)
                    {
                        //Debug.Log(msg.message);
                        messageHandler(msg.message);
                    }
                }
               
            }
            catch
            {
                enemiesMessages.Enqueue(msg);
            }
        }
    }
    // get instance by reference
    public static EnemyManager GetInstance()
    {
        return instance;

    }

    // sends a message
    // method is used from other instances to tell that an audio event has occured
    public void SendEnemyMessageForDispatch(string message)
    {
        EnemyMessage msg = new EnemyMessage(); // creates a new spanwer message
        msg.message = message; // the message
        enemiesMessages.Enqueue(msg); // add this message to queue for dispatch
    }

    // used from instances of other classes to tell that they are interested for messages of type audio
    public void AddMessageHandler(EnemyMessageHandler messageHandler)
    {
        enemiesMessageHandlers.Add(messageHandler);
    }

    // used from instances of other classes to tell that they are no longer interested for messages of type audio
    public void RemoveMessageHandler(EnemyMessageHandler messageHandler)
    {
        enemiesMessageHandlers.Remove(messageHandler);
    }

    // get all alive enemies
    public List<GameObject> GetActiveEnemies()
    {
        return activeEnemies;
    }
    // add an enemy to aliveEnemies list
    public void AddToList(GameObject enemy)
    {
        if (enemy.tag.StartsWith("Skel")) {
            numberOfSkeletons++;   
        }
        activeEnemies.Add(enemy);
    }
    // remove an enemy from aliveEnemies list
    public void RemoveFromList(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            if (enemy.tag.StartsWith("Skel"))
            {
                numberOfSkeletons--;
                if (numberOfSkeletons <= 3)
                {
                    AudioManager.GetInstance().SetIfLoopingFsteps(false);
                }
            }
            activeEnemies.Remove(enemy);
            if (activeEnemies.Count == 0)
            {
                SendEnemyMessageForDispatch("EnClear");
            }
        }


    }
    // get an alive enemy from the list
    public GameObject GetItemFromList(int index)
    {
        return activeEnemies[index];
    }

}
class EnemyMessage
{
    public string message;
}

