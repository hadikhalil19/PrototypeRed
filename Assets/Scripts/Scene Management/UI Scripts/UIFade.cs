using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image fadeToBlack;
    [SerializeField] private float fadeSpeed = 1f;

    [SerializeField] private GameObject fadeObject;

    private IEnumerator fadeRoutine;

    public void FadeIn(float fadeSpeedMult) {
        if(fadeRoutine != null) {
            StopCoroutine(fadeRoutine);
        }

        fadeObject.SetActive(true);

        fadeRoutine = FadeRoutine(1, fadeSpeedMult);
        StartCoroutine(fadeRoutine);
    }



    public void FadeOut(float fadeSpeedMult) {
        if(fadeRoutine != null) {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(0, fadeSpeedMult);
        StartCoroutine(fadeRoutine);

    }



    private IEnumerator FadeRoutine(float targetAlpha, float fadeSpeedMult) {
        while (!Mathf.Approximately(fadeToBlack.color.a, targetAlpha)) {
            float alpha = Mathf.MoveTowards(fadeToBlack.color.a, targetAlpha, fadeSpeed*fadeSpeedMult*Time.deltaTime);
            fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, alpha);
            yield return null;
        }
        if (targetAlpha == 0) {
                fadeObject.SetActive(false);
        } 
    }
}
