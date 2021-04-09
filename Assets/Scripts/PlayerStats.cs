using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance = null;

    [SerializeField] private int startMoney = 100;
    [SerializeField] private int startMoneyRedTeam = 100;

    [Space(5)]
    [SerializeField] private Text textMoney;
    [SerializeField] private Text textMoneyRedTeam;
    
    private int currentMoney = 0;
    private int currentMoneyRedTeam = 0;

    private void Awake()
    {
        if(instance != null) { Debug.LogError("PlayerStats instance is compare!!!");}
        instance = this;
    }

    private void Start()
    {
        currentMoney = startMoney;
        currentMoneyRedTeam = startMoneyRedTeam;
        SetTextMoney();
    }

    private void SetTextMoney()
    {
        textMoney.text = currentMoney + "$";
        textMoneyRedTeam.text = currentMoneyRedTeam + "$";
    }
    public void Sell(int price,bool isBlueTeam)
    {
        if (isBlueTeam)
        {
            currentMoney+=price;
            if(currentMoney > startMoney) { currentMoney = startMoney;}
            SetTextMoney();
        }
        else
        {
            currentMoneyRedTeam += price;
            if (currentMoneyRedTeam > startMoneyRedTeam) { currentMoneyRedTeam = startMoneyRedTeam; }
            SetTextMoney();
        }
    }

    public bool Buy(int price, bool isBlueTeam)
    {
        if (isBlueTeam)
        {

        
        if(currentMoney-price < 0)
        {
            return false;
        }
        else
        {
            currentMoney-=price;
            SetTextMoney();
            return true;
        }
        }
        else
        {
            if (currentMoneyRedTeam - price < 0)
            {
                return false;
            }
            else
            {
                currentMoneyRedTeam -= price;
                SetTextMoney();
                return true;
            }
        }
    }
}
