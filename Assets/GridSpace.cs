using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridSpace : MonoBehaviour
{
    //get type from GameController
    private GameController myController;

    //define button vars and player's team input
    public Button myButton;
    public TextMeshProUGUI buttonText;
    public string currentPlayer;

    //set components to var reference
    private void Start()
    {
        myButton = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    //set spaces to display current player's text, disable that button and end the turn
    public void SetSpace()
    {
        buttonText.text = myController.GetCurrentPlayer();
        myButton.interactable = false;
        myController.WinCondition();
    }

    //get ref to GameController for use here
    public void SetController(GameController controller)
    {
        myController = controller;
    }
}
