using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketInfo
{
    public int ticketAmount;
    public System.DateTime ticketChargeStartTime;
    
    public TicketInfo(int amount, System.DateTime time)
    {
        this.ticketAmount = amount;
        this.ticketChargeStartTime = time;
    }
}
