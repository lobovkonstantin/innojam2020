using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public  class NameGenerator
{
    public static String nameGenerate(ArrayList PredicateList, ArrayList AdjectiveList1, ArrayList AdjectiveList2, String tag)
    {
        String itemName = null;

        int levelnumber = WorldVariablesHandler.Instance.getLevel();

        if (levelnumber <= 10)
        {
            itemName = (string)AdjectiveList1[Random.Range(0, AdjectiveList1.Count)] + " " + tag ;

        }

        if (levelnumber <= 20 && levelnumber > 10)
        {
            itemName = tag + " " + (string)PredicateList[Random.Range(0, PredicateList.Count)];

        }

        if (levelnumber <= 30 && levelnumber > 20)
        {
            itemName = (string)AdjectiveList1[Random.Range(0, AdjectiveList1.Count)] + " " +  tag  + " " + (string)PredicateList[Random.Range(0, PredicateList.Count)];

        }

        if (levelnumber > 30)
        {
            itemName = (string)AdjectiveList2[Random.Range(0, AdjectiveList2.Count)] + " " +(string)AdjectiveList1[Random.Range(0, AdjectiveList1.Count)] + " " + tag + " " + (string)PredicateList[Random.Range(0, PredicateList.Count)];
        }


        return itemName;
    }

    
}
