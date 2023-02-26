using System.Xml;
using System.Xml.Linq;
using FluentAssertions;

namespace TreeSplitter.Tests.Unit
{
    public class WhenSplitting
    {
        private const string Html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
        private readonly List<Segment> _segments = Splitter.Split(XElement.Parse(Html), 0);

        [Fact]
        public void GivenTagWithNestingDepthTwo_ReturnsFourSegments()
        {
            _segments.Should().HaveCount(4);
        }

        [Fact]
        public void GivenTagWithNestingDepthTwo_SegmentTextValuesMatch()
        {
            var theConcatenatedSegments = string.Join("|", _segments.Select(s => s.Text));

            theConcatenatedSegments
                .Should()
                .Be("some |nested |bold| text");
        }

        [Fact]
        public void GivenTagWithNestingDepthTwo_SegmentOffsetsMatch()
        {
            _segments[0].Start.Should().Be(0);
            _segments[0].End.Should().Be(5);
            _segments[1].Start.Should().Be(5);
            _segments[1].End.Should().Be(12);
            _segments[2].Start.Should().Be(12);
            _segments[2].End.Should().Be(16);
            _segments[3].Start.Should().Be(16);
            _segments[3].End.Should().Be(21);
        }

        [Fact]
        public void GivenTagWithNestingDepthTwo_SegmentElementsMatch()
        {
            _segments[0].Element.NodeType.Should().Be(XmlNodeType.Text);
            _segments[1].Element.NodeType.Should().Be(XmlNodeType.Text);
            _segments[2].Element.NodeType.Should().Be(XmlNodeType.Text);
            _segments[3].Element.NodeType.Should().Be(XmlNodeType.Text);
        }

        [Fact]
        public void GivenTagWithNestingDepthTwo_SegmentParentElementsMatch()
        {
            _segments[0].ParentElement!.Should().BeOfType<XElement>().Which.Name.LocalName.Should().Be("div");
            _segments[1].ParentElement!.Should().BeOfType<XElement>().Which.Name.LocalName.Should().Be("span");
            _segments[2].ParentElement!.Should().BeOfType<XElement>().Which.Name.LocalName.Should().Be("strong");
            _segments[3].ParentElement!.Should().BeOfType<XElement>().Which.Name.LocalName.Should().Be("span");
        }
    }
}