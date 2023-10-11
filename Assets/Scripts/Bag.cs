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
        for(int y = 0; y < 3; y++)
        {
            for(int i = 0; i < 2; i++)
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

    public bool DiscardToBag()
    {
        if (discard.pile.Count == 0)
            return false;
        foreach (MarbleId m in discard.pile)
        {
            sack.Add(m);
        }
        discard.pile.Clear();
        return true;
    }

    public MarbleId Draw(int level = 0)
    {
        MarbleId highest = new MarbleId()
        {
            Level = -1
        };
        for (int i = 0; i < Mathf.Pow(2, level); i++)
        {
            int rand = Random.Range(0, sack.Count-1);
            if (sack[rand].Level > highest.Level)
                highest = sack[rand];
        }
        
        sack.Remove(highest);
        return highest;
    }
}