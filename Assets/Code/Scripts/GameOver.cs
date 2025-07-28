using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the Game Over UI screen, displaying the final score and handling the restart functionality.
/// </summary>
public class GameOver : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Canvas containing the Game Over UI")]
    [SerializeField] private Canvas gameOverCanvas;

    [Tooltip("Button that restarts the game when clicked")]
    [SerializeField] private Button playAgainButton;

    [Tooltip("Text element to show the player's score")]
    [SerializeField] private TextMeshProUGUI congratsText;

    private void Awake()
    {
        // If not assigned in Inspector, find the Canvas component on this GameObject
        if (gameOverCanvas == null)
            gameOverCanvas = GetComponent<Canvas>();

        // Locate the Play Again button in the hierarchy if needed
        if (playAgainButton == null)
            playAgainButton = transform.Find("PlayAgainButton").GetComponent<Button>();

        // Locate the score text under the CongratsPanel if not set
        if (congratsText == null)
            congratsText = transform.Find("CongratsPanel/congratsText").GetComponent<TextMeshProUGUI>();

        // Attach the restart handler to the button
        playAgainButton.onClick.AddListener(OnPlayAgainClicked);
    }

    /// <summary>
    /// Handler for the 'Play Again' button click.
    /// Reloads the current scene to restart the game.
    /// </summary>
    private void OnPlayAgainClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Displays the Game Over screen and updates the score message.
    /// </summary>
    /// <param name="score">Player's score as a percentage.</param>
    public void ShowGameOverScreen(int score)
    {
        // Bring the Game Over canvas to the front of other UI layers
        gameOverCanvas.sortingOrder = 10;

        // Update the congratulatory text with the player's score
        congratsText.text = $"Congratulations!\nYou scored {score}%";
    }
}
