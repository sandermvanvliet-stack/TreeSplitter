using System.Xml.Linq;

namespace TreeSplitter.Tests.Unit;

public class Segment
{
    public Segment(XNode element, XElement? parentElement, int start, int end, string text)
    {
        Element = element;
        ParentElement = parentElement;
        Start = start;
        End = end;
        Text = text;
    }

    public XNode Element { get; }
    public XElement? ParentElement { get; }
    public int Start { get; }
    public int End { get; }
    public string Text { get; }
}