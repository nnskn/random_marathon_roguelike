using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // アトリビュートを付与
public class ParameterEntity
{
    public int id;
    public string name; // publicでエクセルでインポートしたい型と名前を定義
    public int maxhp;
    public int pow;
    public int vit;
    public Element element;

}
    public enum Element
    {
        NONE,
        FIRE,
        ICE,
        THUNDER

    }
