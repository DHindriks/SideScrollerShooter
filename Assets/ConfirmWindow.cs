using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmWindow : MonoBehaviour
{
    public TextMeshProUGUI MainText;
    public TextMeshProUGUI CancelText;
    public TextMeshProUGUI ConfirmText;

    public Button CancelBtn;
    public Button ConfirmBtn;

    void OnDisable() //reset button behaviours
    {
        CancelBtn.onClick.RemoveAllListeners();
        ConfirmBtn.onClick.RemoveAllListeners();

        ConfirmBtn.onClick.AddListener(() => gameObject.SetActive(false));
        CancelBtn.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
