using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public string recipeName;
    public List<string> ingredients; // 组成成分（需要完全匹配）
}
