using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BloodEnabledButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI refText;
    [SerializeField] Color EnabledColor = Color.white;
    [SerializeField] Color DisabledColor = Color.red;

    private void Start()
    {
        if (GameManager.Instance.GetBloodStatus())
        {
            refText.text = "Blood Enabled";
            GetComponent<Image>().color = EnabledColor;
        }
        else
        {
            refText.text = "Blood Disabled";
            GetComponent<Image>().color = DisabledColor;
        }   
    }

    public void ChangeStatus()
    {
        FindFirstObjectByType<GameManager>().ChangeBloodStatus();
        if (FindFirstObjectByType<GameManager>().GetBloodStatus())
        {
            refText.text = "Blood Enabled";
            GetComponent<Image>().color = EnabledColor;
        }
        else
        {
            refText.text = "Blood Disabled";
            GetComponent<Image>().color = DisabledColor;
        }
    }
}
