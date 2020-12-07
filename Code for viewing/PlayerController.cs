using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//for Text Objects
using UnityEngine.SceneManagement; // for scenes

public class PlayerController : MonoBehaviour
{
    //Unorganized variables...
    public GameObject GiantCube;
    bool nearby;
    float distanceFCubeNum;
    bool started;
    float rotY;
    float rotYY;
    float posX;
    float posZ;
    public bool isWalking;
    Rigidbody rb; //Reference to the players Rigidbody
    public float speed; //Players speed
    public Text scoreText, winText, timer; // Text objects
    public int score; //keep track of score
    int time; //keep track of timer value
    bool isGrounded;
    public bool isHoldingChild;
    public Text cubeText;
    public bool canPickup;
    bool canThrow;
   // public GameObject lightt;
    public GameObject babyCube;
    public GameObject tpArea;
    GameObject[] enemies;
    public Text intructions,distanceFCube, estrEg;
    int count;
    bool win;
    int winTime;
    bool one = false;
    bool two = false;
    bool three = false;
    bool four = false;
    float curRotY;
    bool easterEgg;
    float[] netDistance = new float[2];//1 is x, 2 is y

    // Use this for initialization
    void Start()
    {
        easterEgg = false;
        distanceFCube.text = "";
        estrEg.text = "";
        //Unorganized initialization...
        intructions.text = "-Your goal is to escape your child, X^3 + 2 (yes, that's it's name).  \n\n-Use keys [W] [A] [S] [D] to move; use key [E] to drop your child.  \n\n-Last seven seconds without your child before you run out of points to win!";

        nearby = false;//change if spotlight zone is wanted
        rotY = transform.rotation.y;
        started = false;
        winTime = 8;
        canThrow = true;
        win = false;
        canPickup = true;
        cubeText.text = "";
        rb = GetComponent<Rigidbody>(); //Initialize Rigibody
        speed = 17f; //Initialize Speed
        scoreText.text = ""; //starting score text
        winText.text = "Press [ENTER] to start."; //wintext is empty to start
        score = 5000; //initialize score value
        time = 45; //initialize timer
        timer.text = "Time: " + time;
        
        StartCoroutine(BabyCubeTP());
        isGrounded = true;
        enemies = GameObject.FindGameObjectsWithTag("Pickup");
        isHoldingChild = true;
        StartCoroutine(Kill());
        StartCoroutine(LoseScreen());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("Justin Decru - 3D cube game");
        }

        if(win)
        {
            intructions.text = "Your score is: " + score + " \n\nPress [R] to restart!";

            if (score > 4500)//Easter egg
            {
                easterEgg = true;
                GiantCube.transform.position = transform.position + new Vector3(0,5,0);
                estrEg.text = "Foolish mortal, the commandments of the cube are far deeper than you have the ability to comprehend.  Due to your betrayal, you have been sentenced to a private execution over an open flame that burns amongst the depths of your deepest fears and anxieties.  You are now marked by the counsel and nothing you can do will redeem your soul worthy, for you cannot escape your own mind.";

                scoreText.text = "";
                winText.text = "";
                timer.text = "";
                cubeText.text = "";

            }
        }

        if(started && easterEgg == false)
        scoreText.text = "Score: " + score;

        if(score < 0 && easterEgg == false)
        {
            score = 0;
        }

        //Displaying distance from baby cube
        if(started && win == false)
        {
            netDistance[0] = Mathf.Abs(transform.position.x - babyCube.transform.position.x);//x difference
            netDistance[1] = Mathf.Abs(transform.position.z - babyCube.transform.position.z);//z difference



            distanceFCubeNum = (netDistance[0] * netDistance[0]) + (netDistance[1] * netDistance[1]);//a^2 + b^2 = c^2  for distance
            distanceFCubeNum = Mathf.Sqrt(distanceFCubeNum);//Find Square
            distanceFCubeNum = Mathf.Round(distanceFCubeNum);//Take away yucky decimals

            if (distanceFCubeNum != 0)
                distanceFCube.text = "Distance: " + distanceFCubeNum + " Meters";

            else if (distanceFCubeNum == 0)
                distanceFCube.text = "Distance: -";
        }

        if(win)
        {
            distanceFCube.text = "";
        }
        
        if (Input.GetKey(KeyCode.Return) && started == false)
        {
  
            intructions.text = "";
            winText.text = "";
            started = true;
            StartCoroutine(Timer());
            
        }

        curRotY = transform.eulerAngles.y;

        //ignore these...don't delete
        one = false;
        two = false;
        three = false;
        four = false;


        posX = transform.position.x;
        posZ = transform.position.z;

