using System.Collections;
using UnityEngine;

public class EnemyShotMoveScript : MonoBehaviour
{
    private float startTime1 = 0f;
    private float startTime2 = 0f;
    private Vector3 endPosition;

    private Vector3 startScale = new Vector3(0f, 0f, 0f);
    private Vector3 endScale = new Vector3(0.2f, 0.2f, 0.2f);

    private ThisGameManager thisGameManager;

    // Start is called before the first frame update
    void Start()
	{
        thisGameManager = GameObject.Find("Canvas").GetComponent<ThisGameManager>();
        endPosition = new Vector3(transform.position.x, -6f, transform.position.z);
        transform.localScale = startScale;
        StartCoroutine(GenerateShot());
    }

    IEnumerator GenerateShot()
    {
        float time1 = 1f;
        float diff = 0;
        while (transform.localScale != endScale)
        {
            diff += Time.deltaTime - startTime1;
            if (diff > time1)
            {
                transform.localScale = endScale;
            }
            var rate = diff / time1;

            transform.localScale = Vector3.Lerp(startScale, endScale, rate);
            yield return new WaitForFixedUpdate();
        }
        if (transform.localScale == endScale)
        {
            StartCoroutine(MoveShot());
        }
    }

    IEnumerator MoveShot()
    {
        float time1 = 100f;
        float diff = 0;
        while (transform.position != endPosition)
        {
            diff += Time.deltaTime - startTime2;
            if (diff > time1)
            {
                transform.position = endPosition;
            }
            var rate = diff / time1;
            transform.position = Vector3.Lerp(transform.position, endPosition, rate);
            if (transform.position.y <= -5f)
            {
                thisGameManager.Damage();
                Destroy(gameObject);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
