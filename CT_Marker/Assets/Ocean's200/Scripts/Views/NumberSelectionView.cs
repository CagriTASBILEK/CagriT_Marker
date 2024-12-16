using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberSelectionView : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField[] numberInputs;
    [SerializeField] private TextMeshProUGUI[] errorTexts;
    [SerializeField] private Button startButton;

    private NumberSelectionViewModel _viewModel;

    private void Awake()
    {
        _viewModel = GetComponent<NumberSelectionViewModel>();
        SetupUI();
    }

    private void SetupUI()
    {
        for (int i = 0; i < numberInputs.Length; i++)
        {
            int index = i; 
            numberInputs[i].onValueChanged.AddListener((value) => 
            {
                _viewModel.OnNumberInput(index, value);
            });
        }

        if(startButton != null)
        {
            startButton.onClick.AddListener(_viewModel.OnStartButtonClicked);
            startButton.interactable = false;
        }
    }

    public void ShowError(int index, string message)
    {
        if (index < errorTexts.Length && errorTexts[index] != null)
        {
            errorTexts[index].text = message;
            errorTexts[index].gameObject.SetActive(true);
        }
    }

    public void ClearError(int index)
    {
        if (index < errorTexts.Length && errorTexts[index] != null)
        {
            errorTexts[index].gameObject.SetActive(false);
        }
    }

    public void SetStartButtonState(bool interactable)
    {
        if(startButton != null)
            startButton.interactable = interactable;
    }
    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        if(startButton != null)
            startButton.onClick.RemoveListener(_viewModel.OnStartButtonClicked);

        for (int i = 0; i < numberInputs.Length; i++)
        {
            numberInputs[i].onValueChanged.RemoveAllListeners();
        }
    }
}
