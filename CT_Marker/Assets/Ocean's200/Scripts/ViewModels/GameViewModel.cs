using System.Collections.Generic;
using UnityEngine;

public class GameViewModel : MonoBehaviour
{
    [SerializeField] private DiceViewModel diceViewModel;
    [SerializeField] private NumberSelectionView numberSelectionPopup;
    
    private GameModel _gameModel;
    private GameView _view;
    private bool _canRoll = false;
    private bool _isProcessingRoll = false; 
    private void Awake()
    {
        _gameModel = new GameModel();
        _view = GetComponent<GameView>();
    }

    private void OnEnable()
    {
        OceanGameEvent.OnRollCompleted += HandleRollCompleted;
        OceanGameEvent.OnNumbersSelected += HandleNumbersSelected;
    }

    private void OnDisable()
    {
        OceanGameEvent.OnRollCompleted -= HandleRollCompleted;
        OceanGameEvent.OnNumbersSelected -= HandleNumbersSelected; 
    }
    
    private void HandleNumbersSelected(List<int> selectedNumbers)
    {
        StartGame(selectedNumbers);
    }
    
    public void StartGame(List<int> selectedNumbers)
    {
        if(_gameModel.StartGame(selectedNumbers))
        {
            _canRoll = true;
            OceanGameEvent.TriggerGameStarted();
            UpdateUI();
        }
    }
    public void OnRollButtonClicked()
    {
        if(!_canRoll || diceViewModel.IsRolling || _isProcessingRoll) return;

        _isProcessingRoll = true;
        int[] diceValues = _gameModel.CalculateDiceValues();
        diceViewModel.SetDiceValues(diceValues);
    }
    private void HandleRollCompleted(int rollTotal)
    {
        if (!_isProcessingRoll) return;
        
        _gameModel.AddRollResult(rollTotal);
        OceanGameEvent.TriggerTotalScoreUpdated(_gameModel.GetTotalSum());

        if(!_gameModel.IsGameActive())
        {
            _canRoll = false;
            OceanGameEvent.TriggerGameCompleted();
        }
        _isProcessingRoll = false;
        UpdateUI();
    }
    private void UpdateUI()
    {
        _view.UpdateUI(
            _gameModel.GetTotalSum(),
            _gameModel.GetCurrentRoll(),
            _canRoll,
            diceViewModel.IsRolling
        );
    }

    public void ResetGame()
    {
        _gameModel.Reset();
        _canRoll = false;
        UpdateUI();
    }
}
