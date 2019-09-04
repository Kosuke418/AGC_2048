using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TileStyle
{
    public int Number;
    public Sprite TileSprite;
}


public class TileStyleHolder : MonoBehaviour
{
    // SINGLETON
    public static TileStyleHolder Instance;

    public TileStyle[] TileStyles;

    private void Awake()
    {
        Instance = this;
    }
}
