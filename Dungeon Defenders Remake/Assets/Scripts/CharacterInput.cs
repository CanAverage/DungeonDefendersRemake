using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    bool canPlayerMove = true;
    bool isCameraTopDown = false;
    public float speed = 0.0f;

    [SerializeField] private GameObject[] Towers = null;
    [SerializeField] private Camera cam = null;

    private int towerNumber = 0;
    private GameObject objectBeingSpawned = null;
    void Update()
    {
        float moveHorizontal = -Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (canPlayerMove)
        {
            Vector3 movement = new Vector3(moveVertical, 0.0f, moveHorizontal);
            transform.Translate(movement * speed * Time.deltaTime, Space.World);
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            changeCameraPosition(isCameraTopDown);
            towerNumber = 0;
            objectBeingSpawned = Instantiate(Towers[towerNumber], gameObject.transform);
        }
        if(isCameraTopDown == true)
        {

        } 
    }


    void changeCameraPosition(bool isTopDown)
    {
        if(!isTopDown)
        {
            cam.transform.position += new Vector3(0, 3, 0);
            cam.transform.rotation = Quaternion.Euler(50, 90, 0);
            isCameraTopDown = true;
        } else
        {
            cam.transform.position += new Vector3(0, -3, 0);
            cam.transform.rotation = Quaternion.Euler(15, 90, 0);
            isCameraTopDown = false;
        }
    }
}
