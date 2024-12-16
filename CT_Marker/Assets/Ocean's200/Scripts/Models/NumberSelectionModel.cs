using System.Collections.Generic;

public class NumberSelectionModel
{
    private List<int> selectedNumbers;
    private const int MIN_VALUE = 3;
    private const int MAX_VALUE = 18;

    public NumberSelectionModel()
    {
        selectedNumbers = new List<int>(3);
    }

    public bool ValidateNumber(int number)
    {
        return number >= MIN_VALUE && number <= MAX_VALUE;
    }

    public bool SetNumber(int index, int number)
    {
        if (!ValidateNumber(number)) return false;

        while (selectedNumbers.Count <= index)
        {
            selectedNumbers.Add(0);
        }

        selectedNumbers[index] = number;
        return true;
    }

    public List<int> GetSelectedNumbers()
    {
        return new List<int>(selectedNumbers);
    }

    public bool AreAllNumbersValid()
    {
        if (selectedNumbers.Count != 3) return false;

        foreach (int number in selectedNumbers)
        {
            if (!ValidateNumber(number)) return false;
        }

        return true;
    }
}

