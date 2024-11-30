using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public enum BattleState//一个游戏的
{
    PlayerAction, EnemyAction//在Game Start中加载数据
}





public class AAllCardFunction
{
    public Dictionary<string, Action<GameObject, GameObject>> m_FunctionDictionary;
    public AAllCardFunction()
    {
        m_FunctionDictionary = new Dictionary<string, Action<GameObject, GameObject>>();
        m_FunctionDictionary.Add("AttackCard", attack);
        m_FunctionDictionary.Add("DefenseCard", defense);
        m_FunctionDictionary.Add("RestoreCard", restore);
    }

    /// <summary>
    /// 根据卡牌的名字返回方法,基础卡牌载入类型
    /// </summary>
    /// <param name="_cardName"></param>
    /// <returns></returns>
    public Action<GameObject, GameObject> GetFunction(String _cardName)
    {
        if (m_FunctionDictionary.ContainsKey("AttackCard"))
        {
            return m_FunctionDictionary[_cardName];
        }
        else if (m_FunctionDictionary.ContainsKey("DefenseCard"))
        {
            return m_FunctionDictionary[_cardName];
        }
        else if (m_FunctionDictionary.ContainsKey("RestoreCard"))
        {
            return m_FunctionDictionary[_cardName];
        }
        else
        {
            // 处理键不存在的情况  
            return null;
        }

    }

    /// <summary>
    /// 攻击行为
    /// </summary>
    /// <param name="_Initiator"></param>
    /// <param name="_target"></param>
    void attack(GameObject _Initiator, GameObject _target)
    {
        if (_Initiator != null && _target != null)
        {
            //int atk = _Initiator.GetComponent<BattleDataEvent>().attackpower;
            //int targetHealth = _target.GetComponent<BossEvent>().currentHealth;
            //int Damage = attack_CalculateDamage(_Initiator.GetComponent<BattleDataEvent>().shuxing, _target.GetComponent<BossEvent>().Bosswuxing, atk);
            //Debug.Log("攻击力是：" + atk + "，伤害是：" + Damage);
            //_target.GetComponent<BossEvent>().currentHealth = targetHealth - Damage;
        }
    }


    // 计算实际伤害  
    int attack_CalculateDamage(WuXing attackerWuXing, WuXing targetWuXing, int attack_power)
    {
        int baseDamage = attack_power;
        float damageMultiplier = 1.0f; // 默认伤害倍率为1（即无额外效果）  

        // 使用switch语句和if条件来判断相克关系  
        switch (attackerWuXing)
        {
            case WuXing.Jin:
                if (targetWuXing == WuXing.Mu)
                {
                    damageMultiplier = 2f; // 金克木
                }
                break;
            case WuXing.Mu:
                if (targetWuXing == WuXing.Tu)
                {
                    damageMultiplier = 3f; // 木克土
                }
                break;
            case WuXing.Shui:
                if (targetWuXing == WuXing.Huo)
                {
                    damageMultiplier = 3f; // 水克火
                }
                break;
            case WuXing.Huo:
                if (targetWuXing == WuXing.Jin)
                {
                    damageMultiplier = 3f; // 火克金
                }
                break;
            case WuXing.Tu:
                if (targetWuXing == WuXing.Shui)
                {
                    damageMultiplier = 2f; // 土克水
                }
                break;
        }

        // 计算实际伤害  
        int damage = (int)(baseDamage * damageMultiplier);

        return damage;
    }
    /// <summary>
    /// 防御行为
    /// </summary>
    /// <param name="_Initiator"></param>
    /// <param name="_target"></param>
    void defense(GameObject _Initiator, GameObject _target)
    {
        Debug.Log("防御");



    }
    /// <summary>
    /// 恢复行为
    /// </summary>
    /// <param name="_Initiator"></param>
    /// <param name="_target"></param>
    void restore(GameObject _Initiator, GameObject _target)
    {
        Debug.Log("恢复");
    }
}

public class ASetOfCard : AAllCardFunction// ASetOfCard aSetOfCard = new ASetOfCard();//这行代码等效于在现实生活中拿出一副卡牌，只是这是电子的卡牌，而且是在电子现实中掏出来的
{
    public Data_LoadTo_Card m_emuAllCardList;//所有的牌
    public int[] m_iarrIsSelectCard;//那个01的东西
    public List<Card> m_allCardList = new List<Card>();//储存所有卡牌，用于加载在 地图界面的背包列表里
    public ASetOfCard()
    {
        fnWhenYouCreateThisClass();
        fnLoadiarrIsSelectCard();

    }



