using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReadyUp : MonoBehaviour
{
    private bool player1 = false;
    private bool player2 = false;

    [SerializeField] GameObject defaultpose1;
    [SerializeField] GameObject defaultpose2;

    [SerializeField] GameObject readypose1;
    [SerializeField] GameObject readypose2;

    [SerializeField] TextMeshProUGUI readyup1;
    [SerializeField] TextMeshProUGUI readyup2;

    [SerializeField] GameObject wizard1Body;

    public KeyCode ready1 = KeyCode.W;
    public KeyCode ready2 = KeyCode.UpArrow;

    private bool toggle1 = false;

    [SerializeField] GameObject loadingScreen;
    private void Start()
    {
        loadingScreen.SetActive(true);
        wizard1Body.SetActive(false);
    }
    private void Update()
    {
        if (Aurdino.button1 == 1 && Aurdino.button2 == 1)
        {
            if (toggle1 == false)
            {
                defaultpose1.SetActive(false);
                readypose1.SetActive(true);
                toggle1 = true;
                readyup1.text = "BATTLE";

                StartCoroutine(ReadyUpBuffer());
                if (toggle1 == true && player2 == true || player1 == true && player2 == true)
                {
                    //GO INTO GAME
                    wizard1Body.SetActive(true);
                    loadingScreen.SetActive(false);
                }
            }/*
            else if (toggle1)
            {

                StartCoroutine(ReadyUpBuffer());
                defaultpose1.SetActive(true);
                readypose1.SetActive(false);
                toggle1 = false;
                readyup1.text = "ready Up";
                
            }*/
        }
    }

    public void Ready1()
    {
        
        if (player1)                //NOTREADY
        {
            defaultpose1.SetActive(true);
            readypose1.SetActive(false);
            player1 = false;
            readyup1.text = "ready Up";
            StartCoroutine(ReadyUpBuffer());
        }
        else if(player1== false)    // READY
        {
            defaultpose1.SetActive(false);
            readypose1.SetActive(true);
            player1 = true;
            readyup1.text = "BATTLE";
            StartCoroutine(ReadyUpBuffer());
    {

            }
            if (player1 == true && player2 == true)
            {
                //GO INTO GAME
                wizard1Body.SetActive(true);
                loadingScreen.SetActive(false);
                
            }
        }
    }
    public void Ready2()
    {
        if (player2 || Input.GetKey(ready2))
        {
            defaultpose2.SetActive(true);
            readypose2.SetActive(false);
            player2 = false;
            readyup2.text = "ReaDY up";
            StartCoroutine(ReadyUpBuffer());
        }
        else if (player2 == false || !Input.GetKey(ready2))
        {
            defaultpose2.SetActive(false);
            readypose2.SetActive(true);
            player2 = true;
            readyup2.text = "BATTLE";
            StartCoroutine(ReadyUpBuffer());
            if (player1 == true && player2 == true || toggle1 == true && player2 == true)
            {

                //GO INTO GAME
                wizard1Body.SetActive(true);
                loadingScreen.SetActive(false);
                
            }
        }
    }

    IEnumerator ReadyUpBuffer()
    {
        yield return new WaitForSeconds(2);
        if (toggle1)
        {

            StartCoroutine(ReadyUpBuffer());
            defaultpose1.SetActive(true);
            readypose1.SetActive(false);
            toggle1 = false;
            readyup1.text = "ready Up";
            if (player1 == true && player2 == true)
            {
                //GO INTO GAME
                wizard1Body.SetActive(true);
                loadingScreen.SetActive(false);
            }
        }
    }
}
