using System.Collections;
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

    // Update is called once per frame
    void Update()
    {
        if (gm.State == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                //right move
                gm.Move(MoveDirection.Right);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                //left move
                gm.Move(MoveDirection.Left);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                //up move
                gm.Move(MoveDirection.Up);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                //down move
                gm.Move(MoveDirection.Down);
            }
        }
    }
}
