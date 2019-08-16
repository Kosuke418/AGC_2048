using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(MoveDirection md)
    {
        Debug.Log(md.ToString() + " move. ");
    }
}
