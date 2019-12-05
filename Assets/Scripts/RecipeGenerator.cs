﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Linq;

public class RecipeGenerator : MonoBehaviour
{
    public ScoreScript SS;

    private List<string> generatedRecipe;   //The recipe generated by GenerateRecipe()
    private string[] recipeNameBankArray; //Array of words that can be used for creating a recipe name
    private List<string> recipeNameBank;    //List of words used for creating recipe name
    private string generatedRecipeName;     // The name of the recipe the player is creating
    private List<string> chosenIngredients; //The list of ingredients that the player has chosen in this round

    private string[] ingredientList;
    int ingredientCount;

    // Start is called before the first frame update
    void Start()
    {
        generatedRecipe = new List<string>();
        chosenIngredients = new List<string>();
        recipeNameBank = new List<string>();

        generatedRecipeName = "";

        ReadIngredients();

        ReadNameBank();

        GenerateRecipeName();

        Debug.Log(GetCurrentRecipeName());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Generates a new Recipe based on the difficulty and the number of players
    /// Easy = 4 Items (+1 item for every player over 4)
    /// Medium = 5 Items (+1 item for every player over 5)
    /// Hard = 7 Items (+1 item for every player over 7, up to a max of 8)
    /// </summary>
    /// <returns>List<string> of ingredients in the recipe</returns>
    public List<string> GenerateRecipe(int count)
    {
        generatedRecipe.Clear();
        chosenIngredients.Clear();

        for (int i = 0; i < count; i++)
        {
            generatedRecipe.Add(ingredientList[Random.Range(0, ingredientCount)].Split('-')[0]);
            
        }
        //Debug.Log(generatedRecipe);
        return generatedRecipe;
    }


    /// <summary>
    /// Selects 4 random ingredients from a category, and compiles them into an array
    /// One of the "random" ingredients must be an item from the generatedRecipe
    /// None of these items should be in both the chosenIngredients and generatedRecipe lists
    /// </summary>
    /// <returns>List<string> of ingredients for the player to choose from</returns>
    public List<string> GenerateIngredientList()
    {
        List<string> ingredients = new List<string>();

        //Randomly pick the first ingredient, making sure it is one of the ingredients on the recipe
        int index = Random.Range(0, ingredientCount);
        ingredients.Add(ingredientList[index].Split('-')[0]);

        //Get the list of attributes associated with the chosen ingredient
        string[] attributes = ingredientList[index].Split('-')[1].Split(' ');

        int count = 0;

        while(ingredients.Count < 4 && count < 200)
        {
            int newIndex = Random.Range(0, ingredientCount);

            //Get ingredient and the list of attributes pertaining to the new ingredient
            string newIngredient = ingredientList[newIndex].Split('-')[0];
            string[] newAttributes = ingredientList[newIndex].Split('-')[1].Split(' ');

            /////////////////////
            ///
            /*
            Debug.Log("ATTRIBUTES: ");
            foreach (string s in attributes)
            {
                Debug.Log(s);
            }
            Debug.Log("\nNEW ATTRIBUTES: ");
            foreach (string s in newAttributes)
            {
                Debug.Log(s);
            }
            */
            //////////////////////

            //If the new ingredient has a matching attribute, and it has already not been selected, add it to the list
            //for (int i = 0; i < newAttributes.Count(); i++)
            //{
            //    if (/*attributes.Contains(newAttributes[i]) && */ ingredients.Contains(newIngredient) == false)
            //    {
            //        if ((generatedRecipe.Contains(newIngredient) && chosenIngredients.Contains(newIngredient)) == false)
            //        {
            //            ingredients.Add(newIngredient);
            //            break;
            //        }
            //    }
            //}

            count++;
            ingredients.Add(newIngredient);
        }

        return ingredients;
    }

    /// <summary>
    /// Randomly generates a recipe name, and sets the current recipe name to be the new generated name
    /// </summary>
    public void GenerateRecipeName()
    {
        recipeNameBank = recipeNameBankArray.ToList();
        string temp;   //hold the choosen item so it can be moved to the end of the list
        int randomNum;  //Int to hold the random number generated
        for(int i = 0; i < 3; i++)
        {
            randomNum = Random.Range(0, recipeNameBank.Count-i);
            generatedRecipeName += recipeNameBank[randomNum] + " ";
            temp = recipeNameBank[randomNum];
            recipeNameBank.RemoveAt(randomNum);
            recipeNameBank.Add(temp);
        }
        generatedRecipeName += "Juice!";

        //generatedRecipeName = "This needs implementation";
    }


    /// <summary>
    /// This is a getter for other scripts so that they have access to the current recipe's name;
    /// </summary>
    /// <returns></returns>
    public string GetCurrentRecipeName()
    {
        return generatedRecipeName;
    }

    /// <summary>
    /// Called when the user chooses an ingredient to add to the recipe
    /// Adds a new ingredient to the list of chosenIngredients
    /// </summary>
    /// <param name="newIngredient"></param>
    public void AddChosenIngredient(string newIngredient)
    {
        chosenIngredients.Add(newIngredient);
    }

    /// <summary>
    /// Reads in text file and generates list of ingredients with each associated attribute.
    /// Also calculates the number of ingredients from the list
    /// </summary>
    private void ReadIngredients()
    {
        string path = "Assets/IngredientsList.txt";

        StreamReader reader = new StreamReader(path);
        ingredientCount = File.ReadLines(path).Count();
        ingredientList = reader.ReadToEnd().Split('\n');
    }

    /// <summary>
    /// Reads in text file and creates an array of the words that can be used for making the recipe name
    /// </summary>
    private void ReadNameBank()
    {
        string path = "Assets/NameBank.txt";

        StreamReader reader = new StreamReader(path);
        recipeNameBankArray = new string[File.ReadLines(path).Count()];
        recipeNameBankArray = reader.ReadToEnd().Split('\n');
    }

    /// <summary>
    /// This function checks to see if the number of chosen ingredients is equal to the number of total ingredients in the recipe
    /// </summary>
    /// <returns></returns>
    public bool CheckRecipeCompletion()
    {
        bool enoughIngredients = false;
        if (chosenIngredients.Count == generatedRecipe.Count)
        {
            enoughIngredients = true;
        }

        if (enoughIngredients)
        {
            // Calculate this Recipe's score and add it to our final score
            SS.AddScore(generatedRecipe, chosenIngredients, generatedRecipeName);
        }


        return enoughIngredients;
    }

    public void AddIncompleteRecipeToScore()
    {
        SS.AddScore(generatedRecipe, chosenIngredients, generatedRecipeName);
    }
}