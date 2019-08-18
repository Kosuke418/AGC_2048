using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisGameManager : MonoBehaviour
{
    private Tile[,] AllTiles = new Tile[4, 4];
    private List<Tile> EmptyTiles = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        Tile[] AllTilesOneDim = GameObject.FindObjectsOfType<Tile> ();
        foreach (Tile t in AllTilesOneDim)
        {
            t.Number = 0;
            AllTiles[t.indRow, t.indCol] = t;
            EmptyTiles.Add(t);
        }
    }

    void Ganerate() //タイル作る関数
    {
        if (EmptyTiles.Count > 0)
        {
            int indexForNewNumber = Random.Range(0, EmptyTiles.Count);
            int randomNum = Random.Range(0, 10);
            if(randomNum==0) // 10分の１の確率で4が出るようにしている
                EmptyTiles[indexForNewNumber].Number = 4;
            else
                EmptyTiles[indexForNewNumber].Number = 2;


            EmptyTiles.RemoveAt(indexForNewNumber);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            Ganerate();
    }

    public void Move(MoveDirection md)
    {
        Debug.Log(md.ToString() + " move. ");
    }
}
