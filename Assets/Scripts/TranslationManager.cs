using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationManager : MonoBehaviour
{
    public Vector3 speed;
    public Vector3 size;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= initialPosition.x + size.x / 2 || transform.position.x <= initialPosition.x - size.x / 2) speed.x = -speed.x;
        if (transform.position.y >= initialPosition.y + size.y / 2 || transform.position.y <= initialPosition.y - size.y / 2) speed.y = -speed.y;
        if (transform.position.z >= initialPosition.z + size.z / 2 || transform.position.z <= initialPosition.z - size.z / 2) speed.z = -speed.z;

        transform.Translate(speed * Time.deltaTime);
    }
}
