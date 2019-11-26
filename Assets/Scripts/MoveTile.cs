using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveTile : MonoBehaviour
{

    [SerializeField, Range(0, 10)]
    float time = 1;

    Vector3 endPosition = new Vector3 (0,3.5f,0);

    private float startTime;
    private Vector3 startPosition;
    private GameObject obj;

    public Image enemy;

    private void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = ThisGameManager.NumberSprite;
        obj = (GameObject)Resources.Load("Explosion");
    }

    void OnEnable()
    {
        if (time <= 0)
        {
            transform.position = endPosition;
            enabled = false;
            return;
        }

        startTime = Time.timeSinceLevelLoad;
        startPosition = transform.position;
    }

    void Update()
    {
        var diff = Time.timeSinceLevelLoad - startTime;
        if (diff > time)
        {
            transform.position = endPosition;
            enabled = false;
        }

        var rate = diff / time;
        //var pos = curve.Evaluate(rate);

        transform.position = Vector3.Lerp(startPosition, endPosition, rate);
        //transform.position = Vector3.Lerp (startPosition, endPosition, pos);

        if (transform.position == endPosition)
        {
            ThisGameManager.Click = true;
            ThisGameManager.damage = false;
            Instantiate(obj, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}