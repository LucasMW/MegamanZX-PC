using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class PlayerStatManager : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerControl playerControl;
    float dashAmount;
    float hpAmount;
    float visualHPAmount;
    public float maxDash;
    public float maxHP;
    public GameObject VisualHP, VisualDash, DashAmount, HPAmount;
    private TextMeshProUGUI HP;
    private TextMeshProUGUI Dash;
    public float dodgeBonus;
    SoundManager soundManager ;
    GameObject Explosion;
    bool dying = false;
    void Start()
    {
        Explosion = transform.Find("PlayerExplode").gameObject;
       soundManager = SoundManager._instance;
        playerControl = GetComponent<PlayerControl>();
        hpAmount = maxHP;
        dashAmount = maxDash = playerControl.totalDash;
        HP = HPAmount.GetComponent<TextMeshProUGUI>();
        Dash = DashAmount.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        dashAmount = Mathf.SmoothStep(dashAmount, playerControl.currentDash, 0.2f);
        visualHPAmount = Mathf.SmoothStep(visualHPAmount, hpAmount, 0.2f);
        VisualHP.GetComponent<Image>().fillAmount = visualHPAmount / 100;
        VisualDash.GetComponent<Image>().fillAmount = dashAmount/100;
        Dash.text = (Mathf.RoundToInt(dashAmount)).ToString();
        HP.text = (Mathf.RoundToInt(visualHPAmount)).ToString();
        if(hpAmount == 0&&!dying)
        {
            Death();
            dying = true;
        }
    }
    public void Dodge()
    {
        Debug.Log("Dodge called");
        soundManager.PlaySound("Dodge");
        HPchange(10);
        if(playerControl.currentDash <= maxDash)
        {
            if (playerControl.currentDash + dodgeBonus < maxDash)
            {
                playerControl.currentDash += dodgeBonus;
                dashAmount += dodgeBonus;
            }
            else
            {
                playerControl.currentDash = maxDash;
                dashAmount = maxDash;
            }
        }
    }
    public void HPchange(float amount)
    {
        if(hpAmount+amount > 0)
        {
            if (hpAmount + amount > maxHP)
            {
                hpAmount = maxHP;
            }
            else
            {
                hpAmount += amount;
            }
        }
        else
        {
            hpAmount = 0;
        }
    }
    private void Death()
    {
        transform.Find("Hurtbox").GetComponent<HurtboxPlayer>().dead = true;
        transform.Find("DodgeBox").GetComponent<DodgeBox>().dead = true;
        GetComponent<Animator>().SetTrigger("Death");
        GetComponent<PlayerControl>().enabled = false;
        soundManager.PlaySound("ZX Death");
    }
    private void Explode()
    {
        soundManager.PlaySound("EnemyBasicDie");
        GetComponent<SpriteRenderer>().enabled = false;
        Explosion.SetActive(true);
    }
}