    // 保存或者加载iarrIsSelectCard
    // 保存或者加载iarrIsSelectCard
    // 保存或者加载iarrIsSelectCard

    public void fnSaveiarrIsSelectCard()//这是一个保存，保存iarrIsSelectCard的数据
    {
        var path = Path.Combine(Application.persistentDataPath, "Datas/PlayerDatas/");//路径的位置
        var sPath = Path.Combine(path, "DeckCard.json");//文件的路径
        bool bNeverUse = fnLoadiarrIsSelectCard_2(path);//先看路径在不在，不再就创建
        if (!fnLoadiarrIsSelectCard_3(sPath))//判断下文件存不存在,不存在就创建一个
        {
            fnSaveiarrIsSelectCard_2(sPath);
            return;
        }
        else//存在的话就直接保存
        {
            fnSaveiarrIsSelectCard_2(sPath);

        }
    }
    void fnSaveiarrIsSelectCard_2(string _path)
    {
        DeckCard_Json_String deckCard_Json_String = new DeckCard_Json_String();
        deckCard_Json_String.DeckCardList = m_iarrIsSelectCard;
        var jsonstring = JsonUtility.ToJson(deckCard_Json_String);
        File.WriteAllText(_path, jsonstring);
    }

    public void fnLoadiarrIsSelectCard()//这是一个加载,加载iarrIsSelectCard的数据
    {

        var path = Path.Combine(Application.persistentDataPath, "Datas/PlayerDatas/");//路径的位置
        var sPath = Path.Combine(path, "DeckCard.json");//文件的路径
        bool bNeverUse = fnLoadiarrIsSelectCard_2(path);//先看路径在不在，不再就创建
        m_iarrIsSelectCard = new int[200];
        if (!fnLoadiarrIsSelectCard_3(sPath))//判断下文件存不存在,不存在就创建一个
        {
            int[] iarrTmp = new int[200];
            for (int i = 0; i < 200; i++)
            {
                iarrTmp[i] = 0;
            }
            m_iarrIsSelectCard = iarrTmp;
            return;
        }
        else
        {
            DeckCard_Json_String deckCard_Json_String = new DeckCard_Json_String();
            deckCard_Json_String = JsonUtility.FromJson<DeckCard_Json_String>(File.ReadAllText(sPath));
            m_iarrIsSelectCard = deckCard_Json_String.DeckCardList;
        }
    }
    bool fnLoadiarrIsSelectCard_2(string _sPath)//这是一个判断路径是否存在的函数
    {
        // 检查路径是否存在
        if (Directory.Exists(_sPath))
        {
            // 路径存在，返回 true
            return true;
        }
        else
        {
            // 路径不存在，创建路径
            try
            {
                // 获取路径中的各个目录部分
                string[] directories = _sPath.Split(Path.DirectorySeparatorChar);
                string currentPath = "";

                foreach (string directory in directories)
                {
                    if (!string.IsNullOrEmpty(directory))
                    {
                        currentPath = Path.Combine(currentPath, directory);

                        // 如果当前路径不存在，则创建它
                        if (!Directory.Exists(currentPath))
                        {
                            Directory.CreateDirectory(currentPath);
                        }
                    }
                }

                // 路径创建成功，返回 false（因为路径原本不存在）
                return false;
            }
            catch (Exception ex)
            {
                // 处理创建目录时可能发生的异常
                Debug.LogError("Error creating directory: " + ex.Message);
                return false;
            }
        }
    }

    bool fnLoadiarrIsSelectCard_3(string filePath)//这是一个判断文件是否存在的函数，存在返回true，不存在返回false
    {
        // 使用 File.Exists 方法检查文件是否存在
        return File.Exists(filePath);
    }

    // 保存或者加载iarrIsSelectCard
    // 保存或者加载iarrIsSelectCard
    // 保存或者加载iarrIsSelectCard


    //以下是游戏开始时的准备工作，一般不进行调用
    //以下是游戏开始时的准备工作，一般不进行调用
    //以下是游戏开始时的准备工作，一般不进行调用

    void fnWhenYouCreateThisClass()
    {
        m_emuAllCardList = new Data_LoadTo_Card();
        m_emuAllCardList.attackCards = new List<AttackCard>();
        m_emuAllCardList.defenseCards = new List<DefenseCard>();
        m_emuAllCardList.restoreCards = new List<RestoreCard>();

        fnInstanceCardData();
    }


