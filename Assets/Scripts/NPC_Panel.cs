using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Panel : MonoBehaviour
{
    public NPC n;
    public void Update()
    {
        
    }
    public void giveButton()
    {
        if(n.selectedRizzItem != null)
        {
            this.gameObject.SetActive(false);
            Inventory i = n.player.GetComponent<Inventory>();
            i.removeRizzItem(n.selectedRizzItem._name, n.selectedRizzItem._desc);
            foreach (GameObject obj in n.charmToGive)
            {
                GameObject o = obj;
                PickUpAble p = o.GetComponent<PickUpAble>();
                i.addCharm(o.GetComponentInChildren<SpriteRenderer>().sprite, p.charmType, p.itemName, p.description);
            }
            GameObject f = GameObject.FindGameObjectWithTag("RizzItemSelectFrame");
            f.SetActive(false);
            n.rizzed = true;
        }
    }
    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }
}
