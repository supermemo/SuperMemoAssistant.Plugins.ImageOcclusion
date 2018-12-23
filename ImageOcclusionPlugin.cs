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
// Created On:   2018/12/19 14:43
// Modified On:  2018/12/19 17:13
// Modified By:  Alexis

#endregion




using System.Linq;
using System.Windows.Input;
using SuperMemoAssistant.Interop.Plugins;
using SuperMemoAssistant.Interop.SuperMemo.Components.Controls;
using SuperMemoAssistant.Interop.SuperMemo.Components.Models;
using SuperMemoAssistant.Services;
using SuperMemoAssistant.Sys.IO.Devices;

namespace SuperMemoAssistant.Plugins.ImageOcclusion
{
  // ReSharper disable once UnusedMember.Global
  // ReSharper disable once ClassNeverInstantiated.Global
  public class ImageOcclusionPlugin : SMAPluginBase<ImageOcclusionPlugin>
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
    protected override void OnInit()
    {
      Svc<ImageOcclusionPlugin>.KeyboardHotKey.RegisterHotKey(new HotKey(true,
                                                                         false,
                                                                         false,
                                                                         true,
                                                                         Key.O,
                                                                         "Create/Edit Occlusion"),
                                                              CreateOrEditOcclusion);
    }

    #endregion




    #region Methods

    private bool CreateOrEditOcclusion()
    {
      var ctrlGroup = Svc.SMA.UI.ElementWindow.ControlGroup;

      if (ctrlGroup == null)
        return true;

      var imgCtrlCount = ctrlGroup.Count(ctrl => ctrl.Type == ComponentType.Image);

      if (imgCtrlCount == 2 && ctrlGroup.Count == 2)
        EditOcclusion(ctrlGroup);

      else if (imgCtrlCount == 1)
        CreateOcclusion(ctrlGroup);
      
      return true;
    }

    private void EditOcclusion(IControlGroup ctrlGroup)
    {

    }

    private void CreateOcclusion(IControlGroup ctrlGroup)
    {

    }

    #endregion
  }
}