        /*
        if(netDistance[0] < 4.5 && netDistance[1] < 4.5)
        {
            nearby = true;
        }
        else
        {
            nearby = false;
        }
        */

        //up, right anim
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
        {
            rotY = 45f;
            rotYY = -315f;

            if (curRotY != rotY)
                StartCoroutine(Rotation(rotY, rotYY, curRotY));

            print("Up,right");
        }

        //down, left anim
        else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
        {
            rotY = 225f;
            rotYY = -135f;

            if (curRotY != rotY)
                StartCoroutine(Rotation(rotY, rotYY, curRotY));

            print("Down, left");
        }

        //up, left anim
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
        {
            rotY = 315f;
            rotYY = -45f;

            if (curRotY != rotY)
                StartCoroutine(Rotation(rotY, rotYY, curRotY));

            print("Up, left");
        }

        //down, right anim
        else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
        {
            rotY = 135f;
            rotYY = -225f;

            if (curRotY != rotY)
                StartCoroutine(Rotation(rotY, rotYY, curRotY));


            print("Down, right");
        }



        //up anim
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rotY = 0;//desired rotation
            rotYY = 360;

            if (curRotY != rotY)
                StartCoroutine(Rotation(rotY, rotYY, curRotY));

            print("Up");
        }



        //down anim
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rotY = 180f;
            rotYY = -180f;

            if (curRotY != rotY)
                StartCoroutine(Rotation(rotY,rotYY, curRotY));

            print("Down");
        }

        //right anim
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rotY = 90f;
            rotYY = -270f;

            if (curRotY != rotY)
                StartCoroutine(Rotation(rotY, rotYY, curRotY));

            print("Right");
        }

        //left anim
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rotY = 270f;
            rotYY = -90f;

            if (curRotY != rotY)
                StartCoroutine(Rotation(rotY, rotYY, curRotY));

            print("Left");
        }

        //print("rotY value = " + rotY);
        // transform.Rotate(0, rotY, 0);








        //if cube walks off edge
        if (posX > 35.5 || posX < -35.5 || posZ > 35.5 || posZ < -35.5)
        {
            if(win == false)
            score = score + 100;

            transform.position = new Vector3(0, 1.829f, 0);//teleports to spawn
            rb.velocity = new Vector3(0, 0, 0);//resets velocity
            StartCoroutine(ThrowDebounce());//starts debounce
            StartCoroutine(CubeVoid());//text
        }

        //Take movement input from user
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //Create movement vector
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);



        //Apply moevement to the ball
        // rb.AddForce(movement*speed);//exponential movement


        //if the player holds A, move left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) && started)
        {
            //RaycastHit2D hit = Physics2D.Linecast(transform.position - new Vector3(.45f, 0,0), transform.position);

            // if ((hit.collider == GetComponent<Collider2D>()))
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            one = true;



        }
        //if the player holds D, move right
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) && started)
        {
            //RaycastHit2D hit = Physics2D.Linecast(transform.position - new Vector3(-.45f, 0,0), transform.position);
            two = true;
            //if ((hit.collider == GetComponent<Collider2D>()))
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);





        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && started)
        {
            // RaycastHit2D hit = Physics2D.Linecast(transform.position - new Vector3(0,0, -.45f), transform.position);
            three = true;
            // if ((hit.collider == GetComponent<Collider2D>()))
            transform.position += new Vector3(0, 0, speed * Time.deltaTime);

        }

        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) && started)
        {
            //RaycastHit2D hit = Physics2D.Linecast(transform.position - new Vector3(0, 0,.45f), transform.position); // 2d game code (basically useless here)
            four = true;
            //if ((hit.collider == GetComponent<Collider2D>()))
            transform.position -= new Vector3(0, 0, speed * Time.deltaTime);


        }

        if (one == false && two == false && three == false && four == false)//if player stops moving, they stop walking
        {
            //Currently empty...
        }

        //jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)  //Hit space bar
        {
            //apply an upward force
            rb.AddForce(new Vector3(0, 200, 0));//adds upward force
            isGrounded = false;
        }


        //update tpArea to above our position
        tpArea.transform.position = transform.position + new Vector3(0, 2.5f);//teleports mini cube tp area
        //lightt.transform.position = transform.position + new Vector3(0, 10);//teleports spotlight

        if (isHoldingChild && canPickup)
        {
            print("attemping to teleport");
            //babyCube.transform.position = tpArea.transform.position;//replace
            babyCube.GetComponent<BabyController>().isDropped = false;

            babyCube.transform.position = tpArea.transform.position; //moves baby cube to teleport area
        }

        else
        {
            babyCube.GetComponent<BabyController>().isDropped = true;
        }



        if (Input.GetKeyDown(KeyCode.E) && isHoldingChild && canThrow && started)
        {
            babyCube.GetComponent<BoxCollider>().isTrigger = false;
            isHoldingChild = false;

            StartCoroutine(delay());

        }


    }

    //method that happens when we hit a trigger
    void OnTriggerEnter(Collider other)
    {

        //if the trigger is a pickup
        if (other.tag == "Pickup" && isHoldingChild == false && canPickup)
        {

            babyCube = other.gameObject; //labels baby cube

            print("detected object");

            isHoldingChild = true;
            StartCoroutine(ThrowDebounce());
            babyCube.GetComponent<BoxCollider>().isTrigger = true;
        }

        if (other.tag == "Abyss")
        {
            transform.position = new Vector3(0, 1, 0);//resets position
            rb.velocity = new Vector3(0, 0, 0);//resets velocity
            StartCoroutine(ThrowDebounce());
            StartCoroutine(CubeVoid());
        }

        if (other.tag == "Finish")
        {
            isHoldingChild = false;

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //reset jump
        isGrounded = true;
    }


    IEnumerator BabyCubeTP()
    {
        print("Coroutine started");

        while (isHoldingChild)
        {


            print("Attempting to teleport");

            babyCube.transform.position = tpArea.transform.position;//teleports mini cube

            yield return new WaitForSeconds(1 / 100);


        }

        yield return new WaitForSeconds(1 / 100);
    }

    //coroutine to handle the timer
    IEnumerator Timer()
    {
        score = 5000;

        while (score > 0 && win == false)
        {

            score = score - 50;//lose 50 points every second
            yield return new WaitForSeconds(1);
        }
       
    }

    IEnumerator delay()
    {
        canPickup = false;
        babyCube.GetComponent<BabyController>().chasing = false;
        yield return new WaitForSeconds(3);
        babyCube.GetComponent<BabyController>().chasing = true;
        canPickup = true;
    }

    IEnumerator CubeVoid()
    {
        if (easterEgg == false) 
            cubeText.text = "You're setting a bad example right now.";
        if (easterEgg == false)
            yield return new WaitForSeconds(5);
        if (easterEgg == false)
            cubeText.text = "";
        
    }

    IEnumerator Kill()
    {
        while (true)
        {
            if (started && win == false && easterEgg == false)
            {
                winText.text = "";
                count = 0;



                while (isHoldingChild == false && nearby == false && easterEgg == false)
                {
                    
                    count++;


                    if (count == winTime && win == false && easterEgg == false)
                    {
                        winText.text = "You have successfully abandoned X^3 + 2!";

                        


                        Destroy(babyCube);
                        win = true;
                    }


                    if (win == false && easterEgg == false)
                        winText.text = "Seconds until abandonment: " + (winTime - count);
                    yield return new WaitForSeconds(1);
                }
            }
            yield return new WaitForSeconds(0);
        }
    }

    IEnumerator ThrowDebounce()
    {
        canThrow = false;
        yield return new WaitForSeconds(1);
        canThrow = true;
    }

    IEnumerator Rotation(float des, float des2, float cur)
    {
        bool desClose;

        print(cur);

        if (Mathf.Abs(des - cur) >= Mathf.Abs(des2 - cur))
        {
            desClose = false;//if des is farther away, choose other option
        }

        else
        {
            desClose = true;
        }


        if (Mathf.Abs(cur - des) < .5f || (Mathf.Abs(cur - des2) < .5f))
        {
            //stop rotating
        }

        else if (((cur > -135 && cur < 0) && des == 45) || ((cur > -90 && cur < 0)&& des == 90))
        {
            transform.Rotate(0, 5, 0);
        }

        else if (desClose)
        {

            if (cur > des)//if current rotation is greater than desired pos
            {
                transform.Rotate(0, -5, 0);//rotate by tiny amounts
            }

            else if (cur < des)//if current rotation is less than desired pos
            {
                transform.Rotate(0, 5, 0);
            }

           
        }

        else if (desClose == false)
        {

            if (cur > des2)//if current rotation is greater than desired pos
            {
                transform.Rotate(0, -5, 0);//rotate by tiny amounts
            }

            else if (cur < des2)//if current rotation is less than desired pos
            {
                transform.Rotate(0, 5, 0);
            }


        }

         yield return new WaitForSeconds(0);
        
    }


    IEnumerator LoseScreen()
    {

    

        while (true)
        {
            while (win == false && score == 0)
            {
                win = true;
                Destroy(babyCube);

                winText.color = Color.red;

                winText.text = "You've failed to abandon X^3 + 2... I hope you enjoy child support:)";
                yield return new WaitForSeconds(0);
            }


            yield return new WaitForSeconds(0);


        }
    }
}