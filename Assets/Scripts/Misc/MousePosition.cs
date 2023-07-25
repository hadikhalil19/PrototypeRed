using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    public Vector2 MouseDirectionSampled = new Vector2 (0,0);

    private float timer = 0f;
    private float checkInterval = 0.1f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= checkInterval)
        {
            timer = 0f;
            MousePosSampled();
        }
    }

    private void MousePosSampled()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        MouseDirectionSampled = (mousePos - playerScreenPoint).normalized;
    }
}
