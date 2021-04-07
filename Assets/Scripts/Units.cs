using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="Mob",menuName ="Unit/mob")]
public class Units : ScriptableObject
{
    [Header("Object mob")]
    public GameObject mob = null;

    [Space(5)]
    [Header("Parametors")]
    public string Name = "name";
    public int Price = 10;
    public Image image = null;
    


}
