using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider floatingSlider;

    public void UpdateFloatingHealthBar(int currentValue, int maxValue) {
        floatingSlider.maxValue = maxValue;
        floatingSlider.value = currentValue;
    }
    
    void Update()
    {
        
    }
}
