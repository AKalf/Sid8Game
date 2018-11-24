using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    //[SerializeField]
    //Image dashCooldown = null; // the dash icon

    //[SerializeField]
    //Image mainTextPanel = null; // the main text window of the game

    [SerializeField]
    Slider playerHealthSlider = null; // player's health slider

    [SerializeField]
    Slider playerManaSlider = null;

    [SerializeField]
    Slider playerExpSlider = null;

    [SerializeField]
    Text LevelText = null;
  
    //[SerializeField]
    //Image loosePanel = null; // loose window

    //[SerializeField]
    //Image PauseMenuPanel = null;

    private static GUIManager inst; // the only instance of the class
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
    #region LoosePanel
    //public void ShowLoosePanel()
    //{
    //    loosePanel.GetComponent<Image>().enabled = true;
    //    foreach (Transform child in loosePanel.transform)
    //    {
    //        child.gameObject.SetActive(true);
    //    }

    //}
    //public void HideLoosePanel()
    //{
    //    loosePanel.GetComponent<Image>().enabled = false;
    //    foreach (Transform child in loosePanel.transform)
    //    {
    //        child.gameObject.SetActive(false);
    //    }
    //}
    #endregion
    #region DashCooldown
    //// inform cooldown u.i icon for how much time has passed
    //public void InformDashCooldown(float time)
    //{
    //    dashCooldown.fillAmount = time;
    //}
    //// inform maintext for what should appear
    //public void InformMainText(string text)
    //{
    //    EnableMainText();
    //    mainTextPanel.GetComponentInChildren<Text>().text = text;

    //}
    #endregion
    // infrom hp slider for player's current health
    #region StatsSliders
    public void InformPlayerHPSlider(int amount)
    {
        playerHealthSlider.value = amount;
    }
    public void SetPlayerMaxHealth(int value) {
        playerHealthSlider.maxValue = value;
    }
    public void InformPlayerManaSlider(int amount) {
        playerManaSlider.value = Mathf.Lerp(playerManaSlider.value, amount, Time.deltaTime * Mathf.Abs(playerManaSlider.value - amount));
    }
    public void SetMaxMana(int value) {
        playerManaSlider.maxValue = value;
    }
    public void InformPlayerExpSlider(int amount) {
        playerExpSlider.value = Mathf.Lerp(playerExpSlider.value, amount, Time.deltaTime * Mathf.Abs(playerExpSlider.value - amount));
    }
    public void SetMaxExp(int value) {
        playerExpSlider.maxValue = value;
    }
    public void InformLevelText(string value) {
        LevelText.text = value;
    }
    #endregion

    // make maintext appear
    //public void EnableMainText()
    //{

    //    mainTextPanel.enabled = true;

    //}
    //// make maintext dissapear
    //public void DisableMainText()
    //{
    //    mainTextPanel.enabled = false;
    //    mainTextPanel.GetComponentInChildren<Text>().text = "";

    //}
    //public void ShowPauseMenu()
    //{
    //    PauseMenuPanel.gameObject.SetActive(true);
    //}
    //public void HidePauseMenu()
    //{
    //    PauseMenuPanel.gameObject.SetActive(false);
    //}

}

