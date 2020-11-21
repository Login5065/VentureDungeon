using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PARALAX_DOWN : MonoBehaviour
{
    private float lenght,starpos;
    public GameObject cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        starpos = transform.position.y;
        lenght = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.y * (1 - parallaxEffect));
        float distance = (cam.transform.position.y * parallaxEffect);
        transform.position = new Vector3(transform.position.x, starpos + distance, transform.position.z);

        if (temp > starpos + lenght) starpos += lenght;
        if (temp < starpos - lenght) starpos -= lenght;
    }
}
