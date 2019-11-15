using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<Text> chefsRecipeList;  // The list of ingredients presented to the head chef at the start of a round
    public Text recipeName; // The title of the chef canvas
    public List<Text> apprenticeIngredientList; // The list of ingredients presented to an apprentice at the start of their turn in a round

    private RecipeGenerator RG;

    // Start is called before the first frame update
    void Start()
    {
        RG = GameObject.FindGameObjectWithTag("RecipeGenerator").GetComponent<RecipeGenerator>();
        //chefsRecipeList = new List<Text>();
        //apprenticeIngredientList = new List<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Calls the generate recipe function in RecipeGenerator, and sets the chef's canvas to the returned list of ingredients
    /// Also sets the name of the recipe
    /// </summary>
    public void GenerateAndSetNewRecipe()
    {
        List<string> recipe = RG.GenerateRecipe();

        int x = 0;
        foreach (string ingredient in recipe)
        {
            chefsRecipeList[x].text = ingredient;
            x++;
        }

        recipeName.text = RG.GetCurrentRecipeName();
    }

    /// <summary>
    /// Calls the generateIngredientsList function in RecipeGenerator, and sets the apprentice's canvas to the returned list of ingredients
    /// TODO: We need a list of sprites for all of these things, so I can look them up and apply the appropriate texture to each button. For now I'm just setting some text
    /// </summary>
    public void GenerateAndSetRandomIngredients()
    {
        List<string> ingredients = RG.GenerateIngredientList();

        int x = 0;
        foreach (string ingredient in ingredients)
        {
            apprenticeIngredientList[x].text = ingredient;
            x++;
        }
    }
}
