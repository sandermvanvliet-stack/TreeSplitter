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

        [Fact]
        public void Eight_One()
        {
            string html = "<body><p>This is a paragraph with text in it that is reasonably long so we can test some things</p></body>";
            var highlights = new List<(string startSelector, int startOffset, string endSelector, int endoffset)>
            {
                ("/body/p[1]/text()[1]", 5, "/body/p[1]/text()[1]", 7),
                ("/body/p[1]/text()[2]", 13, "/body/p[1]/text()[2]", 17),
            };


            XElement result = null;

            foreach (var (startSelector, startOffset, endSelector, endOffset) in highlights)
            {
                result = Highlighter.Highlight(html, startOffset, startSelector, endOffset, endSelector, CreateHighlightNode);
                result = TreeShaker.Shake(result, HighlightNodeName);
                html = result.ToString();
            }


            ShouldBe(result, "<body><p>This <xx>is</xx> a paragraph <xx>with</xx> text in it that is reasonably long so we can test some things</p></body>");
        }

        [Fact]
        public void Eight_One_Reverse()
        {
            var html = "<body><p>This is a paragraph with text in it that is reasonably long so we can test some things</p></body>";
            var highlights = new List<(string startSelector, int startOffset, string endSelector, int endoffset)>
            {
                ("/body/p[1]/text()[2]", 13, "/body/p[1]/text()[2]", 17),
                ("/body/p[1]/text()[1]", 5, "/body/p[1]/text()[1]", 7),
            };
            
            XElement result = null;

            foreach (var (startSelector, startOffset, endSelector, endOffset) in highlights)
            {
                result = Highlighter.Highlight(html, startOffset, startSelector, endOffset, endSelector, CreateHighlightNode);
                result = TreeShaker.Shake(result, HighlightNodeName);
                html = result.ToString();
            }
            
            ShouldBe(result, "<body><p>This <xx>is</xx> a paragraph <xx>with</xx> text in it that is reasonably long so we can test some things</p></body>");
        }

        //[Fact]
        public void Eleven() {
       var html = @"
<p>Lorem ipsum dolor sit amet, <strong>consectetur adipiscing elit</strong>. Vivamus porttitor nec nibh ac molestie. Etiam non mi augue. Donec eu velit bibendum, vestibulum lacus ut, fringilla leo. Nam lacinia, mi et ullamcorper blandit, nulla nulla sodales erat, ut tincidunt urna dolor pretium lorem. Integer eu ipsum vitae quam <strong>pellentesque condimentum. Suspendisse sit amet ullamcorper nisl,</strong> vitae commodo neque. Vestibulum eu nunc dapibus, consectetur libero nec, scelerisque nisl. Sed bibendum rhoncus neque. Donec vehicula libero non erat tincidunt placerat.</p>
<p><a href=""https://stackoverflowteams.local/c/testteam/images/s/6e175caf-ba4f-4187-8ff9-6bb11691d863.png""><img src=""https://stackoverflowteams.local/c/testteam/images/s/6e175caf-ba4f-4187-8ff9-6bb11691d863.png"" alt=""enter image description here"" /></a></p>
<p>Aliquam auctor congue tempus. Vivamus tincidunt sit amet justo et viverra. Pellentesque vitae sollicitudin metus, in eleifend eros. Quisque et orci vitae lectus egestas finibus euismod n<span id=""inline-comment-158"" class=""js-inline-comment c-pointer bg-yellow-300"" role=""button"" aria-label=""Show comment for this selection"" data-id=""158"">ec nisl. In hac habitasse platea dictumst. Duis diam libero, imperdiet porta hendrerit id, finibus vel magna. Pellentesque posuere arcu eu pharetra dignissim. Curabitur est elit, luctus in sapien in, posuere dapibus nunc. Nam egestas quam sed tincidunt sagittis. Nulla velit ipsum, imperdiet nec nisi vel, luctus interdum nunc. In eget venenatis neque, vel lobortis ante. Maecenas fringill</span>a ante tempus, fermentum libero vitae, molestie erat. Nulla in tortor suscipit, bibendum augue ac, faucibus lectus. Praesent ut justo et arcu semper rutrum. Morbi vitae augue dui.</p>
<ul>
<li><p>Even with list items</p>
</li>
<li><p><span id=""inline-comment-159"" class=""js-inline-comment c-pointer bg-yellow-300"" role=""button"" aria-label=""Show comment for this selection"" data-id=""159"">the selection will support it</span></p>
</li>
<li><p><span id=""inline-comment-159"" class=""js-inline-comment c-pointer bg-yellow-300"" role=""button"" aria-label=""Show comment for this selection"" data-id=""159"">best of that, it even works for partial highlights in a list</span></p>
</li>
<li><p>or across a list and the next paragraph</p>
</li>
</ul>
<p>Praesent sit amet dui egestas, luctus lorem in, auctor urna. Phasellus ultrices, elit vitae feugiat tincidunt, dolor sem porta neque, at congue risus massa quis justo. Duis luctus tellus lorem, et imperdiet ex efficitur ac. Nam iaculis lorem eu rutrum semper. Donec cursus massa tortor, vitae tincidunt libero rhoncus rhoncus. Mauris suscipit nulla tellus, id rhoncus lacus elementum eget. Fusce tincidunt vehicula ligula vel gravida. Ut fringilla vel dui eu luctus. Nullam consectetur libero nec luctus egestas. Praesent vitae orci interdum, rutrum sem et, auctor eros. Morbi mattis ipsum nec ante venenatis molestie. Vivamus risus eros, tempor vitae rhoncus in, iaculis vitae enim. Nullam non nisl lorem. In gravida lacus nulla, eu commodo eros laoreet vitae. Praesent dapibus quis odio at pellentesque.</p>
<p>Mauris mi odio, scelerisque vitae rhoncus et, eleifend ac mauris. Nunc in velit enim. Sed maximus felis nisi, sit amet elementum libero dapibus vitae. Proin euismod nulla eget mauris accumsan tristique. Suspendisse mollis viverra vestibulum. Vivamus eleifend egestas nisl, id accumsan risus gravida sit amet. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nam malesuada dui eget ipsum posuere eleifend. Vestibulum urna leo, dapibus vel ipsum in, mattis porttitor risus.</p>
<p>Praesent tristique velit mauris, quis feugiat tellus facilisis quis. Aliquam bibendum tortor lacus, sed faucibus nisl finibus eget. Suspendisse convallis faucibus nulla vel porttitor. Maecenas accumsan ultrices ex, vel iaculis mauris. Phasellus semper, quam eget maximus eleifend, eros eros ultricies nulla, at convallis orci erat a sem. Praesent vel nibh ac quam suscipit laoreet. Mauris volutpat enim a erat malesuada gravida. Sed eget rhoncus justo, vel dictum felis. Donec sit amet lobortis elit.</p>
<pre class=""lang-typescript s-code-block""><code class=""hljs language-typescript""><span class=""hljs-title function_"">it</span>(<span class=""hljs-string"">""selecting the text of the second paragraph""</span>, <span class=""hljs-function"">() =&gt;</span> {
    <span class=""hljs-keyword"">const</span> rootNode = <span class=""hljs-title function_"">givenRootNodeWithHtml</span>(<span class=""hljs-string"">'&lt;div&gt;&lt;p id=""p1""&gt;para 1&lt;/p&gt;&lt;p id=""p2""&gt;para 2&lt;/p&gt;&lt;p id=""p3""&gt;para 3&lt;/p&gt;&lt;/div&gt;'</span>);
    <span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160""></span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160""><span class=""hljs-keyword"">const</span></span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160""> anchorNode = rootNode.</span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160""><span class=""hljs-title function_"">querySelector</span></span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160"">(</span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160""><span class=""hljs-string"">'#p2'</span></span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160"">).</span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160""><span class=""hljs-property"">firstChild</span></span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160"">;</span>

    <span class=""hljs-keyword"">const</span> selector = <span class=""hljs-title function_"">buildSelectorFor</span>(&lt;<span class=""hljs-title class_"">HTMLElement</span>&gt;rootNode.<span class=""hljs-property"">firstChild</span>, anchorNode);

    <span class=""hljs-title function_"">expect</span>(selector).<span class=""hljs-title function_"">toBe</span>(<span class=""hljs-string"">'&gt;p[2]&gt;#text[1]'</span>);
});
</code></pre>
<p>Another paragraph after the code</p>";

       //const string startSelector = "/body/div[1]/span[1]/strong[1]/text()[1]";
       //const string endSelector = "/body/div[2]/strong[1]/text()[1]";
        var startSelector = "/p[1]/strong[2]/text()[1]";
        var endSelector = "/p[1]/text()[4]";
            
        var result =Highlighter.Highlight(html, 45, startSelector, 117, endSelector, CreateRealHighlightNode);
        result = TreeShaker.Shake(result, element => element.Name.LocalName == "span" && element.Attribute(XName.Get("id")).Value == "161");

        var expected = @"
<p>Lorem ipsum dolor sit amet, <strong>consectetur adipiscing elit</strong>. Vivamus porttitor nec nibh ac molestie. Etiam non mi augue. Donec eu velit bibendum, vestibulum lacus ut, fringilla leo. Nam lacinia, mi et ullamcorper blandit, nulla nulla sodales erat, ut tincidunt urna dolor pretium lorem. Integer eu ipsum vitae quam <strong>pellentesque condimentum. Suspendisse sit ame<span id=""inline-comment-161"" class=""js-inline-comment c-pointer bg-yellow-300"" role=""button"" aria-label=""Show comment for this selection"" data-id=""161"">t ullamcorper nisl,</span></strong><span id=""inline-comment-161"" class=""js-inline-comment c-pointer bg-yellow-300"" role=""button"" aria-label=""Show comment for this selection"" data-id=""161""> vitae commodo neque. Vestibulum eu nunc dapibus, consectetur libero nec, scelerisque nisl. Sed bibendum rhoncus neque. Donec vehicula libero non erat tincidunt placerat.</span></p>
<p><a href=""https://stackoverflowteams.local/c/testteam/images/s/6e175caf-ba4f-4187-8ff9-6bb11691d863.png""><img src=""https://stackoverflowteams.local/c/testteam/images/s/6e175caf-ba4f-4187-8ff9-6bb11691d863.png"" alt=""enter image description here"" /></a></p>
<p>Aliquam auctor congue tempus. Vivamus tincidunt sit amet justo et viverra. Pellentesque vitae sollicitudin metus, in eleifend eros. Quisque et orci vitae lectus egestas finibus euismod n<span id=""inline-comment-158"" class=""js-inline-comment c-pointer bg-yellow-300"" role=""button"" aria-label=""Show comment for this selection"" data-id=""158"">ec nisl. In hac habitasse platea dictumst. Duis diam libero, imperdiet porta hendrerit id, finibus vel magna. Pellentesque posuere arcu eu pharetra dignissim. Curabitur est elit, luctus in sapien in, posuere dapibus nunc. Nam egestas quam sed tincidunt sagittis. Nulla velit ipsum, imperdiet nec nisi vel, luctus interdum nunc. In eget venenatis neque, vel lobortis ante. Maecenas fringill</span>a ante tempus, fermentum libero vitae, molestie erat. Nulla in tortor suscipit, bibendum augue ac, faucibus lectus. Praesent ut justo et arcu semper rutrum. Morbi vitae augue dui.</p>
<ul>
<li><p>Even with list items</p>
</li>
<li><p><span id=""inline-comment-159"" class=""js-inline-comment c-pointer bg-yellow-300"" role=""button"" aria-label=""Show comment for this selection"" data-id=""159"">the selection will support it</span></p>
</li>
<li><p><span id=""inline-comment-159"" class=""js-inline-comment c-pointer bg-yellow-300"" role=""button"" aria-label=""Show comment for this selection"" data-id=""159"">best of that, it even works for partial highlights in a list</span></p>
</li>
<li><p>or across a list and the next paragraph</p>
</li>
</ul>
<p>Praesent sit amet dui egestas, luctus lorem in, auctor urna. Phasellus ultrices, elit vitae feugiat tincidunt, dolor sem porta neque, at congue risus massa quis justo. Duis luctus tellus lorem, et imperdiet ex efficitur ac. Nam iaculis lorem eu rutrum semper. Donec cursus massa tortor, vitae tincidunt libero rhoncus rhoncus. Mauris suscipit nulla tellus, id rhoncus lacus elementum eget. Fusce tincidunt vehicula ligula vel gravida. Ut fringilla vel dui eu luctus. Nullam consectetur libero nec luctus egestas. Praesent vitae orci interdum, rutrum sem et, auctor eros. Morbi mattis ipsum nec ante venenatis molestie. Vivamus risus eros, tempor vitae rhoncus in, iaculis vitae enim. Nullam non nisl lorem. In gravida lacus nulla, eu commodo eros laoreet vitae. Praesent dapibus quis odio at pellentesque.</p>
<p>Mauris mi odio, scelerisque vitae rhoncus et, eleifend ac mauris. Nunc in velit enim. Sed maximus felis nisi, sit amet elementum libero dapibus vitae. Proin euismod nulla eget mauris accumsan tristique. Suspendisse mollis viverra vestibulum. Vivamus eleifend egestas nisl, id accumsan risus gravida sit amet. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nam malesuada dui eget ipsum posuere eleifend. Vestibulum urna leo, dapibus vel ipsum in, mattis porttitor risus.</p>
<p>Praesent tristique velit mauris, quis feugiat tellus facilisis quis. Aliquam bibendum tortor lacus, sed faucibus nisl finibus eget. Suspendisse convallis faucibus nulla vel porttitor. Maecenas accumsan ultrices ex, vel iaculis mauris. Phasellus semper, quam eget maximus eleifend, eros eros ultricies nulla, at convallis orci erat a sem. Praesent vel nibh ac quam suscipit laoreet. Mauris volutpat enim a erat malesuada gravida. Sed eget rhoncus justo, vel dictum felis. Donec sit amet lobortis elit.</p>
<pre class=""lang-typescript s-code-block""><code class=""hljs language-typescript""><span class=""hljs-title function_"">it</span>(<span class=""hljs-string"">""selecting the text of the second paragraph""</span>, <span class=""hljs-function"">() =&gt;</span> {
    <span class=""hljs-keyword"">const</span> rootNode = <span class=""hljs-title function_"">givenRootNodeWithHtml</span>(<span class=""hljs-string"">'&lt;div&gt;&lt;p id=""p1""&gt;para 1&lt;/p&gt;&lt;p id=""p2""&gt;para 2&lt;/p&gt;&lt;p id=""p3""&gt;para 3&lt;/p&gt;&lt;/div&gt;'</span>);
    <span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160""></span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160""><span class=""hljs-keyword"">const</span></span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160""> anchorNode = rootNode.</span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160""><span class=""hljs-title function_"">querySelector</span></span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160"">(</span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160""><span class=""hljs-string"">'#p2'</span></span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160"">).</span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160""><span class=""hljs-property"">firstChild</span></span><span class=""js-inline-comment js-inline-comment-cs c-pointer bg-orange-300 js-inline-comment-selected"" id=""inline-comment-160"" role=""button"" aria-label=""Show comment for this selection"" data-id=""160"">;</span>

    <span class=""hljs-keyword"">const</span> selector = <span class=""hljs-title function_"">buildSelectorFor</span>(&lt;<span class=""hljs-title class_"">HTMLElement</span>&gt;rootNode.<span class=""hljs-property"">firstChild</span>, anchorNode);

    <span class=""hljs-title function_"">expect</span>(selector).<span class=""hljs-title function_"">toBe</span>(<span class=""hljs-string"">'&gt;p[2]&gt;#text[1]'</span>);
});
</code></pre>
<p>Another paragraph after the code</p>";
        ShouldBe(result, expected);
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

        private static XElement CreateRealHighlightNode()
        {
            var element = new XElement(XName.Get("span"));
            element.SetAttributeValue(XName.Get("id"), "inline-comment-161");
            element.SetAttributeValue(XName.Get("class"), "js-inline-comment c-pointer bg-yellow-300");
            element.SetAttributeValue(XName.Get("role"), "button");
            element.SetAttributeValue(XName.Get("aria-label"), "Show comment for this selection");
            element.SetAttributeValue(XName.Get("data-id"), "161");
            return element;
        }
    }
}