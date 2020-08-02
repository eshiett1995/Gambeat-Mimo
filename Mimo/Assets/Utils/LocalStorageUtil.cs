using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalStorageUtil
{

     public enum Keys
    {
        firstName,
        lastName,
        email,  
        games, 
        wins,
        draws,
        losses,
        cash,
        authorization,
    }


    static public void saveAuthKey(string value)
    {
        PlayerPrefs.SetString(Keys.authorization.ToString(),value);
    }

    static public string getAuthKey()
    {
        return PlayerPrefs.GetString(Keys.authorization.ToString());
    }
    

    static public void saveAuthKey(string key, string value)
    {
        PlayerPrefs.SetString(key,value);
    }

    static public void save(string key, string value)
    {
        PlayerPrefs.SetString(key,value);
    }

    static public void save(string key, int value)
    {
        PlayerPrefs.SetInt(key,value);
    }

    static public string get(string key)
    {
        return PlayerPrefs.GetString(key);
    }
}
