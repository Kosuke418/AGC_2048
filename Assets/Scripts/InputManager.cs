﻿using System.Collections;
using UnityEngine;


public enum MoveDirection
{
    Left,
    Right,
    Up,
    Down
}
public class InputManager : MonoBehaviour
{

    private ThisGameManager gm;

    private void Awake()
    {
        gm = GameObject.FindObjectOfType<ThisGameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // right move
            gm.Move(MoveDirection.Right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // left move
            gm.Move(MoveDirection.Left);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Up move
            gm.Move(MoveDirection.Up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Down move
            gm.Move(MoveDirection.Down);
        }
    }
}