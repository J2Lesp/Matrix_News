using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    
    public int gridSizeX = 55;
    public int gridSizeY = 31;

    public char[][] relevantWordGrid;

    public TextMeshProUGUI[][] grid;


    [SerializeField]
    Transform prefabContainer;

    [SerializeField]
    GameObject prefabLetter;

    Coroutine placingWordCoroutine;

    NewsGridManager newsGridManager;

    [SerializeField]
    bool horizontal = true;
    [SerializeField]
    bool vertical = true;
    [SerializeField]
    bool autoShow = true;

    [SerializeField]
    float timerWait = 1.0f;
    void Start()
    {
        SetGrid();
        newsGridManager = GetComponent<NewsGridManager>();

        relevantWordGrid = new char[gridSizeX][];
        grid = new TextMeshProUGUI[gridSizeX][];

        int i = 0;
        foreach (char[] tmp in relevantWordGrid)
        {
            relevantWordGrid[i] = new char[gridSizeY];
            i++;
        }

        i = 0;
        foreach (TextMeshProUGUI[] tmp in grid)
        {
            grid[i] = new TextMeshProUGUI[gridSizeY];
            i++;
        }

        int x = 0;
        int y = 0;
        while (y < gridSizeY)
        {
            while (x < gridSizeX)
            {
                GameObject go = GameObject.Instantiate(prefabLetter);
                go.transform.SetParent(prefabContainer, false);
                TextMeshProUGUI tmpGo = go.GetComponent<TextMeshProUGUI>();
                tmpGo.text = x.ToString();
                go.transform.position = getGridPos(x, y);
                relevantWordGrid[x][y] = ' ';
                go.SetActive(true);
                grid[x][y] = tmpGo;
                x++;
            }
            x = 0;
            y++;
        }

        if (placingWordCoroutine != null)
            StopCoroutine(placingWordCoroutine);
        placingWordCoroutine = StartCoroutine(PlacingWord());
    }

    IEnumerator PlacingWord()
    {
        yield return new WaitForSeconds(timerWait);

        if (newsGridManager.words.Count == 0)
        {
            placingWordCoroutine = StartCoroutine(PlacingWord());
            yield break;
        }
        bool isHorizontal = false;

        if (horizontal)
            isHorizontal = true;
        if (vertical)
            isHorizontal = false;
        if (horizontal && vertical)
            isHorizontal = UnityEngine.Random.value < 0.5f;


        string relevantword = newsGridManager.words[0];
        newsGridManager.words.RemoveAt(0);
        

        if (isHorizontal)
        {
            int x = UnityEngine.Random.Range(0, gridSizeX - relevantword.Length);
            int y = UnityEngine.Random.Range(0, gridSizeY);
            StartCoroutine(RemovingWord(relevantword, x, y, isHorizontal));
            Debug.LogError(relevantword);
            foreach (char c in relevantword)
            {
                relevantWordGrid[x][y] = Char.ToUpper(c);
                GetComponent<GameManager>().StartCustomLine(x, UnityEngine.Random.Range(0.0f, 1.0f));
                x++;
            }
        }
        else
        {
            int x = UnityEngine.Random.Range(0, gridSizeX);
            int y = UnityEngine.Random.Range(0, gridSizeY - relevantword.Length);
            StartCoroutine(RemovingWord(relevantword, x, y, isHorizontal));
            foreach (char c in relevantword)
            {
                relevantWordGrid[x][y] = Char.ToUpper(c);
                GetComponent<GameManager>().StartCustomLine(x, UnityEngine.Random.Range(0.0f, 1.0f));
                y++;
            }
        }
        

        placingWordCoroutine = StartCoroutine(PlacingWord());
    }

    IEnumerator RemovingWord(string word,int x, int y, bool isHorizontal)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(9.0f, 10.0f));

        if (isHorizontal)
        {
            foreach (char c in word)
            {
                relevantWordGrid[x][y] = ' ';
                x++;
            }
        }
        else
        {
            foreach (char c in word)
            {
                relevantWordGrid[x][y] = ' ';
                y++;
            }
        }
    }

    void SetGrid()
    {
        gridSizeX = Screen.width / 20;
        gridSizeY = Screen.height / 20;
    }

    Vector3 getGridPos(int x, int y)
    {
        float xPos = ((Screen.width / gridSizeX) * x);
        float yPos = ((Screen.height / gridSizeY) * -y + Screen.height);

        return new Vector3(xPos, yPos, 0);
    }

}
