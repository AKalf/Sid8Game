using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public int playersMaxHP = 10;
    public int playersStartingHealth = 10;

    public int playersCurrentHP = 0;


    private PlayerHealth()
    {

    }
    void Awake()
    {


    }

    void Start()
    {
        playersCurrentHP = playersStartingHealth;
        GUIManager.GetInstance().InformPlayerHPSlider(playersStartingHealth);
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Decrease player's health, checks if he dies and informs the U.I element for health
    public void LooseHP(int dmg)
    {
        playersCurrentHP = playersCurrentHP - dmg;
        GUIManager.GetInstance().InformPlayerHPSlider(playersCurrentHP);
        if (playersCurrentHP <= 0)
        {
            PlayerDeath();

        }

    }

    // Things to do before you die
    void PlayerDeath()
    {
        //GlobalGameStateMachine.GetInstance().ChangeCurrentGameState(LooseGameState.GetInstance());

    }
    public int GetMaxHP()
    {
        return playersMaxHP;
    }

}
