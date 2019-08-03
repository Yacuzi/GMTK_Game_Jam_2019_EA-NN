using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger : Resetables
{
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Character_Move>())
        {
            Debug.Log("Kill");
            collision.gameObject.GetComponent<Character_Move>().Kill();
        }
    }

    public override void Reset()
    {
        
    }
}
