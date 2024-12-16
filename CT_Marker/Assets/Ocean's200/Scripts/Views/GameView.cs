using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [Header("UI References")] [SerializeField]
    private TextMeshProUGUI totalText;

    [SerializeField] private TextMeshProUGUI rollCountText;
    [SerializeField] private Button rollButton;
    [SerializeField] private DiceView diceView;

    [Header("Selected Numbers UI")] [SerializeField]
    private TextMeshProUGUI[] selectedNumberTexts;

    private GameViewModel _viewModel;

    private void Awake()
    {
        _viewModel = GetComponent<GameViewModel>();
        SetupUI();
    }

    private void OnEnable()
    {
        OceanGameEvent.OnNumbersSelected += UpdateSelectedNumbers;
        OceanGameEvent.OnTotalScoreUpdated += UpdateTotalScore;
        OceanGameEvent.OnGameStarted += OnGameStarted;
        OceanGameEvent.OnGameCompleted += OnGameCompleted;
    }

    private void OnDisable()
    {
        OceanGameEvent.OnNumbersSelected -= UpdateSelectedNumbers;
        OceanGameEvent.OnTotalScoreUpdated -= UpdateTotalScore;
        OceanGameEvent.OnGameStarted -= OnGameStarted;
        OceanGameEvent.OnGameCompleted -= OnGameCompleted;
    }

    private void SetupUI()
    {
        if (rollButton != null)
            rollButton.onClick.AddListener(OnRollButtonClicked);
    }

    public void UpdateUI(int totalSum, int currentRoll, bool canRoll, bool isRolling)
    {
        if (totalText != null)
            totalText.text = $"Total: {totalSum}";

        if (rollCountText != null)
            rollCountText.text = $"Roll: {currentRoll}/20";

        if (rollButton != null)
            rollButton.interactable = canRoll && !isRolling;
    }

    private void UpdateSelectedNumbers(List<int> numbers)
    {
        for (int i = 0; i < selectedNumberTexts.Length && i < numbers.Count; i++)
        {
            if (selectedNumberTexts[i] != null)
                selectedNumberTexts[i].text = $"Selected {i + 1}: {numbers[i]}";
        }
    }

    private void UpdateTotalScore(int total)
    {
        if (totalText != null)
            totalText.text = $"Total: {total}";
    }

    private void OnGameStarted()
    {
        if (rollButton != null)
            rollButton.interactable = true;
    }

    private void OnGameCompleted()
    {
        if (rollButton != null)
            rollButton.interactable = false;
    }

    private void OnRollButtonClicked()
    {
        _viewModel.OnRollButtonClicked();
    }

    private void OnDestroy()
    {
        if (rollButton != null)
            rollButton.onClick.RemoveListener(OnRollButtonClicked);
    }
}