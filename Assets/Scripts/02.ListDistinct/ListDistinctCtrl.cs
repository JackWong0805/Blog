using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

/// <summary>
/// 集合重复项去除控制器.
/// </summary>
public class ListDistinctCtrl : UnityEngine.MonoBehaviour
{
    private void Start()
    {

        //List<int> intList = new List<int>() { 1, 2, 3, 4, 5, 3, 3, 2, 1};
        //Debug.Log("去掉重复前: " + LogHelper.List2Str(intList));

        //var removeRepeatList = intList.Distinct();
        //Debug.Log("去掉重复后: " + LogHelper.List2Str(removeRepeatList));

        List<Person> persons = new List<Person>();
        persons.Add(new Person("张", "三"));
        persons.Add(new Person("李", "四"));
        persons.Add(new Person("王", "五"));

        persons.Add(new Person("张", "三"));
        persons.Add(new Person("张", "三"));
        Debug.Log("去掉重复前: " + LogHelper.List2Str(persons));
        var persionsRemoveRepeatList = persons.Distinct(new PersonCompare());
        Debug.Log("去掉重复后: " + LogHelper.List2Str(persionsRemoveRepeatList));

    }
}

