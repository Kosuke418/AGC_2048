using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{

    public bool mergedThisTurn = false;

    public int indRow;
    public int indCol;

    public int Number
    {
        get// Numberから値を取得するときはnumberを吐き出す
        {
            return number;
        }
        set// Numberに値を代入するとき(value=代入した値)は0のときSetEmpty実行それ以外でApplyStyleSetVis実行
        {
            number = value;
            if (number == 0)
                SetEmpty();
            else
            {
                ApplyStyle(number);
                SetVisible();
            }
        }
    }

    private int number;

    private Image TileImage;
    private GameObject obj;
    public GameObject canvas;
    private Animator anim;

    private void Awake()
    {
        // TileText = GetComponentInChildren<Text>();
        anim = GetComponent<Animator>();
        TileImage = transform.Find("NumberedCell").GetComponent<Image>();
        obj = (GameObject)Resources.Load("MoveTile");
    }

    public void PlayMergeAnimation()
    {
        anim.SetTrigger("Merge");
    }

    public void PlayAppearAnimation()
    {
        anim.SetTrigger("Appear");
    }

    // TileStyleHolderから呼び出した[index]の文字，文字色，画像色をこのタイルに入れる
    void ApplyStyleFromHolder(int index)　
    {
        /*
        TileText.text = TileStyleHolder.Instance.TileStyles[index].Number.ToString();
        TileText.color = TileStyleHolder.Instance.TileStyles[index].TextColor;
        TileImage.color = TileStyleHolder.Instance.TileStyles[index].TileColor;
        */
        TileImage.sprite = TileStyleHolder.Instance.TileStyles[index].TileSprite;
    }

    void ApplyStyle(int num)
    {
        switch (num)
        {
            case 2:
                ApplyStyleFromHolder(0);
                break;
            case 4:
                ApplyStyleFromHolder(1);
                break;
            case 8:
                ApplyStyleFromHolder(2);
                break;
            case 16:
                ApplyStyleFromHolder(3);
                break;
            case 32:
                ApplyStyleFromHolder(4);
                break;
            case 64:
                ApplyStyleFromHolder(5);
                break;
            case 128:
                ApplyStyleFromHolder(6);
                break;
            case 256:
                ApplyStyleFromHolder(7);
                break;
            case 512:
                ApplyStyleFromHolder(8);
                break;
            case 1024:
                ApplyStyleFromHolder(9);
                break;
            case 2048:
                ApplyStyleFromHolder(10);
                break;
            default:
                Debug.LogError("Check the numbers that you pass to ApplyStyle!");
                break;
        }
    }

    private void SetVisible()
    {
        TileImage.enabled = true;//画像を見えるように
    }

    private void SetEmpty()
    {
        TileImage.sprite = null;
        TileImage.enabled = false;//画像を見えないように
    }

    // クリックされたとき同じナンバーのオブジェクトをインスタンス化する
    public void OnClickAct()
    {
        if (TileImage.sprite != null)
        {
            ThisGameManager.TileNumber = this.Number;
            ThisGameManager.NumberSprite = TileImage.sprite;
            this.Number = 0;
            Instantiate(obj, this.transform.position, Quaternion.identity);
        }
    }
}
