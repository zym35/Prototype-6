using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MarbleId
{
    public MarbleType Type;
    public int Level;
}

public enum MarbleType
{
    Attack,
    Block,
    Heal,
    Shuffle
}

public class Marble : MonoBehaviour
{
    public MarbleId marbleId;
    public Tray tray;
    public Discard discard;
    public bool selected;
    public bool inMarket;

    // Update is called once per frame
    private void Update()
    {
        if (!tray.active) return;
        
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to world coordinates
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Check if the mouse click hits this GameObject's collider
            Collider2D collider = GetComponent<Collider2D>();

            if (collider == null || !collider.OverlapPoint(mousePosition)) 
                return;
            
            if (inMarket)
            {
                if (tray.active)
                {
                    tray.discard.AddToDiscard(marbleId);
                }
                else
                {
                    tray.enemyTray.discard.AddToDiscard(marbleId);
                }
            }
            else
            {
                foreach (var m in FindObjectsOfType<Marble>())
                {
                    m.selected = false;
                }
                selected = true;
        
                // TODO highlight
            }
        }
    }

    public void Use()
    {
        Discard();
        switch (marbleId.Type)
        {
            case MarbleType.Attack:
                tray.enemyTray.Attacked(marbleId.Level + 1);
                break;
            case MarbleType.Block:
                tray.IncreaseBlock(marbleId.Level + 1);
                break;
            case MarbleType.Heal:
                tray.Heal(marbleId.Level + 1);
                break;
            case MarbleType.Shuffle:
                tray.TrayToDiscard();
                tray.bag.DiscardToBag();
                tray.Draw();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Discard()
    {
        tray.tray.Remove(gameObject);
        discard.AddToDiscard(marbleId);
        Destroy(gameObject);
    }

    public void Sell()
    {
        Discard();
        tray.IncreaseMoney(marbleId.Level + 1);
        //TODO sell
    }
}
