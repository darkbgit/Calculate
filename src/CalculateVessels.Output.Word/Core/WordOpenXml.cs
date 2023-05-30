using CalculateVessels.Output.Word.Enums;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using ImageMagick;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace CalculateVessels.Output.Word.Core;

internal static class WordOpenXml
{
    public static Paragraph AddParagraph(this Body body, string text = "")
    {
        var p = body.AppendChild(new Paragraph());
        var r = p.AppendChild(new Run());
        r.AppendChild(new Text(text)
        {
            Space = SpaceProcessingModeValues.Preserve
        });
        return p;
    }

    public static Run AddRun(this Paragraph p, string runText = "")
    {
        var r = p.AppendChild(new Run());
        r.AppendChild(new Text(runText)
        {
            Space = SpaceProcessingModeValues.Preserve
        });
        return r;
    }

    public static Run AddRun(this Run r, string runText = "")
    {
        if (r.Parent is not Paragraph p) throw new InvalidOperationException();

        var newRun = p.AppendChild(new Run());
        r.AppendChild(new Text(runText)
        {
            Space = SpaceProcessingModeValues.Preserve
        });
        return newRun;
    }

    public static Paragraph AppendEquation(this Paragraph p, string text)
    {
        var pM = p.AppendChild(new DocumentFormat.OpenXml.Math.Paragraph());
        var m = new DocumentFormat.OpenXml.Math.OfficeMath(new Run(new Text(text)));
        pM.AppendChild(m);
        return p;
    }

    public static Paragraph AppendEquation(this Run r, string text)
    {
        if (r.Parent is not Paragraph p) throw new InvalidOperationException();

        var pM = p.AppendChild(new DocumentFormat.OpenXml.Math.Paragraph());
        var m = new DocumentFormat.OpenXml.Math.OfficeMath(new Run(new Text(text)));
        pM.AppendChild(m);
        return p;
    }

    public static Paragraph Alignment(this Paragraph p, AlignmentType alignmentType)
    {
        if (!p.Elements<ParagraphProperties>().Any())
        {
            p.PrependChild(new ParagraphProperties());
        }

        var pPr = p.Elements<ParagraphProperties>().First();

        pPr.Justification = alignmentType switch
        {
            AlignmentType.Center => new Justification { Val = JustificationValues.Center },
            AlignmentType.Right => new Justification { Val = JustificationValues.Right },
            AlignmentType.Left => new Justification { Val = JustificationValues.Left },
            _ => pPr.Justification
        };

        return p;
    }

    public static Paragraph Heading(this Paragraph p, HeadingType headingType)
    {
        if (!p.Elements<ParagraphProperties>().Any())
        {
            p.PrependChild(new ParagraphProperties());
        }

        var pPr = p.Elements<ParagraphProperties>().First();

        pPr.ParagraphStyleId = headingType switch
        {
            HeadingType.Heading1 => new ParagraphStyleId { Val = "1" },
            _ => pPr.ParagraphStyleId
        };

        return p;
    }

    public static Paragraph Bold(this Paragraph p)
    {
        var r = p.GetFirstChild<Run>() ?? throw new InvalidOperationException();

        if (!r.Elements<RunProperties>().Any())
        {
            r.PrependChild(new RunProperties());
        }

        var rPr = r.Elements<RunProperties>().First();

        rPr.Bold = new Bold();

        return p;
    }

    public static Run Bold(this Run r)
    {
        if (!r.Elements<RunProperties>().Any())
        {
            r.PrependChild(new RunProperties());
        }

        var rPr = r.Elements<RunProperties>().First();

        rPr.Bold = new Bold();

        return r;
    }

    public static Paragraph Italic(this Paragraph p)
    {
        var r = p.GetFirstChild<Run>() ?? throw new InvalidOperationException();

        if (!r.Elements<RunProperties>().Any())
        {
            r.PrependChild(new RunProperties());
        }

        var rPr = r.Elements<RunProperties>().First();

        rPr.Italic = new Italic();

        return p;
    }

    public static Run Italic(this Run r)
    {
        if (!r.Elements<RunProperties>().Any())
        {
            r.PrependChild(new RunProperties());
        }

        var rPr = r.Elements<RunProperties>().First();

        rPr.Italic = new Italic();

        return r;
    }

    public static Paragraph Underline(this Paragraph p)
    {
        var r = p.GetFirstChild<Run>() ?? throw new InvalidOperationException();

        if (!r.Elements<RunProperties>().Any())
        {
            r.PrependChild(new RunProperties());
        }

        var rPr = r.Elements<RunProperties>().First();

        rPr.Underline = new Underline
        {
            Val = new EnumValue<UnderlineValues>(UnderlineValues.Single)
        };

        return p;
    }

    public static Run Underline(this Run r)
    {
        if (!r.Elements<RunProperties>().Any())
        {
            r.PrependChild(new RunProperties());
        }

        var rPr = r.Elements<RunProperties>().First();

        rPr.Underline = new Underline();

        return r;
    }

    public static Paragraph Color(this Paragraph p, System.Drawing.Color color)
    {
        var r = p.GetFirstChild<Run>() ?? throw new InvalidOperationException();

        if (!r.Elements<RunProperties>().Any())
        {
            r.PrependChild(new RunProperties());
        }

        var rPr = r.Elements<RunProperties>().First();

        rPr.Color = new Color
        {
            Val = color.ToString()
        };

        return p;
    }

    public static Run Color(this Run r, System.Drawing.Color color)
    {
        if (!r.Elements<RunProperties>().Any())
        {
            r.PrependChild(new RunProperties());
        }

        var rPr = r.Elements<RunProperties>().First();

        rPr.Color = new Color
        {
            Val = color.ToString()
        };

        return r;
    }

