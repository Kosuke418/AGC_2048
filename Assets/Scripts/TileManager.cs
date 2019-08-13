using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            Debug.Log("up");
        }
        if (Input.GetKey("s"))
        {
            Debug.Log("down");
        }
        if (Input.GetKey("a"))
        {
            Debug.Log("left");
        }
        if (Input.GetKey("d"))
        {
            Debug.Log("right");
        }
    }

    void MoveUp()
    {
        if ()
        {

        }
    }
}
