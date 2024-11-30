using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum WuXing// 定义五行属性及其枚举  
{
    Jin, // 金  
    Mu,  // 木  
    Shui, // 水  
    Huo,  // 火  
    Tu   // 土  
}
public enum CardState_inHand_Or_inLibrary
{
    inHand,//在手上
    inLibrary//在牌堆里
}

public enum CardState_Player_Or_Enemy
{
    Player,//在玩家手上
    Enemy//在敌人手上
}

public struct Data_LoadTo_Card
{
    public List<AttackCard> attackCards;
    public List<DefenseCard> defenseCards;
    public List<RestoreCard> restoreCards;


    public int fnFromCardTakeChildValue(Card _Card)
    {
        if(_Card.CardType== "AttackCard") return (((AttackCard)_Card)).Attack_power;
        if(_Card.CardType== "DefenseCard") return (((DefenseCard)_Card)).Defense_power;
        if(_Card.CardType== "RestoreCard")return (((RestoreCard)_Card)).Restore_power;
        return 0;
    }
    public Data_LoadTo_Card fnFromCardIdListRetrunEmuAllCardList(List<int>  _iarrCardID, List<Card> _allCardListTmp)
    {
        Data_LoadTo_Card Tmp = new Data_LoadTo_Card();
        Tmp.attackCards = new List<AttackCard>();
        Tmp.defenseCards = new List<DefenseCard>();
        Tmp.restoreCards = new List<RestoreCard>();
        if (_iarrCardID == null)
        {
            Debug.Log("加载失败");
            return Tmp;
        }
        else
        {
            for (int i = 0; i < _iarrCardID.Count; i++)
            {
                switch (_allCardListTmp[_iarrCardID[i]].CardType)
                {
                    case "AttackCard":
                        AttackCard atkCard = _allCardListTmp[_iarrCardID[i]] as AttackCard;
                        Tmp.attackCards.Add(atkCard);
                        break;
                    case "DefenseCard":
                        DefenseCard defCard = _allCardListTmp[_iarrCardID[i]] as DefenseCard;
                        Tmp.defenseCards.Add(defCard);
                        break;
                    case "RestoreCard":
                        RestoreCard resCard = _allCardListTmp[_iarrCardID[i]] as RestoreCard;
                        Tmp.restoreCards.Add(resCard);
                        break;
                }
            }

        }
        return Tmp;
    }

    public void fnOneAction(string[] _CardName, Action<string[]> _atkAction, Action<string[]> _defAction, Action<string[]> _resAction)
    {
        switch (_CardName[0])
        {
            case "AttackCard":
                _atkAction(_CardName);
                break;
            case "DefenseCard":
                _defAction(_CardName);
                break;
            case "RestoreCard":
                _resAction(_CardName);
                break;
            default:
                Debug.Log(_CardName[0]);
                break;

        }
    }


}
struct DeckCard_Json_String
{
    public int[] DeckCardList;
}

public class CardTool
{
    public CardState_inHand_Or_inLibrary cardState_InHand_Or_InLibrary;//在手还是卡池里
    public CardState_Player_Or_Enemy cardState_Player_Or_Enemy;//在玩家还是敌人手上
    public bool CardState_Can_Or_Not_Play = false;//能不能出牌，默认不可以。等轮到该回合的时候再打开这个开关就可以出牌了
}

abstract public class Card//基类
{
    //在CardStore里定义
    public string CardType;//对抗unity无法序列化多态的操作
    public int id;//卡牌的ID
    public string cardName;
    public string element;
    public  WuXing wuXing_element;

    //在CardList里定义
    public Action<GameObject, GameObject> action;

    public CardTool m_emuForBattle;
    

    //构造函数：用于构造完整cardList的东西
    public Card(string _cardType,int _id, string _cardName,string _element, Action<GameObject, GameObject> _action)
    {
        this.CardType = _cardType;
        this.id = _id;
        this.cardName = _cardName;
        this.element = _element;
        this.wuXing_element=GetWuXingFromString(_element);
        this.action = _action;
        m_emuForBattle = fnLoadCardTool();

    }
    //构造函数：用于构造Json的东西
    public Card(string _cardType, int _id, string _cardName, string _element)
    {
        this.CardType = _cardType;
        this.id = _id;
        this.cardName = _cardName;
        this.element = _element;
        this.wuXing_element = GetWuXingFromString(_element);
        m_emuForBattle = fnLoadCardTool();
    }

