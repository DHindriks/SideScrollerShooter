using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

enum PickableParts
{
    Ship,
    Weapon,
}

public class ShipBuilder : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI TitleTextBox;
    [SerializeField] Transform PreviewContainer;
    [SerializeField] Button ResetBtn;
    [SerializeField] Button ConfirmBtn;
    [SerializeField] Button LaunchBtn;
    [SerializeField] Button LeftBtn;
    [SerializeField] Button RightBtn;

    [SerializeField] List<ShipData> PlayableShips;
    [SerializeField] List<WeaponBase> PlayableWeapons;

    GameManager gameManager;
    PickableParts CurrentlyPicking;
    int currentIndex;
    int WeaponsPicked;
    // Start is called before the first frame update
    public void Start()
    {
        CurrentlyPicking = PickableParts.Ship;
        ResetPicker();
        WeaponsPicked = 0;
        gameManager = GameManager.Instance;
        TitleTextBox.text = "Choose a ship";
        ConfirmBtn.interactable = true;
        LaunchBtn.interactable = false;
        ResetBtn.interactable = false;
        LeftBtn.interactable = true;
        RightBtn.interactable = true;
    }

    public void ResetPicker()
    {
        currentIndex = 0;
        RefreshPreview();
    }

    public void AddToIndex(int NumToAdd)
    {
        currentIndex += NumToAdd;
        //wrap index
        switch (CurrentlyPicking)
        {
            case PickableParts.Ship:
                if (currentIndex > (PlayableShips.Count - 1))
                {
                    currentIndex = 0;
                }else if (currentIndex < 0)
                {
                    currentIndex = PlayableShips.Count - 1;
                }
                break;
            case PickableParts.Weapon:
                if (currentIndex > (PlayableWeapons.Count - 1))
                {
                    currentIndex = 0;
                }else if(currentIndex < 0)
                {
                    currentIndex = PlayableWeapons.Count - 1;
                }
                break;
        }
        

        RefreshPreview();
    }

    void RefreshPreview()
    {
        foreach(Transform obj in PreviewContainer)
        {
            Destroy(obj.gameObject);
        }

        GameObject NewObj;

        switch (CurrentlyPicking)
        {
            case PickableParts.Ship:
                NewObj = Instantiate(PlayableShips[currentIndex].gameObject, PreviewContainer);
                NewObj.transform.position = PreviewContainer.position;
                break;

            case PickableParts.Weapon:
                NewObj = Instantiate(PlayableWeapons[currentIndex].gameObject, PreviewContainer);
                NewObj.transform.position = PreviewContainer.position;
                break;
        }
    }

    public void ConfirmChoice()
    {
        switch (CurrentlyPicking)
        {
            case PickableParts.Ship:
                gameManager.CurrentShip = PlayableShips[currentIndex];
                CurrentlyPicking = PickableParts.Weapon;
                TitleTextBox.text = "Pick a weapon";
                ResetBtn.interactable = true;
                ResetPicker();
                break;
            case PickableParts.Weapon:
                gameManager.weapons.Add(PlayableWeapons[currentIndex]);
                WeaponsPicked++;
                if (WeaponsPicked == gameManager.CurrentShip.WeaponSlots.Count)
                {
                    ConfirmBtn.interactable = false;
                    LaunchBtn.interactable = true;
                    LeftBtn.interactable = false;
                    RightBtn.interactable = false;
                    TitleTextBox.text = "Build complete, ready to launch";
                    break;
                }
                TitleTextBox.text = "Pick a weapon for slot " + (WeaponsPicked + 1);
                break;
        }
    }

    public void Launch()
    {
        SceneManager.LoadScene("InGame", LoadSceneMode.Single);
    }
}
