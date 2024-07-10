using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipConfig : MonoBehaviour
{
    public List<Transform> WeaponSlots;
    public List<Texture2D> Skins;
    public int ShipValue;

    public ShipData data;

    void SaveData()
    {
        SaveSystem.SaveShip(data);
    }

    void LoadData()
    {
        data = SaveSystem.LoadShip(data);
    }

    void Start()
    {
        LoadData();
        GetComponent<MeshRenderer>().material.mainTexture = Skins[data.Skin];
    }

    public void UnlockShip()
    {
        data.Unlocked = true;
        SaveSystem.SaveShip(data);
    }

    public void SetSkin(int skinIndex)
    {
        data.Skin = skinIndex;
        SaveData();
        GetComponent<MeshRenderer>().material.mainTexture = Skins[data.Skin];
    }
}

[System.Serializable]
public class ShipData
{
    public string Name;
    public bool Unlocked;
    public int Skin;
}
