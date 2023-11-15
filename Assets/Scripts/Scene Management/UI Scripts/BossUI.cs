using System.Collections;
using System.Collections.Generic;
using Proto.UI;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    private Slider healthSlider;
    private ShowHideUI showHideUI;
    private GameObject bossHealth;
    [SerializeField] bool showBossHealth = false;

    const string BOSSHEALTH_TEXT = "BossHealth";
    const string BOSSUI_TEXT = "BossUI";

    public void BossHealthStartUp() {
        if (showHideUI == null) {
            showHideUI = GameObject.Find(BOSSUI_TEXT).GetComponent<ShowHideUI>();
            showHideUI.SetVisibility(true);
        }
        if (healthSlider == null) {
            healthSlider = GameObject.Find(BOSSHEALTH_TEXT).GetComponent<Slider>();
        }
        
        UpdateBossHealthBar(10,10); //Start up filler health until it gets updated by enemyHealth.
    }

    public void UpdateBossHealthBar(int currentValue, int maxValue) {
        if (healthSlider == null) {
            healthSlider = GameObject.Find(BOSSHEALTH_TEXT).GetComponent<Slider>();
        }
        
        healthSlider.maxValue = maxValue;
        healthSlider.value = currentValue;
    }

    public void ToggleBossHealthBar() {
        showHideUI.Toggle();
    }

    public void SetActiveState(bool state) {
        showHideUI.SetVisibility(state);
    }

}
