using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSelectionViewModel : MonoBehaviour
{
    private NumberSelectionModel _model;
    private NumberSelectionView _view;

    private void Awake()
    {
        _model = new NumberSelectionModel();
        _view = GetComponent<NumberSelectionView>();
    }

    public void OnNumberInput(int index, string value)
    {
        if (int.TryParse(value, out int number))
        {
            if (_model.SetNumber(index, number))
            {
                _view.ClearError(index);
            }
            else
            {
                _view.ShowError(index, "Number must be between 3-18");
            }
        }
        else
        {
            _view.ShowError(index, "Invalid number");
        }
        _view.SetStartButtonState(_model.AreAllNumbersValid());
    }

    public void OnStartButtonClicked()
    {
        if (_model.AreAllNumbersValid())
        {
            OceanGameEvent.TriggerNumbersSelected(_model.GetSelectedNumbers());
            _view.ClosePopup();
        }
    }
}
