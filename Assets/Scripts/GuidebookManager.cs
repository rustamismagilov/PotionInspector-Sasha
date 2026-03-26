using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GuidebookManager : MonoBehaviour
{
    [SerializeField] private TMP_Text leftTitle;
    [SerializeField] private TMP_Text leftBodyText;
    [SerializeField] private TMP_Text rightTitle;
    [SerializeField] private TMP_Text rightBodyText;

    [SerializeField] private List<PageSO> pages;
    [SerializeField] private TMP_Text[] pageText;
    [SerializeField] private float textFadeSpeed = 0.5f;
    [SerializeField] private float pageTurnSpeed = 0.5f;
    
    private PageSO currentPage;
    private BookManager bookManager;
    private int pageIndex = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetPage();
        bookManager = FindFirstObjectByType<BookManager>();
        
        foreach (var text in pageText) //set initial text alpha value to 0, so the text can fade in upon opening the menu
        {
            text.alpha = 0;
        }
    }

    private void GetPage() //fetch data from pageSO and place text in appropriate locations
    {
        currentPage = pages[pageIndex];
        
        leftTitle.text = currentPage.GetLeftTitle();
        leftBodyText.text = currentPage.GetLeftBody();
        rightTitle.text = currentPage.GetRightTitle();
        rightBodyText.text = currentPage.GetRightBody();

    }
    
    private void FadeInText() //page text fades in to full alpha
    {
        foreach (var text in pageText)
        {
            text.DOFade(1, textFadeSpeed);
        }
    }
    
    private void FadeOutText() //page text fades from full alpha to 0 
    {
        foreach (var text in pageText)
        {
            text.DOFade(0, textFadeSpeed);
        }
    }
    
    private void PageCountUp() 
    {
        if (pageIndex >= pages.Count - 1)
        {
            pageIndex = 0;
        }
        else pageIndex++;
    }

    private void PageCountDown()
    {
        if (pageIndex <= 0)
        {
            pageIndex = pages.Count;
        } 
        pageIndex--;
    }

    IEnumerator NextPage() //turn to next page
    { 
        FadeOutText(); //text fades out before page flip anim plays
        bookManager.TurnPageNext();
        yield return new WaitForSeconds(pageTurnSpeed); //new page contents will not display until page has turned
        PageCountUp();
        GetPage();
        FadeInText();
    }
    
    IEnumerator PreviousPage() //turn to previous page
    {
        FadeOutText(); //text fades out before page flip anim plays
        bookManager.TurnPageBack();
        yield return new WaitForSeconds(pageTurnSpeed); //new text does not display until page flip anim plays
        PageCountDown();
        GetPage();
        FadeInText();
    }

    public void OnBookOpen()
    {
        GetPage();
        FadeInText();
    }

    public void OnBookClose()
    {
        GetPage();
        FadeOutText();
    }
    
    public void TurnToNextPage()
    {
        StartCoroutine(NextPage());
    }

    public void TurnToPrevPage()
    {
        StartCoroutine(PreviousPage());
    }
}
