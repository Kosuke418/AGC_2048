using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotScript : MonoBehaviour
{
    public GameObject enemyShotP;
    public GameObject enemyShotG;
    public GameObject enemy;

    void EnemyShot(int num)
    {
        int Pos = Random.Range(0, 12);
        if(num==0)
        Instantiate(enemyShotP, new Vector3 (enemy.transform.position.x + SetPosition(Pos).x,enemy.transform.position.y + SetPosition(Pos).y, enemy.transform.position.z), Quaternion.identity);
        else if(num==1)
        Instantiate(enemyShotG, new Vector3(enemy.transform.position.x + SetPosition(Pos).x, enemy.transform.position.y + SetPosition(Pos).y, enemy.transform.position.z), Quaternion.identity);

    }

    Vector3 SetPosition(int Num)
    {
        Vector3 ans = new Vector3();
        ans.x = 2*Mathf.Cos(Num * 30 * (Mathf.PI / 180));
        ans.y = 2*Mathf.Sin(Num * 30 * (Mathf.PI / 180));
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
        int shotNum;
        int ColorNum;
        while (true)
        {
            num = Random.Range(0f, 1.5f);
            shotNum = Random.Range(0, 2);
            ColorNum = Random.Range(0, 2);
                for(int i = 0; i < shotNum; i++)
                {
                    EnemyShot(ColorNum);
                }
            yield return new WaitForSeconds(num);
        }
    }
}
