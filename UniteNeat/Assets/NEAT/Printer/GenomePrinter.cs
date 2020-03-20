using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenomePrinter : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject nodePrefab;

    private List<GameObject> lineList;
    private List<GameObject> nodeList;
    private Vector3[] locations;

    private Vector3 topLeft;
    private float factor = 0.5f;

    void Awake()
    {
        lineList = new List<GameObject>();
        nodeList = new List<GameObject>();
        topLeft = transform.position;
    }
    public void Draw(Genome genome)
    {
        Clear();
        int InputCount = 0;
        int OutputCount = 0;

        foreach (KeyValuePair<int, Node> node in genome.Nodes)
        {
            if (node.Value.Type == Node.NodeType.INPUT)
            {
                InputCount++;
            }
        }
        foreach (KeyValuePair<int, Node> node in genome.Nodes)
        {
            if (node.Value.Type == Node.NodeType.OUTPUT)
            {
                OutputCount++;
            }
        }

        int numberOfInputs = InputCount;
        int numberOfOutputs = OutputCount;
        int numberOfNodes = genome.Nodes.Count;
        //int numberOfHiddens = numberOfNodes - (numberOfInputs + numberOfOutputs);
        int hiddenStartIndex = numberOfInputs + numberOfOutputs;
        locations = new Vector3[genome.Nodes.Count];
        int locationIndex = 0;

        //Create input node objects 
        float startY = topLeft.y;
        for (int i = 0; i < numberOfInputs; i++)
        {
            Vector3 loc = new Vector3(topLeft.x, startY, 0);
            GameObject node = Instantiate(nodePrefab, loc, nodePrefab.transform.rotation);
            node.transform.parent = transform;
            node.GetComponent<Renderer>().material.color = Color.green;
            nodeList.Add(node);
            startY = startY - (factor);

            locations[locationIndex] = loc;
            locationIndex++;
        }

        //create output node objects    
        startY = (topLeft.y);
        for (int i = numberOfInputs; i < hiddenStartIndex; i++)
        {
            Vector3 loc = new Vector3(topLeft.x + 7f, startY, 0);
            GameObject node = Instantiate(nodePrefab, loc, nodePrefab.transform.rotation);
            node.transform.parent = transform;
            node.GetComponent<Renderer>().material.color = Color.white;
            nodeList.Add(node);
            startY = startY - (factor);

            locations[locationIndex] = loc;
            locationIndex++;
        }

        float offset = 0.5f;
        for (int i = hiddenStartIndex; i < numberOfNodes; i++)
        {
            float x;
            float y;
            float z;
            Vector3 v;

            if (i - hiddenStartIndex < History.PrinterHistory.Count)
            {
                x = History.PrinterHistory[i - hiddenStartIndex].x;
                y = History.PrinterHistory[i - hiddenStartIndex].y;
                z = 0;

                v = new Vector3(x, y, z);
            }
            else
            {
                x = Random.Range(topLeft.x + offset, topLeft.x + 6f);
                y = Random.Range(topLeft.y - offset, topLeft.y - 6f);
                z = 0;

                v = new Vector3(x, y, z);
                History.AddNodeToPrinter(v);
            }

            GameObject node = Instantiate(nodePrefab, v, nodePrefab.transform.rotation);
            node.transform.parent = transform;
            node.GetComponent<Renderer>().material.color = Color.red;
            nodeList.Add(node);

            locations[locationIndex] = v;
            locationIndex++;

        }
        float[][] geneConnections = genome.GetGeneDrawConnections(genome); //get gene connection list
        int colSize = geneConnections.GetLength(0);
        //create line connection objects
        for (int i = 0; i < colSize; i++)
        {
            GameObject lineObj = Instantiate(linePrefab);
            lineObj.transform.parent = transform;
            lineList.Add(lineObj);
            LineRenderer lineRen = lineObj.GetComponent<LineRenderer>();
            lineRen.SetPosition(0, locations[(int)geneConnections[i][0] - 1]);
            if ((int)geneConnections[i][0] != (int)geneConnections[i][1])
                lineRen.SetPosition(1, locations[(int)geneConnections[i][1] - 1]);
            else
                lineRen.SetPosition(1, locations[(int)geneConnections[i][1] - 1] + new Vector3(1f, 0, 0));

            float size = 0.1f;
            float weight = geneConnections[i][2];

            float factorW = Mathf.Abs(weight);
            Color color;

            if (weight > 0)
                color = Color.green;
            else if (weight < 0)
                color = Color.cyan;
            else
                color = Color.red;

            size = size * factorW;
            if (size < 0.05f)
                size = 0.05f;
            if (size > 0.15f)
                size = 0.15f;

            lineRen.startColor = color;
            lineRen.endColor = color;
            lineRen.startWidth = size;
            lineRen.endWidth = size;
        }
    }
    public void Clear()
    {
        for (int i = 0; i < lineList.Count; i++)
        {
            Destroy(lineList[i]);
        }

        for (int i = 0; i < nodeList.Count; i++)
        {
            Destroy(nodeList[i]);
        }

        lineList.Clear();
        nodeList.Clear();

    }

    /*
    public static bool NearlyEqual(float f1, float f2)
    {
        return Mathf.Abs(f1 - f2) < 0.00001;
    }
    */
}
