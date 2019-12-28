using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSwitcher : MonoBehaviour
{
    public void SetLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
    private void Update()
    {
        if (Input.GetAxis("Submit") > 0)
        {
            this.gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }
}
