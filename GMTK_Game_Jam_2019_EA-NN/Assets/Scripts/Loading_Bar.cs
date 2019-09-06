using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_Bar : MonoBehaviour
{
    private Vector3 scale_ini;

    void Change_Scale()
    {
        float theScale = Mathf.Clamp((Time_Lord.The_Timer * scale_ini.x) + 0.3f, 0f, 7f);

        if (Time_Lord.Acting)
            transform.localScale = new Vector3(theScale, scale_ini.y, scale_ini.z);
    }

    void Change_Alpha()
    {
        if (GetComponent<SpriteRenderer>().color.a >= 0f)
        {
            Color myColor = GetComponent<SpriteRenderer>().color;
            myColor.a = 1 - Time_Lord.The_Timer;

            GetComponent<SpriteRenderer>().color = myColor;
        }
    }

    void ResetScale()
    {
        transform.localScale = Vector3.zero;
    }

    void ResetAlpha()
    {
        Color myColor = GetComponent<SpriteRenderer>().color;
        myColor.a = 1;

        GetComponent<SpriteRenderer>().color = myColor;
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
        scale_ini = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time_Lord.Acting)
        {
            Change_Scale();
        }

        if (Time_Lord.Preparing)
        {
            Change_Alpha();

            if (Time_Lord.The_Timer >= 1f)
            {
                ResetScale();
                ResetAlpha();
            }
        }
    }
}