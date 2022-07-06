/* Класс для создания двусвязного списка типа ListRand,
 * реализующий возможность (де)сериализации своих элементов 
*/
using System;
using System.IO;
using System.Collections.Generic;

class ListRand
{
    public ListNode Head;
    public ListNode Tail;
    public int Count;

    public void Serialize(FileStream s)
    {
        Dictionary<ListNode, int> nodeCurIdDict = new Dictionary<ListNode, int>();
        Dictionary<ListNode, int> nodeRefIdDict = new Dictionary<ListNode, int>();

        // Словарь нода-id 
        int i = 0;
        for (ListNode curNode = Head; curNode != null; curNode = curNode.Next)
        {
            nodeCurIdDict.Add(curNode, i);
            i++;
        }

        // Словарь нода-ref_id
        for (ListNode curNode = Head; curNode != null; curNode = curNode.Next)
        {
            if (curNode.Rand != null)
            {
                nodeRefIdDict.Add(curNode, nodeCurIdDict[curNode.Rand]);
            }
            else // Если вдруг RAND оказался null, ссылаем текущую ноду на себя же
            {
               nodeRefIdDict.Add(curNode, nodeCurIdDict[curNode]);
            }
        }

        using (BinaryWriter writer = new BinaryWriter(s))
        {
            for (ListNode curNode = Head; curNode != null; curNode = curNode.Next)
            {
                writer.Write(curNode.Data);
                writer.Write(nodeRefIdDict[curNode]);
            }
        }

    }
    public void Deserialize(FileStream s)
    {
        Dictionary<int, ListNode> nodeCurIdDict = new Dictionary<int, ListNode>();
        ListNode curNode;

        // Словарь id-нода 
        int i = 0;
        for (curNode = Head; curNode != null; curNode = curNode.Next)
        {
            nodeCurIdDict.Add(i, curNode);
            i++;
        }

        curNode = Head;

        using (BinaryReader reader = new BinaryReader(s))
        {
            while ((reader.PeekChar() > -1) || (curNode != null))
            {
                curNode.Data = reader.ReadString();
                int refId = reader.ReadInt32();
                curNode.Rand = nodeCurIdDict[refId];
                curNode = curNode.Next;
            }    
        }
    }
}


