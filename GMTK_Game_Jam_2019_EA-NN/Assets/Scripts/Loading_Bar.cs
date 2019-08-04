using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_Bar : MonoBehaviour
{
    private Vector3 scale_ini;

    void Change_Scale()
    {
        transform.localScale = new Vector3(Time_Lord.The_Timer * scale_ini.x, scale_ini.y, scale_ini.z);
    }

    private void Start()
    {
        scale_ini = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Change_Scale();        
    }
}