using System.Collections.Generic;
using UnityEngine;

public class GameModel
{
    private const int TOTAL_ROLLS = 20;
    private const int TARGET_TOTAL = 200;

    private List<int> selectedNumbers;
    private List<int> previousRollTotals;
    private int currentRollCount;
    private int totalSum;
    private bool isGameActive;

    public GameModel()
    {
        selectedNumbers = new List<int>(3);
        previousRollTotals = new List<int>(20);
        Reset();
    }

    public void Reset()
    {
        selectedNumbers.Clear();
        previousRollTotals.Clear();
        currentRollCount = 0;
        totalSum = 0;
        isGameActive = false;
        OceanGameEvent.TriggerTotalScoreUpdated(totalSum);
    }
    
    public bool StartGame(List<int> numbers)
    {
        if (numbers.Count != 3) return false;

        bool isValid = true;
        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers[i] < 3 || numbers[i] > 18)
            {
                isValid = false;
                break;
            }
        }

        if (!isValid) return false;

        selectedNumbers = new List<int>(numbers);
        isGameActive = true;
        return true;
    }
    
    public int[] CalculateDiceValues()
    {
        if (!isGameActive) return new int[] { 1, 1, 1 };

        int remainingRolls = TOTAL_ROLLS - currentRollCount;
        int remainingSum = TARGET_TOTAL - totalSum;

        
        if (remainingRolls == 1)
        {
            return DistributeValueToDice(remainingSum);
        }
        
        int targetRollSum;
        if (ShouldUseSelectedNumber(out int selectedNumber))
        {
            targetRollSum = selectedNumber;
        }
        else
        {
            
            int averageNeeded = remainingSum / remainingRolls;
            
            targetRollSum = Random.Range(
                Mathf.Max(3, averageNeeded - 2),
                Mathf.Min(18, averageNeeded + 2)
            );
        }

        return DistributeValueToDice(targetRollSum);
    }
    
    private bool ShouldUseSelectedNumber(out int selectedNumber)
    {
        selectedNumber = 0;
        
        if (currentRollCount < 10 && !HasNumberAppeared(selectedNumbers[0]))
        {
            selectedNumber = selectedNumbers[0];
            return true;
        }
        
        if (currentRollCount >= 5 && currentRollCount < 15 && !HasNumberAppeared(selectedNumbers[1]))
        {
            selectedNumber = selectedNumbers[1];
            return true;
        }
        
        if (currentRollCount >= 10 && !HasNumberAppeared(selectedNumbers[2]))
        {
            selectedNumber = selectedNumbers[2];
            return true;
        }

        return false;
    }
    
    private int[] DistributeValueToDice(int targetSum)
    {
        int[] diceValues = new int[3];
        
        diceValues[0] = Random.Range(1, 7);
        diceValues[1] = Random.Range(1, 7);
        
        int neededLastDice = targetSum - diceValues[0] - diceValues[1];
        
        if (neededLastDice < 1 || neededLastDice > 6)
        {
            return DistributeValueToDice(targetSum);
        }

        diceValues[2] = neededLastDice;
        OceanGameEvent.TriggerDiceRolled(diceValues);
        return diceValues;
    }
    
    public void AddRollResult(int rollTotal)
    {
        if (!isGameActive) return;

        previousRollTotals.Add(rollTotal);
        currentRollCount++;
        totalSum += rollTotal;
        OceanGameEvent.TriggerTotalScoreUpdated(totalSum);
        
        if (currentRollCount >= TOTAL_ROLLS)
        {
            isGameActive = false;
            OceanGameEvent.TriggerGameCompleted();
        }
    }
    private bool HasNumberAppeared(int number)
    {
        for (int i = 0; i < previousRollTotals.Count; i++)
        {
            if (previousRollTotals[i] == number)
            {
                return true;
            }
        }

        return false;
    }

    public int GetCurrentRoll() => currentRollCount;
    public int GetTotalSum() => totalSum;
    public bool IsGameActive() => isGameActive;
    public List<int> GetSelectedNumbers() => selectedNumbers;
}