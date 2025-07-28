using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject representing a quiz question, including question text,
/// a list of possible answers, and the index of the correct answer.
/// </summary>
[CreateAssetMenu(fileName = "New Question", menuName = "QuizMaster/Quiz Question")]
public class QuestionSO : ScriptableObject
{
    [Header("Question Settings")]
    [Tooltip("The text of the quiz question.")]
    [TextArea(2, 6)]
    [SerializeField]
    private string question = "Enter new question text here";

    [Tooltip("Array of possible answer options.")]
    [TextArea(2, 6)]
    [SerializeField]
    private string[] answers = new string[4];

    [Tooltip("Index (0-based) of the correct answer in the answers array.")]
    [Range(0, 3)]
    [SerializeField]
    private int correctAnswerIndex;

    /// <summary>
    /// Gets the question text.
    /// </summary>
    public string Question => question;

    /// <summary>
    /// Returns the answer at the specified index, or null if out of range.
    /// </summary>
    /// <param name="index">0-based index of the answer.</param>
    public string GetAnswer(int index)
    {
        if (index < 0 || index >= answers.Length)
        {
            Debug.LogError($"GetAnswer: Index {index} is out of bounds. Valid range: 0 to {answers.Length - 1}.");
            return null;
        }
        return answers[index];
    }

    /// <summary>
    /// Returns a new List containing all answer strings.
    /// </summary>
    public List<string> Answers => new List<string>(answers);

    /// <summary>
    /// Gets the index of the correct answer.
    /// </summary>
    public int CorrectAnswerIndex => correctAnswerIndex;
}
