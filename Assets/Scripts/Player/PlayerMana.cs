using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proto.Saving;

public class PlayerMana : Singleton<PlayerMana>, ISaveable
{
   public int CurrentMana { get; private set; }
    [SerializeField] private float timeBetweenManaRefresh = 0.2F;

    private Slider healthSlider;
    private int startingMana = 100;
    private int maxMana;
    const string MANA_SLIDER_TEXT = "Mana Slider";

    protected override void Awake() {
        base.Awake();

        maxMana = startingMana;
        CurrentMana = startingMana;
    }

    private void Start() {
        UpdateManaSlider();
    }

    public void UseMana(int ManaCost) {
        CurrentMana = CurrentMana - ManaCost;
        UpdateManaSlider();
    }

    public void RefreshMana() {
        if (CurrentMana < maxMana) {
            CurrentMana++;
        }
        UpdateManaSlider();
    }

    private IEnumerator RefreshManaRoutine() {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenManaRefresh);
            RefreshMana();
        }
    }

    public void ManaGlobeRefresh(int manaGlobeValue) {
        if (CurrentMana < maxMana - manaGlobeValue) {
            CurrentMana = CurrentMana + manaGlobeValue;
        } else {
            CurrentMana = maxMana;
        }
        UpdateManaSlider();
    }

    private void UpdateManaSlider() {
        if (healthSlider == null) {
            healthSlider = GameObject.Find(MANA_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxMana;
        healthSlider.value = CurrentMana;
        
        if (CurrentMana < maxMana) {
            StopAllCoroutines();
            StartCoroutine(RefreshManaRoutine());
        }

    }

    public object CaptureState()
        {
            return CurrentMana;
        }

        public void RestoreState(object state)
        {
            CurrentMana = (int)state;
            UpdateManaSlider();
        }


}
