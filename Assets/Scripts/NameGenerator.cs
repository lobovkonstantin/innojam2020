using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class NameGenerator
{
    public String nameGenerate(ArrayList  SubjectList, ArrayList PredicateList, ArrayList AdjectiveList1,
        ArrayList AdjectiveList2, int levelnumber )
    {
        String itemName = null;

        Random randomNubmer = new Random();

        if (levelnumber <= 10)
        {
            itemName = (string) SubjectList[randomNubmer.Next(SubjectList.Count)];

        }

        if (levelnumber <= 20 && levelnumber > 10)
        {
            itemName = (string)SubjectList[randomNubmer.Next(SubjectList.Count)] + " " + (string)PredicateList[randomNubmer.Next(PredicateList.Count)];

        }

        if (levelnumber <= 30 && levelnumber > 20)
        {
            itemName = (string)AdjectiveList1[randomNubmer.Next(AdjectiveList1.Count)] + " " + (string)SubjectList[randomNubmer.Next(SubjectList.Count)]+ " " + (string)PredicateList[randomNubmer.Next(PredicateList.Count)];

        }

        if (levelnumber > 30)
        {
            itemName = (string)AdjectiveList2[randomNubmer.Next(AdjectiveList2.Count)] + " " +(string)AdjectiveList1[randomNubmer.Next(AdjectiveList1.Count)] + " " + (string)SubjectList[randomNubmer.Next(SubjectList.Count)] + " " + (string)PredicateList[randomNubmer.Next(PredicateList.Count)];

        }


        return itemName;
    }

    
}
