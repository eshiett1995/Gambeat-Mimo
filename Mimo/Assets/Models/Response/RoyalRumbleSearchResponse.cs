﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoyalRumbleSearchResponse : ResponseModel
{
    public int number;
    public int size;
    public int numberOfElements;
    public List<FormattedMatch> content = new List<FormattedMatch>();
    public bool hasContent;
    public bool isFirst;
    public bool isLast;
    public bool hasNext;
    public bool hasPrevious;
    public int totalPages;
    public long getTotalElements;

    [Serializable]
    public class FormattedMatch {
        public string id;
        public string name;
        public long entryFee;
        public string matchStatus;
        public bool registered;
        public string matchState;
        public int numberOfCompetitors;
        public string winners;
        public int competitorLimit;
    }
}
