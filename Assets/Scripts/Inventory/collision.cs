using UnityEngine;


public class Collision : MonoBehaviour
{
    private bool hasEntered;

    void OnCollisionEnter(UnityEngine.Collision other)
    {
        if (other.gameObject.CompareTag("player") && !hasEntered)
        {
            hasEntered = true;
        }
    }
}
