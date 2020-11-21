using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PARALAX : MonoBehaviour
{
private float lenght,starpos;
public GameObject cam;
public float parallaxEffect;


    // Start is called before the first frame update
    void Start()
    {
        starpos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;


    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float distance = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(starpos + distance,transform.position.y,transform.position.z);

        if (temp > starpos + lenght) starpos += lenght;
        if (temp < starpos - lenght) starpos -= lenght;
    }
}
