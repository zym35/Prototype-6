using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public Transform removePanelParent, removePanel;
    public GameObject removeMarbleAttack, removeMarbleBlock, removeMarbleHeal, removeMarbleShuffle;
    public GameObject gameEnd;

    public void OnButtonClick(string action)
    {
        if (!active) return;
        if (action == "End")
        {
            TrayToDiscard();
            
            money = 0;
            FindObjectOfType<Market>().Refill();
            
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
            m.transform.DOMove(discard.transform.position, 1).onComplete += delegate { Destroy(m); };
        }
        tray.Clear();
    }

    public void IncreaseMoney(int delta)
    {
        money += delta;
    }

    public bool TryBuy(int price)
    {
        if (money < price)
            return false;
        money -= price;
        return true;
    }
    
    public void Attacked(int power)
    {
        var delta = Mathf.Max(0, power - block);
        HP -= delta;
        block = Mathf.Max(0, block - power);

        if (HP == 0)
        {
            gameEnd.SetActive(true);
            gameEnd.GetComponentInChildren<TMP_Text>().text = enemyTray.name + " Wins!";
            return;
        }
        if (delta == 0) return;
        
        for (int i = 0; i < removePanel.childCount; i++)
        {
            Destroy(removePanel.GetChild(i).gameObject);
        }

        foreach (MarbleId id in bag.sack)
        {
            InstantiateRemoveMarble(id, bag.sack);
        }
        
        foreach (MarbleId id in discard.pile)
        {
            InstantiateRemoveMarble(id, discard.pile);
        }
        
        removePanelParent.gameObject.SetActive(true);

        void InstantiateRemoveMarble(MarbleId id, List<MarbleId> from)
        {
            var rm = Instantiate(id.Type switch
            {
                MarbleType.Attack => removeMarbleAttack,
                MarbleType.Block => removeMarbleBlock,
                MarbleType.Heal => removeMarbleHeal,
                MarbleType.Shuffle => removeMarbleShuffle,
                _ => throw new ArgumentOutOfRangeException()
            }, removePanel);
            rm.GetComponent<Image>().color = id.Level switch
            {
                0 => Color.grey,
                1 => new Color(90 / 255f, 188 / 255f, 216 / 255f),
                2 => Color.magenta,
                3 => Color.yellow,
                _ => throw new ArgumentOutOfRangeException()
            };
            rm.GetComponent<Button>().onClick.AddListener(delegate
            {
                from.Remove(id);
                removePanelParent.gameObject.SetActive(false);
            });
        }
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

    public void Draw(int level = 0)
    {
        tray.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (bag.sack.Count == 0)
                if (!bag.DiscardToBag())
                    return;
            MarbleId newMarb = bag.Draw(level);
            GameObject marblePrefab = newMarb.Type switch
            {
                MarbleType.Attack => attackMarble,
                MarbleType.Block => blockMarble,
                MarbleType.Heal => healMarble,
                MarbleType.Shuffle => shuffleMarble,
                _ => null
            };
        
            var m = Instantiate(marblePrefab, bag.transform.position, Quaternion.identity);
            m.transform.DOMove(new Vector3(xpos[i], ypos, -1f), 1);
            Marble marbleInfo = m.GetComponent<Marble>();
            marbleInfo.tray = this;
            marbleInfo.discard = discard;
            marbleInfo.marbleId = newMarb;
            marbleInfo.price = -1;
            
            tray.Add(m);
        }
    }
}