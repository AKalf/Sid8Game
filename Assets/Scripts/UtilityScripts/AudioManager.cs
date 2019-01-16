using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField]
    AudioSource audioSourceFootSteps = null;
    bool playLoopFootSteps = false;
    private static AudioManager inst = null;
    private AudioClip theClip = null;

    void Awake()
    {
        inst = this;
    }
    private void Start()
    {
        // Listen for messages
        MessageDispatch.GetInstance().AddMessageHandler(MessageReceived);
    }
    public static AudioManager GetInstance()
    {
        return inst;
    }
    public void PlayTheSound(AudioClip sound, AudioSource source)
    {
        if (source != null)
        {
#if UNITY_EDITOR
            Debug.Log("Sound played: " + sound);
#endif
            source.clip = sound;
            source.Play();
        }

    }

    public void MessageReceived(string message, AudioSource source)
    {

        if (message.EndsWith("Up"))
        {
            string startsWith = message.Substring(0, message.Length - 6);
            switch (startsWith)
            {
                case "Exp":
                    theClip = Resources.Load<AudioClip>("Sounds/PickUpsSounds/" + "ExpPickUpSound");
                    PlayTheSound(theClip, source);
                    break;
                case "Hp":
                    Debug.Log("enemy died sound");
                    theClip = Resources.Load<AudioClip>("Sounds/PickUpsSounds/" + "HealthPickUpSound");
                    PlayTheSound(theClip, source);
                    break;
                case "Mana":
                    theClip = Resources.Load<AudioClip>("Sounds/PickUpsSounds/" + "ManaPickUpSound");
                    PlayTheSound(theClip, source);
                    break;
            }
        }
        else if (message.EndsWith("Skel"))
        {
            string startsWith = message.Substring(0, message.Length - 4);
            switch (startsWith)
            {
                case "Fstep":
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/SkeletonSounds/" + "SkeletonFootStep (1)");
                    PlayTheSound(theClip, source);
                    break;
                case "At":
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/SkeletonSounds/" + "SwordSlash");
                    PlayTheSound(theClip, source);
                    break;
                case "Death":
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/SkeletonSounds/" + "SkelDeath");
                    PlayTheSound(theClip, source);
                    break;
            }
        }
        else {
           
        }
        
       
    }

    public void OnAuidioEvent(string eventName)
    {
        throw new System.NotImplementedException();
    }
    public bool GetIfLoopingFsteps() {
        return playLoopFootSteps;
    }
    public void SetIfLoopingFsteps(bool b) {
        playLoopFootSteps = b;
        if (playLoopFootSteps)
        {
            if (!audioSourceFootSteps.isPlaying)
            {
                audioSourceFootSteps.loop = true;
                audioSourceFootSteps.Play();
            }
        }
        else {
            audioSourceFootSteps.Stop();
        }
    }

}
