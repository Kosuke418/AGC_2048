using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Playing,
    GameOver,
    WaitingForMoveToEnd
}

public class ThisGameManager : MonoBehaviour
{

    public GameState State;
    float delay = 0.05f;
    private bool moveMade;
    private bool[] lineMoveComplete = new bool[4] { true, true, true, true };

    private Tile[,] AllTiles = new Tile[4, 4];
    private List<Tile[]> columns = new List<Tile[]>(); //list 長さ可変配列の型
    private List<Tile[]> rows = new List<Tile[]>();   //tile配列型の長さ可変配列型の変数
    private List<Tile> EmptyTiles = new List<Tile>(); //tile型の長さ可変配列の型の変数

    [SerializeField]
    private Text damageText;
    [SerializeField]
    private Text myDamageText;
    [SerializeField]
    private Text HPText;
    [SerializeField]
    private Text MyHPText;
    [SerializeField]
    private Image EnemyImage;

    public static Sprite NumberSprite;
    public static int TileNumber;
    public static bool Click = false;
    public static bool damage = false;
    public static bool myDamage = false;

    private int HP = 100;
    private int MyHP = 100;

    Slider HPSlider;
    Slider MyHPSlider;
    Image damageColor;

    float level = 1f;
    float level3 = 1f;

    float Timer = 0;

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

        State = GameState.Playing;

        Generate();
        Generate();

        HPSlider = GameObject.Find("Slider").GetComponent<Slider>();
        MyHPSlider = GameObject.Find("Slider (1)").GetComponent<Slider>();
        damageColor = GameObject.Find("DamageColor").GetComponent<Image>();
        HPSlider.value = HP;
        MyHPSlider.value = MyHP;
        HPText.text = "HP" + HP.ToString();
        MyHPText.text = "HP" + MyHP.ToString();
    }

    bool CanMove()
    {
        if (EmptyTiles.Count > 0)
            return true;
        else
        {
            // check columns
            for (int i = 0; i < columns.Count; i++)
                for (int j = 0; j < rows.Count - 1; j++)
                    if (AllTiles[j, i].Number == AllTiles[j + 1, i].Number)
                        return true;

            // check rows
            for (int i = 0; i < rows.Count; i++)
                for (int j = 0; j < columns.Count - 1; j++)
                    if (AllTiles[i, j].Number == AllTiles[i, j + 1].Number)
                        return true;

        }
        return false;
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
                LineOfTIles[i].PlayMergeAnimation();
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
                LineOfTIles[i].PlayMergeAnimation();
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

            EmptyTiles[indexForNewNumber].PlayAppearAnimation();

            EmptyTiles.RemoveAt(indexForNewNumber);
        }
        else
        {
            Debug.Log(1);
        }
    }

    private void Update()
    {
        Timer += Time.deltaTime;
        // Debug.Log(EmptyTiles.Count);

        if (Timer >= 5f)
        {
            damageColor.enabled = true;
            float level2 = Mathf.Abs(Mathf.Sin(Time.time * 10)) - 0.5f;
            level3 -= 0.02f;
            damageColor.color = new Color(1f, 0f, 0f, level2);
            myDamageText.color = new Color(1f, 0f, 0f, level3);
            if (!myDamage)
            {
                MyHP -= 10;
                myDamageText.text = "-" + 10;
                MyHPSlider.value = MyHP;
                MyHPText.text = "HP" + MyHP.ToString();
                myDamage = true;
            }
            if (level3 <= 0f)
            {
                damageColor.color = new Color(0f, 0f, 0f, 0f);
                damageColor.enabled = false;
                Timer = 0;
                level3 = 1f;
                myDamage = false;
            }
        }

        if (EmptyTiles.Count == 15)
        {
            Generate();
        }

        if(HP <= 0)
        {
            // SceneManager.LoadScene("");
            HP = 100;
            HPSlider.value = HP;
            HPText.text = "HP" + HP.ToString();
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
        moveMade = false;

        ResetMergedFlags();

        if (delay > 0)
            StartCoroutine(MoveCoroutine(md));
        else
        {
            for (int i = 0; i < rows.Count; i++)
            {
                switch (md)
                {
                    case MoveDirection.Down:
                        while (MakeOneMoveUpIndex(columns[i]))
                        {
                            moveMade = true;
                        }
                        break;
                    case MoveDirection.Left:
                        while (MakeOneMoveDownIndex(rows[i]))
                        {
                            moveMade = true;
                        }
                        break;
                    case MoveDirection.Right:
                        while (MakeOneMoveUpIndex(rows[i]))
                        {
                            moveMade = true;
                        }
                        break;
                    case MoveDirection.Up:
                        while (MakeOneMoveDownIndex(columns[i]))
                        {
                            moveMade = true;
                        }
                        break;
                }
            }
        }

        if (moveMade)
        {
            UpdateEmptyTiles();
            Generate();
        }
    }

    IEnumerator MoveOneLineUpIndexCoroutine(Tile[] line, int index)
    {
        lineMoveComplete[index] = false;
        while (MakeOneMoveUpIndex(line))
        {
            moveMade = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveComplete[index] = true;
    }

    IEnumerator MoveOneLineDownIndexCoroutine(Tile[] line, int index)
    {
        lineMoveComplete[index] = false;
        while (MakeOneMoveDownIndex(line))
        {
            moveMade = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveComplete[index] = true;
    }


    IEnumerator MoveCoroutine(MoveDirection md)
    {
        State = GameState.WaitingForMoveToEnd;

        // start moving each line with delays depending on MoveDirection md
        switch (md)
        {
            case MoveDirection.Down:
                for (int i = 0; i < columns.Count; i++)
                    StartCoroutine(MoveOneLineUpIndexCoroutine(columns[i], i));
                break;
            case MoveDirection.Left:
                for (int i = 0; i < rows.Count; i++)
                    StartCoroutine(MoveOneLineDownIndexCoroutine(rows[i], i));
                break;
            case MoveDirection.Right:
                for (int i = 0; i < rows.Count; i++)
                    StartCoroutine(MoveOneLineUpIndexCoroutine(rows[i], i));
                break;
            case MoveDirection.Up:
                for (int i = 0; i < columns.Count; i++)
                    StartCoroutine(MoveOneLineDownIndexCoroutine(columns[i], i));
                break;

        }

        // Wait until the move is over in all lines
        while (!(lineMoveComplete[0] && lineMoveComplete[1] && lineMoveComplete[2] && lineMoveComplete[3]))
            yield return null;

        if (moveMade)
        {
            UpdateEmptyTiles();
            Generate();
        }

        State = GameState.Playing;
        StopAllCoroutines();
    }
}

