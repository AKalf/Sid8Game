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
        source.pitch = 1;

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
                    theClip = Resources.Load<AudioClip>("Sounds/PickUpsSounds/" + "HealthPickUpSound");
                    PlayTheSound(theClip, source);
                    break;
                case "Mana":
                    theClip = Resources.Load<AudioClip>("Sounds/PickUpsSounds/" + "ManaPickUpSound");
                    PlayTheSound(theClip, source);
                    break;
            }
        }
        else if (message.EndsWith("Pl"))
        {
            string startsWith = message.Substring(0, message.Length - 2);
            float rndP = 0.0f;
            switch (startsWith)
            {
                case "OnHit":
                    rndP = Random.Range(source.pitch - 0.20f, source.pitch + 0.15f);
                    if (rndP <= 0.1f)
                    {
                        rndP *= 2.5f;
                    }
                    source.pitch = rndP;
                    rndP = Random.Range(1, 4);
                    theClip = Resources.Load<AudioClip>("Sounds/PlayerSounds/" + ("OnHit " + rndP));
                    PlayTheSound(theClip, source);
                    break;
                case "OnHitL":
                    rndP = Random.Range(source.pitch - 0.20f, source.pitch + 0.15f);
                    if (rndP <= 0.1f)
                    {
                        rndP *= 2.5f;
                    }
                    source.pitch = rndP;
                    rndP = Random.Range(1, 3);
                    theClip = Resources.Load<AudioClip>("Sounds/PlayerSounds/" + ("OnHitLong " + rndP));
                    PlayTheSound(theClip, source);
                    break;
                case "Jump":
                    rndP = Random.Range(source.pitch - 0.18f, source.pitch + 0.15f);
                    if (rndP <= 0.1f)
                    {
                        rndP *= 2.5f;
                    }
                    source.pitch = rndP;
                    rndP = Random.Range(1, 3);
                    theClip = Resources.Load<AudioClip>("Sounds/PlayerSounds/" + ("Jump " + rndP));
                    PlayTheSound(theClip, source);
                    break;
                case "GainHp":
                    rndP = Random.Range(source.pitch - 0.18f, source.pitch + 0.14f);
                    if (rndP <= 0.1f)
                    {
                        rndP *= 2.5f;
                    }
                    rndP = Random.Range(1, 2);
                    source.pitch = rndP;
                    theClip = Resources.Load<AudioClip>("Sounds/PlayerSounds/" + ("GainHp " + rndP));
                    PlayTheSound(theClip, source);
                    break;

            }
        }
        else if (message.EndsWith("Skel"))
        {
            string startsWith = message.Substring(0, message.Length - 4);
            float rndP = 0.0f;
            switch (startsWith)
            {
                case "Fstep":
                    rndP = Random.Range(source.pitch - 0.26f, source.pitch + 0.18f);
                    if (rndP <= 0.1f)
                    {
                        rndP *= 2.5f;
                    }
                    source.pitch = rndP;
                    // source.volume = Random.Range(source.volume - 1.25f, source.volume + 1.05f);
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/SkeletonSounds/" + "SkeletonFootStep");
                    PlayTheSound(theClip, source);
                    break;
                case "At":
                    source.pitch = Random.Range(source.pitch - 0.18f, source.pitch + 0.16f);
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/SkeletonSounds/" + "SwordSlash");
                    PlayTheSound(theClip, source);
                    break;
                case "Death":
                    int rnd = Random.Range(0, 100);
                    if (rnd <= 50)
                    {
                        theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/SkeletonSounds/" + "SkelDeath0");
                    }
                    else
                    {
                        theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/SkeletonSounds/" + "SkelDeath1");
                    }
                    PlayTheSound(theClip, source);
                    break;
                case "OnHit":
                    rndP = Random.Range(source.pitch - 0.25f, source.pitch + 0.25f);
                    if (rndP <= 0.3f)
                    {
                        rndP *= 2.5f;
                    }
                    source.pitch = rndP;
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/SkeletonSounds/" + "SkelOnHit");
                    PlayTheSound(theClip, source);
                    break;
            }
        }
        else if (message.EndsWith("FDeam"))
        {
            string startsWith = message.Substring(0, message.Length - 5);
            float rndP = 0.0f;
            switch (startsWith)
            {
                case "At":
                    source.pitch = Random.Range(source.pitch - 0.18f, source.pitch + 0.16f);
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/FireDeamonSounds/" + "FDeamAttack");
                    PlayTheSound(theClip, source);
                    break;
                case "OnHit":
                    rndP = Random.Range(source.pitch - 0.12f, source.pitch + 0.18f);
                    if (rndP <= 0.3f)
                    {
                        rndP *= 2.5f;
                    }
                    source.pitch = rndP;
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/FireDeamonSounds/" + "FDeamOnHit");
                    PlayTheSound(theClip, source);
                    break;
                case "Death":
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/FireDeamonSounds/" + "FDeamDeath");
                    PlayTheSound(theClip, source);
                    break;
                case "Spawn":
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/FireDeamonSounds/" + "FDeamSpawn");
                    PlayTheSound(theClip, source);
                    break;
            }
        }
        else if (message.EndsWith("Golem"))
        {
            string startsWith = message.Substring(0, message.Length - 5);
            float rndP = 0.0f;
            switch (startsWith)
            {
                case "At":
                    source.pitch = Random.Range(source.pitch - 0.22f, source.pitch + 0.12f);
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/GolemSounds/" + "GolemAttack");
                    PlayTheSound(theClip, source);
                    break;
                case "SpSkill":
                    source.pitch = Random.Range(source.pitch - 0.22f, source.pitch + 0.12f);
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/GolemSounds/" + "GolemSkill");
                    PlayTheSound(theClip, source);
                    break;
                case "Death":
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/GolemSounds/" + "GolemDeath");
                    PlayTheSound(theClip, source);
                    break;
                case "FtSp":
                    theClip = Resources.Load<AudioClip>("Sounds/EnemySounds/GolemSounds/" + "GolemFtSp");
                    PlayTheSound(theClip, source);
                    break;
            }
        }
        else
        {

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
