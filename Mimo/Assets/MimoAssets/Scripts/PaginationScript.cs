using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaginationScript : MonoBehaviour
{
    public Text firstText, secText, thirdText, firstHilText, secHilText, thirdHilText;
    public GameObject firstHighlight, secHighlight, thirdHighlight, paginationPanel, thirdPage;
    private int curPage, totalPages;

    public void sort(int curPage, int totalPages)
    {
        Debug.Log("Sorting Pagination");
        this.curPage = curPage;
        this.totalPages = totalPages;

        firstHighlight.SetActive(false);
        secHighlight.SetActive(false);
        thirdHighlight.SetActive(false);

        if (totalPages > 1)
        {

            if (curPage < 3 || (curPage == 3 && totalPages == 3))
            {
                firstText.text = 1 + "";
                firstHilText.text = 1 + "";
                secText.text = 2 + "";
                secHilText.text = 2 + "";

                if (totalPages > 2)
                {
                    thirdText.text = 3 + "";
                    thirdHilText.text = 3 + "";
                   thirdPage.SetActive(true);
                }
                else
                   thirdPage.SetActive(false);
            }
            else if (curPage < totalPages)
            {

               thirdPage.SetActive(true);

                firstText.text = curPage - 1 + "";
                firstHilText.text = curPage - 1 + "";
                secText.text = curPage + "";
                secHilText.text = curPage + "";
                thirdText.text = curPage + 1 + "";
                thirdHilText.text = curPage + 1 + "";
            }
            else if(curPage == totalPages)
            {
               thirdPage.SetActive(true);

                firstText.text = curPage - 2 + "";
                firstHilText.text = curPage - 2 + "";
                secText.text = curPage - 1 + "";
                secHilText.text = curPage - 1 + "";
                thirdText.text = curPage + "";
                thirdHilText.text = curPage + "";
            }

        }
        else
            paginationPanel.SetActive(false);

        Debug.Log("Current:" + curPage + " Total:" + totalPages);

        if (curPage == 1)
            firstHighlight.SetActive(true);
        else if (curPage == totalPages && curPage != 2)
            thirdHighlight.SetActive(true);
        else
            secHighlight.SetActive(true);

        FindObjectOfType<RoyalRumbleScript>().goToPage(curPage);
    }


    public void goToNextPage()
    {
        if (curPage < totalPages)
        {
            curPage++;
            FindObjectOfType<PaginationScript>().sort(curPage, totalPages);
        }
    }

    public void goToPrevPage()
    {
        if (curPage > 1)
        {
            curPage--;
            sort(curPage, totalPages);
        }
    }

    public void goToLeftPage()
    {
        int page = Convert.ToInt32(firstText.text);

        if (curPage != page)
        {
            curPage = page;
            sort(curPage, totalPages);
        }
    }
    public void goToRightPage()
    {
        int page = Convert.ToInt32(thirdText.text);

        if (curPage != page)
        {
            curPage = page;
            sort(curPage, totalPages);
        }
    }

    public void goToMiddlePage()
    {
        int page = Convert.ToInt32(secText.text);

        if (curPage != page)
        {
            curPage = page;
            sort(curPage, totalPages);
        }
    }

}
