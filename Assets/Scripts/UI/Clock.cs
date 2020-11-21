using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    IEnumerator UpdateClock()
    {
        Text text = GetComponent<Text>();
        while(true)
        {
            var dateTime = System.DateTime.Now;
            text.text = $"{dateTime.Hour:00}:{dateTime.Minute:00}";
            yield return new WaitForSeconds(0.2f); 
        }
    }
    
    void Start()
    {
        StartCoroutine(UpdateClock());
    }
}
