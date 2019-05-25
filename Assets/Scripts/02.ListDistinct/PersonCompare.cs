using System.Collections.Generic;

class PersonCompare : IEqualityComparer<Person>
{
    public bool Equals(Person x, Person y)
    {
        //UnityEngine.Debug.Log("比较相等 x:" + x + " y:" + y);

        if ((x.FirstName == y.FirstName)  && (x.LastName == y.LastName))
        {
            //UnityEngine.Debug.Log("比较相等....");
            return true;
        }
        return false;
    }

    public int GetHashCode(Person obj)
    {
        //UnityEngine.Debug.Log("GetHashCode: " + obj.ToString());
        return obj.FirstName.GetHashCode() ^ obj.LastName.GetHashCode();
    }
}
