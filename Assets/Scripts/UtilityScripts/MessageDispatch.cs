using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageDispatch : MonoBehaviour
{

    private static MessageDispatch inst = null;

    // handles the message
    public delegate void AudioMessageHandler(string message, AudioSource source);
    private List<AudioMessageHandler> audioMessageHandlers;
    private Queue<AudioMessage> audioMessages = new Queue<AudioMessage>();


    public static MessageDispatch GetInstance()
    {
        return inst;
    }

    // a queue that holds messages to be done


    MessageDispatch()
    {
        audioMessageHandlers = new List<AudioMessageHandler>();
    }

    private void Awake()
    {
       
            inst = this;
       
    }

    private void Update()
    {

        // if audioMessages queue has message(s) for dispatch
        while (audioMessages.Count != 0)
        {
            // take the first message added to the queue 
            AudioMessage msg = audioMessages.Dequeue();

            // send the message to the handlers that are interested
            foreach (AudioMessageHandler messageHandler in audioMessageHandlers)
            {
                //Debug.Log(msg.message);
                messageHandler(msg.message, msg.source);
            }
            
        }
        
    }

    // sends an audio message with parameters the name of the sound that should be player and the audio source that the sound should be played from
    // method is used from other instances to tell that an audio event has occured
    public void SendAudioMessageForDispatch(string message, AudioSource source)
    {
        AudioMessage msg = new AudioMessage(); // creates a new audio message
        msg.message = message; // the name of the message (the sound to be played)
        msg.source = source; // the audio source that the sound should be played from
        audioMessages.Enqueue(msg); // add this message to queue for dispatch
    }
    

    // used from instances of other classes to tell that they are interested for messages of type audio
    public void AddMessageHandler(AudioMessageHandler messageHandler)
    {
        audioMessageHandlers.Add(messageHandler);
    }

    // used from instances of other classes to tell that they are no longer interested for messages of type audio
    public void RemoveMessageHandler(AudioMessageHandler messageHandler)
    {
        audioMessageHandlers.Remove(messageHandler);
    }

    // a class that represents an audio message
    private class AudioMessage
    {
        public string message;
        public AudioSource source;
    }

}
