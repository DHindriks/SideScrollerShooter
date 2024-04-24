using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public CameraScript cameraScript;

    public LevelManager CurrentLevel;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
