﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<Text> chefsRecipeList;  // The list of ingredients presented to the head chef at the start of a round
    public Text recipeName; // The title of the chef canvas
    public List<Text> apprenticeIngredientList; // The list of ingredients presented to an apprentice at the start of their turn in a round

    public Text confirmScreenText; // The words that appear on the confirmation screen, telling players to pass the phone to the next player
    public GameObject confirmScreenToChef;  // The button that thakes the player to the Chef's view. Corresponds to 0
    public GameObject confirmScreenToApprentice;    // The button that takes the player to the Apprentice's view. Corresponds to 1

    public Canvas scoreScreen;  // The score screen canvas needs to be enabled by code
    public List<Text> scoredJuicesList; // The list of juices players completed or started working on. Presented on the score screen
    public List<Text> scoredJuicesScoreList;   // The list of juice scores for each juice players completed or started working on. Presented on the score screen
    public ScoreScript SS;

    //public Canvas timerCanvas;  //The canvas containing our timer, only present while the round is active
    public Text timerText;  // The text in the timerCanvas that actually displays the time remaining

    private RecipeGenerator RG;

    private int numberOfPlayers = 4;

    public List<Sprite> ingredientSprites;  // The list of sprites of different ingredients. names should correspond to the name given to each ingredient
    public List<Image> ingredientImageHolders; // The actual holders for each image

    public Sprite starSprite;   // The sprite that displays, with 1-3 stars, how well the player did
    public List<Sprite> resultSprites;  // The gunk and smoothie result icons

    public List<Image> starSpritesScoreScreen;   // The list of our star sprite lists
    public List<Image> resultSpritesScoreScreen;    // The list of our resultSprites on the score screen

    // TODO need a list of all the stars in the score screen so we can set them to active

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
        List<string> recipe = RG.GenerateRecipe(numberOfPlayers);

        int x = 0;
        foreach (string ingredient in recipe)
        {
            if (chefsRecipeList[x] == null)
            {
                break;
            }
            chefsRecipeList[x].text = ingredient;
            x++;
        }

        //Set the text and the images
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
            

            //set the images. The name of the ingredient and the name of the sprite have to be identitcal
            for (int i = 0; i < ingredientSprites.Count; i++)
            {
                if (ingredientSprites[i].name.Equals(ingredient))
                {
                    ingredientImageHolders[x].sprite = ingredientSprites[i];
                    //Debug.Log("Added Image: " + ingredientSprites[i].name.ToString());
                }
            }

            x++;
        }

    }

    /// <summary>
    /// Called when an apprentice selects an ingredient to add to the juice
    /// </summary>
    public void IngredientSelected(Text ingredientName)
    {
        RG.AddChosenIngredient(ingredientName.text);

        // If we have completed this recipe, we need to set our confirmation screen to take play back to the chef's screen
        if (RG.CheckRecipeCompletion())
        {
            SetConfirmScreenButton(0);
            GenerateAndSetNewRecipe(); // Let's also begin a new recipe here!
        }
    }

    /// <summary>
    /// Sets which button will be active on the confirm screen-- this will decide whether the chef screen will be shown, or the apprentice screen will be shown.
    /// Passing in 0 here will take players to the chef screen
    /// Passing in 1 here will take players to the Apprentice screen
    /// </summary>
    /// <param name="button"></param>
    public void SetConfirmScreenButton(int button)  //THIS ISNT BEING CALLED WHEN THE PLAYER HITS START ON THE CHEF SCREEN?
    {
        switch (button)
        {
            case 0:
                confirmScreenToChef.SetActive(true);
                confirmScreenToApprentice.SetActive(false);
                confirmScreenText.text = "New Recipe! Pass to Juice Miser";
                break;
            case 1:
                confirmScreenToChef.SetActive(false);
                confirmScreenToApprentice.SetActive(true);
                confirmScreenText.text = "Pass to Next Fledgling";
                break;
        }
    }

    /// <summary>
    /// Updates the timer with the current time
    /// </summary>
    /// <param name="timeRemaining"></param>
    public void UpdateTimer(float timeRemaining)
    {

        // Convert the float into minutes and seconds
        string digitalView = "";
        int minutes = (int)Mathf.Floor(timeRemaining / 60.0f);
        int seconds = (int)(timeRemaining - (minutes * 60.0f));
        digitalView = minutes.ToString() + ":" + seconds.ToString();

        timerText.text = digitalView;
    }

    /// <summary>
    /// Called when the game reaches 0 on its timer
    /// </summary>
    public void GameOver()
    {
        RG.AddIncompleteRecipeToScore();    // Add the score from our incomplete juice

        // Update the score screen with all the correct juice names
        for (int i = 0; i < SS.juiceNames.Count; i++)
        {
            if (i == scoredJuicesList.Count)
            {
                break;
            }
            scoredJuicesList[i].text = SS.juiceNames[i];
            scoredJuicesScoreList[i].text = SS.juiceScores[i].ToString();
        }

        //SS.juicePercentages

        // Set the appropriate number of stars active
        /*for (int i = 0; i <= 3; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                if (SS.juicePercentages[i] < .33f * k)
                {
                    for (int m = 0; m < k; m++)
                    {
                        starSprites[m*i].enabled = true;
                    }
                    break;
                }
            }
            
        }*/

        int starNum = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                if (SS.juicePercentages.Count > i)
                {
                    
                    if (SS.juicePercentages[i] > .33f * k)
                    {
                        starSpritesScoreScreen[starNum + k].enabled = true;
                    }
                }
                
            }

            starNum += 3;
        }
        // set the result icon
        for (int i = 0; i < resultSpritesScoreScreen.Count; i++)
        {
            if (SS.juicePercentages.Count > i)
            {
                if (SS.juicePercentages[i] < .33f)
                {
                    resultSpritesScoreScreen[i].sprite = resultSprites[0];
                }
                else if (SS.juicePercentages[i] < .66f)
                {
                    resultSpritesScoreScreen[i].sprite = resultSprites[1];
                }
                else
                {
                    resultSpritesScoreScreen[i].sprite = resultSprites[2];
                }

            }
        }


        // Enable the score screen
        scoreScreen.enabled = true;
    }

    /// <summary>
    /// Reset the entire canvas back to the main menu, with only the main menu active. Called 
    /// At the end of the score screen.
    /// </summary>
    public void ResetToMainMenu()
    {
        SS.ResetJuices();

        Canvas[] toDeactivate = this.gameObject.GetComponentsInChildren<Canvas>();
        for (int i = 2; i < toDeactivate.Length; i++)
        {
            toDeactivate[i].enabled = false;
        }

        //Reset our score screen
        for (int i = 0; i < starSpritesScoreScreen.Count; i++)
        {
                starSpritesScoreScreen[i].enabled = false;
        }
    }

    public void SetNumOfPlayers(int count)
    {
        numberOfPlayers = count;
    }
}
