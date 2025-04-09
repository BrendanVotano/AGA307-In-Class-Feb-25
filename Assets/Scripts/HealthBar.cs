using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : GameBehaviour
{
    [SerializeField] private TMP_Text myName;
    [SerializeField] private Image healthFill;

    public void SetName(string _name)
    {
        myName.text = _name;
    }

    public void UpdateHealthBar(int _health, int _maxHealth)
    {
        healthFill.fillAmount = MapTo01(_health, 0, _maxHealth);
    }
}
