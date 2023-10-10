using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tray : MonoBehaviour
{
    public List<GameObject> tray;
    public TextMeshPro statusText;
    public Bag bag;
    public Discard discard;
    public Tray enemyTray;
    public GameObject attackMarble, critMarble, healMarble, shuffleMarble;
    public float[] xpos = new float[4];
    public float ypos;
    public bool active;

    public void OnButtonClick(string action)
    {
        if (!active) return;
        if (action == "End")
        {
            foreach (GameObject m in tray)
            {
                discard.AddToDiscard(m.GetComponent<Marble>().marbleId);
                Destroy(m);
            }
            tray.Clear();

            enemyTray.active = true;
            enemyTray.Draw();
            active = false;
            return;
        }
        
        foreach (GameObject m in tray)
        {
            var marble = m.GetComponent<Marble>();
            if (!marble.selected) continue;

            switch (action)
            {
                case "Use":
                    marble.Use();
                    break;
                case "Sell":
                    marble.Sell();
                    break;
            }
            
            return;
        }
    }

    private void Awake()
    {
        tray = new List<GameObject>();
        
        bag.InitialFill();
        if (active)
            Draw();
        
        statusText.text = "Player 1 playing";
    }

    private void Draw()
    {
        tray.Clear();
        for (int i = 0; i < 4; i++)
        {
            MarbleId newMarb = bag.Draw();
            GameObject marblePrefab = newMarb.Type switch
            {
                MarbleType.Attack => attackMarble,
                MarbleType.Block => critMarble,
                MarbleType.Heal => healMarble,
                MarbleType.Shuffle => shuffleMarble,
                _ => null
            };
        
            var m = Instantiate(marblePrefab, new Vector3(xpos[i], ypos, -1f), Quaternion.identity);
            Marble marbleInfo = m.GetComponent<Marble>();
            marbleInfo.tray = this;
            marbleInfo.discard = discard;
            marbleInfo.marbleId = newMarb;
            
            tray.Add(m);
        }
    }
}