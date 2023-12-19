using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderGradient : MonoBehaviour
{
    [SerializeField] private Gradient _gradient = null;
    [SerializeField] private Image _image = null;
 
 
    private void Update()
    {
        _image.color = _gradient.Evaluate(_image.fillAmount);
    }
}
