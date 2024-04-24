using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{

    [SerializeField]
    Image DestructionBar;
    public int ShotsFired;
    public List<GameObject> LevelBlocks;
    public float PercentageDestroyed;

    [SerializeField]
    GameObject EndScreen;

    [SerializeField]
    TextMeshProUGUI DestroyedPercent;

    [SerializeField]
    TextMeshProUGUI LaunchedAmount;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.CurrentLevel = this;
    }

    public void CheckBlocks()
    {
        float CurrentDestroyed = 0;
        for (int i = 0; i < LevelBlocks.Count; i++)
        {
            if (LevelBlocks[i] == null)
            {
                CurrentDestroyed++;
            }
        }

        PercentageDestroyed = CurrentDestroyed / LevelBlocks.Count;
        DestructionBar.fillAmount = PercentageDestroyed;
        if (PercentageDestroyed > 0.75f)
        {
            DestructionBar.gameObject.GetComponent<Button>().interactable = true;
            DestructionBar.GetComponentInParent<Animator>().SetBool("Flashing", true);
        }
    }

    public void EndLevel()
    {
        EndScreen.SetActive(true);
        DestroyedPercent.text = (Mathf.Round(PercentageDestroyed * 100)).ToString() + "% destroyed";
        LaunchedAmount.text = (ShotsFired).ToString() + " Slimes launched";
    }
}
