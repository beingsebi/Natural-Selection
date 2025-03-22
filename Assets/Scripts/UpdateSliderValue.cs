using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateSliderValue : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _valueText;
    void Start()
    {
        _slider.onValueChanged.AddListener((v)=>_valueText.text = v.ToString());
    }
}
