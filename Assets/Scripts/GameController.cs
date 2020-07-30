using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int position; // Race position
    private bool win;
    private bool finish;

    private List<GameObject> points;
    private int currentPoint;
    private Vector3 initialPosition;

    private Image progressBar;

    private float maxSpeed;
    private int count;
    private bool init;

    private Animator animator;
    private float speed;

    private ButtonManager bm;

    void Initialize()
    {
        finish = false;
        win = false;
        init = false;

        maxSpeed = 5.0f;
        speed = 0.0f;
        count = 3;
        position = 1;
        currentPoint = 0;

        initialPosition = transform.position;
        points = new List<GameObject>();
        foreach (Transform child in GameObject.Find("Points").transform)
        {
            points.Add(child.gameObject);
        }

        bm = FindObjectOfType<ButtonManager>();
        progressBar = GameObject.Find("ProgressBar").GetComponent<Image>();
        animator = gameObject.GetComponent<Animator>();
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
        speed = Input.GetAxis("Vertical");
        animator.SetFloat("Speed", speed);

        if (!finish && init && speed > 0)
        {
            transform.position = new Vector3(transform.position.x + transform.forward.x * maxSpeed * Time.deltaTime,
                transform.position.y,
                transform.position.z + transform.forward.z * maxSpeed * Time.deltaTime);
        }
        

        // Next point
        float distance = Vector3.Distance(transform.position, points[currentPoint].transform.position);

        Vector3 prevPos = initialPosition;
        if (currentPoint > 0) prevPos = points[currentPoint - 1].transform.position;

        float initialDistance = Vector3.Distance(prevPos, points[currentPoint].transform.position);
        float percentageDone = 1 - (distance / initialDistance);
        float sizePoint = Screen.width / (points.Count);
        progressBar.GetComponent<RectTransform>().offsetMax = new Vector2(-Screen.width + percentageDone * sizePoint + (currentPoint * sizePoint), 100);
        
        if (Vector3.Distance(transform.position, points[currentPoint].transform.position) < 2f && currentPoint < points.Count - 1)
        {
            currentPoint++;
            transform.LookAt(points[currentPoint].transform, Vector3.zero);
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
            bm.SetMenu(true);
            
            if (GetPosition() > 1)
            {
                animator.SetBool("Defeat", true);
                GameObject.Find("VictoryText").GetComponent<Text>().text = "FAIURE...";
            } else
            {
                animator.SetBool("Dance", true);
                GameObject.Find("VictoryText").GetComponent<Text>().text = "YOU WON!";
            }

            win = (position <= 1);
            
            CameraController cam = Camera.main.GetComponent<CameraController>();
            cam.LookObject();
        }
    }

    private void Countdown()
    {
        if (count > 1)
        {
            count -= 1;
            GameObject.Find("Countdown").GetComponent<Text>().text = count.ToString();
            Invoke("Countdown", 1f);
        } else if (count >= 0)
        {
            count -= 1;
            init = true;
            if (count == 0) GameObject.Find("Countdown").GetComponent<Text>().text = "GO!";
            Invoke("Countdown", 0.5f);
        } else
        {
            GameObject.Find("Countdown").SetActive(false);
        }
    }

    public bool GetWin()
    {
        return win;
    }

    public void AddPosition()
    {
        position++;
    }

    public int GetPosition()
    {
        return position;
    }

}
