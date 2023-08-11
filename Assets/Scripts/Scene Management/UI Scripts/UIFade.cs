using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image fadeToBlack;
    [SerializeField] private float fadeSpeed = 1f;

    private IEnumerator fadeRoutine;

    public void FadeIn() {
        if(fadeRoutine != null) {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(1);
        StartCoroutine(fadeRoutine);
    }



    public void FadeOut() {
        if(fadeRoutine != null) {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(0);
        StartCoroutine(fadeRoutine);
    }



    private IEnumerator FadeRoutine(float targetAlpha) {
        while (!Mathf.Approximately(fadeToBlack.color.a, targetAlpha)) {
            float alpha = Mathf.MoveTowards(fadeToBlack.color.a, targetAlpha, fadeSpeed*Time.deltaTime);
            fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, alpha);
            yield return null;
        }
    }
}