    public Card(int _id,string _cardName)
    {
        this.id = _id;
        this.cardName = _cardName;
    }
    /// <summary>
    /// 卡牌行为函数
    /// </summary>
    /// <param name="_Initiator">发动者</param>
    /// <param name="_target">发动者的目标</param>
    virtual public void Behavioral_Function(GameObject _Initiator, GameObject _target)
    {
        action(_Initiator, _target);
    }
    // 根据字符串获取五行枚举  
    private WuXing GetWuXingFromString(string wuxingshuxin)
    {
        switch (wuxingshuxin)
        {
            case "金": return WuXing.Jin;
            case "木": return WuXing.Mu;
            case "水": return WuXing.Shui;
            case "火": return WuXing.Huo;
            case "土": return WuXing.Tu;
            default: throw new System.ArgumentException("不在五行之中");
        }
    }


    public void DebugValue()
    {
        Debug.Log("CardType的值是："+CardType);
        Debug.Log("id的值是：" + id);
        Debug.Log("cardName的值是：" + cardName);
        Debug.Log("element的值是：" + element);
    }


    CardTool fnLoadCardTool()
    {
        CardTool TmpemuForBattle = new CardTool();
        TmpemuForBattle.CardState_Can_Or_Not_Play = false;
        TmpemuForBattle.cardState_InHand_Or_InLibrary = CardState_inHand_Or_inLibrary.inLibrary;
        return TmpemuForBattle;
    }
}



public class BaseCard:Card
{
    public GameObject Initiator;//加载攻击者的对象
    public GameObject Target;//加载被攻击者的对象


    
    public BaseCard(string _cardType, int _id, string _cardName,string _element, Action<GameObject, GameObject> _action) : base(_cardType, _id, _cardName,_element,_action)
    {//构造函数：用于构造完整cardList的东西

    }
    public BaseCard(string _cardType, int _id, string _cardName, string _element) : base(_cardType, _id, _cardName, _element)
    {//构造函数：用于构造Json的东西

    }
}

public class AttackCard : BaseCard
{
    public int Attack_power;

    //public AttackCard(int _id, string _cardName, string _element, int attack_power) : base(_id, _cardName, _element)
    //{
    //    this.Attack_power = attack_power;
    //}
    public AttackCard(string _cardType, int _id, string _cardName, string _element, Action<GameObject, GameObject> _action) : base(_cardType, _id, _cardName, _element, _action)
    {//构造函数：用于构造完整cardList的东西

    }
    public AttackCard(string _cardType, int _id, string _cardName, string _element) : base(_cardType, _id, _cardName, _element)
    {//构造函数：用于构造Json的东西

    }
    public override void Behavioral_Function(GameObject _Initiator, GameObject _target)
    {
        if(m_emuForBattle.CardState_Can_Or_Not_Play)
        action(_Initiator, _target);
    }

}
public class DefenseCard : BaseCard
{
    public int Defense_power;

    //public DefenseCard(int _id, string _cardName, string _element, int _defense_power) : base(_id, _cardName, _element)
    //{
    //    this.Defense_power = _defense_power;
    //}
    public DefenseCard(string _cardType, int _id, string _cardName, string _element, Action<GameObject, GameObject> _action) : base(_cardType, _id, _cardName, _element, _action)
    {//构造函数：用于构造完整cardList的东西

    }
    public DefenseCard(string _cardType, int _id, string _cardName, string _element) : base(_cardType, _id, _cardName, _element)
    {//构造函数：用于构造Json的东西

    }
    public override void Behavioral_Function(GameObject _Initiator, GameObject _target)
    {
        if (m_emuForBattle.CardState_Can_Or_Not_Play)
            action(_Initiator, _target);
    }

}

public class RestoreCard : BaseCard
{
    public int Restore_power;

    //public RestoreCard(int _id, string _cardName, string _element,int _restore_power) : base(_id, _cardName, _element)
    //{
    //    this.Restore_power = _restore_power;
    //}
    public RestoreCard(string _cardType, int _id, string _cardName, string _element, Action<GameObject, GameObject> _action) : base(_cardType,_id, _cardName, _element, _action)
    {//构造函数：用于构造完整cardList的东西

    }
    public RestoreCard(string _cardType, int _id, string _cardName, string _element) : base(_cardType, _id, _cardName, _element)
    {//构造函数：用于构造Json的东西

    }

    public override void Behavioral_Function(GameObject _Initiator, GameObject _target)
    {
        if (m_emuForBattle.CardState_Can_Or_Not_Play)
            action(_Initiator, _target);
    }
}


