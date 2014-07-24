using UnityEngine;
using System.Collections;

public class ExplosiveScript : MonoBehaviour
{
    public bool isColliding = false, isPlaced = false;
    private GameObject collidingGameObject;
    private float colliderSize = 0;
    // Use this for initialization
    void Start()
    {
        colliderSize = transform.GetComponent<BoxCollider>().size.y;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider collision)
    {
        isColliding = true;
        if (!isPlaced && collision.transform.CompareTag("Wood"))
        {
            transform.parent = collision.transform; ;
            transform.localEulerAngles = Vector3.zero;
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
            //collision.transform.position = new Vector3(transform.position.x, collision.transform.position.y, collision.transform.position.z);
            collidingGameObject = collision.gameObject;


        }
    }
    void OnTriggerStay(Collider collision)
    {
        if (!isPlaced)
        {
            if (collision.transform.CompareTag("Wood"))
            {
                if (collidingGameObject != collision.gameObject)
                {
                    isColliding = true;
                    print("I found the difference");
                    transform.parent = collision.transform;
                    transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
                    transform.localEulerAngles = Vector3.zero;
                    collidingGameObject = collision.gameObject;

                }

            }
            else
            {
                isColliding = false;
                transform.parent = collidingGameObject.transform.parent;
                collidingGameObject = null;
                transform.eulerAngles = Vector3.zero;
            }
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (!isPlaced)
        {
            isColliding = false;
            transform.parent = collidingGameObject.transform.parent;
            collidingGameObject = null;
            transform.eulerAngles = Vector3.zero;
        }
    }

    public float getXposition()
    {
        return collidingGameObject.transform.position.x;
    }
    public GameObject getcollidingObject()
    {
        return collidingGameObject;
    }
    public bool actionUpPerformed()
    {
        if (isColliding)
        {
            transform.parent = collidingGameObject.transform;
            isPlaced = true;
            return true;
        }
        else
            return false;
    }
    Vector3 tempPos;
    public void setProperPosition()
    {
        tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempPos.z = 0;
        transform.position = tempPos;
        if (isColliding && collidingGameObject && (Vector3.Distance(tempPos, transform.position) < colliderSize + 1))
        {
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }
        //else
        //{
        //    transform.position = tempPos;
        //}
    }
}
