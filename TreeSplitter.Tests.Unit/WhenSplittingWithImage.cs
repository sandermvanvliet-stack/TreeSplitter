using System.Xml;
using System.Xml.Linq;
using FluentAssertions;

namespace TreeSplitter.Tests.Unit
{
    public class WhenSplittingWithImage
    {
        private const string Html = "<div><img id=\"someImage\" src=\"someImage.png\"/></div>";

        private readonly List<Segment> _segments = Splitter.Split(XElement.Parse(Html), 0);

        [Fact]
        public void GivenTagWithNestingDepthTwo_ReturnsFourSegments()
        {
            _segments.Should().HaveCount(1);
        }

        [Fact]
        public void GivenTagWithNestingDepthTwo_SegmentTextValuesMatch()
        {
            var theConcatenatedSegments = string.Join("|", _segments.Select(s => s.Text));

            theConcatenatedSegments
                .Should()
                .Be("");
        }

        [Fact]
        public void GivenTagWithNestingDepthTwo_SegmentOffsetsMatch()
        {
            _segments[0].Start.Should().Be(0);
            _segments[0].End.Should().Be(0);
        }

        [Fact]
        public void GivenTagWithNestingDepthTwo_SegmentElementsMatch()
        {
            _segments[0].Element.NodeType.Should().Be(XmlNodeType.Element);
        }

        [Fact]
        public void GivenTagWithNestingDepthTwo_SegmentParentElementsMatch()
        {
            _segments[0].ParentElement!.Should().BeOfType<XElement>().Which.Name.LocalName.Should().Be("div");
        }
    }
}