using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    public Transform[] positions;
    public GameObject attackMarble, blockMarble, healMarble, shuffleMarble;
    public Tray tray1;
    public List<GameObject> marbles;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            marbles.Add(InstantiateNewMarble(positions[i].position));
        }
    }

    private GameObject InstantiateNewMarble(Vector3 pos)
    {
        MarbleId newMarb = new MarbleId()
        {
            Level = 1,
            Type = MarbleType.Shuffle
        };
        GameObject marblePrefab = newMarb.Type switch
        {
            MarbleType.Attack => attackMarble,
            MarbleType.Block => blockMarble,
            MarbleType.Heal => healMarble,
            MarbleType.Shuffle => shuffleMarble,
            _ => null
        };
        
        var m = Instantiate(marblePrefab, pos, Quaternion.identity);
        Marble marbleInfo = m.GetComponent<Marble>();
        marbleInfo.tray = tray1;
        marbleInfo.discard = tray1.discard;
        marbleInfo.marbleId = newMarb;

        return m;
    }

    public void Refill()
    {
        for (int i = 0; i < 5; i++)
        {
            
        }
    }
}
