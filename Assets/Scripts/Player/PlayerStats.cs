using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int money, cotton, whiteDye, blackDye, blueDye, redDye, yellowDye;
    public TMP_Text moneyText, whiteText, blackText, blueText, redText, yellowText, cottonText;

    public Image trendSprite;
    
    public PlayerUnlocks unlock;

    public GameObject myShop;

    public GameObject openedGUI;

    public int currentColorTrend;
    public int currentClothingTrend;

    public AudioSource selectSound, buySound, brokeSound, badSound, packagedSound;
    public AudioSource music;
    public float soundVolume, musicVolume; 

    public Slider soundSlider, musicSlider;

    private float trendTime;
    private float totalTrendTime;

    void Start()
    {
        NewTrend();
    }
    void Update()
    {
        moneyText.text = money.ToString();
        whiteText.text = whiteDye.ToString();
        blackText.text = blackDye.ToString();
        blueText.text = blueDye.ToString();
        redText.text = redDye.ToString();
        yellowText.text = yellowDye.ToString();
        cottonText.text = cotton.ToString();

        trendSprite.color = GameObject.Find("ProductValueList").GetComponent<ValueList>().colors[currentColorTrend].GetComponent<ProductValue>().myColor;
        trendSprite.sprite = GameObject.Find("ProductValueList").GetComponent<ValueList>().clothing[currentClothingTrend].GetComponent<ProductValue>().myClothing;

        trendTime += Time.deltaTime;
        if (trendTime >= totalTrendTime)
        {
            NewTrend();
        }

        selectSound.volume = soundVolume;
        buySound.volume = soundVolume;
        brokeSound.volume = soundVolume;
        badSound.volume = soundVolume;
        packagedSound.volume = soundVolume;
        music.volume = musicVolume;
    }

    public void NewSound()
    {
        soundVolume = 1 - soundSlider.value;
    }

    public void NewMusic()
    {  
        musicVolume = 1 - musicSlider.value;
    }

    private void NewTrend()
    {
        currentColorTrend = Random.Range(0, 40);
        currentClothingTrend = Random.Range(0, 10);
        trendTime = 0;
        totalTrendTime = Random.Range(1, 6) * 60;
    }

    public void OpenShop(GameObject firstOpen)
    {
        myShop.SetActive(true);
        myShop.GetComponent<ShopPanel>().Open(firstOpen);
    }
}
