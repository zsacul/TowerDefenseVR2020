using UnityEngine;

public class HealthBar : MonoBehaviour {
    
    [SerializeField]
    public UnityEngine.UI.Slider slider;

    private void start() {
        slider.maxValue = 1f;
        slider.value = 1f;
    }

    public void updateBar(float health) {
        slider.value = health;
    }
}
