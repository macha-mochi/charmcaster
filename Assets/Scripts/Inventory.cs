using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    [SerializeField] UIManager uim;
    [SerializeField] GameManager gm;

    [Header("Charm amounts")]
    [SerializeField] int startingMovementCharms;
    [SerializeField] int startingRotationCharms;
    [SerializeField] int startingJumpCharms;
    [SerializeField] int startingPushCharms;
    [SerializeField] int startingTeleportCharms;
    [SerializeField] private AudioSource charmSFX;

    [Header("PickUpAble")]
    [SerializeField] PickUpAble movement;
    [SerializeField] PickUpAble rotation;
    [SerializeField] PickUpAble jump;
    [SerializeField] PickUpAble push;
    [SerializeField] PickUpAble teleport;

    public int movementCharmsLeft;
    public class CharmData
    {
        public Sprite image;
        public string type; //ex jump, teleport, etc
        public string itemName;
        public string description;
        public int amt = 1;
        public CharmData(Sprite i, string t, string n, string d)
        {
            image = i;
            type = t;
            itemName = n;
            description = d;
        }
    }
    public class RizzItemData
    {
        public Sprite image;
        public string itemName;
        public string description;
        public int amt = 1;
        public RizzItemData(Sprite i, string n, string d)
        {
            image = i;
            itemName = n;
            description = d;
        }
    }
    public List<CharmData> charms;
    public List<RizzItemData> rizzItems;
    // Start is called before the first frame update
    void Start()
    {
        movementCharmsLeft = startingMovementCharms;
        charms = new List<CharmData>();
        rizzItems = new List<RizzItemData>();
        CharmData c = new CharmData(movement.gameObject.GetComponentInChildren<SpriteRenderer>().sprite, movement.charmType, movement.itemName, movement.description);
        c.amt = startingMovementCharms;
        if(c.amt > 0)
            charms.Add(c);

        c = new CharmData(rotation.gameObject.GetComponentInChildren<SpriteRenderer>().sprite, rotation.charmType, rotation.itemName, rotation.description);
        c.amt = startingRotationCharms;
        if (c.amt > 0)
            charms.Add(c);

        c = new CharmData(jump.gameObject.GetComponentInChildren<SpriteRenderer>().sprite, jump.charmType, jump.itemName, jump.description);
        c.amt = startingJumpCharms;
        if(c.amt > 0)
            charms.Add(c);

        c = new CharmData(push.gameObject.GetComponentInChildren<SpriteRenderer>().sprite, push.charmType, push.itemName, push.description);
        c.amt = startingPushCharms;
        if(c.amt > 0)
            charms.Add(c);

        c = new CharmData(teleport.gameObject.GetComponentInChildren<SpriteRenderer>().sprite, teleport.charmType, teleport.itemName, teleport.description);
        c.amt = startingTeleportCharms;
        if(c.amt > 0)
            charms.Add(c);

        uim.updateCharmUI();

    }
    public void addCharm(Sprite img, string type, string name, string desc)
    {
        if (type == "shard")
        {
            gm.numShardsHave++;
            Debug.Log("You picked up a shard");
        }
        else if (type == "goal")
        {
            gm.win();
            return;
        }
        foreach (CharmData c in charms)
        {
            if(type == c.type)
            {
                c.amt++;
                uim.updateCharmUI();
                return;
            }
        }
        charms.Add(new CharmData(img, type, name, desc));
        uim.updateCharmUI();
    }
    public void addRizzItem(Sprite img, string name, string desc)
    {
        foreach (RizzItemData r in rizzItems)
        {
            if (name == r.itemName && desc == r.description)
            {
                r.amt++;
                uim.updateRizzItemUI();
                return;
            }
        }
        rizzItems.Add(new RizzItemData(img, name, desc));
        uim.updateRizzItemUI();
    }
    public void removeCharm(string type)
    {
        for (int i = charms.Count - 1; i >= 0; i--)
        {
            if (charms[i].type == type)
            {
                charmSFX.Play();
                charms[i].amt--;
                Debug.Log(charms[i].amt);
                if (charms[i].amt == 0) charms.RemoveAt(i);
                break;
            }
        }
        uim.updateCharmUI();
        if(charms.Count == 0)
        {
            gm.lose();
        }
    }
    public void removeRizzItem(string name, string desc)
    {
        for (int i = rizzItems.Count - 1; i >= 0; i--)
        {
            if (rizzItems[i].itemName == name)
            {
                rizzItems[i].amt--;
                if (rizzItems[i].amt == 0) rizzItems.RemoveAt(i);
                break;
            }
        }
        uim.updateRizzItemUI();
    }
    public void debugLogCharms()
    {
        Debug.Log("-------------");
        foreach(CharmData c in charms)
        {
            Debug.Log(c.itemName + " " + c.description);
        }
        Debug.Log("-------------");
    }
    public void debugLogRizzItems()
    {
        Debug.Log("-------------");
        foreach (RizzItemData r in rizzItems)
        {
            Debug.Log(r.itemName + " " + r.description);
        }
        Debug.Log("-------------");
    }
}
