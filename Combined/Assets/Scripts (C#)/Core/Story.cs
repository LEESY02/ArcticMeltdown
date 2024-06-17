using UnityEngine;
using UnityEngine.SceneManagement;

public class Story : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;
    private int pageCount;
    private Tracker tracker;

    // Start is called before the first frame update
    void Start()
    {
        pageCount = 0;
        for (int i = 0; i < pages.Length; i++) // deactivate all pages
        {
            pages[i].SetActive(false);
        }
        pages[pageCount].SetActive(true); // activate the first page
        tracker = FindFirstObjectByType<Tracker>();
    }

    public void MainMenu()
    {
        tracker.loadCount++;
        SceneManager.LoadScene(0);
    }

    public void NextPage()
    {
        if (pageCount < pages.Length - 1)
        {
            pageCount++;
            pages[pageCount].SetActive(true); //activate next page
            pages[pageCount - 1].SetActive(false); //deactivate current page
        }
    }

    public void PreviousPage()
    {
        if (pageCount > 0)
        {
            pageCount--;
            pages[pageCount + 1].SetActive(false); //deactivate current page
            pages[pageCount].SetActive(true); //actiavte previous page
        }
    }
}
