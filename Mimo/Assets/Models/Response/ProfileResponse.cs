using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProfileResponse : ResponseModel
{
    public string firstName;
    public string lastName;
    public string email;
    public string loginProvider;
    public long walletBalance;
    public long wins;
    public long losses;
    public long draws;
    public long highestScore;
}
