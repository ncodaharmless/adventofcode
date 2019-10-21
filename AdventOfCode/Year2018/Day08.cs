using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Year2018
{
    public class Day08
    {
        class Node
        {
            public List<int> MetaData = new List<int>();
            public List<Node> Children = new List<Node>();

            public int MetaDataSum()
            {
                int count = MetaData.Sum();
                foreach (var child in Children)
                    count += child.MetaDataSum();
                return count;
            }

            public int NodeValue()
            {
                if (!Children.Any())
                    return MetaData.Sum();

                int value = 0;
                foreach (int metaDataIndex in MetaData)
                {
                    //metadata is stored as 1-based index
                    int index = metaDataIndex - 1;
                    if (index >= 0 && index < Children.Count)
                    {
                        value += Children[index].NodeValue();
                    }
                }
                return value;
            }

            public void Read(System.Collections.IEnumerator next)
            {
                next.MoveNext();
                int childCount = Convert.ToInt32(next.Current);
                next.MoveNext();
                int metaCount = Convert.ToInt32(next.Current);

                for (int i = 0; i < childCount; i++)
                {
                    Node child = new Node();
                    child.Read(next);
                    Children.Add(child);
                }

                for (int i = 0; i < metaCount; i++)
                {
                    next.MoveNext();
                    MetaData.Add(Convert.ToInt32(next.Current));
                }
            }
        }
        public void Run()
        {
            List<Node> nodes = new List<Node>();
            string[] numbers = System.IO.File.ReadAllText("day08.txt").Split(' ');
            var nextNum = numbers.GetEnumerator();
            var rootNode = new Node();
            rootNode.Read(nextNum);

            int metaDataCount = rootNode.MetaDataSum();
            Console.WriteLine("MetaData Sum: " + metaDataCount);

            int nodeVal = rootNode.NodeValue();
            Console.WriteLine("Node Value: " + nodeVal);

        }



    }
}