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
// Modified On:  2019/04/24 02:26
// Modified By:  Alexis

#endregion




using System.Linq;
using SuperMemoAssistant.Interop.SuperMemo.Content.Controls;
using SuperMemoAssistant.Interop.SuperMemo.Content.Models;
using SuperMemoAssistant.Services;
using SuperMemoAssistant.Services.Sentry;

namespace SuperMemoAssistant.Plugins.ImageOcclusion
{
  // ReSharper disable once UnusedMember.Global
  // ReSharper disable once ClassNeverInstantiated.Global
  public class ImageOcclusionPlugin : SentrySMAPluginBase<ImageOcclusionPlugin>
  {
    #region Constructors

    public ImageOcclusionPlugin() { }

    #endregion




    #region Properties Impl - Public

    /// <inheritdoc />
    public override string Name => "ImageOcclusion";

    #endregion




    #region Methods Impl

    /// <inheritdoc />
    protected override void PluginInit()
    {
      //Svc.KeyboardHotKey.RegisterHotKey(
      //  new HotKey(true,
      //             false,
      //             false,
      //             true,
      //             Key.O,
      //             "Create/Edit Occlusion"),
      //CreateOrEditOcclusion);
    }

    #endregion




    #region Methods

    private void CreateOrEditOcclusion()
    {
      var ctrlGroup = Svc.SMA.UI.ElementWindow.ControlGroup;

      if (ctrlGroup == null)
        return;

      var imgCtrlCount = ctrlGroup.Count(ctrl => ctrl.Type == ComponentType.Image);

      if (imgCtrlCount == 2 && ctrlGroup.Count == 2)
        EditOcclusion(ctrlGroup);

      else if (imgCtrlCount == 1)
        CreateOcclusion(ctrlGroup);
      
    }

    // ReSharper disable once UnusedParameter.Local
    private void EditOcclusion(IControlGroup ctrlGroup) { }

    // ReSharper disable once UnusedParameter.Local
    private void CreateOcclusion(IControlGroup ctrlGroup) { }

    #endregion
  }
}
