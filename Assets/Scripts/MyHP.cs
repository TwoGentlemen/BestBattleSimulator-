using System;
using UnityEngine;

public class MyHP : MonoBehaviour
{
    [SerializeField]protected int hp = 10;
    public void Damage(int damage) //����� ���������� �� ��������� ����� �� ������ �����
    {
        hp -= damage;
        if (hp <= 0) { Death(); }
    }

    virtual public void Death()
    {
        
    }
}
