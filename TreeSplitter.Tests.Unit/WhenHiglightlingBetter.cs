using System.Xml.Linq;
using FluentAssertions;

namespace TreeSplitter.Tests.Unit
{
    public class WhenHighlighting
    {
        private const string HighlightNodeName = "xx";

        [Fact]
        public void Zero()
        {
            const string html = "<div>some simple text</div>";
            const string startSelector = "/div[1]/text()[1]";
            const string endSelector = "/div[1]/text()[1]";

            var result = Highlighter.Highlight(html, 5, startSelector, 11, endSelector, CreateHighlightNode);

            ShouldBe(result, "<div>some <xx>simple</xx> text</div>");
        }

        [Fact]
        public void ZeroPointOne()
        {
            const string html = "<div>some simple text</div>";
            const string startSelector = "/div[1]/text()[1]";
            const string endSelector = "/div[1]/text()[1]";

            var result = Highlighter.Highlight(html, 0, startSelector, 11, endSelector, CreateHighlightNode);

            ShouldBe(result, "<div><xx>some simple</xx> text</div>");
        }

        [Fact]
        public void ZeroPointTwo()
        {
            const string html = "<div>some simple text</div>";
            const string startSelector = "/div[1]/text()[1]";
            const string endSelector = "/div[1]/text()[1]";

            var result = Highlighter.Highlight(html, 5, startSelector, 16, endSelector, CreateHighlightNode);

            ShouldBe(result, "<div>some <xx>simple text</xx></div>");
        }

        [Fact]
        public void One()
        {
            const string html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
            const string startSelector = "/div[1]/span[1]/text()[1]";
            const string endSelector = "/div[1]/span[1]/strong[1]/text()[1]";

            var result = Highlighter.Highlight(html, 3, startSelector, 2, endSelector, CreateHighlightNode);

            ShouldBe(result, "<div>some <span>nes<xx>ted </xx><strong><xx>bo</xx>ld</strong> text</span></div>");
        }

        [Fact]
        public void One_With_TreeShake()
        {
            const string html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
            const string startSelector = "/div[1]/span[1]/text()[1]";
            const string endSelector = "/div[1]/span[1]/strong[1]/text()[1]";

            var result = Highlighter.Highlight(html, 3, startSelector, 2, endSelector, CreateHighlightNode);

            result = TreeShaker.Shake(result, HighlightNodeName);

            ShouldBe(result, "<div>some <span>nes<xx>ted </xx><strong><xx>bo</xx>ld</strong> text</span></div>");
        }

        [Fact]
        public void Two()
        {
            const string html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
            const string startSelector = "/div[1]/span[1]/text()[1]";
            const string endSelector = "/div[1]/span[1]/strong[1]/text()[1]";

            var result = Highlighter.Highlight(html, 3, startSelector, 4, endSelector, CreateHighlightNode);

            ShouldBe(result, "<div>some <span>nes<xx>ted </xx><xx><strong>bold</strong></xx> text</span></div>");
        }

        [Fact]
        public void Two_With_TreeShake()
        {
            const string html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
            const string startSelector = "/div[1]/span[1]/text()[1]";
            const string endSelector = "/div[1]/span[1]/strong[1]/text()[1]";

            var result = Highlighter.Highlight(html, 3, startSelector, 4, endSelector, CreateHighlightNode);

            result = TreeShaker.Shake(result, HighlightNodeName);

            ShouldBe(result, "<div>some <span>nes<xx>ted <strong>bold</strong></xx> text</span></div>");
        }

        [Fact]
        public void Three()
        {
            const string html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
            const string startSelector = "/div[1]/span[1]/text()[1]";
            const string endSelector = "/div[1]/span[1]/text()[2]";

            var result = Highlighter.Highlight(html, 3, startSelector, 3, endSelector, CreateHighlightNode);

            ShouldBe(result,
                "<div>some <span>nes<xx>ted </xx><xx><strong>bold</strong></xx><xx> te</xx>xt</span></div>");
        }

        [Fact]
        public void Three_With_TreeShake()
        {
            const string html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
            const string startSelector = "/div[1]/span[1]/text()[1]";
            const string endSelector = "/div[1]/span[1]/text()[2]";

            var result = Highlighter.Highlight(html, 3, startSelector, 3, endSelector, CreateHighlightNode);

            result = TreeShaker.Shake(result, HighlightNodeName);

            ShouldBe(result,
                "<div>some <span>nes<xx>ted <strong>bold</strong> te</xx>xt</span></div>");
        }

