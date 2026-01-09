using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }
    public int rank;

    public Suit suit;

    public Card(int rank, int suit)
    {
        this.rank = rank;
        this.suit = (Suit)suit;
    }

    public override string ToString()
    {
        string val;
        switch (rank)
        {
            case 1:
                val = "Ace";
                break;
            case 11:
                val = "Jack";
                break;
            case 12:
                val = "Queen";
                break;
            case 13:
                val = "King";
                break;
            default:
                val = "" + rank;
                break;
        }
        //Debug.Log("Gets seen here: " + val + " of " + suit);
        return val + " of " + suit;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Card))
        {
            return false;
        }
        else
        {
            return this.rank == ((Card)obj).rank && this.suit == ((Card)obj).suit;
        }
    }
}
