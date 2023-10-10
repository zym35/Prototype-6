using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Discard : MonoBehaviour
{
    public List<MarbleId> pile = new List<MarbleId>();
    public TextMeshPro text;
    
    void Start()
    {
        text.text = "0";
    }

    private void Update()
    {
        text.text = pile.Count.ToString();
    }

    public void AddToDiscard(MarbleId id)
    {
        pile.Add(id);
    }
}