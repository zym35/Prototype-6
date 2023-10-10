using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class Bag : MonoBehaviour
{
    public List<MarbleId> sack = new List<MarbleId>();
    public TextMeshPro text;
    public Discard discard;

    public void InitialFill()
    {
        for(int y = 0; y < 4; y++)
        {
            for(int i = 0; i < 1; i++)
            {
                sack.Add(new MarbleId()
                {
                    Level = 0,
                    Type = (MarbleType)y
                });
            }
        }
    }

    private void Update()
    {
        text.text = sack.Count.ToString();
    }

    public MarbleId Draw()
    {
        if (sack.Count == 0)
        {
            foreach (MarbleId m in discard.pile)
            {
                sack.Add(m);
            }
            discard.pile.Clear();
        }
        int rand = Random.Range(0, sack.Count-1);
        MarbleId val = sack[rand];
        sack.RemoveAt(rand);
        return val;
    }
}