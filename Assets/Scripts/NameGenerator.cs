using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public  class NameGenerator
{
    public static String nameGenerate(ArrayList PredicateList, ArrayList AdjectiveList1, ArrayList AdjectiveList2, String tag)
    {
        String itemName = null;

        int levelnumber = WorldVariablesHandler.Instance.getLevel();

        Random randomNubmer = new Random();

        if (levelnumber <= 10)
        {
            itemName = tag;

        }

        if (levelnumber <= 20 && levelnumber > 10)
        {
            itemName = tag + " " + (string)PredicateList[randomNubmer.Next(PredicateList.Count)];

        }

        if (levelnumber <= 30 && levelnumber > 20)
        {
            itemName = (string)AdjectiveList1[randomNubmer.Next(AdjectiveList1.Count)] + " " +  tag  + " " + (string)PredicateList[randomNubmer.Next(PredicateList.Count)];

        }

        if (levelnumber > 30)
        {
            itemName = (string)AdjectiveList2[randomNubmer.Next(AdjectiveList2.Count)] + " " +(string)AdjectiveList1[randomNubmer.Next(AdjectiveList1.Count)] + " " + tag + " " + (string)PredicateList[randomNubmer.Next(PredicateList.Count)];

        }


        return itemName;
    }

    
}
