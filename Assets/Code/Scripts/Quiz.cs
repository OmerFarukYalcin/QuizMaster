using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the quiz flow: displays questions and answers, handles input, timing, and score updates.
/// </summary>
public class Quiz : MonoBehaviour
{
    [Header("Quiz Configuration")]
    [Tooltip("Collection of quiz questions to present.")]
    [SerializeField] private List<QuestionSO> questions;

    [Tooltip("Reference to the GameOver UI manager.")]
    [SerializeField] private GameOver gameOverUI;

    [Header("Feedback Sprites")]
    [Tooltip("Sprite to show when the user selects the correct answer.")]
    [SerializeField] private Sprite correctAnswerSprite;

    [Tooltip("Sprite to show when the user selects an incorrect answer.")]
    [SerializeField] private Sprite incorrectAnswerSprite;

    [Header("UI References")]
    [Tooltip("Text element for displaying the question.")]
    [SerializeField] private TextMeshProUGUI questionText;

    [Tooltip("Text element for displaying the current score percentage.")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Tooltip("Slider indicating quiz progress (current question index).")]
    [SerializeField] private Slider progressSlider;

    [Tooltip("UI Image whose fill represents the remaining time.")]
    [SerializeField] private Image timerFillImage;

    [Tooltip("Parent transform containing all answer buttons.")]
    [SerializeField] private Transform answerButtonContainer;

    private int currentQuestionIndex = 0;
    private QuestionSO currentQuestion;
    private bool isQuizActive;
    private float timer;
    private const float questionTimeLimit = 10f;
    private ScoreManager scoreManager = new ScoreManager();

    private void Start()
    {
        // Initialize button callbacks and start the first question
        InitializeAnswerButtons();
        ShowNextQuestion();
    }

    private void Update()
    {
        if (!isQuizActive)
            return;

        // Update timer countdown and fill image
        timer -= Time.deltaTime;
        timerFillImage.fillAmount = Mathf.Clamp01(timer / questionTimeLimit);

        if (timer <= 0f)
        {
            // Time expired: treat as wrong answer
            Debug.Log("Time's up!");
            OnAnswerSelected(-1);
        }
    }

    /// <summary>
    /// Attaches click listeners to each answer button.
    /// </summary>
    private void InitializeAnswerButtons()
    {
        for (int i = 0; i < answerButtonContainer.childCount; i++)
        {
            int index = i;
            Button btn = answerButtonContainer.GetChild(i).GetComponent<Button>();
            btn.onClick.AddListener(() => OnAnswerSelected(index));
        }
    }

    /// <summary>
    /// Prepares and displays the next question or ends the quiz if done.
    /// </summary>
    private void ShowNextQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            // All questions answered: show game over UI
            gameOverUI.ShowGameOverScreen(scoreManager.CalculateSuccessRate());
            Debug.LogWarning("Quiz complete: no more questions.");
            return;
        }

        // Reset state for new question
        isQuizActive = true;
        currentQuestion = questions[currentQuestionIndex];
        timer = questionTimeLimit;

        // Update UI elements
        progressSlider.value = currentQuestionIndex;
        questionText.text = currentQuestion.Question;
        scoreText.text = $"Score: {scoreManager.CalculateSuccessRate()}%";

        // Display answer texts and ensure buttons active
        for (int i = 0; i < answerButtonContainer.childCount; i++)
        {
            var buttonObj = answerButtonContainer.GetChild(i).gameObject;
            if (i < currentQuestion.Answers.Count)
            {
                buttonObj.SetActive(true);
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.GetAnswer(i);
                buttonObj.GetComponent<Image>().sprite = correctAnswerSprite; // reset sprite
            }
            else
            {
                buttonObj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Handles an answer selection or timeout event.
    /// </summary>
    /// <param name="selectedIndex">Index of the selected answer, or -1 if timed out.</param>
    private void OnAnswerSelected(int selectedIndex)
    {
        if (!isQuizActive)
            return;

        isQuizActive = false;

        bool isCorrect = selectedIndex == currentQuestion.CorrectAnswerIndex;
        scoreManager.RecordAnswer(isCorrect);

        if (!isCorrect)
        {
            StartCoroutine(ShowCorrectAnswerFeedback());
        }
        else
        {
            // Immediate transition for correct answers
            currentQuestionIndex++;
            ShowNextQuestion();
        }
    }

    /// <summary>
    /// Coroutine that briefly highlights the correct answer before moving on.
    /// </summary>
    private IEnumerator ShowCorrectAnswerFeedback()
    {
        // Highlight correct answer text
        var correctBtn = answerButtonContainer.GetChild(currentQuestion.CorrectAnswerIndex).GetComponent<Button>();
        questionText.text = $"Sorry, correct answer was: {currentQuestion.GetAnswer(currentQuestion.CorrectAnswerIndex)}";
        correctBtn.image.sprite = incorrectAnswerSprite;

        yield return new WaitForSeconds(1f);

        correctBtn.image.sprite = correctAnswerSprite;

        // Advance to next question
        currentQuestionIndex++;
        ShowNextQuestion();
    }
}