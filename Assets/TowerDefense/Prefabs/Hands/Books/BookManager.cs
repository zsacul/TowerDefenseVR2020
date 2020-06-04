using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public GameObject Lmarker, Rmarker;
    public GameObject Lpage, Rpage;
    public GameObject Mpage;

    public bool BookOpened = false;
    public bool VRPageControll = false;
    public float FloatingPagePosition = 0.0f;
    private int bookpage = 0;
    public void EnableMarkers()
    {
        Lmarker.SetActive(true);
        Rmarker.SetActive(true);
    }

    public void DisableMarkers()
    {
        Lmarker.SetActive(false);
        Rmarker.SetActive(false);
    }

    public void TurnLeft()
    {
        if (bookpage > 0)
        {
            Debug.Log("call to flip left!");
            bookpage -= 2;
            Lpage.GetComponent<BookPageMngr>().Display_nth(bookpage);
            Rpage.GetComponent<BookPageMngr>().Display_nth(bookpage);
            BookOpened = true;
            Mpage.SetActive(true);
        }
    }

    public void TurnRight()
    {
        if (bookpage < 10)
        {
            Debug.Log("call to flip right!");
            bookpage += 2;
            Lpage.GetComponent<BookPageMngr>().Display_nth(bookpage);
            Rpage.GetComponent<BookPageMngr>().Display_nth(bookpage);
            BookOpened = true;
            Mpage.SetActive(true);
        }
    }

    public void LeaveBook()
    {
        VRPageControll = false;
        Mpage.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 85.0f);
        FloatingPagePosition = Mpage.transform.localRotation.eulerAngles.z;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (VRPageControll == false && BookOpened == true)
        {
            if (FloatingPagePosition > 90.0f)
            {
                FloatingPagePosition += 45.0f * Time.deltaTime;
            }
            else
            {
                FloatingPagePosition -= 45.0f * Time.deltaTime;
            }

            Mpage.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, FloatingPagePosition);

            if (FloatingPagePosition > 180.0f)
            {
                BookOpened = false;
                EnableMarkers();
                Mpage.SetActive(false);
                Mpage.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            }

            if (FloatingPagePosition < 0.0f)
            {
                BookOpened = false;
                EnableMarkers();
                Mpage.SetActive(false);
                Mpage.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            }
        }
    }
}
