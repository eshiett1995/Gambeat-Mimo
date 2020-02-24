using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoyalRumbleSearchResponse : ResponseModel
{
    public int number;
    public int size;
    public int numberOfElements;
    public List<FormattedMatch> content;
    public bool hasContent;
    public bool isFirst;
    public bool isLast;
    public bool hasNext;
    public bool hasPrevious;
    public int totalPages;
    public long getTotalElements;
    
    public class FormattedMatch {
        public string id;
        public string name;
        public long entryFee;
        public string matchStatus;
        public string matchState;
        public int numberOfCompetitors;
        public string winners;
    }
}
