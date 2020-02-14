using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleScript : MonoBehaviour
{
    public GameObject magicCircleP;
    public GameObject magicCircleG;
    public Vector3 setPos;
    public List<GameObject> InstansMagicCircle;

    public bool isCanShot = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCanShot)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                GenerateMagicCircle(0);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                GenerateMagicCircle(1);
            }
        }
    }

    public void Canshot()
    {
        isCanShot = false;
        StartCoroutine(DelayCanshot());
    }

    IEnumerator DelayCanshot()
    {
        yield return new WaitForSeconds(0.5f);
        isCanShot = true;
    }

    void GenerateMagicCircle(int color)
    {
        if(color==0) InstansMagicCircle.Add(Instantiate(magicCircleP, setPos, Quaternion.identity));
        else if(color==1) InstansMagicCircle.Add(Instantiate(magicCircleG, setPos, Quaternion.identity));
    }
}
