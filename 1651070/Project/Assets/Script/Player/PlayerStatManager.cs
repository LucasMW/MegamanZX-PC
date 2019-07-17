using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class PlayerStatManager : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerControl playerControl;
    float dashAmount;
    float hpAmount;
    public float maxDash;
    public float maxHP;
    public GameObject VisualHP, VisualDash, DashAmount, HPAmount;
    private TextMeshProUGUI HP;
    private TextMeshProUGUI Dash;
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        hpAmount = maxHP;
        dashAmount = maxDash = playerControl.totalDash;
        HP = HPAmount.GetComponent<TextMeshProUGUI>();
        Dash = DashAmount.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (playerControl.canCDdash)
        {
            dashAmount = Mathf.Lerp(dashAmount, playerControl.currentDash, playerControl.currentDash*Time.deltaTime);
        }else*/
        dashAmount = Mathf.SmoothStep(dashAmount, playerControl.currentDash, 0.2f);
        VisualHP.GetComponent<Image>().fillAmount = hpAmount/100;
        VisualDash.GetComponent<Image>().fillAmount = dashAmount/100;
        Dash.text = (Mathf.RoundToInt(dashAmount)).ToString();
        HP.text = ((int)hpAmount).ToString();
    }
}
