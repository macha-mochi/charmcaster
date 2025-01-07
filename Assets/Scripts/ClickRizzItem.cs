using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickRizzItem : MonoBehaviour
{
    public string _name;
    public string _desc;
    public GameObject npc_panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void onClick()
    {
        openPopup();
        if (npc_panel.activeInHierarchy)
        {
            npc_panel.GetComponent<NPC_Panel>().n.selectedRizzItem = this;
            GameObject parent = this.gameObject.transform.parent.gameObject;
            RectTransform[] children = parent.GetComponentsInChildren<RectTransform>(true);
            foreach(RectTransform c in children)
            {
                if(c.tag == "RizzItemSelectFrame")
                {
                    Debug.Log("frame found");
                    GameObject f = c.gameObject;
                    f.SetActive(true);
                    f.transform.position = this.transform.position;
                }
            }
        }
    }
    public void openPopup()
    {
        RectTransform[] children = GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform c in children)
        {
            if (c.gameObject.name == "Panel")
            {
                c.gameObject.SetActive(true);
                TextMeshProUGUI[] panelchildren = c.gameObject.GetComponentsInChildren<TextMeshProUGUI>(true);
                foreach(TextMeshProUGUI ch in panelchildren){
                    if(ch.gameObject.name == "Description")
                    {
                        ch.SetText(_desc);
                        break;
                    }
                }
                break;
            }
        }
    }
    public void closePopup()
    {
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
