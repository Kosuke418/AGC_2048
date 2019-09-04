using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ThisGameManager : MonoBehaviour
{

    private Tile[,] AllTiles = new Tile[4, 4];
    private List<Tile[]> columns = new List<Tile[]>(); //list 長さ可変配列の型
    private List<Tile[]> rows = new List<Tile[]>();   //tile配列型の長さ可変配列型の変数
    private List<Tile> EmptyTiles = new List<Tile>(); //tile型の長さ可変配列の型の変数

    [SerializeField]
    private Text damageText;
    [SerializeField]
    private Text HPText;
    [SerializeField]
    private Image EnemyImage;

    public static Sprite NumberSprite;
    public static int TileNumber;
    public static bool Click = false;
    public static bool damage = false;

    private int HP = 100;

    Slider HPSlider;

    float level = 1f;

    // Use this for initialization
    void Start()
    { // 初期化
        Tile[] AllTilesOneDim = GameObject.FindObjectsOfType<Tile>(); // tile配列の変数Allを初期化
        foreach (Tile t in AllTilesOneDim) // foreach Allの中から順に取り出す
        {
            t.Number = 0;　// 代入するset
            AllTiles[t.indRow, t.indCol] = t;// numberが0のtileとして初期化
            EmptyTiles.Add(t);
        }

        columns.Add(new Tile[] { AllTiles[0, 0], AllTiles[1, 0], AllTiles[2, 0], AllTiles[3, 0] });
        columns.Add(new Tile[] { AllTiles[0, 1], AllTiles[1, 1], AllTiles[2, 1], AllTiles[3, 1] });
        columns.Add(new Tile[] { AllTiles[0, 2], AllTiles[1, 2], AllTiles[2, 2], AllTiles[3, 2] });
        columns.Add(new Tile[] { AllTiles[0, 3], AllTiles[1, 3], AllTiles[2, 3], AllTiles[3, 3] });

        rows.Add(new Tile[] { AllTiles[0, 0], AllTiles[0, 1], AllTiles[0, 2], AllTiles[0, 3] });
        rows.Add(new Tile[] { AllTiles[1, 0], AllTiles[1, 1], AllTiles[1, 2], AllTiles[1, 3] });
        rows.Add(new Tile[] { AllTiles[2, 0], AllTiles[2, 1], AllTiles[2, 2], AllTiles[2, 3] });
        rows.Add(new Tile[] { AllTiles[3, 0], AllTiles[3, 1], AllTiles[3, 2], AllTiles[3, 3] });

        Generate();
        Generate();

        HPSlider = GameObject.Find("Slider").GetComponent<Slider>();
        HPSlider.value = HP;
        HPText.text = "HP" + HP.ToString();
    }

    //動き
    bool MakeOneMoveDownIndex(Tile[] LineOfTIles)
    {
        for (int i = 0; i < LineOfTIles.Length - 1; i++)
        {
            //Move block
            if (LineOfTIles[i].Number == 0 && LineOfTIles[i + 1].Number != 0)
            {
                LineOfTIles[i].Number = LineOfTIles[i + 1].Number;
                LineOfTIles[i + 1].Number = 0;

                return true;
            }
            //MERGE BLOCK
            if (LineOfTIles[i].Number != 0 && LineOfTIles[i].Number == LineOfTIles[i + 1].Number &&
                LineOfTIles[i].mergedThisTurn == false && LineOfTIles[i + 1].mergedThisTurn == false)
            {
                LineOfTIles[i].Number *= 2;
                LineOfTIles[i + 1].Number = 0;
                LineOfTIles[i].mergedThisTurn = true;
                return true;
            }
        }
        return false;
    }
    bool MakeOneMoveUpIndex(Tile[] LineOfTIles)
    {
        for (int i = LineOfTIles.Length - 1; i > 0; i--)
        {
            //Move block
            if (LineOfTIles[i].Number == 0 && LineOfTIles[i - 1].Number != 0)
            {
                LineOfTIles[i].Number = LineOfTIles[i - 1].Number;
                LineOfTIles[i - 1].Number = 0;
                return true;
            }
            //MERGE BLOCK
            if (LineOfTIles[i].Number != 0 && LineOfTIles[i].Number == LineOfTIles[i - 1].Number &&
                LineOfTIles[i].mergedThisTurn == false && LineOfTIles[i - 1].mergedThisTurn == false)
            {
                LineOfTIles[i].Number *= 2;
                LineOfTIles[i - 1].Number = 0;
                LineOfTIles[i].mergedThisTurn = true;
                return true;
            }
        }
        return false;
    }


    void Generate()
    {

        if (EmptyTiles.Count > 0)
        {

            int indexForNewNumber = Random.Range(0, EmptyTiles.Count);
            EmptyTiles[indexForNewNumber].Number = 2;

            EmptyTiles.RemoveAt(indexForNewNumber);
            // Debug.Log(EmptyTiles[indexForNewNumber]);
        }
        else
        {
            Debug.Log(1);
        }
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {


            Generate();

        }

    }
    */

    private void Update()
    {
        // Debug.Log(EmptyTiles.Count);
        if(EmptyTiles.Count == 15)
        {
            Generate();
        }

        if (Click)
        {
            UpdateEmptyTiles();
            damageText.enabled = true;
            damageText.text = "-" + TileNumber.ToString();
            if (!damage)
            {
                HP -= TileNumber;
                HPSlider.value = HP;
                HPText.text = "HP" + HP.ToString();
                damage = true;
            }
            level -= 0.05f;
            Debug.Log(level);
            damageText.color = new Color(1f, 0f, 0f, level);
            if (level <= 0f)
            {
                Click = false;
            }
        }
        else
        {
            damageText.enabled = false;
            damageText.color = new Color(0f, 0f, 0f, 1f);
            level = 1f;
        }
    }

    private void ResetMergedFlags()
    {
        foreach (Tile t in AllTiles)
            t.mergedThisTurn = false;
    }

    public void UpdateEmptyTiles()
    {
        EmptyTiles.Clear();
        foreach (Tile t in AllTiles)
        {
            if (t.Number == 0)
                EmptyTiles.Add(t);
        }
    }

    public void Move(MoveDirection md)
    {
        Debug.Log(md.ToString() + " move,");

        ResetMergedFlags();
        for (int i = 0; i < rows.Count; i++)
        {
            switch (md)
            {
                case MoveDirection.Down:
                    while (MakeOneMoveUpIndex(columns[i])) { }
                    break;
                case MoveDirection.Left:
                    while (MakeOneMoveDownIndex(rows[i])) { }
                    break;
                case MoveDirection.Right:
                    while (MakeOneMoveUpIndex(rows[i])) { }
                    break;
                case MoveDirection.Up:
                    while (MakeOneMoveDownIndex(columns[i])) { }
                    break;
            }
        }

        UpdateEmptyTiles();
        Generate();
    }
}
