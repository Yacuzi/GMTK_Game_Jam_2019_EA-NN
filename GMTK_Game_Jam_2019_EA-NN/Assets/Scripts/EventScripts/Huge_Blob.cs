using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huge_Blob : MonoBehaviour
{
    public float shakeTime;
    public float blobShakeCor;

    private bool shake;
    private float shakeTimer;
    private Vector3 blobIni;
    private bool playOnce;

    private void shakyBlob()
    {
        if (shake)
        {
            shakeTimer += Time.deltaTime;
            transform.position = blobIni + new Vector3(Random.value * blobShakeCor, Random.value * blobShakeCor, 0);

            if (shakeTimer >= shakeTime)
            {
                GetComponent<Animator>().SetBool("Suffer", false);
                shake = false;
                shakeTimer = 0;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        blobIni = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((int)Time.time % 5 == 2 && !playOnce)
        {
            GetComponent<AudioSource>().Play();
            playOnce = true;
            shake = true;
            GetComponent<Animator>().SetBool("Suffer", true);
        }

        if ((int)Time.time % 5 == 4)
        {
            playOnce = false;
        }

        shakyBlob();
    }
}
