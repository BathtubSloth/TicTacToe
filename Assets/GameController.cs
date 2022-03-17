using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Player
{
    public Image panel;
    public TextMeshProUGUI text;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

class Move
{
    public int index;
};

public class GameController : MonoBehaviour
{

    //set vars for player data
    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    public TextMeshProUGUI xWinLabel;
    public TextMeshProUGUI oWinLabel;
    public int xWinCount;
    public int oWinCount;

    //var for minimax. default score to 0 (will remain 0 in event of a tie)
    public int score = 0;

    //set vars for game over screen
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    bool gameWon;

    //ref to reset button
    public GameObject resetButton;

    //create a list of each grid slots buttons.
    public TextMeshProUGUI[] gridTextList;

    //vars related to player
    private string currentPlayer;
    private int moveCount;
    public string[] currentBoard;

    private void Awake()
    {
        //set default player as person running the game on X
        currentPlayer = "X";
        //set default player colors
        setPlayerColors(playerX, playerO);
        //set movecount and wincounts to 0
        moveCount = 0;
        xWinCount = 0;
        oWinCount = 0;
        //set default state of postgame vars to false
        gameOverPanel.SetActive(false);
        resetButton.SetActive(false);

        SetControllerOnPress();
    }

    //set controller on each text in grid
    void SetControllerOnPress()
    {
        for(int i = 0; i < gridTextList.Length; i++)
        {
            gridTextList[i].GetComponentInParent<GridSpace>().SetController(this);
        }
    }

    //check which side is playing currently
    public string GetCurrentPlayer()
    {
        //TODO: this will return X or O based on turn
        return currentPlayer;
    }

    //check if any player has gotten 3 in a row
    public void WinCondition()
    {
        moveCount++;

        //Top Row
        if(gridTextList[0].text == currentPlayer && gridTextList[1].text == currentPlayer && gridTextList[2].text == currentPlayer)
        {
            GameOver(currentPlayer);
            gameWon = true;
            IterateWin();
            getScore();
        }

        //Middle Row
        else if (gridTextList[3].text == currentPlayer && gridTextList[4].text == currentPlayer && gridTextList[5].text == currentPlayer)
        {
            GameOver(currentPlayer);
            gameWon = true;
            IterateWin();
            getScore();
        }

        //Bottom Row
        else if (gridTextList[6].text == currentPlayer && gridTextList[7].text == currentPlayer && gridTextList[8].text == currentPlayer)
        {
            GameOver(currentPlayer);
            gameWon = true;
            IterateWin();
            getScore();
        }

        //Left Column
        else if (gridTextList[0].text == currentPlayer && gridTextList[3].text == currentPlayer && gridTextList[6].text == currentPlayer)
        {
            GameOver(currentPlayer);
            gameWon = true;
            IterateWin();
            getScore();
        }

        //Middle Column
        else if (gridTextList[1].text == currentPlayer && gridTextList[4].text == currentPlayer && gridTextList[7].text == currentPlayer)
        {
            GameOver(currentPlayer);
            gameWon = true;
            IterateWin();
            getScore();
        }

        //Right Column
        else if (gridTextList[2].text == currentPlayer && gridTextList[5].text == currentPlayer && gridTextList[8].text == currentPlayer)
        {
            GameOver(currentPlayer);
            gameWon = true;
            IterateWin();
            getScore();
        }

        //Left Diag
        else if (gridTextList[0].text == currentPlayer && gridTextList[4].text == currentPlayer && gridTextList[8].text == currentPlayer)
        {
            GameOver(currentPlayer);
            gameWon = true;
            IterateWin();
            getScore();
        }

        //Right Diag
        else if (gridTextList[2].text == currentPlayer && gridTextList[4].text == currentPlayer && gridTextList[6].text == currentPlayer)
        {
            GameOver(currentPlayer);
            gameWon = true;
            IterateWin();
            getScore();
        }

        //check for draw
        else if (moveCount >= 9 && gameWon == false)
        {
            GameOver("Draw!");
        }

        //Stashing this here, we check for win condition on every click so if no win has happened, this is essentially the else that says keep going.
        else
        {
            ChangePlayer();
            if(currentPlayer == "O")
            {
                AITurn();
            }
        }    
    }

    void IterateWin()
    {
        if (currentPlayer == "X")
        {
            xWinCount = xWinCount + 1;
            xWinLabel.text = "Wins: " + xWinCount;
        }
        else if (currentPlayer == "O")
        {
            oWinCount = oWinCount + 1;
            oWinLabel.text = "Wins: " + oWinCount;
        }
    }
    //after win condition is triggered handle postgame funcs
    void GameOver(string winner)
    {
        //disable buttons when game is over
        SetBoardInteractable(false);

        //enable reset button
        resetButton.SetActive(true);

        if(winner == "Draw!")
        {
            SetGameOverText("Draw!");
        }
        else
            SetGameOverText(winner + " wins!"); 
    }

    //change current player
    void ChangePlayer()
    {
        //change to the player that currently isnt playing when called
        currentPlayer = (currentPlayer == "X") ? "O" : "X";

        //set colors
        if(currentPlayer == "X")
        {
            setPlayerColors(playerX, playerO);
        }
        else
            setPlayerColors(playerO, playerX);
    }

    void SetGameOverText(string overText)
    {
        //set gameover text
        gameOverText.text = overText;
        //set gameover panel to true
        gameOverPanel.SetActive(true);
    }

    //func to reset game when it is over
    public void RestartGame()
    {
        //reset game values
        gameWon = false;
        currentPlayer = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);
        resetButton.SetActive(false);
        setPlayerColors(playerX, playerO);
        score = 0;

        //re-enable buttons when game is restarted
        SetBoardInteractable(true);
        for (int i = 0; i < gridTextList.Length; i++)
        {
            gridTextList[i].text = "";
        }
    }

    //better func to toggle interactable
    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < gridTextList.Length; i++)
        {
            gridTextList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    //set which player's turn it is colors
    void setPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;

        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    //check if board has moves left
    bool isMovesLeft(string[] board)
    {
        for (int i = 0; i < board.Length; i++)
                if (board[i] == "")
                    return true;
        return false;
    }

    //handle AI movement
    void AITurn()
    {
        currentBoard = new string[]{gridTextList[0].text, gridTextList[1].text, gridTextList[2].text,
                                    gridTextList[3].text, gridTextList[4].text, gridTextList[5].text,
                                    gridTextList[6].text, gridTextList[7].text, gridTextList[8].text};
        
        int bestMove = findBestMove(currentBoard);
        Debug.Log(bestMove);
        if(gridTextList[bestMove].GetComponentInParent<Button>().IsInteractable())
        {
            gridTextList[bestMove].GetComponentInParent<Button>().onClick.Invoke();
        }
    }
    

    void getScore()
    {
        if(currentPlayer == "X")
        {
            score = score + 10;
        }

        if(currentPlayer == "O")
        {
            score = score - 10;
        }
    }

    //vars necessary for minimax
    public string playerMarker = "X"; 
    public string opponentMarker = "O";
    

    int minimax(string[] gameBoard, int depth, int alpha, int beta, bool maximizingPlayer)
    {
        
        
        if (score == 10)
            return -1;

        if (score == -10)
            return 1;

        if(isMovesLeft(currentBoard) == false)
        {
            return 0;
        }

        if (maximizingPlayer)
        {
            int best = -1000;

            for (int i = 0; i < gameBoard.Length; i++)
            {
                // Check if cell is empty
                if (gameBoard[i] == "")
                {
                    // Make the move
                    gameBoard[i] = playerMarker;
                    //recursion of minimax
                    int minimaxScore = minimax(gameBoard, depth + 1, alpha, beta, false);
                    //check best of 2 scores
                    best = System.Math.Max(best, minimaxScore);
                    // Undo the move
                    gameBoard[i] = "";
                    //alpha beta pruning
                    alpha = System.Math.Max(alpha, minimaxScore);
                    if (beta <= alpha)
                        break;
                }
            }
            return best;
        }

        else
        {
            int best = 1000;

            // Traverse all cells
            for (int i = 0; i < gameBoard.Length; i++)
            {
                // Check if cell is empty
                if (gameBoard[i] == "")
                {
                    // Make the move
                    gameBoard[i] = opponentMarker;
                    //recursion of minimax
                    int minimaxScore = minimax(gameBoard, depth + 1, alpha, beta, true);
                    //check best of 2 scores
                    best = System.Math.Min(best, minimaxScore);
                    // Undo the move
                    gameBoard[i] = "";
                    //alpha beta pruning
                    beta = System.Math.Min(beta, minimaxScore);
                    if (beta <= alpha)
                        break;
                }
            }
            return best;
        }
    }

    int findBestMove(string[] gameBoard)
    {
        int bestScore = -1000;
        int bestMove = 0;

        for (int i = 0; i < gameBoard.Length; i++)
        {
            // Check if cell is empty
            if (gameBoard[i] == "")
            {
                // Make the move
                gameBoard[i] = opponentMarker;

                int score = minimax(gameBoard, 0, -1000, 1000, false);

                // Undo the move
                gameBoard[i] = "";
                // If the value of the current move is better, update the best
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }         
            }
        }
        return bestMove;
    }
}
