using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetCredits : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CredCounter;
    // Start is called before the first frame update
    void Start()
    {
        UpdateCounter();
    }

    public void UpdateCounter()
    {
        CredCounter.text = GameManager.Instance.TotalCredits.ToString();
    }
}
