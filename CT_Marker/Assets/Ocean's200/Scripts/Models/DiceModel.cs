using UnityEngine;

public class DiceModel
{
    public int[] Values { get; private set; }

    public DiceModel()
    {
        Values = new int[3] { 1, 1, 1 };
    }

    public void SetValues(int[] values)
    {
        if (values.Length != 3)
        {
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            Values[i] = Mathf.Clamp(values[i], 1, 6);
        }
    }
}
