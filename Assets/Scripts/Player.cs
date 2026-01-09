using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string name;
    Hand hand;
    int resources = 0;
    int roundWager = 0;
    public bool wagering = false;
    public int wager;
    public int roundBet;

    /*
    -1 fold,
    0 checked
    1 raised
    2 all in
    */
    int roundState = -1;

    void Update()
    {

    }

    public bool freshHand()
    {
        hand = new Hand();
        return true;
    }

    public void SetRoundState(int state)
    {
        roundState = state;
    }

    public int GetRoundState()
    {
        return roundState;
    }

    public bool addToHand(Card card)
    {
        if (card is null)
        {
            Debug.Log("There is no card " + card);
            return false;
        }
        //hand.addCard(card);
        return true;
    }
    public Hand getHand()
    {
        return hand;
    }

    public override string ToString()
    {
        return name;
    }

    public void NewBet(int call)
    {
        roundBet = call;
        wagering = true;
    }

    //In raise and check we should subtract at the action
    public void Check()
    {
        wager = roundBet;
        wagering = false;
        roundState = 0;
    }
    public void Fold()
    {
        //flag that will pull them from the 
        wagering = false;
        roundState = -1;
    }
    public void Raise()
    {
        //Something to trigger the raise and then a listerner or something?
        wagering = false;
        roundState = 0;

    }
}
