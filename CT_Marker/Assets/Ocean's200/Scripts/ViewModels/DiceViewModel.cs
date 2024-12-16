using UnityEngine;

public class DiceViewModel : MonoBehaviour
{
    private DiceModel _model;
    private bool _isRolling;
    
    public int[] CurrentValues => _model.Values;
    public bool IsRolling => _isRolling;
   
    private void Awake()
    {
        _model = new DiceModel();
    }
    
    public void SetDiceValues(int[] values)
    {
        if (_isRolling) return;

        _model.SetValues(values);
        if (TryGetComponent<DiceView>(out DiceView view))
        {
            view.UpdateVisuals();
        }
       
    }
    public void StartRolling()
    {
        _isRolling = true;
    }
    public void StopRolling()
    {
        _isRolling = false;
    }
}
