using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLocalization : MonoBehaviour
{
    public void ChangeLanguage(string language)
    {
        LocalizationManager.Language = language;
    }
}
