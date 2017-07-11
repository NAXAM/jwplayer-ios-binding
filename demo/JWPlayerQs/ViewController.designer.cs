// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace JWPlayerQs
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UITextView callbacksView { get; set; }

        [Outlet]
        UIKit.UIButton insertAdButton { get; set; }

        [Outlet]
        UIKit.UILabel playbackTime { get; set; }

        [Outlet]
        UIKit.UIButton playButton { get; set; }
        
        void ReleaseDesignerOutlets ()
        {
            if (callbacksView != null) {
                callbacksView.Dispose ();
                callbacksView = null;
            }

            if (insertAdButton != null) {
                insertAdButton.Dispose ();
                insertAdButton = null;
            }

            if (playbackTime != null) {
                playbackTime.Dispose ();
                playbackTime = null;
            }

            if (playButton != null) {
                playButton.Dispose ();
                playButton = null;
            }
        }
    }
}
