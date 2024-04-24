using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomColor : MonoBehaviour
{
    [SerializeField]
    List<Color> colors;

    [SerializeField]
    List<Image> Elements;
    // Start is called before the first frame update
    void Start()
    {
        Color Picked = colors[Random.Range(0, colors.Count)];
        foreach(Image image in Elements)
        {
            Picked.a = image.color.a;
            image.color = Picked;
        }
    }
}
