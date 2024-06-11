using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PropLight : MonoBehaviour
{
    [SerializeField]
    Light2D propLight;

    private void Update()
    {
        float alpha = Mathf.PingPong(Time.time * 0.2f, 0.5f) + 0.3f;
        Color newColor = propLight.color;
        newColor.a = alpha;
        propLight.color = newColor;
    }
}