    public static void AddImage(this Paragraph p, string relationshipId, byte[] image)
    {
        var (width, height) = GetImageSize(image);

        // Define the reference of the image.
        var element =
             new Drawing(
                 new DW.Inline(
                     new DW.Extent { Cx = width, Cy = height },
                     new DW.EffectExtent
                     {
                         LeftEdge = 0L,
                         TopEdge = 0L,
                         RightEdge = 0L,
                         BottomEdge = 0L
                     },
                     new DW.DocProperties
                     {
                         Id = (UInt32Value)(uint)image.GetHashCode(),
                         Name = "Picture 1"
                     },
                     new DW.NonVisualGraphicFrameDrawingProperties(
                         new A.GraphicFrameLocks { NoChangeAspect = true }),
                     new A.Graphic(
                         new A.GraphicData(
                             new PIC.Picture(
                                 new PIC.NonVisualPictureProperties(
                                     new PIC.NonVisualDrawingProperties()
                                     {
                                         Id = (UInt32Value)(uint)image.GetHashCode(),
                                         Name = "New Bitmap Image.jpg"
                                     },
                                     new PIC.NonVisualPictureDrawingProperties()),
                                 new PIC.BlipFill(
                                     new A.Blip(
                                         new A.BlipExtensionList(
                                             new A.BlipExtension()
                                             {
                                                 Uri =
                                                    "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                             })
                                     )
                                     {
                                         Embed = relationshipId,
                                         CompressionState =
                                         A.BlipCompressionValues.Print
                                     },
                                     new A.Stretch(
                                         new A.FillRectangle())),
                                 new PIC.ShapeProperties(
                                     new A.Transform2D(
                                         new A.Offset() { X = 0L, Y = 0L },
                                         new A.Extents { Cx = width, Cy = height }),
                                     new A.PresetGeometry(
                                         new A.AdjustValueList()
                                     )
                                     { Preset = A.ShapeTypeValues.Rectangle }))
                         )
                         { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                 )
                 {
                     DistanceFromTop = (UInt32Value)0U,
                     DistanceFromBottom = (UInt32Value)0U,
                     DistanceFromLeft = (UInt32Value)0U,
                     DistanceFromRight = (UInt32Value)0U,
                     EditId = "50D07946"
                 });

        p.AppendChild(new Run(element));
    }

    public static Table AddTable(this Body body)
    {
        Table table = new();

        // Create a TableProperties object and specify its border information.
        TableProperties tblProp = new(
            new TableBorders(
                new TopBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single)
                },
                new BottomBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single)
                },
                new LeftBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single)
                },
                new RightBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single)
                },
                new InsideHorizontalBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single)
                },
                new InsideVerticalBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single)
                }
            )
        );
        table.AppendChild(tblProp);

        return table;
    }

    public static TableRow AddRow(this Table table)
    {
        TableRow tr = new();
        table.AppendChild(tr);
        return tr;
    }

    public static TableRow AddRowWithOneCell(this Table table, string text, JustificationValues justification = JustificationValues.Center, int gridSpanCount = 0)
    {
        if (table.GetFirstChild<TableRow>() == null)
        {
            throw new InvalidOperationException();
        }

        TableRow tr = new();

        TableCell tc = new();

        tc.Append(new TableCellProperties(
            new GridSpan
            {
                Val = gridSpanCount == 0 ? table.ChildElements.OfType<TableRow>().Last().ChildElements.Count : gridSpanCount
            }),
            //new Justification { Val = justification }),
            new Paragraph(
                new ParagraphProperties(new Justification
                {
                    Val = justification
                }),
                new Run(new Text(text)
                {
                    Space = SpaceProcessingModeValues.Preserve
                })));

        tr.Append(tc);

        table.Append(tr);
        return tr;
    }

    public static TableRow AddCell(this TableRow tr, string text = "")
    {
        TableCell tc = new();

        tc.AppendChild(new Paragraph(new Run(new Text(text)
        {
            Space = SpaceProcessingModeValues.Preserve
        })));

        tr.AppendChild(tc);

        return tr;
    }


    public static TableRow AppendText(this TableRow tr, string text)
    {
        if (tr.Elements<TableCell>().Last() is not { } tc)
            throw new InvalidOperationException();

        var p = tc.GetFirstChild<Paragraph>();

        if (p == null)
        {
            tc.AppendChild(new Paragraph());
        }

        tc.GetFirstChild<Paragraph>()!.AppendChild(new Run(new Text(text)
        {
            Space = SpaceProcessingModeValues.Preserve
        }));
        return tr;
    }


    public static TableRow AppendEquation(this TableRow tr, string text)
    {
        var tc = tr.Elements<TableCell>().Last();
        var p = tc.GetFirstChild<Paragraph>();
        if (p == null)
        {
            tc.AppendChild(new Paragraph());
        }
        var pM = p!.AppendChild(new DocumentFormat.OpenXml.Math.Paragraph());
        var m = new DocumentFormat.OpenXml.Math.OfficeMath(new Run(new Text(text)));
        pM.AppendChild(m);
        return tr;
    }

    public static void InsertTable(this Body body, Table table)
    {
        body.AppendChild(table);
    }

    private static (long width, long height) GetImageSize(byte[] image)
    {
        const int emusPerPixel = 9525;

        using var img = new MagickImage(image);

        long iWidth = img.Page.Width;
        long iHeight = img.Page.Height;

        iWidth = (long)Math.Round((decimal)iWidth * emusPerPixel);
        iHeight = (long)Math.Round((decimal)iHeight * emusPerPixel);

        return (iWidth, iHeight);
    }
}