    void fnInstanceCardData()
    {
        string sData = "#,编号,卡名,属性\n" +
            "AttackCard,0,攻击_金,金\n" +
            "AttackCard,1,攻击_木,木\n" +
            "AttackCard,2,攻击_水,水\n" +
            "AttackCard,3,攻击_火,火\n" +
            "AttackCard,4,攻击_土,土\n" +
            "DefenseCard,5,防御_金,金\n" +
            "DefenseCard,6,防御_木,木\n" +
            "DefenseCard,7,防御_水,水\n" +
            "DefenseCard,8,防御_火,火\n" +
            "DefenseCard,9,防御_土,土\n" +
            "RestoreCard,10,恢复_金,金\n" +
            "RestoreCard,11,恢复_木,木\n" +
            "RestoreCard,12,恢复_水,水\n" +
            "RestoreCard,13,恢复_火,火\n" +
            "RestoreCard,14,恢复_土,土\n";
        string[] sarrData = sData.Split("\n");
        foreach (string s in sarrData)
        {
            fnInstanceCardData_2(s);
        }
    }

    void fnInstanceCardData_2_atk(string[] _sarrArray)
    {
        string cardType = _sarrArray[0];
        int cardId = int.Parse(_sarrArray[1]);
        string cardName = _sarrArray[2];
        string cardElement = _sarrArray[3];
        AttackCard tmpAtk = new AttackCard(cardType, cardId, cardName, cardElement, GetFunction(_sarrArray[0]));
        m_emuAllCardList.attackCards.Add(tmpAtk);
        m_allCardList.Add(tmpAtk);
    }
    void fnInstanceCardData_2_def(string[] _sarrArray)
    {
        string cardType = _sarrArray[0];
        int cardId = int.Parse(_sarrArray[1]);
        string cardName = _sarrArray[2];
        string cardElement = _sarrArray[3];
        DefenseCard tmpDef = new DefenseCard(cardType, cardId, cardName, cardElement, GetFunction(_sarrArray[0]));
        m_emuAllCardList.defenseCards.Add(tmpDef);
        m_allCardList.Add(tmpDef);
    }

    void fnInstanceCardData_2_res(string[] _sarrArray)
    {
        string cardType = _sarrArray[0];
        int cardId = int.Parse(_sarrArray[1]);
        string cardName = _sarrArray[2];
        string cardElement = _sarrArray[3];
        RestoreCard tmpRes = new RestoreCard(cardType, cardId, cardName, cardElement, GetFunction(_sarrArray[0]));
        m_emuAllCardList.restoreCards.Add(tmpRes);
        m_allCardList.Add(tmpRes);
    }

    void fnInstanceCardData_2(string _s)
    {
        string[] sarrArray = _s.Split(',');
        m_emuAllCardList.fnOneAction(sarrArray, fnInstanceCardData_2_atk, fnInstanceCardData_2_def, fnInstanceCardData_2_res);
    }
}


public class ASetOfCard_Map : ASetOfCard
{


    public ASetOfCard_Map()
    {

    }

    public void fnMap_ShowAllCard(GameObject parentGameObject, GameObject MapCardPerfabs)//在地图界面展示所有卡片，第一个参数时候的父物体，第二个参数是卡牌预制体
    {
        fnLoadiarrIsSelectCard();//先更新下数据
        if (parentGameObject != null && MapCardPerfabs != null)//防止报错机制
        {
            foreach (var card in m_allCardList)
            {
                GameObject mid = GameObject.Instantiate(MapCardPerfabs);//在xx位置生成了一个卡牌
                fnMap_ShowAllCard_2(mid, card);
                mid.transform.SetParent(parentGameObject.transform);
            }
        }

    }
    void fnMap_ShowAllCard_2(GameObject _Card, Card card)//此方法是设置MapCard
    {
        _Card.GetComponent<MapCard>().isMapCard = card;

        if (m_iarrIsSelectCard[card.id] == 1)
        {
            
            _Card.GetComponent<MapCard>().xuanzetubiao.SetActive(true);
            _Card.GetComponent<MapCard>().bXuanzetubiao_active_or = true;
        }

        //_Card.GetComponent<MapCardScript>().MapCard = card;//将card中的值赋给挂载在MapCard预制体中的MapCard脚本中保存卡牌数据的变量
        //这个GetComponent<MapCardScript>组件应该有两个变量
        //1.bool 用于卡牌的UI显示，true为显示，false为不显示
        //2.Card 用于保存卡牌数据
    }

