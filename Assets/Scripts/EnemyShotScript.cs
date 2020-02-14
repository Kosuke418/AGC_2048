using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotScript : MonoBehaviour
{
    public GameObject enemyShotP;
    public GameObject enemyShotG;
    public GameObject player;

    void EnemyShot(int num)
    {
        int Pos = Random.Range(75,106);
        if(num==0)
        Instantiate(enemyShotP, new Vector3(player.transform.position.x + SetPosition(Pos).x, player.transform.position.y + SetPosition(Pos).y, player.transform.position.z), Quaternion.identity);
        else if(num==1)
        Instantiate(enemyShotG, new Vector3(player.transform.position.x + SetPosition(Pos).x, player.transform.position.y + SetPosition(Pos).y, player.transform.position.z), Quaternion.identity);

    }

    Vector3 SetPosition(int randomPos)
    {
        Vector3 ans = new Vector3();
        ans.x = 8*Mathf.Cos(randomPos * (Mathf.PI / 180));
        ans.y = 8*Mathf.Sin(randomPos * (Mathf.PI / 180));
        return ans;
    }

    private void Start()
    {
        StartCoroutine("EnemyShotRoutine");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            EnemyShot(0);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            EnemyShot(1);
        }
    }

    IEnumerator EnemyShotRoutine()
    {
        float num;
        int ColorNum;
        while (true)
        {
            num = Random.Range(0.2f, 1.5f);
            ColorNum = Random.Range(0, 2);
            EnemyShot(ColorNum);
            yield return new WaitForSeconds(num);
        }
    }
}
