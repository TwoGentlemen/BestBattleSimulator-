using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance = null;

    [SerializeField] private int startMoney = 100;

    [Space(5)]
    [SerializeField] private Text textMoney;
    
    private int currentMoney = 0;

    private void Awake()
    {
        if(instance != null) { Debug.LogError("PlayerStats instance is compare!!!");}
        instance = this;
    }

    private void Start()
    {
        currentMoney = startMoney;
        SetTextMoney();
    }

    private void SetTextMoney()
    {
        textMoney.text = currentMoney + "$";
    }
    public void Sell(int price)
    {
        currentMoney+=price;

        if(currentMoney > startMoney) { currentMoney = startMoney;}

        SetTextMoney();
    }

    public bool Buy(int price)
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
}
