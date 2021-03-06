﻿using System.Collections;
using System.Collections.Generic;

public class History
{
    private static List<Connection> GenerationalHistory = new List<Connection>();
    private static int innovation = 0;

    public static List<UnityEngine.Vector3> PrinterHistory = new List<UnityEngine.Vector3>();
    
    private static int IncrementAndGetInnovation()
    {
        innovation++;
        return innovation;
    }

    private static int GetCurrentInnovation()
    {
        return innovation;
    }
  

    public static void ClearHistory()
    {
        GenerationalHistory.Clear();
    }

    // Check if contains history if it does use historical innovation, if not then increment use new innovation
    public static void AlterInnovationBasedOnHistory(Connection con)
    {
        foreach (Connection c in GenerationalHistory)
        {
            if (c.InNode == con.InNode && c.OutNode == con.OutNode)
            {
                con.Innovation = c.Innovation;
                return;
            }
        }

        con.Innovation = IncrementAndGetInnovation();
        Connection newC = new Connection(con.InNode, con.OutNode, 0.0f, false, con.Innovation);
        
        GenerationalHistory.Add(newC);
        return;
    }

    // Add node to printer history
    public static void AddNodeToPrinter(UnityEngine.Vector3 v)
    {
        PrinterHistory.Add(new UnityEngine.Vector3(v.x, v.y, v.z));
    }

    // ===================== DEBUG ===========================
    public static void SetInnovationDebug(int i)
    {
        innovation = i;
    }

    // Add connection to innovation history
    public static void AddConnectionToInnovationHistoryDebug(Connection c)
    {
        GenerationalHistory.Add(new Connection(c));
    }

}
