using System;
using System.Collections.Generic;

public class MatchPlayedRequest
{
    public String matchID;
    public String userID;
    public List<int> scores = new List<int>();
    public long entryFee;
    public String matchType;
}
