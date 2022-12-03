using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI letter;
    // Update is called once per frame
    public float fadeoutSpeed = 0.4f;
    void FixedUpdate()
    {
        if (letter.color.a > 0.1f)
            letter.color = new Color(letter.color.r, letter.color.g, letter.color.b, letter.color.a - (fadeoutSpeed * Time.deltaTime));
    }
}
