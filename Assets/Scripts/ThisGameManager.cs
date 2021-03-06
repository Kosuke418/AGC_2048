﻿using System.Collections;
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
    private List<Tile[]> columns = new List<Tile[]>();
    private List<Tile[]> rows = new List<Tile[]>();
    private List<Tile> EmptyTiles = new List<Tile>();

    [SerializeField]
    private GameObject damageUI;
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

    private int enemyHP = 100;
    private int MyHP = 100;

    Slider HPSlider;
    Slider MyHPSlider;
    Image damageColor;

    float level = 1f;
    float level3 = 1f;

    float Timer = 0;

    public bool isMistakeCircle = false;

    bool isDamage = false;

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

        HPSlider = GameObject.Find("EnemyHP").GetComponent<Slider>();
        MyHPSlider = GameObject.Find("PlayerHP").GetComponent<Slider>();
        damageColor = GameObject.Find("DamageColor").GetComponent<Image>();
        damageColor.enabled = false;
        HPSlider.value = enemyHP;
        MyHPSlider.value = MyHP;
        HPText.text = "HP" + enemyHP.ToString();
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
    bool MakeOneMoveDownIndex(Tile[] LineOfTiles)
    {
        for (int i = 0; i < LineOfTiles.Length - 1; i++)
        {
            //Move block
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i + 1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i + 1].Number;
                LineOfTiles[i + 1].Number = 0;

                return true;
            }
            //MERGE BLOCK
            if (LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i + 1].Number &&
                LineOfTiles[i].mergedThisTurn == false && LineOfTiles[i + 1].mergedThisTurn == false)
            {
                LineOfTiles[i].Number *= 2;
                LineOfTiles[i + 1].Number = 0;
                LineOfTiles[i].mergedThisTurn = true;
                LineOfTiles[i].PlayMergeAnimation();
                return true;
            }
        }
        return false;
    }
    bool MakeOneMoveUpIndex(Tile[] LineOfTiles)
    {
        for (int i = LineOfTiles.Length - 1; i > 0; i--)
        {
            //Move block
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i - 1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i - 1].Number;
                LineOfTiles[i - 1].Number = 0;
                return true;
            }
            //MERGE BLOCK
            if (LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i - 1].Number &&
                LineOfTiles[i].mergedThisTurn == false && LineOfTiles[i - 1].mergedThisTurn == false)
            {
                LineOfTiles[i].Number *= 2;
                LineOfTiles[i - 1].Number = 0;
                LineOfTiles[i].mergedThisTurn = true;
                LineOfTiles[i].PlayMergeAnimation();
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
        if (EmptyTiles.Count == 15)
        {
            Generate();
        }

        if(enemyHP <= 0)
        {
            enemyHP = 100;
            HPSlider.value = enemyHP;
            HPText.text = "HP" + enemyHP.ToString();
        }

        if (Click)
        {
            UpdateEmptyTiles();
            if (!damage)
            {
                GameObject damageText = (GameObject)Instantiate(damageUI, new Vector3(0, 0, 0), Quaternion.identity);
                damageText.GetComponentInChildren<Text>().text = TileNumber.ToString();
                enemyHP -= TileNumber;
                HPSlider.value = enemyHP;
                HPText.text = "HP" + enemyHP.ToString();
                damage = true;
            }
            level -= 0.05f;
            if (level <= 0f)
            {
                Click = false;
            }
        }

        if (isDamage)
        {
            float level4 = Mathf.Abs(Mathf.Sin(Time.time * 10)/2);
            damageColor.color = new Color(1f, 0f, 0f, level4);
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

    public void Damage()
    {
        damageColor.enabled = true;
        isDamage = true;
        MyHP -= 5;
        MyHPSlider.value = MyHP;
        MyHPText.text = "HP" + MyHP.ToString();
        StartCoroutine(ColorCoroutine());
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

    IEnumerator ColorCoroutine()
    {
        // 1秒間処理を止める
        yield return new WaitForSeconds(1);

        // １秒後ダメージフラグをfalseにして点滅を戻す
        isDamage = false;
        damageColor.enabled = false;
        damageColor.color = new Color(1f, 1f, 1f, 1f);
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

