using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main headr")]
    [SerializeField] private string header = "Best Battle Simulator";
    [SerializeField] private Text textMainHeader;
    [SerializeField] private float allTimePrint = 8;

    [SerializeField] private Animator animator;

    [SerializeField] private GameObject panelCompany;
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelSettings;


    IEnumerator SlowTextPrinting()
    {
        float timePause = allTimePrint / header.Length;
        textMainHeader.text = "";

        for (int i =0; i < header.Length; i++)
        {
            textMainHeader.text +=header[i];
            yield return new WaitForSeconds(timePause);
        }
    }

    public void PressButtonCompany()
    {
        panelMenu.SetActive(false);
        animator.SetTrigger("ShowCompany");
    }
    public void PressButtonBackMenu()
    {
        panelCompany.SetActive(false);
        animator.SetTrigger("ShowMenu");
    }
    public void PressButtonBackMenuSettings()
    {
        panelSettings.SetActive(false);
        animator.SetTrigger("ShowMenuSettings");
    }
    public void PressButtonSettings()
    {
        panelMenu.SetActive(false);
        animator.SetTrigger("ShowSettings");
    }
    public void ShowPanelSettings()
    {
        panelSettings.SetActive(true);
    }

    public void ShowPanelMenu()
    {
        panelMenu.SetActive(true);
    }
    public void ShowPanelCompany()
    {
        panelCompany.SetActive(true);
    }

    public void ButtonPressExit()
    {
        Application.Quit();
    }

    private void Start()
    {
        StartCoroutine(SlowTextPrinting());
    }
}
