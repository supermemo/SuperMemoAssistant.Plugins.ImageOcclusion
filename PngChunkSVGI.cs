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
// Created On:   2019/03/02 18:29
// Modified On:  2019/04/24 02:48
// Modified By:  Alexis

#endregion




using System.Text;
using Hjg.Pngcs;
using Hjg.Pngcs.Chunks;

namespace SuperMemoAssistant.Plugins.ImageOcclusion
{
  // ReSharper disable once InconsistentNaming
  internal class PngChunkSVGI : PngChunkSingle
  {
    #region Constants & Statics

    // ID must follow the PNG conventions: four ascii letters,
    // ID[0] : lowercase (ancillary)
    // ID[1] : lowercase if private, upppecase if public
    // ID[3] : uppercase if "safe to copy"
    // ReSharper disable once InconsistentNaming
    public const string ID = "svGi";

    #endregion




    #region Properties & Fields - Non-Public

    private string _svg;

    #endregion




    #region Constructors

    public PngChunkSVGI(ImageInfo info)
      : base(ID, info)
    {
      _svg = string.Empty;
    }

    #endregion




    #region Methods Impl

    public override ChunkOrderingConstraint GetOrderingConstraint()
    {
      // change this if you don't require this chunk to be before IDAT, etc
      return ChunkOrderingConstraint.BEFORE_IDAT;
    }

    // in this case, we have that the chunk data corresponds to the serialized object
    public override ChunkRaw CreateRawChunk()
    {
      byte[] arr = Encoding.UTF8.GetBytes(_svg);
      var c = createEmptyChunk(arr.Length, true);
      c.Data = arr;

      return c;
    }

    public override void ParseFromRaw(ChunkRaw c)
    {
      _svg = Encoding.UTF8.GetString(c.Data);
    }

    public override void CloneDataFromRead(PngChunk other)
    {
      PngChunkSVGI otherx = (PngChunkSVGI)other;
      _svg = otherx._svg; // shallow clone, we could implement other copying
    }

    #endregion




    #region Methods

    public string GetSVG()
    {
      return _svg;
    }

    public void SetSVG(string osvg)
    {
      _svg = osvg;
    }

    #endregion
  }
}
