using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DiceView : MonoBehaviour
{
   [Header("References")]
    [SerializeField] private Image[] diceImages;
    [SerializeField] private Sprite[] diceSprites; 
    
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.01f;
    [SerializeField] private float spinDelay = 0.1f;

    private DiceViewModel _viewModel;
    private Coroutine _animationCoroutine;

    private void Awake()
    {
        _viewModel = GetComponent<DiceViewModel>();
        
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_viewModel.IsRolling)
        {
            int[] newValues = new int[3];
            for (int i = 0; i < 3; i++)
            {
                newValues[i] = Random.Range(1, 7);
            }
            
            _viewModel.SetDiceValues(newValues);
        }
    }
    
    private void Start()
    {
        ShowValues(_viewModel.CurrentValues);
    }

    public void UpdateVisuals()
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _animationCoroutine = StartCoroutine(AnimateDiceRolls());
    }

    private void ShowValues(int[] values)
    {
        for (int i = 0; i < 3; i++)
        {
            if (values[i] < 1 || values[i] > 6)
            {
                continue;
            }

            diceImages[i].sprite = diceSprites[values[i] - 1];
        }
    }

    private IEnumerator AnimateDiceRolls()
    {
        _viewModel.StartRolling();

        float elapsedTime = 0f;
        
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            
            for (int i = 0; i < 3; i++)
            {
                int randomValue = Random.Range(0, 6);
                diceImages[i].sprite = diceSprites[randomValue];
            }
            
            yield return new WaitForSeconds(spinDelay);
        }
        
        ShowValues(_viewModel.CurrentValues);
        
        _viewModel.StopRolling();
        _animationCoroutine = null;
        
        OceanGameEvent.TriggerDiceRolled(_viewModel.CurrentValues);
        
        int total = _viewModel.CurrentValues.Sum();
        Debug.Log(total+" : dice total");
        OceanGameEvent.TriggerRollCompleted(total);
        
        
    }
}
