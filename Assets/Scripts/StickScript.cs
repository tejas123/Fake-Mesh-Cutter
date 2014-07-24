using UnityEngine;
using System.Collections;

//This script is placed on the wood object.
public class StickScript : MonoBehaviour
{
    // bombs are placed as child of stick transform
    private GameObject[] bombs;

    //object which will be instanciated
    public GameObject gameObjectPrefeb;

    private float woodColliderSize, storeRotation = 0, startingPoint, endPoint, distance, newScale;

    private GameObject tempGameObject;

    private float radius = 15.0F;
    private float power = 2000.0F;


    void Start()
    {

    }

    public void blastClicked()
    {
        //if there is no bomb to blast
        if (transform.childCount == 0)
            return;

        //initializing bomb array
        bombs = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            bombs[i] = transform.GetChild(i).gameObject;


        //setting rotation to zero  if wood is rotated to any other angle
        if (transform.eulerAngles.z != 0)
        {
            storeRotation = transform.eulerAngles.z;
            transform.eulerAngles = Vector3.zero;
        }

        //bombs array will be arranges in assending order by there y coordinate.
        arrangeBombs();

        //fetching size of collider
        woodColliderSize = GetComponent<BoxCollider>().size.y;
        

        //creating object above first bomb.
        //new objects will be instanciated as child of wood transform.
        createFirstObject();

        //creating objects below each bomb. 
        for (int i = 0; i < bombs.Length; i++)
            createObjectBelowBomb(bombs[i], i);

        //rotating wood object on its initial rotation.
        transform.eulerAngles = new Vector3(0, 0, storeRotation);

        //removing wood object's rendering and collider
        Destroy(GetComponent<SpriteRenderer>() as SpriteRenderer);
        Destroy(GetComponent<BoxCollider>() as BoxCollider);

        //new objects will be blasted and bombs will be destroyed.
        manageChildren();
    }


    private void arrangeBombs()
    {
        GameObject tempY;

        for (int i = 0; i < bombs.Length - 1; i++)
        {
            for (int j = 0; j < bombs.Length - i - 1; j++)
            {
                if (bombs[j].transform.position.y < bombs[j + 1].transform.position.y)
                {
                    tempY = bombs[j];
                    bombs[j] = bombs[j + 1];
                    bombs[j + 1] = tempY;
                }
            }
        }

    }
    private void createFirstObject()
    {
        startingPoint = findStartingPointY(gameObject);
        endPoint = findStartingPointY(bombs[0]);

        distance = Mathf.Abs(startingPoint - endPoint);

        if (distance <= 0.1f)
            return;


        newScale = (Mathf.Abs(startingPoint - endPoint)) / woodColliderSize;

        tempGameObject = (GameObject)Instantiate(gameObjectPrefeb, new Vector3(transform.position.x, endPoint + (distance / 2), transform.position.z), Quaternion.identity);
        tempGameObject.transform.localScale = new Vector3(1, newScale, 1);
        tempGameObject.transform.parent = bombs[0].transform.parent;
    }

    private void createObjectBelowBomb(GameObject currentBomb, int index)
    {
        startingPoint = findEndingPointY(currentBomb);

        if (index + 1 < bombs.Length)
            endPoint = findStartingPointY(bombs[index + 1]);
        else
            endPoint = findEndingPointY(gameObject);

        distance = Mathf.Abs(startingPoint - endPoint);
        if (distance <= 0.1f)
            return;

        newScale = (Mathf.Abs(startingPoint - endPoint)) / woodColliderSize;

        tempGameObject = (GameObject)Instantiate(gameObjectPrefeb, new Vector3(transform.position.x, startingPoint - (distance / 2), transform.position.z), Quaternion.identity);
        tempGameObject.transform.localScale = new Vector3(1, newScale, 1);
        tempGameObject.transform.parent = bombs[0].transform.parent;

    }

    private void manageChildren()
    {
        foreach (Transform childTransform in transform)
        {
            if (childTransform.tag.Equals("Explosive"))
            {
                Collider[] colliders = Physics.OverlapSphere(childTransform.position, radius);

                foreach (Collider hit in colliders)
                {
                    if (hit && hit.rigidbody)
                    {
                        hit.rigidbody.AddExplosionForce(power, childTransform.position, radius, 3.0F);
                    }
                }
                Destroy(childTransform.gameObject);
            }
        }
    }
    private float findStartingPointY(GameObject tempObject)
    {
        return (tempObject.transform.position.y + ((tempObject.GetComponent<BoxCollider>().size.y) / 2));
    }
    private float findEndingPointY(GameObject tempObject)
    {
        return (tempObject.transform.position.y - ((tempObject.GetComponent<BoxCollider>().size.y) / 2));
    }
}