    //MapButton:地图中的选关卡按钮
    //MapButton:地图中的选关卡按钮    //按完之后那个数据保存在m_iarrm_iarrIsSelectCard[199]中
    //MapButton:地图中的选关卡按钮
    public void fnMapButton1()
    {
        m_iarrIsSelectCard[199] = 1;
        fnSaveiarrIsSelectCard();
    }
    public void fnMapButton2() 
    {
        m_iarrIsSelectCard[199] = 2;
        fnSaveiarrIsSelectCard();
    }
    public void fnMapButton3() 
    {
        m_iarrIsSelectCard[199] = 3;
        fnSaveiarrIsSelectCard();
    }
    public void fnMapButton4() 
    {
        m_iarrIsSelectCard[199] = 4;
        fnSaveiarrIsSelectCard();
    }
    public void fnMapButton5() 
    {
        m_iarrIsSelectCard[199] = 5;
        fnSaveiarrIsSelectCard();
    }
    public void fnMapButton6() 
    {
        m_iarrIsSelectCard[199] = 6;
        fnSaveiarrIsSelectCard();
    }



    //MapButton:地图中的选关卡按钮
    //MapButton:地图中的选关卡按钮    //按完之后那个数据保存在m_iarrm_iarrIsSelectCard[199]中
    //MapButton:地图中的选关卡按钮




}

public class ASetOfCard_Battle : ASetOfCard
{
    public Data_LoadTo_Card m_emuCardYouChose;//这个是玩家选择（也就是你在地图背包里选择的卡）的卡牌
    public Data_LoadTo_Card m_emuCardEnemyChose;//这个是敌人选择的卡牌

    public ASetOfCard_Battle()
    {
        fnFrom0101LoadData_TurnTo_Card();
        fnFrom0101LoadData_TurnTo_Card_For_Enemey();
    }










   





    //以下是游戏开始时的准备工作，一般不进行调用
    //以下是游戏开始时的准备工作，一般不进行调用
    //以下是游戏开始时的准备工作，一般不进行调用



    void fnFrom0101LoadData_TurnTo_Card()//将0101里的数据转成卡牌数据
    {
        bool bTmp = false;//用于判断0101那个数组里有没有1，要是有返回true，没有返回false
        m_emuCardYouChose = new Data_LoadTo_Card();
        m_emuCardYouChose.attackCards = new List<AttackCard>();
        m_emuCardYouChose.defenseCards = new List<DefenseCard>();
        m_emuCardYouChose.restoreCards = new List<RestoreCard>();
        for (int i = 0; i < m_allCardList.Count; i++)
        {
            if (m_iarrIsSelectCard[i] == 1)
            {
                bTmp = true;
                break;
            }
        }
        if (!bTmp)
        {
            Debug.Log("加载失败");
            return;
        }
        if (bTmp)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < m_allCardList.Count; i++)
            {
                if (m_iarrIsSelectCard[i] == 1)
                {
                    list.Add(i);
                }
            }

            m_emuCardYouChose= m_emuCardYouChose.fnFromCardIdListRetrunEmuAllCardList(list,m_allCardList);
        }
    }

   void fnFrom0101LoadData_TurnTo_Card_For_Enemey()
    {
        m_emuCardEnemyChose = new Data_LoadTo_Card();
        m_emuCardEnemyChose.attackCards = new List<AttackCard>();
        m_emuCardEnemyChose.defenseCards = new List<DefenseCard>();
        m_emuCardEnemyChose.restoreCards = new List<RestoreCard>();
        int iTmp = m_iarrIsSelectCard[199];//地图界面选的关卡，在这里使用，用来区分地图的关卡
        List<int> iarrTmpCardId = new List<int>();
        switch (iTmp)
        {
            case 1:
                iarrTmpCardId =new List<int> { 1,2,3,4,5,6};
                break;

            case 2:
                iarrTmpCardId = new List<int> { 1, 2, 3, 4, 5, 6 };
                break;

            case 3:
                iarrTmpCardId = new List<int> { 1, 2, 3, 4, 5, 6 };
                break;

            case 4:
                iarrTmpCardId = new List<int> { 1, 2, 3, 4, 5, 6 };
                break;

            case 5:
                iarrTmpCardId = new List<int> { 1, 2, 3, 4, 5, 6 };
                break;
            case 6:
                iarrTmpCardId = new List<int> { 1, 2, 3, 4, 5, 6 };
                break;
            default:
                iarrTmpCardId = new List<int> { 1, 2, 3, 4, 5, 6 };
                break;
        }
        fnFrom0101LoadData_TurnTo_Card_For_Enemey_2(iarrTmpCardId);
    }
    void fnFrom0101LoadData_TurnTo_Card_For_Enemey_2(List<int> _arr)
    {
        m_emuCardEnemyChose=new Data_LoadTo_Card();
        m_emuCardEnemyChose.attackCards = new List<AttackCard>();
        m_emuCardEnemyChose.defenseCards = new List<DefenseCard>();
        m_emuCardEnemyChose.restoreCards = new List<RestoreCard>();
        if (_arr == null)
        {
            Debug.Log("加载失败");
            return;
        }
        else
        {
            m_emuCardEnemyChose = m_emuCardEnemyChose.fnFromCardIdListRetrunEmuAllCardList(_arr,m_allCardList);
        }
}
    
}

