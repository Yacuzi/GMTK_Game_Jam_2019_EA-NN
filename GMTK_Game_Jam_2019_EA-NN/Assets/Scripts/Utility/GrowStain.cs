using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowStain : MonoBehaviour
{
    public float growTime;
    public Vector3 sizeGrow;

    private Vector3 sizeIni;

    private float grow;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        sizeIni = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (grow <= 1f)
            grow += Time.deltaTime / growTime;

        transform.localScale = Vector3.Lerp(sizeIni, sizeGrow, grow);
    }
}