        [Fact]
        public void Four()
        {
            const string html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
            const string startSelector = "/div[1]/span[1]/strong[1]/text()[1]";
            const string endSelector = "/div[1]/span[1]/text()[2]";

            var result = Highlighter.Highlight(html, 2, startSelector, 3, endSelector, CreateHighlightNode);

            ShouldBe(result, "<div>some <span>nested <strong>bo<xx>ld</xx></strong><xx> te</xx>xt</span></div>");
        }

        [Fact]
        public void Four_With_TreeShake()
        {
            const string html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
            const string startSelector = "/div[1]/span[1]/strong[1]/text()[1]";
            const string endSelector = "/div[1]/span[1]/text()[2]";

            var result = Highlighter.Highlight(html, 2, startSelector, 3, endSelector, CreateHighlightNode);
            
            result = TreeShaker.Shake(result, HighlightNodeName);

            ShouldBe(result, "<div>some <span>nested <strong>bo<xx>ld</xx></strong><xx> te</xx>xt</span></div>");
        }

        [Fact]
        public void Five()
        {
            const string html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
            const string startSelector = "/div[1]/span[1]/strong[1]/text()[1]";
            const string endSelector = "/div[1]/span[1]/strong[1]/text()[1]";

            var result = Highlighter.Highlight(html, 0, startSelector, 4, endSelector, CreateHighlightNode);

            ShouldBe(result, "<div>some <span>nested <xx><strong>bold</strong></xx> text</span></div>");
        }

        [Fact]
        public void Five_With_TreeShake()
        {
            const string html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
            const string startSelector = "/div[1]/span[1]/strong[1]/text()[1]";
            const string endSelector = "/div[1]/span[1]/strong[1]/text()[1]";

            var result = Highlighter.Highlight(html, 0, startSelector, 4, endSelector, CreateHighlightNode);
            
            result = TreeShaker.Shake(result, HighlightNodeName);

            ShouldBe(result, "<div>some <span>nested <xx><strong>bold</strong></xx> text</span></div>");
        }

        [Fact]
        public void Six()
        {
            const string html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
            const string startSelector = "/div[1]/span[1]/strong[1]/text()[1]";
            const string endSelector = "/div[1]/span[1]/text()[2]";

            var result = Highlighter.Highlight(html, 0, startSelector, 1, endSelector, CreateHighlightNode);

            ShouldBe(result, "<div>some <span>nested <xx><strong>bold</strong></xx><xx> </xx>text</span></div>");
        }

        [Fact]
        public void Six_With_TreeShake()
        {
            const string html = "<div>some <span>nested <strong>bold</strong> text</span></div>";
            const string startSelector = "/div[1]/span[1]/strong[1]/text()[1]";
            const string endSelector = "/div[1]/span[1]/text()[2]";

            var result = Highlighter.Highlight(html, 0, startSelector, 1, endSelector, CreateHighlightNode);

            result = TreeShaker.Shake(result, HighlightNodeName);

            ShouldBe(result, "<div>some <span>nested <xx><strong>bold</strong> </xx>text</span></div>");
        }

        [Fact]
        public void Seven()
        {
            const string html = "<body><div>some <span>nested <strong>bold</strong> text</span></div><div>ending in the <strong>second</strong> paragraph</div></body>";
            const string startSelector = "/body/div[1]/span[1]/strong[1]/text()[1]";
            const string endSelector = "/body/div[2]/strong[1]/text()[1]";

            var result = Highlighter.Highlight(html, 0, startSelector, 3, endSelector, CreateHighlightNode);

            ShouldBe(result, "<body><div>some <span>nested <xx><strong>bold</strong></xx><xx> text</xx></span></div><div><xx>ending in the </xx><strong><xx>sec</xx>ond</strong> paragraph</div></body>");
        }

        [Fact]
        public void Seven_With_TreeShake()
        {
            const string html = "<body><div>some <span>nested <strong>bold</strong> text</span></div><div>ending in the <strong>second</strong> paragraph</div></body>";
            const string startSelector = "/body/div[1]/span[1]/strong[1]/text()[1]";
            const string endSelector = "/body/div[2]/strong[1]/text()[1]";

            var result = Highlighter.Highlight(html, 0, startSelector, 3, endSelector, CreateHighlightNode);

            result = TreeShaker.Shake(result, HighlightNodeName);

            ShouldBe(result, "<body><div>some <span>nested <xx><strong>bold</strong> text</xx></span></div><div><xx>ending in the </xx><strong><xx>sec</xx>ond</strong> paragraph</div></body>");
        }

        private static void ShouldBe(XNode result, string expected)
        {
            var html = result.ToString(SaveOptions.OmitDuplicateNamespaces | SaveOptions.DisableFormatting);

            html
                .Should()
                .Be(expected);
        }

        private static XElement CreateHighlightNode()
        {
            return new XElement(XName.Get(HighlightNodeName));
        }
    }
}