using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tray : MonoBehaviour
{
    public List<GameObject> tray;
    public TMP_Text statusText, HPText, moneyText, blockText, energyText;
    public Bag bag;
    public Discard discard;
    public Tray enemyTray;
    public GameObject attackMarble, blockMarble, healMarble, shuffleMarble;
    public float[] xpos = new float[4];
    public float ypos;
    public bool active;
    public int money, HP, block, energy;

    public void OnButtonClick(string action)
    {
        if (!active) return;
        if (action == "End")
        {
            TrayToDiscard();
            
            money = 0;
            enemyTray.active = true;
            enemyTray.Draw();
            enemyTray.energy = 4;
            enemyTray.block = 0;
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
                    if (energy > 0)
                    {
                        marble.Use();
                        energy--;
                    }
                    break;
                case "Sell":
                    marble.Sell();
                    break;
            }
            
            return;
        }
    }

    public void TrayToDiscard()
    {
        foreach (GameObject m in tray)
        {
            discard.AddToDiscard(m.GetComponent<Marble>().marbleId);
            Destroy(m);
        }
        tray.Clear();
    }

    public void IncreaseMoney(int delta)
    {
        money += delta;
    }
    
    public void Attacked(int power)
    {
        var delta = Mathf.Max(0, power - block);
        HP -= delta;
        block = Mathf.Max(0, block - power);
    }

    public void Heal(int delta)
    {
        HP += delta;
        if (HP > 6)
            HP = 6;
    }

    public void IncreaseBlock(int delta)
    {
        block += delta;
    }

    private void Awake()
    {
        tray = new List<GameObject>();
        
        bag.InitialFill();
        if (active)
            Draw();
        
        statusText.text = "Player 1 playing";
    }

    private void Update()
    {
        moneyText.text = $"Money: {money}";
        HPText.text = $"HP: {HP}";
        blockText.text = $"Block: {block}";
        energyText.text = $"Energy: {energy}";
    }

    public void Draw()
    {
        tray.Clear();
        for (int i = 0; i < 4; i++)
        {
            MarbleId newMarb = bag.Draw();
            GameObject marblePrefab = newMarb.Type switch
            {
                MarbleType.Attack => attackMarble,
                MarbleType.Block => blockMarble,
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