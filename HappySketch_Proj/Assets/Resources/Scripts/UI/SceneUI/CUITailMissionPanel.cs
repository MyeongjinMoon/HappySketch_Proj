using HakSeung;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CUITailMissionPanel : CUIScene
{
    enum ETailImage
    {
        BACKGROUND,
        DINO
    }

    private const int TAILLIFENUM = 3;
    private const int TAILLIFEIMAGENUM = 2;
    
    [SerializeField] private GameObject[] tailLifes = new GameObject[TAILLIFENUM];
    [SerializeField] private GameObject[] dinoImageObjects = new GameObject[TAILLIFENUM];
    
    private Image[,] tailLifeImages                 = new Image[TAILLIFENUM, TAILLIFEIMAGENUM];

    private float startScale;
    private float spinSpeed;
    private int currentLife;

    protected override void InitUI()
    {
        currentLife = 0;
        spinSpeed = 30f;
        startScale = tailLifes[0].transform.localScale.x;

        for (int imageIndex = 0; imageIndex < tailLifes.Length; imageIndex++)
        {
            Image[] images = tailLifes[imageIndex].GetComponentsInChildren<Image>();

            tailLifeImages[imageIndex, (int)ETailImage.BACKGROUND] = images[(int)ETailImage.BACKGROUND];
            tailLifeImages[imageIndex, (int)ETailImage.DINO]       = images[(int)ETailImage.DINO];

            tailLifeImages[imageIndex, (int)ETailImage.BACKGROUND].color = Color.white;
            tailLifeImages[imageIndex, (int)ETailImage.DINO].color       = Color.white;

            dinoImageObjects[imageIndex].SetActive(true);
        }
    }

    public override void Show()
    {
        base.Show();
        currentLife = 0;

        for (int imageIndex = 0; imageIndex < tailLifes.Length; imageIndex++)
        {
            tailLifeImages[imageIndex, (int)ETailImage.BACKGROUND].color = Color.white;
            tailLifeImages[imageIndex, (int)ETailImage.DINO].color = Color.white;

            tailLifes[imageIndex].transform.localScale = new Vector3(startScale,
                                                                tailLifes[imageIndex].transform.localScale.y,
                                                                1);

            dinoImageObjects[imageIndex].SetActive(true);
        }
    }

    public void OnFailedEvent()
    {
        if (currentLife < TAILLIFENUM)
        {
            StartCoroutine(SpinImages(currentLife));
            currentLife++;
        }
    }

    private IEnumerator SpinImages(int lifeIndex)
    {
        
        while(-startScale <= tailLifes[lifeIndex].transform.localScale.x)
        {
            tailLifes[lifeIndex].transform.localScale = new Vector3(tailLifes[lifeIndex].transform.localScale.x - spinSpeed * Time.deltaTime,
                                                                    tailLifes[lifeIndex].transform.localScale.y,
                                                                    1);
            if (dinoImageObjects[lifeIndex].activeSelf == tailLifes[lifeIndex].transform.localScale.x <= 0)
                dinoImageObjects[lifeIndex].SetActive(false);

            yield return null;
        }

        tailLifes[lifeIndex].transform.localScale = new Vector3(-startScale,
                                                                tailLifes[lifeIndex].transform.localScale.y,
                                                                1);


        while (startScale >= tailLifes[lifeIndex].transform.localScale.x)
        {
            tailLifes[lifeIndex].transform.localScale = new Vector3(tailLifes[lifeIndex].transform.localScale.x + spinSpeed * Time.deltaTime,
                                                                    tailLifes[lifeIndex].transform.localScale.y,
                                                                    1);

            if (!dinoImageObjects[lifeIndex].activeSelf == tailLifes[lifeIndex].transform.localScale.x >= 0)
                dinoImageObjects[lifeIndex].SetActive(true);

            yield return null;
        }

        tailLifes[lifeIndex].transform.localScale = new Vector3(startScale,
                                                                tailLifes[lifeIndex].transform.localScale.y,
                                                                1);

        for (int imageIndex = 0; imageIndex < TAILLIFEIMAGENUM; imageIndex++)
            tailLifeImages[lifeIndex, imageIndex].color = Color.gray;

    }
}
