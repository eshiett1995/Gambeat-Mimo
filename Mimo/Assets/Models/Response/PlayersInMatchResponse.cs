﻿using System;
using System.Collections.Generic;

[Serializable]
public class PlayersInMatchResponse : ResponseModel
{

    private List<Player> winners;

    private List<Player> players;

    [Serializable]
    public class Player
    {
        public string firstName;
        public string lastName;
        public string photoUrl;
        public string email;
        public List<int> score;
        public long position;
    }
}
