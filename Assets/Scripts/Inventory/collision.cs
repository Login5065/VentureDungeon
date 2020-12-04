using Dungeon.Creatures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class collision : MonoBehaviour
{
    private bool hasEntered;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("player") && !hasEntered)
        {
            hasEntered = true;
          /*  heartscript.livesLost += 1;
            Debug.Log(heartscript.livesLost);*/
        }
    }
}
