using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shuffle : MonoBehaviour
{
    public List<int> pile = new List<int>();
    public TextMeshPro text;
    public Bag b;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        if (b.sack.Count == 0)
        {
            b.sack = pile;
            b.text.text = b.sack.Count + "";
            pile = new List<int>();
        }
    }
}