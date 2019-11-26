using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleScript : MonoBehaviour
{
    public GameObject magicCircleP;
    public GameObject magicCircleG;
    public Vector3 setPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            GenerateMagicCircle(0);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GenerateMagicCircle(1);
        }
    }

    void GenerateMagicCircle(int color)
    {
        if(color==0) Instantiate(magicCircleP, setPos, Quaternion.identity);
        else if(color==1) Instantiate(magicCircleG, setPos, Quaternion.identity);
    }
}
