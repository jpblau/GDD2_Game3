using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public int totalScore;

    private float pointsCorrectIngredient;
    public List<string> juiceNames;    // The names of each juice the player works on
    public List<int> juiceScores;  // The score associated with each juice the player works on
    public List<float> juicePercentages;    // The percentage of ingredients the user got right

    // Start is called before the first frame update
    void Start()
    {
        totalScore = 0;
        pointsCorrectIngredient = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Calculates the score to add to the player's final score.
    /// TODO change how this is calculated. Currently this just adds 20 points every correct ingredient chosen
    /// </summary>
    /// <param name="recipe"></param>
    /// <param name="chosen"></param>
    public void AddScore(List<string> recipe, List<string> chosen, string nameOfRecipe)
    {

        int addedScore = 0;
        for (int i = 0; i < recipe.Count; i++)
        {
            if (chosen.Contains(recipe[i]))
                addedScore += (int)pointsCorrectIngredient;
        }

        juiceNames.Add(nameOfRecipe);
        juiceScores.Add(addedScore);
        totalScore += addedScore;

        if (recipe.Count > 0 && pointsCorrectIngredient > 0)
        juicePercentages.Add((float)((addedScore / pointsCorrectIngredient) / (float)recipe.Count));
        Debug.Log(juicePercentages[juicePercentages.Count - 1]);
    }


    /// <summary>
    /// Returns the player's total score-- mostly called by the UIManager to keep the total score updated
    /// </summary>
    /// <returns></returns>
    public int GetTotalScore()
    {
        return totalScore;
    }

    public void ResetJuices()
    {
        juiceNames = new List<string>();
        juiceScores = new List<int>();
        juicePercentages = new List<float>();
    }
}
