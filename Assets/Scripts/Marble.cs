using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TorcheyeUtility;
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
    public int price = -1;
    public GameObject highlight;
    public SpriteRenderer spriteRenderer;

    private void Update()
    {
        highlight.SetActive(selected);
        spriteRenderer.color = marbleId.Level switch
        {
            0 => Color.grey,
            1 => new Color(90 / 255f, 188 / 255f, 216 / 255f),
            2 => Color.magenta,
            3 => Color.yellow,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to world coordinates
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Check if the mouse click hits this GameObject's collider
            Collider2D collider = GetComponent<Collider2D>();

            if (collider == null || !collider.OverlapPoint(mousePosition)) 
                return;
            
            if (price != -1)
            {
                Buy();
            }
            else
            {
                if (!tray.active) return;
                foreach (var m in FindObjectsOfType<Marble>())
                {
                    m.selected = false;
                }
                selected = true;
                AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.Select);
            }
        }
    }

    private void Buy()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.Buy);
        if (tray.active)
        {
            if (!tray.TryBuy(price)) return;
            tray.discard.AddToDiscard(marbleId);
        }
        else
        {
            if (!tray.enemyTray.TryBuy(price)) return;
            tray.enemyTray.discard.AddToDiscard(marbleId);
        }
        Destroy(gameObject);
    }

    public void Use()
    {
        Discard();
        switch (marbleId.Type)
        {
            case MarbleType.Attack:
                tray.enemyTray.Attacked(marbleId.Level + 1);
                AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.Attack);
                break;
            case MarbleType.Block:
                tray.IncreaseBlock(marbleId.Level + 1);
                AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.Block);
                break;
            case MarbleType.Heal:
                tray.Heal(marbleId.Level + 1);
                AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.Heal);
                break;
            case MarbleType.Shuffle:
                tray.TrayToDiscard();
                tray.bag.DiscardToBag();
                tray.Draw(marbleId.Level);
                AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.Shuffle);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Discard()
    {
        tray.tray.Remove(gameObject);
        discard.AddToDiscard(marbleId);
        transform.DOMove(discard.transform.position, 1).onComplete += delegate { Destroy(gameObject); };
    }

    public void Sell()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.Buy);
        Discard();
        tray.IncreaseMoney(1);
    }
}
