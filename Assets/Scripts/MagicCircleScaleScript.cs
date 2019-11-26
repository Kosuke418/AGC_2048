using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleScaleScript : MonoBehaviour
{
    public float scaleSpeed = 0.01f;
    // Start is called before the first frame update

    // Update is called once per frame
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
        }
        else if (other.tag == "EnemyShotG" && this.tag == "MagicCircleG")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "EnemyShotP" && this.tag == "MagicCircleG")
        {

        }
        else if (other.tag == "EnemyShotG" && this.tag == "MagicCircleP")
        {

        }
    }
}
