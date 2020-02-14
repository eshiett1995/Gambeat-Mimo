using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LeaderBoardResponse : ResponseModel
{

    public List<FormattedRank> ranks;

    public bool hasRank;

    public FormattedRank userRank;

    public bool isOnList;

    public class FormattedRank
    {
        public string firstName;
        public string lastName;
        public string email;
        public long score;
        public long position;
    }

}
