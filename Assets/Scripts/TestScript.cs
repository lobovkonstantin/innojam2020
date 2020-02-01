using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    ArrayListGenerator arrGen = new ArrayListGenerator();

    NameGenerator nameGen = new NameGenerator();

    void Start()
    {
        ArrayList subjectList = arrGen.ListGenerate(@"Assets\\Resources\\SubjectList.txt");
        ArrayList predicateList = arrGen.ListGenerate(@"Assets\\Resources\\PredicateList.txt");
        ArrayList adjectiveList1 = arrGen.ListGenerate(@"Assets\\Resources\\Adjective1List.txt");
        ArrayList adjectiveList2 = arrGen.ListGenerate(@"Assets\\Resources\\Adjective2List.txt");

        String itemName = nameGen.nameGenerate(subjectList, predicateList, adjectiveList1, adjectiveList2, 41);

        Debug.Log(itemName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
