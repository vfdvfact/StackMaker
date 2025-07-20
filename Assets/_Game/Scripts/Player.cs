using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    Vector3 startP = new Vector3((float)0.5,0,(float)-1.5);
    Vector3 endP = new Vector3((float)2.5, 0, (float)9.5);
    [SerializeField] GameObject nextUI;
    [SerializeField] GameObject startUI;
    [SerializeField] GameObject playAgainUI;
    int currentStage=1;
    public GameObject stone;
    public Transform head;
    public Transform body;
    private Vector2 startPos;
    private Vector2 endPos;
    public GameObject stage;
    public GameObject stage1, curStage;
    public Transform pos;
    [SerializeField] Button startButton;
    [SerializeField] Button nextButton;
    [SerializeField] Button mainMenuButton;
    float step=0.25f;
    Vector3 target;
    public int count=0;
    public bool isMoving = false;
    bool haveTarget = false;
    public float swipeThreshold = 10f;
    bool inputAllowed = true;
    RaycastHit hit;
    [SerializeField] Vector3 dir = Vector3.zero;
    float speed = 10f;
    int liveBrick=0;
    Stack<GameObject> myStack = new Stack<GameObject>();
    bool done = true;
    void Awake()
    {
        curStage = Instantiate(stage);
        stage.SetActive(true);
        nextButton.onClick.AddListener(NextStage);
        startButton.onClick.AddListener(BeginGame);
        mainMenuButton.onClick.AddListener(PlayAgain);
    }


    void Update()
    {
        if (done || !inputAllowed)
            return;
        if (!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
                //Debug.Log("start");
            }
            if (Input.GetMouseButtonUp(0))
            {
                endPos = Input.mousePosition;
                //Debug.Log("end");
                DetectDir();
            }
        }
        else
        {
            Move();
        }
    }
    void DetectDir()
    {
        Vector2 swipeVector = endPos - startPos;

        if (swipeVector.magnitude < swipeThreshold)
            return;

        if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
        {
            if (swipeVector.x > 0)
                dir = Vector3.right;
            else
                dir = Vector3.left;
        }
        else
        {
            if (swipeVector.y > 0)
                dir = Vector3.forward;
            else
                dir = Vector3.back;
        }
        isMoving = true;
        //Debug.Log("dir");
    }
    void Move()
    {
        if (!haveTarget)
        {
            //Debug.Log("isMoving");
            //Debug.DrawRay(pos.position + dir, Vector3.down, Color.red, 20f);
            if (Physics.Raycast(pos.position + dir, Vector3.down, out hit, 20f)&& hit.collider.tag == "End")
            {
                //string a = hit.collider.tag;
                //Debug.Log(a);
                target = new Vector3(hit.collider.transform.position.x, 0, hit.collider.transform.position.z);
                if (hit.collider.gameObject.name == "S")
                {
                    if (!hit.collider.gameObject.GetComponent<ShowB>().used)
                    {
                        hit.collider.gameObject.GetComponent<ShowB>().Hidee();
                        {
                            BirthBrick();
                        }
                    }
                }
                else if (hit.collider.gameObject.name == "H")
                {
                    if (!hit.collider.gameObject.GetComponent<HideB>().used)
                    {
                        hit.collider.gameObject.GetComponent<HideB>().Showw();
                        KillBrick();
                    }
                }
                haveTarget = true;
                //Debug.Log("haveTarget");
            }
            else
                isMoving = false;
        }
        else
        {
            //Debug.Log("Move");
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (transform.position != target)
                return;
            haveTarget = false;
            StartCoroutine(SlaughterBrick());
        }
    }
    void NextStage()
    {
        GameObject ob = curStage;
        Destroy(ob);
        curStage = Instantiate(stage1);
        transform.position = startP;
        done = false;
        nextUI.SetActive(false);
        currentStage++;
        StartCoroutine(EnableInputAfterDelay());
    }
    void BeginGame()
    {
        done = false;
        startUI.SetActive(false);
        StartCoroutine(EnableInputAfterDelay());
    }
    void PlayAgain()
    {
        GameObject ob = curStage;
        Destroy(ob);
        curStage = Instantiate(stage);
        transform.position = startP;
        done = true;
        startUI.SetActive(true);
        playAgainUI.SetActive(false);
    }
    void KillBrick()
    {
        count--;
        head.localPosition = new Vector3(0, 0 + step * count, 0);
        body.localPosition = new Vector3(0, 0 + step * count, 0);
        GameObject columnBrick = myStack.Pop();
        columnBrick.gameObject.SetActive(false);
    }
    void BirthBrick()
    {
        count++;
        head.localPosition = new Vector3(0, 0 + step * count, 0);
        body.localPosition = new Vector3(0, 0 + step * count, 0);
        myStack.Push(Instantiate(stone, transform));
        myStack.Peek().transform.localPosition = new Vector3(0, 0 + step * count, 0);
    }
     IEnumerator SlaughterBrick()
    {
        if (transform.position == endP)
        {
            liveBrick=count;
            for (int i = 0; i < liveBrick; i++)
            {
                yield return new WaitForSeconds(0.1f);
                KillBrick();
            }
            done = true;
            isMoving = false;
            if (currentStage == 1)
            {
                nextUI.SetActive(true);
            }
            else
            {
                currentStage = 1;
                playAgainUI.SetActive(true);
            }
        }
    }
    IEnumerator EnableInputAfterDelay()
    {
        inputAllowed = false;
        yield return new WaitForSeconds(0.2f); // adjust time as needed
        inputAllowed = true;
    }
}
