using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PurchaseCredits : MonoBehaviour
{
    public void BuyCredits(int creds)
    {
        GameManager.Instance.AddSubCredits(creds);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
