using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;

namespace TreeSplitter.Tests.Unit;

public class Splitter
{
    public static List<Segment> Split(XNode node, int offset)
    {
        var segments = new List<Segment>();
        
        switch (node)
        {
            case XElement element:
            {
                foreach (var child in element.Nodes().ToList())
                {
                    segments.AddRange(Split(child, offset));

                    offset = segments.Last().End;
                }

                break;
            }
            case XText textNode:
                segments.Add(new Segment(
                    textNode,
                    node.Parent,
                    offset + 0,
                    offset + textNode.Value.Length,
                    textNode.Value
                ));

                break;
        }

        return segments;
    }
}