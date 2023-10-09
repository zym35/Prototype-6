using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    public int slot; //slot is 0 - 3
    public bool isPlaying; //true if active player is playing, false if discarding
    public bool playerActive; //true if it is the marble owner's turn, false otherwise
    public int type; //0 = attack, 1 = crit, 2 = heal, 3 = recycle
    public Tray t;
    public Shuffle s;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to world coordinates
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Check if the mouse click hits this GameObject's collider
            Collider2D collider = GetComponent<Collider2D>();

            if (collider != null && collider.OverlapPoint(mousePosition))
            {
                OnClick();
            }
        }
    }

    public void OnClick()
    {
        if (playerActive)
        {
            if (isPlaying)
            {
                play();
                s.pile.Add(type);
                s.text.text = s.pile.Count + "";
                t.draw(slot, false);
            }
            else
            {
                t.draw(slot, true);
            }
        }
    }

    public void play()
    {
        if (type == 0)
        {

        }
        else if (type == 1)
        {

        }
        else if (type == 2)
        {

        }
        else if (type == 3)
        {

        }
        t.enemyTray.setPlaying(false);
        t.enemyTray.setActive(true);
    }
}
