using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float speed = 0.0125f;
    public float rotSpeed = 0.1f;
    public Vector3 offset = new Vector3(0, 0, 0);

    private bool finish = false;

    // Start is called before the first frame update
    void Start()
    {
        //attached = transform.parent.gameObject;
    }

    private void FixedUpdate()
    {
        if (!finish)
        {
            Vector3 desired = target.position + offset;
            Vector3 smooth = Vector3.Lerp(transform.position, desired, speed);
            transform.position = smooth;

            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotSpeed);

        }
    
    }

    public void LookObject()
    {
        finish = true;

        transform.parent = null;
        transform.position = new Vector3(target.parent.position.x + 7f, target.parent.position.y + 6f, target.parent.position.z);
        transform.rotation = Quaternion.Euler(new Vector3(30, -90, 0));
    
    }
}
