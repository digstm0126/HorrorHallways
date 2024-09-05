using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishedGameScript : MonoBehaviour
{
    public TextMeshProUGUI winMessageText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;


    private Player playerScript;

    [SerializeField] private GameObject player;
    [SerializeField] private ScoreManager scoreManager;

    private bool gameFinished = false;

    private void Start()
    {
        winMessageText.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !gameFinished)
        {
            gameFinished = true;

            winMessageText.gameObject.SetActive(true);
            finalScoreText.gameObject.SetActive(true);

            winMessageText.text = "You Win!";
            finalScoreText.text = "Final Score: " + scoreManager.GetScore().ToString();

            healthText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            if (other.CompareTag("Player"))
            {
                playerScript = other.GetComponent<Player>();
                if (playerScript != null)
                {
                    playerScript.speed = 0f;
                }
        }
        }
    }
}
