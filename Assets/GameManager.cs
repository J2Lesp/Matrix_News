using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    string st = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!\"#$%&\'()*+,./0123456789:;<=>?[\\]^_`{|}~";

    [SerializeField]
    GridManager gridManager;

    List<int> listInt = new List<int>();

    void Start()
    {
        StartCoroutine(LineManager(0.18f));
        StartCoroutine(LineManager(0.127f));
        StartCoroutine(LineManager(0.089f));
        StartCoroutine(LineManager(0.151f));
        StartCoroutine(LineManager(0.1f));
        StartCoroutine(LineManager(0.12f));
        StartCoroutine(LineManager(0.09f));
        StartCoroutine(LineManager(0.11f));
    }

    public void StartCustomLine(int x, float delay = 0.0f)
    {
        StartCoroutine(ColoneCoroutine(x, 0, delay));
    }


    IEnumerator LineManager(float timetoWait)
    {
        yield return new WaitForSeconds(timetoWait);
        int x = Random.Range(0, gridManager.gridSizeX);
        if (listInt.Contains(x))
            StartCoroutine(LineManager(timetoWait));
        else
        {
            listInt.Add(x);
            StartCoroutine(ColoneCoroutine(x, 0));
            StartCoroutine(LineManager(timetoWait));
        }
    }

    IEnumerator ColoneCoroutine(int x, int y, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        if (y < gridManager.gridSizeY)
        {
            yield return new WaitForEndOfFrame();
            UpdateLetter(x, y);
            y++;
            StartCoroutine(ColoneCoroutine(x, y));
        }
        else
            listInt.Remove(x);
    }

    void UpdateLetter(int x, int y)
    {
        if (gridManager.relevantWordGrid[x][y] != ' ')
        {
            gridManager.grid[x][y].text = gridManager.relevantWordGrid[x][y] + "";
            gridManager.grid[x][y].color = Color.white;
            gridManager.grid[x][y].GetComponent<LetterController>().fadeoutSpeed = 0.1f;
        }
        else
        {
            gridManager.grid[x][y].color = Color.green;
            gridManager.grid[x][y].text = st[Random.Range(0, st.Length)] + "";
            gridManager.grid[x][y].GetComponent<LetterController>().fadeoutSpeed = 0.4f;
        }
    }
}
