using System;
using System.Collections.Generic;
using UnityEngine;

public class OceanGameEvent
{
    public static event Action OnGameStarted;
    public static event Action OnGameCompleted;
    
    public static event Action<int[]> OnDiceRolled;
    public static event Action<List<int>> OnNumbersSelected;
    
    public static event Action<int> OnRollCompleted;
    public static event Action<int> OnTotalScoreUpdated;
    
    public static void TriggerGameStarted()
    {
        OnGameStarted?.Invoke();
    }

    public static void TriggerGameCompleted()
    {
        OnGameCompleted?.Invoke();
    }

    public static void TriggerDiceRolled(int[] diceValues)
    {
        OnDiceRolled?.Invoke(diceValues);
    }

    public static void TriggerNumbersSelected(List<int> numbers)
    {
        OnNumbersSelected?.Invoke(numbers);
    }

    public static void TriggerRollCompleted(int rollTotal)
    {
        OnRollCompleted?.Invoke(rollTotal);
    }

    public static void TriggerTotalScoreUpdated(int totalScore)
    {
        OnTotalScoreUpdated?.Invoke(totalScore);
    }
    
    public static void ClearEvents()
    {
        OnGameStarted = null;
        OnGameCompleted = null;
        OnDiceRolled = null;
        OnNumbersSelected = null;
        OnRollCompleted = null;
        OnTotalScoreUpdated = null;
    }
}
