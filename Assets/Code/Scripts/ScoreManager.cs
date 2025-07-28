using System;
using UnityEngine;

/// <summary>
/// Manages the player's quiz statistics, tracking correct and wrong answers,
/// and calculating the success rate.
/// </summary>
public class ScoreManager
{
    // Number of correctly answered questions
    private int correctCount = 0;

    // Number of incorrectly answered questions
    private int wrongCount = 0;

    /// <summary>
    /// Records the result of an answered question.
    /// Increments correct or wrong count based on the answer.
    /// </summary>
    /// <param name="isCorrect">True if the answer was correct; false otherwise.</param>
    public void RecordAnswer(bool isCorrect)
    {
        if (isCorrect)
        {
            correctCount++;
        }
        else
        {
            wrongCount++;
        }

        LogStatistics();
    }

    /// <summary>
    /// Calculates the success percentage based on current statistics.
    /// Returns 0 if no answers have been recorded to avoid division by zero.
    /// </summary>
    /// <returns>Integer percentage of correct answers.</returns>
    public int CalculateSuccessRate()
    {
        int total = correctCount + wrongCount;

        if (total == 0)
        {
            return 0;
        }

        float ratio = (float)correctCount / total;
        int percentage = Mathf.RoundToInt(ratio * 100f);

        // Ensure percentage is between 0 and 100
        return Mathf.Clamp(percentage, 0, 100);
    }

    /// <summary>
    /// Logs the current statistics to the Unity Console for debugging.
    /// </summary>
    private void LogStatistics()
    {
        Debug.Log($"[ScoreManager] Correct: {correctCount}, Wrong: {wrongCount}");
    }
}
