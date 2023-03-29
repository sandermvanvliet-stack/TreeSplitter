using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TreeSplitter.Tests.Unit;

public class Highlighter
{
    public static XElement Highlight(string html, int startOffset, string startSelector, int endOffset,
        string endSelector, Func<XElement> createHighlightNode)
    {
        var root = XElement.Parse("<root>" + html + "</root>"); // Wrap in a root element because otherwise we don't have a root node to XPath off

        var startNode = XPathTextNode(startSelector, root);
        var endNode = XPathTextNode(endSelector, root);

        var segments = Splitter.Split(root, 0);

        var matchingSegments = new List<Segment>();

        using var enumerator = segments.GetEnumerator();
        
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Element != startNode)
            {
                continue;
            }

            matchingSegments.Add(enumerator.Current);
            break;
        }
        
        while (enumerator.Current.Element != endNode && enumerator.MoveNext())
        {
            matchingSegments.Add(enumerator.Current);
        }

        foreach (var current in matchingSegments)
        {
            if (current.Element != startNode && current.Element != endNode)
            {
                WrapNodeFull(createHighlightNode, current);
            }
            else
            {
                var start = current.Element == startNode ? startOffset : 0;
                var end = current.Element == endNode ? endOffset : current.Text.Length;

                WrapInsideNode(start, end, createHighlightNode, current);
            }
        }

        return root.FirstNode as XElement;
    }

    private static void WrapNodeFull(Func<XElement> createHighlightNode, Segment current)
    {
        // Full overlap.
        if (current.ParentElement.Nodes().All(n => n.NodeType == XmlNodeType.Text))
        {
            // The entire node is being wrapped and to prevent
            // things like <span><xx>text</xx></span> where
            // what we actually want is <xx><span>text</span></xx>
            // we need to flip some stuff around here and 
            // call ReplaceWith on current.ParentElement instead
            // of current.Element
            var replacement = createHighlightNode();
            replacement.Add(current.ParentElement);
            current.ParentElement!.ReplaceWith(replacement);
        }
        else
        {
            // However for some cases this is wrapping a text node
            // in an element that has other (text) nodes.
            // In that case, only replace this text node with the
            // wrapped version
            var replacement = createHighlightNode();
            replacement.Add(current.Element);
            current.Element.ReplaceWith(replacement);
        }
    }

    private static void WrapInsideNode(int startOffset, int endOffset, Func<XElement> createHighlightNode, Segment current)
    {
        var replacements = new List<XNode>();

        if (startOffset == 0 && current.Text.Length == endOffset)
        {
            // The entire node is being wrapped and to prevent
            // things like <span><xx>text</xx></span> where
            // what we actually want is <xx><span>text</span></xx>
            // we need to flip some stuff around here and 
            // call ReplaceWith on current.ParentElement instead
            // of current.Element
            var replacement = createHighlightNode();
            replacement.Add(current.ParentElement);
            current.ParentElement!.ReplaceWith(replacement);

            return;
        }

        if (startOffset > 0 && current.Text.Length == endOffset)
        {
            // Midway to end
            var nonHighlightedText = current.Text[..startOffset];
            replacements.Add(new XText(nonHighlightedText));

            var highlightedText = current.Text[startOffset..];
            var highlightNode = createHighlightNode();
            highlightNode.Value = highlightedText;

            replacements.Add(highlightNode);
        }
        else if (startOffset == 0 && current.Text.Length > endOffset)
        {
            // Beginning to midway
            var highlightedText = current.Text[..endOffset];
            var highlightNode = createHighlightNode();
            highlightNode.Value = highlightedText;
            replacements.Add(highlightNode);

            var nonHighlightedText = current.Text[endOffset..];
            replacements.Add(new XText(nonHighlightedText));
        }
        else
        {
            // Fully in the middle
            var nonHighlightedText = current.Text[..startOffset];
            replacements.Add(new XText(nonHighlightedText));

            var highlightedText = current.Text[startOffset..endOffset];
            var highlightNode = createHighlightNode();
            highlightNode.Value = highlightedText;
            replacements.Add(highlightNode);
            
            nonHighlightedText = current.Text[endOffset..];
            replacements.Add(new XText(nonHighlightedText));
        }

        current.Element.ReplaceWith(replacements);
    }

    private static XNode XPathTextNode(string startSelector, XNode root)
    {
        var selector = startSelector;
        var selectorParts = selector.Split('/');
        selector = string.Join("/", selectorParts.SkipLast(1));
        var foundNode = root.XPathSelectElement(selector);
        var lastSelector = selectorParts.Last();
        var count = int.Parse(lastSelector.Split('[')[1].Split(']')[0]);
        
        var textNodes = foundNode.Nodes().OfType<XText>().ToList();

        if ((count - 1) < textNodes.Count)
        {
            return textNodes[count - 1];
        }

        return null;
    }
}