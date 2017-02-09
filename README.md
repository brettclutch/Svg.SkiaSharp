# Svg.SkiaSharp
.Net 4.6.1 SkiaSharp renderer -> Waiting for .net standard 2.0 for full crossplatform

This is a fork of the SkiaSharp branch from https://github.com/gentledepp/SVG/tree/skiasharp available under Microsoft Public License: https://svg.codeplex.com/license

It has been re-factored for simplicity and ease of use, currently setup as a .net 4.6.1 project and awaiting .net standard 2.0 for cross platform 'ability'.


Supports:
* Paths with full stroke/pen support
* Filled shapes, simple and complex
* Solid Brush
* Linear Gradient Brush
* Path Gradient Brush
* Translation Transform
* Scale Transform
* Rotate Transform

Iffy Support (Havn't fully tested but in theory could work)
* Text Rendering (Simple Text Rendering works but 'fontfamily' selection needs work)
* Draw Image Support (For embedded raster bitmaps in SVG)
* Texture Brush
* Clipping and Masking

Does not support

* Filters 'blur, bevel, drop shadow, glow' (Internally the library has some support to handle it at the bitmap level, but it should be refactored to handle it with SkiaSharp)

Example Video of a test render case:
https://www.screencast.com/t/VIOf2h9e1hDE

Example Use:

```c#
using Svg.SkiaSharp; 
using SkiaSharp; 

SKPicture svgcache; 
SvgDocument svg;

svg = SvgDocument.Open("Path\\To\\Your\\svgfile.svg");

//Utilize and SKPicture to render the SVG and cache the render calls so that you can easily re-draw it without having to call the SVG library again
if (svgcache == null)
{
	SKPictureRecorder recorder = new SKPictureRecorder();
	SKCanvas recordingCanvas = recorder.BeginRecording(new SKRect(0, 0, svg.Width, svg.Height));
	Graphics svgGraphics = new Svg.SkiaSharp.Graphics(recordingCanvas);
	svg.Draw(svgGraphics);
	svgcache = recorder.EndRecording();
}

//Draw the 'cached' Picture to the render canvas you have setup. 
canvas.DrawPicture(svgcache);
```