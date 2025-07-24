using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kitchen/RecipeBook")]
public class RecipeBook : ScriptableObject
{
    public List<Recipe> recipes;
}
