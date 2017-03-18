/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2017 iText Group NV
    Authors: iText Software.

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License version 3
    as published by the Free Software Foundation with the addition of the
    following permission added to Section 15 as permitted in Section 7(a):
    FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
    ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
    OF THIRD PARTY RIGHTS

    This program is distributed in the hope that it will be useful, but
    WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
    or FITNESS FOR A PARTICULAR PURPOSE.
    See the GNU Affero General Public License for more details.
    You should have received a copy of the GNU Affero General Public License
    along with this program; if not, see http://www.gnu.org/licenses or write to
    the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
    Boston, MA, 02110-1301 USA, or download the license from the following URL:
    http://itextpdf.com/terms-of-use/

    The interactive user interfaces in modified source and object code versions
    of this program must display Appropriate Legal Notices, as required under
    Section 5 of the GNU Affero General Public License.

    In accordance with Section 7(b) of the GNU Affero General Public License,
    a covered work must retain the producer line in every PDF that is created
    or manipulated using iText.

    You can be released from the requirements of the license by purchasing
    a commercial license. Buying such a license is mandatory as soon as you
    develop commercial activities involving the iText software without
    disclosing the source code of your own applications.
    These activities include: offering paid services to customers as an ASP,
    serving PDFs on the fly in a web application, shipping iText with a closed
    source product.

    For more information, please contact iText Software Corp. at this
    address: sales@itextpdf.com */
using System;
using System.IO;
using System.Text.RegularExpressions;
using iText.Html2pdf.Css;
using iText.Html2pdf.Css.Parse;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Versions.Attributes;
using iText.Kernel;

namespace iText.Html2pdf.Attach.Impl {
    public class FontFaceSrcTest {
        private static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/html2pdf/attacher/impl/FontFaceSrcTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void SrcPropertyTest() {
            String fontSrc = "web-fonts/droid-serif-invalid.";
            CssStyleSheet styleSheet = CssStyleSheetParser.Parse(new FileStream(sourceFolder + "srcs.css", FileMode.Open
                , FileAccess.Read));
            CssFontFaceRule fontFaceRule = (CssFontFaceRule)styleSheet.GetStatements()[0];
            CssDeclaration src = fontFaceRule.GetProperties()[0];
            NUnit.Framework.Assert.AreEqual("src", src.GetProperty(), "src expected");
            String[] sources = iText.IO.Util.StringUtil.Split(src.GetExpression(), ",");
            NUnit.Framework.Assert.AreEqual(27, sources.Length, "27 sources expected");
            for (int i = 0; i < sources.Length; i++) {
                Match m = iText.IO.Util.StringUtil.Match(FontFace.FontFaceSrc.UrlPattern, sources[i]);
                NUnit.Framework.Assert.IsTrue(m.Success, "Expression doesn't match pattern: " + sources[i]);
                String format = iText.IO.Util.StringUtil.Group(m, FontFace.FontFaceSrc.FormatGroup);
                String source2 = String.Format("%s(%s)%s", iText.IO.Util.StringUtil.Group(m, FontFace.FontFaceSrc.TypeGroup
                    ), iText.IO.Util.StringUtil.Group(m, FontFace.FontFaceSrc.UrlGroup), format != null ? String.Format(" format(%s)"
                    , format) : "");
                String url = FontFace.FontFaceSrc.Unquote(iText.IO.Util.StringUtil.Group(m, FontFace.FontFaceSrc.UrlGroup)
                    );
                NUnit.Framework.Assert.IsTrue(url.StartsWith(fontSrc), "Invalid url: " + url);
                NUnit.Framework.Assert.IsTrue(format == null || FontFace.FontFaceSrc.ParseFormat(format) != FontFace.FontFormat
                    .None, "Invalid format: " + format);
                NUnit.Framework.Assert.AreEqual(sources[i], source2, "Group check fails: ");
                FontFace.FontFaceSrc fontFaceSrc = FontFace.FontFaceSrc.Create(sources[i]);
                NUnit.Framework.Assert.IsTrue(fontFaceSrc.src.StartsWith(fontSrc), "Invalid url: " + fontSrc);
                String type = "url";
                if (fontFaceSrc.isLocal) {
                    type = "local";
                }
                NUnit.Framework.Assert.IsTrue(sources[i].StartsWith(type), "Type '" + type + "' expected: " + sources[i]);
                switch (fontFaceSrc.format) {
                    case FontFace.FontFormat.OpenType: {
                        NUnit.Framework.Assert.IsTrue(sources[i].Contains("opentype"), "Format " + fontFaceSrc.format + " expected: "
                             + sources[i]);
                        break;
                    }

                    case FontFace.FontFormat.TrueType: {
                        NUnit.Framework.Assert.IsTrue(sources[i].Contains("truetype"), "Format " + fontFaceSrc.format + " expected: "
                             + sources[i]);
                        break;
                    }

                    case FontFace.FontFormat.SVG: {
                        NUnit.Framework.Assert.IsTrue(sources[i].Contains("svg"), "Format " + fontFaceSrc.format + " expected: " +
                             sources[i]);
                        break;
                    }

                    case FontFace.FontFormat.None: {
                        NUnit.Framework.Assert.IsFalse(sources[i].Contains("format("), "Format " + fontFaceSrc.format + " expected: "
                             + sources[i]);
                        break;
                    }
                }
            }
        }
    }
}