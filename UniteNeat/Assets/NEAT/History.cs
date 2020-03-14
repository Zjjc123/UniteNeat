using System.Collections;
using System.Collections.Generic;

public class History
{
    public static List<Connection> GenerationalHistory;
    private int innovation = 0;

    public int GetInnovation()
    {
        innovation++;
        return innovation;
    }

    public void ClearHistory()
    {
        GenerationalHistory.Clear();
    }

}
