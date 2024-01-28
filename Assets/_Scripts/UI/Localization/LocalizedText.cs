using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    public string LocalizationKey;

    void Start()
    {
        Localize();
        LocalizationManager.LocalizationChanged += Localize;
    }

    public void OnDestroy()
    {
        LocalizationManager.LocalizationChanged -= Localize;
    }

    private void Localize()
    {
        GetComponent<TMP_Text>().text = LocalizationManager.Localize(LocalizationKey);
    }
}
