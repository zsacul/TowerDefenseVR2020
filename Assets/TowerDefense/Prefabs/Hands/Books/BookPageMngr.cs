using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPageMngr : MonoBehaviour
{
    public List<GameObject> BookPages = new List<GameObject>();
    private List<GameObject> LeftBookPages = new List<GameObject>();
    private List<GameObject> RightBookPages = new List<GameObject>();

    // Start is called before the first frame update

    public void Display_nth(int page)
    {
        if (page < 0 || page > RightBookPages.Count - 1)
        {
            Debug.Log($"[Book Manager] critical sanity check failed. Page Display_nth has been called with illegal argument! {page}");
            return;
        }

        foreach (GameObject LBP in LeftBookPages)
            LBP.SetActive(false);
        foreach (GameObject RBP in RightBookPages)
            RBP.SetActive(false);

        LeftBookPages[page].SetActive(true);
        RightBookPages[page + 1].SetActive(true);
    }
    void Start()
    {
        GameObject pagehook = transform.GetChild(0).gameObject;

        foreach (GameObject BookPage in BookPages)
        {
            GameObject pp = Instantiate(BookPage, pagehook.transform);
            pp.name = "front_" + pp.name;
            pp.SetActive(false);
            LeftBookPages.Add(pp);

            GameObject pl = Instantiate(BookPage, pagehook.transform);
            Vector3 cTransform = pl.transform.localPosition;
            cTransform.y -= 1.2f;
            pl.transform.localPosition = cTransform;
            pl.SetActive(false);
            RightBookPages.Add(pl);
        }

        Display_nth(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
