using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickCharm : MonoBehaviour
{
    public string type;
    public string _name;
    public string desc;
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onClick()
    {
        if(!(type == "shard" || type == "movement")) openPopup();
        UIManager.instance.updateTutorialUI(gameObject.GetComponent<Image>().sprite, _name, desc);
    }
    public void openPopup()
    {
        RectTransform[] children = GetComponentsInChildren<RectTransform>(true);
        foreach(RectTransform c in children){
            if(c.gameObject.name == "Panel")
            {
                c.gameObject.SetActive(true);
                break;
            }
        }
    }
    public void yes()
    {
        Debug.Log("ok you used the charm");
        //subtract number of charms by 1
        //the player should automatically use walk charms when they move so i wont put it here
        if(type == "shard" || type == "movement")
        {
            //you can't use shard charms, and movement is automatically used
            return;
        }
        if(type == "fog")
        {
            player.clearFog();
        }else if(type == "rotate")
        {
            player.SetRotateActivated(true);
        }else if(type == "teleport")
        {
            player.teleport();
        }else if(type == "jump")
        {
            player.jump();
        }else if(type == "movewall")
        {
            player.wallMove();
        }else if(type == "explode")
        {
            player.explode();
        }
        player.gameObject.GetComponent<Inventory>().removeCharm(type);
        closePopup();
    }
    public void no()
    {
        closePopup();
    }
    public void closePopup()
    {
        UIManager.instance.resetTutorialUI();
        //assuming the close button is a child of a panel called "Panel"
        RectTransform[] children = GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform c in children)
        {
            if (c.gameObject.name == "Panel")
            {
                c.gameObject.SetActive(false);
                break;
            }
        }
    }
}
