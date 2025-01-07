using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpAble : MonoBehaviour
{
    public GameObject player;
    public string charmType;
    public string itemName;
    [TextArea]
    public string description;
    public Sprite img; 
    public bool showPopup;
    public GameObject popup;
    public bool isCharm;
    public bool isRizzItem;

    [Header("Grid Stuff")]
    public int posX;
    public int posY;
    public GridManager gm;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            Inventory i = player.GetComponent<Inventory>();
            if (isCharm)
            {
                i.addCharm(this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite, charmType, itemName, description);
                i.debugLogCharms();
            }
            if (isRizzItem)
            {
                i.addRizzItem(this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite, itemName, description);
                i.debugLogRizzItems();
            }
            if (showPopup)
            {
                openPopup();
            }
            gm.grid[posX, posY] = 0;
            GameObject.Destroy(this.gameObject);
        }
    }
    private void openPopup()
    {
        //honestly just get a reference bruh
        //replace the image, item name, and description
        TextMeshProUGUI[] children = popup.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(TextMeshProUGUI t in children)
        {
            if(t.gameObject.name == "ItemName")
            {
                t.SetText(itemName);
                break;
            }
        }
        UIManager.instance.SetPopupAttributues(itemName, description, img);
        popup.SetActive(true);
    }
}
