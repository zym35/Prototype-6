using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Market : MonoBehaviour
{
    public Transform[] positions;
    public GameObject attackMarble, blockMarble, healMarble, shuffleMarble;
    public Tray tray1;
    public int[] amountBeforeLevelUp;
    public int levelUpCounter;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            var m = InstantiateNewMarble(positions[i]);
            m.GetComponent<Marble>().price = i + 1;
        }
    }

    private GameObject InstantiateNewMarble(Transform parent)
    {
        var level = 0;
        var tempCounter = levelUpCounter;
        for (int i = 0; i < 3; i++)
        {
            if (tempCounter - amountBeforeLevelUp[i] > 0)
            {
                tempCounter -= amountBeforeLevelUp[i];
                level++;
            }
            else
            {
                break;
            }
        }
        MarbleId newMarb = new MarbleId()
        {
            Level = level,
            Type = (MarbleType)Random.Range(0, 4)
        };
        GameObject marblePrefab = newMarb.Type switch
        {
            MarbleType.Attack => attackMarble,
            MarbleType.Block => blockMarble,
            MarbleType.Heal => healMarble,
            MarbleType.Shuffle => shuffleMarble,
            _ => null
        };
        
        var m = Instantiate(marblePrefab, parent);
        m.transform.localPosition = Vector3.zero;
        Marble marbleInfo = m.GetComponent<Marble>();
        marbleInfo.tray = tray1;
        marbleInfo.discard = tray1.discard;
        marbleInfo.marbleId = newMarb;
        marbleInfo.price = 5;

        levelUpCounter++;

        return m;
    }

    public void Refill()
    {
        if (positions[0].childCount > 0)
        {
            Destroy(positions[0].GetChild(0).gameObject);
        }

        int firstEmpty = 0;

        while (firstEmpty < 4)
        {
            for (int i = firstEmpty + 1; i < 6; i++)
            {
                if (i == 5)
                {
                    InstantiateNewMarble(positions[4]);
                    break;
                }
                if (positions[i].childCount > 0)
                {
                    var m = positions[i].GetChild(0);
                    m.SetParent(positions[firstEmpty]);
                    m.localPosition = Vector3.zero;
                    m.GetComponent<Marble>().price = firstEmpty + 1;
                    firstEmpty++;
                    break;
                }
            }
        }
        
        InstantiateNewMarble(positions[4]);
    }
}
