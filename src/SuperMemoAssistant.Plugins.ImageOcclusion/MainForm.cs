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
// Created On:   2018/12/19 14:47
// Modified On:  2018/12/19 15:23
// Modified By:  Alexis

#endregion




using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Windows.Forms;
using Hjg.Pngcs;
using Hjg.Pngcs.Chunks;
using SuperMemoAssistant.Services;
using Svg;

namespace SuperMemoAssistant.Plugins.ImageOcclusion
{
  public partial class MainForm : Form
  {
    #region Constants & Statics

    private const string SvgEditorPath = "svgedit/svg-editor.html";

    #endregion




    #region Constructors

    public MainForm(string backgroundFilePath,
                    string occlusionFilePath)
    {
      InitializeComponent();

      OcclusionFilePath = occlusionFilePath;
      OriginalSvg       = ReadSvgFromChunk();
      Config            = Svc.Configuration.Load<ImageOcclusionCfg>().Result;

      GetImageSize(backgroundFilePath,
                   out var width,
                   out var height);

      OcclusionWidth  = width;
      OcclusionHeight = height;

      wb.DocumentCompleted += Wb_DocumentCompleted;
      wb.Navigate(string.Format("{0}?{1}",
                                GetSvgEditorUri(),
                                GenerateUrlParams(backgroundFilePath,
                                                  width,
                                                  height)));
    }

    #endregion




    #region Properties & Fields - Public

    public string OriginalSvg       { get; }
    public string OcclusionFilePath { get; }
    public int    OcclusionWidth    { get; }
    public int    OcclusionHeight   { get; }

    public ImageOcclusionCfg Config { get; set; }

    #endregion




    #region Methods Impl

    protected override void OnLoad(EventArgs e)
    {
      WindowState = Config.WindowState;
      Location    = Config.WindowLocation;
      Size        = Config.WindowSize;

      base.OnLoad(e);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      Config.WindowState = WindowState;

      switch (WindowState)
      {
        case FormWindowState.Maximized:
          Config.WindowLocation = RestoreBounds.Location;
          Config.WindowSize     = RestoreBounds.Size;
          break;
        case FormWindowState.Normal:
          Config.WindowLocation = Location;
          Config.WindowSize     = Size;
          break;
      }

      Svc.Configuration.Save(Config);

      base.OnClosing(e);
    }

    protected override bool ProcessCmdKey(ref Message msg,
                                          Keys        keyData)
    {
      if (keyData == Keys.Escape)
      {
        Close();

        return true;
      }

      else if (keyData == (Keys.Control | Keys.Shift | Keys.S))
      {
        SaveOcclusion();

        return true;
      }

      else if (keyData == (Keys.Control | Keys.S))
      {
        SaveOcclusionAndExit();

        return true;
      }

      return base.ProcessCmdKey(ref msg,
                                keyData);
    }

    #endregion




    #region Methods

    private void Wb_DocumentCompleted(object                               sender,
                                      WebBrowserDocumentCompletedEventArgs e)
    {
      if (string.IsNullOrWhiteSpace(OriginalSvg) == false)
        SetSvgInBrowser(OriginalSvg);
    }

    private Uri GetSvgEditorUri()
    {
      var appFolder = Application.StartupPath;
      var svgPath = Path.Combine(appFolder,
                                 SvgEditorPath);

      return new Uri($"file:///{svgPath}");
    }

    private string GenerateUrlParams(string backgroundFilePath,
                                     int    width,
                                     int    height)
    {
      var urlParams = HttpUtility.ParseQueryString(string.Empty);

      urlParams.Add("bkgd_url",
                    backgroundFilePath);
      urlParams.Add("dimensions",
                    $"{width},{height}");
      urlParams.Add("initFill[color]",
                    Config.FillColor);
      urlParams.Add("initFill[opacity]",
                    "1");
      urlParams.Add("initStroke[color]",
                    Config.StrokeColor);
      urlParams.Add("initStroke[width]",
                    Config.StrokeWidth.ToString());
      urlParams.Add("initStroke[opacity]",
                    "1");

      return urlParams.ToString();
    }

