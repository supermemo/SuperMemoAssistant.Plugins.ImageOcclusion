#region License & Metadata

// The MIT License (MIT)
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// 
// 
// Created On:   2018/12/19 15:07
// Modified On:  2018/12/19 17:05
// Modified By:  Alexis

#endregion




using System.Drawing;
using System.Windows.Forms;

namespace SuperMemoAssistant.Plugins.ImageOcclusion
{
  // ReSharper disable once ClassNeverInstantiated.Global
  public class ImageOcclusionCfg
  {
    #region Properties & Fields - Public

    public Point WindowLocation { get; set; } = new Point(100,
                                                          100);
    public Size WindowSize { get; set; } = new Size(1080,
                                                    720);
    public FormWindowState WindowState { get; set; } = FormWindowState.Normal;

    public string FillColor   { get; set; } = "FFEBA2";
    public string StrokeColor { get; set; } = "2D2D2D";
    public int    StrokeWidth { get; set; } = 2;

    #endregion
  }
}
