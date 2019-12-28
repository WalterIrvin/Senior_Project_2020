using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    public void EndGame()
    {
        Application.Quit();
    }
    void Update()
    {
        if (Input.GetAxis("Cancel") > 0)
        {
            this.gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }
}
