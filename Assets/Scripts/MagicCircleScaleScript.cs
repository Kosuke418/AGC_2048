using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleScaleScript : MonoBehaviour
{
    public float scaleSpeed = 0.01f;

    ShakeCamera shakeCamera;
    MagicCircleScript magicCircleScript;


    private void Start()
    {
        shakeCamera = GameObject.Find("GameObject").GetComponent<ShakeCamera>();
        magicCircleScript = GameObject.Find("EventSystem").GetComponent<MagicCircleScript>();
    }

    void Update()
    {
        this.transform.localScale += new Vector3(1, 1, 1) * scaleSpeed;

        if (this.transform.localScale.x >= 5f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyShotP" && this.tag=="MagicCircleP")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            if(magicCircleScript.InstansMagicCircle.Count!=0) magicCircleScript.InstansMagicCircle.RemoveAt(0);
        }
        else if (other.tag == "EnemyShotG" && this.tag == "MagicCircleG")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            if (magicCircleScript.InstansMagicCircle.Count != 0) magicCircleScript.InstansMagicCircle.RemoveAt(0);
        }
        else if (other.tag == "EnemyShotP" && this.tag == "MagicCircleG")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            magicCircleScript.Canshot();
            shakeCamera.Shake(0.25f,2f);
            foreach(GameObject obj in magicCircleScript.InstansMagicCircle)
            {
                Destroy(obj);
            }
            magicCircleScript.InstansMagicCircle.Clear();
        }
        else if (other.tag == "EnemyShotG" && this.tag == "MagicCircleP")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            magicCircleScript.Canshot();
            shakeCamera.Shake(0.25f, 2f);
            foreach (GameObject obj in magicCircleScript.InstansMagicCircle)
            {
                Destroy(obj);
            }
            magicCircleScript.InstansMagicCircle.Clear();
        }
    }
}
