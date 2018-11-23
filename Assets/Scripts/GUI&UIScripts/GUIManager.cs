using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour
{

    public Image dashCooldown = null; // the dash icon
    public Image mainTextPanel = null; // the main text window of the game
    public Slider playerHealthSlider = null; // player's health slider
    public Image loosePanel = null; // loose window
    public Image PauseMenuPanel = null;

    private static GUIManager inst; // the only instance of the class
    private bool slideInitialized = false; // tells if player's health slider, max value has been set
                                           //Get the only instance of this class
    public static GUIManager GetInstance()
    {
        return inst;
    }

    void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        //DisableMainText();
        //HidePauseMenu();
        //GUIManager.GetInstance().InformDashCooldown(1);
    }

    private void Update()
    {

    }
    public void ShowLoosePanel()
    {
        loosePanel.GetComponent<Image>().enabled = true;
        foreach (Transform child in loosePanel.transform)
        {
            child.gameObject.SetActive(true);
        }

    }
    public void HideLoosePanel()
    {
        loosePanel.GetComponent<Image>().enabled = false;
        foreach (Transform child in loosePanel.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    // inform cooldown u.i icon for how much time has passed
    public void InformDashCooldown(float time)
    {
        dashCooldown.fillAmount = time;
    }
    // inform maintext for what should appear
    public void InformMainText(string text)
    {
        EnableMainText();
        mainTextPanel.GetComponentInChildren<Text>().text = text;

    }
    // infrom hp slider for player's current health
    public void InformPlayerHPSlider(int amount)
    {

        if (slideInitialized == false)
        {
            playerHealthSlider.maxValue = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerHealth>().GetMaxHP();
            slideInitialized = true;
        }
        playerHealthSlider.value = amount;
    }
    // make maintext appear
    public void EnableMainText()
    {

        mainTextPanel.enabled = true;

    }
    // make maintext dissapear
    public void DisableMainText()
    {
        mainTextPanel.enabled = false;
        mainTextPanel.GetComponentInChildren<Text>().text = "";

    }
    public void ShowPauseMenu()
    {
        PauseMenuPanel.gameObject.SetActive(true);
    }
    public void HidePauseMenu()
    {
        PauseMenuPanel.gameObject.SetActive(false);
    }

}

