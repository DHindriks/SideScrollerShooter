﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void Switch(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Reload()
    {
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);

    }
}
