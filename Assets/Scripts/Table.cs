using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Table : MonoBehaviour
{
    public float pot;
    //List<int> drawnCards;
    public List<Player> players;

    private int[,] tableState = new int[4, 14];

    int currPlayer;

    int bet;

    int round;

    public enum Sleight
    {
        High_Card,
        Pair,
        Two_Pair,
        Three_of_a_Kind,
        Straight,
        Flush,
        Full_House,
        Four_of_a_Kind,
        Straight_Flush
    }


    //int sb = 0;
    //int bb = sb + 1 ;
    int loopStart = 0;
    int roundCounter = 0;

    int gameState = 0;
    // Start is called before the first frame update
    void Start()
    {
        currPlayer = players.Count - 1;
        InitDeck();
        roundCounter = 0;
    }

    void Update()
    {
        /*
        if (gameState == 0)
        {
            switch (round)
            {
                case 0:
                    dealHands();
                    drawFlop();
                    StartBet();
                    break;
                case 1:
                    drawTurn();
                    StartBet();
                    break;
                case 2:
                    drawRiver();
                    StartBet();
                    break;
                case 3:
                    Showdown();
                    break;
            }
            

        }
        */
        //players turn, if one of them is deciding, then don't move the marker.
        if (gameState == 1)
        {
            //work on the initial road
            if (!players[currPlayer].wagering)
            {
                //if they raised, then set new bet
                if (players[currPlayer].GetRoundState() == 1)
                {
                    bet = players[currPlayer].wager;
                    roundCounter = 0;
                    //reset the count
                }
                else
                {
                    roundCounter++;
                }
                currPlayer = (currPlayer + 1) % players.Count;
                players[currPlayer].NewBet(bet);
                Debug.Log("It is now " + currPlayer + "'s turn");
                //check if there's a raise to move the token, if check, get the number of checks there have been
            }
            else
            {
                return;
            }
        }

        if (roundCounter == players.Count)
        {
            gameState = 0;
        }
        else
        {
            gameState = 1;
            round++;
        }
    }

    protected void InitDeck()
    {
        for (int suit = 0; suit < 4; suit++)
        {
            for (int rank = 0; rank < 14; rank++)
            {
                tableState[suit, rank] = -1;
            }
        }
    }

    protected void Draw(int playerID)
    {
        int pulled;
        //check later
        do
        {
            pulled = UnityEngine.Random.Range(0, 52);
        }
        while (tableState[pulled / 4, pulled % 13 + 1] != -1);

        tableState[pulled / 4, pulled % 13 + 1] = playerID;
    }

    protected bool DealHands()
    {
        for (int i = 0; i < players.Count; i++)
        {
            Draw(i);
            Draw(i);
        }
        return true;
    }

    //Calculate the hand strength of a player, called every time cards are drawn
    protected int MeasureHand(int playerID)
    {
        //will bring sleights over later
        int[] strength = new int[9];

        int[] rankFreq = new int[14];
        int[] suitFreq = new int[4];
        int[] flushStreak = new int[5];
        int count = 0;
        int straightCount = 0;

        for (int card = 0; card < 52; card++)
        {
            int s = card / 4;
            int r = card % 13;

            if (tableState[s, r] == playerID)
            {
                suitFreq[s]++;
                rankFreq[r]++;
                flushStreak[count] = card;
                count++;
            }
            else
            {
                count = 0;
            }


            //Record the max
            
            for (int i = 1; i < 14; i++)
            {
                if (tableState[s, r] == playerID)
                {
                    straightCount++;
                }
                else
                {
                    straightCount = 0;
                }
            }

        }

        if (count <= 5)
        {
            //Straight Flush
        }
        if (straightCount <= 5)
        {
            //Straight
        }
        if (Array.Exists(rankFreq, el => el == 4))
        {
            //Four of a kind
            strength[(int)Sleight.Four_of_a_Kind] = Array.FindLast(rankFreq, el => el > 4);
        }
        if (Array.Exists(suitFreq, el => el >= 5))
        {
            //Flush
        }
        if (Array.Exists(rankFreq, el => el == 3))
        {
            if (Array.FindAll(rankFreq, el => el >= 2).Length > 2)
            {
                //Full House
                strength[(int)Sleight.Full_House] = Array.FindLast(rankFreq, el => el > 3);
            }
            else
            {
                //Three of a kind
                strength[(int)Sleight.Three_of_a_Kind] = Array.FindLast(rankFreq, el => el > 3);
            }
        }
        if (Array.Exists(rankFreq, el => el == 2))
        {
            //Two of a kind
            strength[1] = Array.FindLast(rankFreq, el => el > 3);
            //Check for two pair
            if (Array.FindAll(rankFreq, el => el >= 2).Length > 2)
            {
                strength[(int)Sleight.Two_Pair] = Array.FindLast(rankFreq, el => el > 2);
            }
            else
            {
                strength[(int)Sleight.Pair] = Array.FindLast(rankFreq, el => el > 2);
            }
        }

        //Need high card 
        //walk through rankFreq backwards and skip anything used previously with Exists on the strength,
        //not needed for full hands

        return 0;
    }

    protected bool TableDraw(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Draw(0);
        }
        return true;
    }

    protected void Showdown()
    {
        players.Sort(ComparePlayersByHand);
        string order = "";
        foreach (Player p in players)
        {
            order += " " + p;
        }
        Debug.Log(order);
        //reset the round
        NewHand();

    }

    protected void NewHand()
    {
        round = 0;
    }

    protected void StartBet()
    {
        roundCounter = 0;
    }

    private static int ComparePlayersByHand(Player a, Player b)
    {
        //Vestigial Fix later
        /*
        Hand handA = a.getHand();
        Hand handB = b.getHand();
        if (handA is null)
        {
            if (handB is null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else if (b is null)
        {
            return -1;
        }
        else
        {
            if (handB.handRank - handA.handRank != 0)
            {
                return (int)handB.handRank - (int)handA.handRank;
            }
            //tie breakers
            else
            {
                return handB.handRank - handA.handRank;
            }
        }
        */
        return 0;
    }

    public string PrintCard(int i)
    {
        //Fill in later
        return "";
    }
}
