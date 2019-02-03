using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    #region HelathVariables
    [SerializeField]
    int maxHealth = 10;
    [SerializeField]
    int startingHealth = 10;
    [SerializeField]
    int currentHealth = 0;
    #endregion
    #region ManaVariables
    [SerializeField]
    int maxMana = 100;
    [SerializeField]
    float manaRegain = 2.5f;

    float currentMana = 0;
    #endregion
    #region ExpVariables
    [SerializeField]
    int maxExp = 100;

    int currentExp = 0;
    int currentLevel = 0;
    #endregion

    // Use this for initialization
    void Start () {
        currentHealth = startingHealth;
        GUIManager.GetInstance().SetPlayerMaxHealth(maxHealth);
        GUIManager.GetInstance().InformPlayerHPSlider(currentHealth);

        currentMana = maxMana;
        GUIManager.GetInstance().SetMaxMana(maxMana);
        GUIManager.GetInstance().InformPlayerManaSlider((int)currentMana);

        GUIManager.GetInstance().SetMaxExp(maxExp);
        GUIManager.GetInstance().InformPlayerManaSlider(currentExp);
    }
	
	// Update is called once per frame
	void Update () {
        if (currentMana < maxMana)
        {
            currentMana += manaRegain * Time.deltaTime;
        }
        GUIManager.GetInstance().InformPlayerExpSlider(currentExp);
        GUIManager.GetInstance().InformPlayerManaSlider((int)currentMana);
    }
    // Decrease player's health, checks if he dies and informs the U.I element for health
    public void IncDecHealth(int amount)
    {
        currentHealth += amount;
        GUIManager.GetInstance().InformPlayerHPSlider(currentHealth);
        if (amount < 0)
        {
            if (currentHealth <= (maxHealth / 100) * 15)
            {
                MessageDispatch.GetInstance().SendAudioMessageForDispatch("OnHitLPl", MyGameManager.GetInstance().GetPlayer().GetComponent<AudioSource>());
            }
            else
            {
                MessageDispatch.GetInstance().SendAudioMessageForDispatch("OnHitPl", MyGameManager.GetInstance().GetPlayer().GetComponent<AudioSource>());
            }
        }
        else {
            MessageDispatch.GetInstance().SendAudioMessageForDispatch("GainHpPl", MyGameManager.GetInstance().GetPlayer().GetComponent<AudioSource>());
        }
        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead");
        }

    }

    #region HealthFunc
    public int GetMaxHP()
    {
        return maxHealth;
    }
    #endregion
    #region ManaFunc
    public float GetCurrentMana()
    {
        return currentMana;
    }
    public void IncDecMana(float amount)
    {
        currentMana += amount;
        

    }
    public float GetMaxMana()
    {
        return maxMana;
    }
    public void IncDecMaxMana(int amount) {
        maxMana += amount;
        GUIManager.GetInstance().SetMaxMana(maxMana);
    }
    public float GetManaRegain()
    {
        return manaRegain;
    }
    public void IncDecManaRegain(float amount)
    {
        manaRegain += amount;
    }
    #endregion
    #region ExpFunc
    public int GetCurrentExp() {
        return currentExp;
    }
    public void IncDecExp(int amount) {
        currentExp += amount;
        if (currentExp >= maxExp) {
            currentLevel++;
            GUIManager.GetInstance().InformLevelText(currentLevel.ToString());
            IncDecMaxExp(maxExp);
            IncDecExp(-currentExp);
            IncDecMana(maxMana - currentMana);
            IncDecHealth(maxHealth - currentHealth);

        }
        
    }
    public void IncDecMaxExp(int amount) {
        maxExp += amount;
        GUIManager.GetInstance().SetMaxExp(maxExp);
    }
    public int GetMaxExp() {
        return maxExp;
    }
    #endregion
}
