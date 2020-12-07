using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//for Text Objects

public class BabyController : MonoBehaviour {
    Rigidbody rb;
    public Transform[] waypoints;
    public bool chasing;
    public bool isDropped;
    public GameObject Cube;
    public Text statusText;
    int cur = 0;
    public float speed = .1f;

    int score;

    // Use this for initialization
    void Start ()
    {
        

        isDropped = false;
        chasing = true;
        statusText.text = "";
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isDropped == false)
        {
  

            //transform.position = Cube.GetComponent<PlayerController>().tpArea.transform.position;
        }

        if (isDropped && chasing)
        {
            if (transform.position != waypoints[cur].position)
            {
                Vector3 p = Vector3.MoveTowards(transform.position,
                                waypoints[cur].position,
                                speed);
                GetComponent<Rigidbody>().MovePosition(p);
            }
            else cur = (cur + 1) % waypoints.Length;
        }

        if(transform.position.y < -15)
        {
            StartCoroutine(Wait(5));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Abyss")
        {
            if(Cube.GetComponent<PlayerController>().score > 0)
            Cube.GetComponent<PlayerController>().score = Cube.GetComponent<PlayerController>().score - 250;



            StartCoroutine(Wait(5));
            
            print("Touched void");
        }

    }

    IEnumerator Wait(int time)
    {
        rb.velocity = new Vector3(0, 0, 0);
        statusText.text = "You cannot dispose of cubic flesh in the void.";
        Cube.GetComponent<PlayerController>().isHoldingChild = true;

        yield return new WaitForSeconds(time);
        statusText.text = "";
    }

}