    private void GetImageSize(string  filePath,
                              out int width,
                              out int height)
    {
      using (Image img = Image.FromFile(filePath))
      {
        width  = img.Width;
        height = img.Height;
      }
    }

    private void SetSvgInBrowser(string svg)
    {
      svg = svg.Replace("\r",
                        "").Replace("\n",
                                    "");

      if (wb.Document != null)
        wb.Document.InvokeScript("eval",
                                 new object[]
                                 {
                                   $"svgCanvas.setSvgString('{svg}')"
                                 });
    }

    private string GetSvgFromBrowser()
    {
      return wb.Document?.InvokeScript("eval",
                                      new object[] { "svgCanvas.svgCanvasToString()" }).ToString();
    }

    private Bitmap ConvertSvgToImage(string svg,
                                     int    width,
                                     int    height)
    {
      var svgDoc = SvgDocument.FromSvg<SvgDocument>(svg);

      return svgDoc.Draw(width,
                         height);
    }

    private void CreateChunk(PngWriter pngw,
                             string    svg)
    {
      PngChunkSVGI chunk = new PngChunkSVGI(pngw.ImgInfo);
      chunk.SetSVG(svg);
      chunk.Priority = true;

      pngw.GetChunksList().Queue(chunk);
    }

    private Stream ToMemoryStream(string filePath)
    {
      MemoryStream memStream = new MemoryStream();

      using (Stream inStream = File.OpenRead(filePath))
      {
        byte[] buffer = new byte[8192];

        while (inStream.Read(buffer,
                             0,
                             buffer.Length) > 0)
          memStream.Write(buffer,
                          0,
                          buffer.Length);
      }

      memStream.Seek(0,
                     SeekOrigin.Begin);

      return memStream;
    }

    private void WriteSvgToChunk(string tmpOcclusionFilePath,
                                 string svg)
    {
      using (Stream inStream = ToMemoryStream(tmpOcclusionFilePath))
      {
        PngReader pngr = new PngReader(inStream);
        PngWriter pngw = FileHelper.CreatePngWriter(tmpOcclusionFilePath,
                                                    pngr.ImgInfo,
                                                    true);

        pngw.CopyChunksFirst(pngr,
                             ChunkCopyBehaviour.COPY_ALL_SAFE);

        CreateChunk(pngw,
                    svg);

        for (int row = 0; row < pngr.ImgInfo.Rows; row++)
        {
          ImageLine l1 = pngr.ReadRow(row);
          pngw.WriteRow(l1,
                        row);
        }

        pngw.CopyChunksLast(pngr,
                            ChunkCopyBehaviour.COPY_ALL);

        pngr.End();
        pngw.End();
      }
    }

    private string ReadSvgFromChunk()
    {
      var pngr = FileHelper.CreatePngReader(OcclusionFilePath);

      PngChunkSVGI chunk = (PngChunkSVGI)pngr.GetChunksList().GetById1(PngChunkSVGI.ID);

      pngr.End();

      return chunk?.GetSVG();
    }

    private void SaveOcclusion()
    {
      string tmpOcclusionFilePath = Path.GetTempFileName();
      string svg                  = GetSvgFromBrowser();

      using (Bitmap img = ConvertSvgToImage(svg,
                                            OcclusionWidth,
                                            OcclusionHeight))
        img.Save(tmpOcclusionFilePath,
                 ImageFormat.Png);

      WriteSvgToChunk(tmpOcclusionFilePath,
                      svg);

      if (File.Exists(OcclusionFilePath))
        File.Delete(OcclusionFilePath);

      File.Move(tmpOcclusionFilePath,
                OcclusionFilePath);
    }

    private void SaveOcclusionAndExit()
    {
      SaveOcclusion();
      Close();
    }

    private void btnCancel_Click(object    sender,
                                 EventArgs e)
    {
      Close();
    }

    private void btnSave_Click(object    sender,
                               EventArgs e)
    {
      SaveOcclusion();
    }

    private void btnSaveExit_Click(object    sender,
                                   EventArgs e)
    {
      SaveOcclusionAndExit();
    }

    #endregion
  }
}
