using System.Xml.Linq;

namespace TreeSplitter.Tests.Unit;

public class TreeShaker
{
    public static XElement Shake(XElement element, string highlightNodeName)
    {
        if (element.Name.LocalName == highlightNodeName)
        {
            // Walk through the siblings to see if there
            // are adjacent highlight nodes. If there are
            // then merge with ourselves.
            while (element.NextNode is XElement siblingHighlight && 
                   siblingHighlight.Name.LocalName == highlightNodeName)
            {
                element.LastNode!.AddAfterSelf(siblingHighlight.Nodes());
                siblingHighlight.Remove();
            }
        }
        else if (element.HasElements)
        {
            foreach (var child in element.Elements())
            {
                Shake(child, highlightNodeName);
            }
        }

        return element;
    }
}