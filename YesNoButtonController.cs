using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YesNoButtonController : MonoBehaviour
{
    public void ShowButtons(bool b)
    {
        gameObject.SetActive(b);
    }

    public virtual void OnYesButtonClicked()
    {

    }

    public virtual void OnNoButtonClicked()
    {

    }
}