public class BPlayer
{
    public int m_iMaxHealth;//生命值
    public int m_icurHealth;//当前生命值
    public int m_iAttackPower;//攻击力
    public int m_iDefensePower;//防御力
    public WuXing m_emuWuXingElement;//元素属性

    public int m_iMaxCardCount;//手牌上限 
    public Data_LoadTo_Card m_emuCardInHand;//手上的所有牌
    public int m_iplayerActionCount;//这些都是行动点数
    public BPlayer(int _iMaxHealth)
    {
        m_iMaxHealth = _iMaxHealth;
        m_icurHealth = m_iMaxHealth;



        m_iplayerActionCount = 3;//行动点数
    }


    public BPlayer(int _iMaxHealth,int _iAttackPower,int _iDefensePower,WuXing _emuWuXingElement)
    {
        m_iMaxHealth = _iMaxHealth;
        m_icurHealth = m_iMaxHealth;
        m_iplayerActionCount = 3;
        m_iAttackPower = _iAttackPower;
        m_iDefensePower = _iDefensePower;
        m_emuWuXingElement = _emuWuXingElement;
    }



}

class BattleManager_
{
    public float m_fhealth_change_Player;//记录玩家血量变化比例
    public float m_fhealth_change_Enemy;//记录敌人血量变化比例
    public  bool m_isCHange;//是否要检测血量变化的开关

    public bool m_bkeep_or_break;//判断是否要退出当前回合true:表示不退出当前回合，false：表示退出当前回合
    BattleState m_emucurrentState;//记录目前是谁的回合/////////////////////////////////核心
    public BPlayer m_player1;//玩家1
    public BPlayer m_player2;//玩家2
    public ASetOfCard_Battle m_aSetOfCard_Battle;//这是一副卡牌，里面的m_emu...成员变量就是要在里面随的牌



    public BattleManager_(BPlayer _player1, BPlayer _player2)//有参构造函数
    {
        //初始化玩家1和玩家2
        m_player1 = _player1;
        m_player2 = _player2;
        m_aSetOfCard_Battle=new ASetOfCard_Battle();//初始化一套卡牌



        //初始化玩家最大血量
        m_isCHange = false;
        m_fhealth_change_Enemy = 1;
        m_fhealth_change_Player = 1;




        m_emucurrentState = BattleState.PlayerAction;//开始还是玩家先吧
    }

    public void fnBattleUpdate()//拿去每帧调用的东西
    {

        fnHealth();
        fnHealthChange();
    }












    void fnHealth()//回合管理的核心
    {
        if (m_player1.m_icurHealth > 0 && m_player2.m_icurHealth > 0)
        {
            switch (m_emucurrentState)
            {
                case BattleState.PlayerAction:
                    Debug.Log("现在是玩家回合");
                    if (m_bkeep_or_break || m_player1.m_iplayerActionCount <= 0)
                    {
                        fnTurnToEnemyAction_PlayerAction_In_if();
                    }

                    break;

                case BattleState.EnemyAction:
                    Debug.Log("现在是敌人回合");
                    if (m_bkeep_or_break || m_player2.m_iplayerActionCount <= 0)
                    {
                        fnTurnToPlayerAction_EnemyAction_In_if();
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("游戏结束");
        }
    }

    void fnTurnToEnemyAction_PlayerAction_In_if()//放在那个结束玩家回合的case里
    {
        m_emucurrentState = BattleState.EnemyAction;
        m_player1.m_iplayerActionCount = 3;
        m_bkeep_or_break = false;
    }
    void fnTurnToPlayerAction_EnemyAction_In_if()
    {
        m_emucurrentState = BattleState.PlayerAction;
        m_player2.m_iplayerActionCount = 3;
        m_bkeep_or_break = false;
    }


    void fnHealthChange()//检测血量是否变化，并且记录变化比例
    {
        if (m_isCHange)
        {
            m_fhealth_change_Player = (float)m_player1.m_icurHealth / (float)m_player1.m_iMaxHealth;
            m_fhealth_change_Enemy = (float)m_player2.m_icurHealth / (float)m_player2.m_iMaxHealth;
            m_isCHange = false;
        }
    }
}





