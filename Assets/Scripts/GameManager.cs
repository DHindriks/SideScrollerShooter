using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public ShipConfig CurrentShip;
    public List<WeaponBase> weapons;

    public int TotalCredits { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        transform.SetParent(null);
        Instance = this;
        DontDestroyOnLoad(gameObject);
        TotalCredits = PlayerPrefs.GetInt("PlayerCredits", 0);
    }
    
    public void AddSubCredits(int AmountToAddSub)
    {
        TotalCredits += AmountToAddSub;
        PlayerPrefs.SetInt("PlayerCredits", TotalCredits);
    }
}
