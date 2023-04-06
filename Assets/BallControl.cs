using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallControl : MonoBehaviour
{
    public float resetTime = 3.0f;
    public float captureRate = 0.3f;
    public Text result;
    public GameObject effect;

    Vector3 startPos;
    Rigidbody rb;
    bool isReady = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        result.text = "";
    }
    
    // Update is called once per frame
    void Update()
    {
        SetBallPosition(Camera.main.transform);

        if (isReady )
        {
            return;
        }
        if(Input.touchCount>0 && isReady)
        {
            Touch touch =Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                //save the starting location
                startPos = touch.position;
            }
            else if(touch.phase == TouchPhase.Ended) 
            {
                float dragDistance = touch.position.y - startPos.y;
                Vector3 throwAngle = (Camera.main.transform.forward + Camera.main.transform.up).normalized;
                rb.isKinematic = false;
                isReady = false;

                rb.AddForce(throwAngle * dragDistance * 0.005f, ForceMode.VelocityChange);

                Invoke("ResetBall", resetTime);
            }

        }
    }


    void SetBallPosition(Transform anchor)
    {
        Vector3 offset = anchor.forward * 0.5f + anchor.up * -0.2f;

        transform.position = anchor.position+offset;
    }
    void ResetBall()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        isReady = true;

        gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isReady)
        {
            return;
        }

        float draw = Random.Range(0, 1);

        if(draw <= captureRate)
        {
            result.text = "Success!";
            //DB_Manager.instance.UpdateCaptured();

        }
        else
        {
            result.text = "Fail!";

        }
        Destroy(collision.gameObject);
        Instantiate(effect, collision.transform.position, Camera.main.transform.rotation);
        gameObject.SetActive(false);

    }
}
