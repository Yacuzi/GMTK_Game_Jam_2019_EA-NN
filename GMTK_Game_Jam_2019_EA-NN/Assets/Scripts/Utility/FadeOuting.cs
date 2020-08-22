using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeOuting : MonoBehaviour
{
    TextMeshProUGUI myText;


    private void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        if (myText.color.a >= 0f)
        {
            Color myColor = myText.color;
            myColor.a -= Time.deltaTime / 2;

            myText.color = myColor;
        }
    }
}
