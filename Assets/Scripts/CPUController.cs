using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUController : MonoBehaviour
{
    public Camera myCamera;
    public float speed;
    private float maxSpeed;

    private GameController gc;
    private Animator animator;

    private bool init;
    private bool finish;

    private Vector3 initialPosition;
    private List<GameObject> points = new List<GameObject>();
    private int currentPoint;
    private int count;

    private float raycastRange = 5f;
    
    void Initialize()
    {
        count = 3;

        init = false;
        finish = false;

        initialPosition = transform.position;
        maxSpeed = speed;

        currentPoint = 0;
        points = new List<GameObject>();
        foreach (Transform child in GameObject.Find("Points").transform)
        {
            points.Add(child.gameObject);
        }

        animator = gameObject.GetComponent<Animator>();
        gc = FindObjectOfType<GameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        Invoke("Countdown", 1f);
        transform.LookAt(points[currentPoint].transform, Vector3.zero);
    }


    // Update is called once per frame
    void Update()
    {
        if (!finish && init)
        {
            animator.SetFloat("Speed", speed);
            transform.position = new Vector3(transform.position.x + myCamera.transform.forward.x * speed * Time.deltaTime,
                transform.position.y,
                transform.position.z + myCamera.transform.forward.z * speed * Time.deltaTime);
        }
        
        float distance = Vector3.Distance(transform.position, points[currentPoint].transform.position);

        Vector3 prevPos = initialPosition;
        if (currentPoint > 0) prevPos = points[currentPoint - 1].transform.position;

        float initialDistance = Vector3.Distance(prevPos, points[currentPoint].transform.position);
        float percentageDone = 1 - (distance / initialDistance);
        float sizePoint = Screen.width / (points.Count);
        
        if (Vector3.Distance(transform.position, points[currentPoint].transform.position) < 1f && currentPoint < points.Count - 1)
        {
            currentPoint++;
            transform.LookAt(points[currentPoint].transform, Vector3.zero);
        }


        // RAYCAST. Stop and resume the run if there's an object forward
        RaycastHit hit;
        if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, raycastRange))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Damage")
            {
                speed = 0;
            } 
        } else
        {
            speed = maxSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Damage")
        {
            transform.position = initialPosition;
            currentPoint = 0;
            transform.LookAt(points[currentPoint].transform, Vector3.zero);
        }
        else if (other.tag == "Finish")
        {
            finish = true;
            
            if (!gc.GetWin())
            {
                if (gc.GetPosition() <= 1) // WIN!
                {
                    animator.SetBool("Dance", true);
                } else
                {
                    animator.SetBool("Defeat", true);
                }
                gc.AddPosition();
            } else
            {
                animator.SetBool("Defeat", true);
            }

            speed = 0;
        }
    }

    void Countdown()
    {
        if (count > 1f)
        {
            count -= 1;
            Invoke("Countdown", 1f);
        }
        else if (count >= 0)
        {
            count -= 1;
            init = true;
            Invoke("Countdown", 0.5f);
        }
    }
}
