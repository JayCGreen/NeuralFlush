using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Hand : MonoBehaviour
{
    //Vestigial
    /*
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

    public int[] strengthArr = new int[8];

    public Sleight handRank;
    public List<Card> cards = new List<Card>();
    List<int> sleighted;

    public void addCard(Card card)
    {
        cards.Add(card);
        //cards.Sort(CompareCards);
        handRank = Strength();
    }

    protected int StrengthCalculator()
    {
        //Get a grid representing the card values present in the hand
        //15 because we're skipping 0 (+1) and we're count aces twice, as 1 and as 14 (+1) 
        bool[,] cardGrid = new bool[15, 4];
        foreach (Card card in cards)
        {
            cardGrid[card.rank, (int)card.suit] = true;
        }
        //copy the value of the Aces to the 14
        for (int i = 1; i < cardGrid.GetLength(1); i++)
        {
            cardGrid[14, i] = cardGrid[1, i];
        }

        //Walkthrough the grid to find the hand rank

        //Testing for multiplicity
        int freq;
        for (int j = 0; j < cardGrid.GetLength(0); j++)
        {
            freq = 0;
            for (int k = 0; k < cardGrid.GetLength(1); k++)
            {
                if(cardGrid[j, k])
                {
                    freq++;
                }
                
            }
            switch (freq)
            {
                case 2:
                    //check for pair and two pair
                    strengthArr[8 - (float)Sleight.Pair] = Mathf.Max(strengthArr[8 - (float)Sleight.Pair], j);
                case 3:
                    strengthArr[8 - (float)Sleight.Three_of_a_Kind] = Mathf.Max(strengthArr[8 - (float)Sleight.Three_of_a_Kind], j);
                case 4:
                    strengthArr[8 - (float)Sleight.Four_of_a_Kind] = Mathf.Max(strengthArr[8 - (float)Sleight.Four_of_a_Kind], j);
            }
        }

        //Test for Flushes
        int max;
        for (int m = 0; m < cardGrid.GetLength(1); m++)
        {
            freq = 0;
            max = 0;
            for (int n = 0; n < cardGrid.GetLength(0); n++)
            {
                if(cardGrid[n, m])
                {
                    freq++;
                    max = n;
                }
                
            }
            if (freq <= 5)
            {
                strengthArr[8 - (float)Sleight.Flush] = Mathf.Max(strengthArr[8 - (float)Sleight.Flush], max);
            }
        }
    }

    protected Sleight Strength()
    {
        bool[,] cardGrid = new bool[14, 4];
        Sleight ranking = Sleight.High_Card;

        foreach (Card card in cards)
        {
            cardGrid[card.rank, (int)card.suit] = true;
        }

        int flushStreak = 0;
        int maxFlushStreak = 0;
        bool hasAce = false;
        for (int n = 0; n < cardGrid.GetLength(1); n++)
        {
            hasAce = false;
            //streak for Straight Flush, Count for Flush
            flushStreak = 0;
            int count = 0;
            for (int m = 0; m < cardGrid.GetLength(0); m++)
            {
                //flag Ace so can be counted later if present
                if (cardGrid[m, n])
                {
                    flushStreak++;
                    count++;
                    //if its an Ace
                    if (m == 1)
                    {
                        hasAce = true;
                    }
                }
                else
                {
                    maxFlushStreak = maxFlushStreak < flushStreak ? flushStreak : maxFlushStreak;
                    flushStreak = 0;
                }
            }
            if (flushStreak > 0 && hasAce)
            {
                flushStreak += 1;
            }
            maxFlushStreak = maxFlushStreak < flushStreak ? flushStreak : maxFlushStreak;
            if (count >= 5)
            {
                ranking = (Sleight)Mathf.Max((float)Sleight.Flush, (float)ranking);
            }
        }
        if (maxFlushStreak >= 5)
        {
            ranking = (Sleight)Mathf.Max((float)Sleight.Straight_Flush, (float)ranking);
        }

        int straightStreak = 0;
        int maxStraightStreak = 0;
        bool pair;
        bool thrice;
        hasAce = false;
        //goes through suit, then rank
        for (int i = 0; i < cardGrid.GetLength(0); i++)
        {
            //Count for multiplicity
            int count = 0;
            for (int j = 0; j < cardGrid.GetLength(1); j++)
            {
                if (cardGrid[i, j])
                {
                    count++;
                    if (i == 1)
                    {
                        hasAce = true;
                    }
                }
            }
            if (count > 0)
            {
                straightStreak++;

            }
            else
            {
                maxStraightStreak = maxStraightStreak < straightStreak ? straightStreak : maxStraightStreak;
                straightStreak = 0;
            }
            //if check if either the pair flag or 3 of a kind have been flipped
            if (count == 4)
            {
                ranking = (Sleight)Mathf.Max((float)Sleight.Four_of_a_Kind, (float)ranking);
            }
            else if (count == 3)
            {
                if (ranking == Sleight.Two_Pair || ranking == Sleight.Pair || ranking == Sleight.Three_of_a_Kind)
                {
                    ranking = (Sleight)Mathf.Max((float)Sleight.Full_House, (float)ranking);
                }
                else
                {
                    ranking = (Sleight)Mathf.Max((float)Sleight.Three_of_a_Kind, (float)ranking);
                }
            }
            else if (count == 2)
            {
                if (ranking == Sleight.Pair)
                {
                    ranking = (Sleight)Mathf.Max((float)Sleight.Two_Pair, (float)ranking);
                }
                else
                {
                    ranking = (Sleight)Mathf.Max((float)Sleight.Pair, (float)ranking);
                    strengthArr[8 - (float)Sleight.Pair] = Mathf.Max(strengthArr[8 - (float)Sleight.Pair], i);
                }
            }
        }
        if (straightStreak > 0 && hasAce)
        {
            straightStreak += 1;
        }
        maxStraightStreak = maxStraightStreak < straightStreak ? straightStreak : maxStraightStreak;

        if (maxStraightStreak >= 5)
        {
            ranking = (Sleight)Mathf.Max((float)Sleight.Straight, (float)ranking);
        }

        return ranking;
        //if the max streak is greater than or ewual to five then we have a straight
    }

    private static int CompareCards(Card a, Card b)
    {
        if (a is null)
        {
            if (b is null)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        else if (b is null)
        {
            return 1;
        }
        else
        {
            return b.rank - a.rank;
        }
    }

    public override string ToString()
    {
        string handString = "";
        foreach (Card card in cards)
        {
            handString += string.IsNullOrEmpty(handString) ? card.ToString() : ", " + card.ToString();
        }
        Debug.Log("Best Hand is " + handRank);
        return handString;
    }
    */

}
