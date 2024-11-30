using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapCard : MonoBehaviour, IPointerClickHandler
{
    public GameObject xuanzetubiao;
    public bool bXuanzetubiao_active_or;//判断现在那个打勾的图标有没有显示出来：如果true就是有，false就是没有

    public Card isMapCard;//地图卡牌

    public MapManager mapManager;//地图管理器
    // Start is called before the first frame update
    void Start()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        LoadCardBackGroundImage();
    }




    public void OnPointerClick(PointerEventData eventData)
    {
        //选择的动画部分
        switch (bXuanzetubiao_active_or)
        {
            case true: bXuanzetubiao_active_or = false; break;
            case false: bXuanzetubiao_active_or = true; break;
        }
        xuanzetubiao.SetActive(bXuanzetubiao_active_or);
        //选择的数据部分
        if (bXuanzetubiao_active_or)
        {
            mapManager.ASetOfCard_Map1.m_iarrIsSelectCard[isMapCard.id] = 1;
        }
        else
        {
            mapManager.ASetOfCard_Map1.m_iarrIsSelectCard[isMapCard.id] = 0;
        }
    }



    private void LoadCardBackGroundImage()//加载卡牌背景
    {
        string path = "CardImage/" + isMapCard.cardName;
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
    }
}
