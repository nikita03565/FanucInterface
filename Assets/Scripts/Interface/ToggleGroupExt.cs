using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public static class ToggleGroupExt
{
    public static Toggle GetActive(this ToggleGroup aGroup)
    {
        return aGroup.ActiveToggles().FirstOrDefault();
    }

    public static void SetActive(this ToggleGroup aGroup, int n)
    {
        var toggles = aGroup.GetComponentsInChildren<Toggle>();
        toggles[n].isOn = true;
    }
}
