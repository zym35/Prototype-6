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

            if (collider != null && collider.OverlapPoint(mousePosition))
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
        switch (marbleId.Type)
        {
            case MarbleType.Attack:
                break;
            case MarbleType.Block:
                break;
            case MarbleType.Heal:
                break;
            case MarbleType.Shuffle:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Discard();
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
        
        //TODO sell
        
    }
}
