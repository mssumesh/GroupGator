﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version:2.0.40302.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace Microsoft.Sample.Compression {
    using System;
    using System.IO;
    using System.Resources;
    
    
    // This class was auto-generated by the Strongly Typed Resource Builder
    // class via a tool like ResGen or Visual Studio.NET.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    sealed class Resource1 {
        
        private static System.Resources.ResourceManager _resMgr;
        
        private Resource1() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if ((_resMgr == null)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager(typeof(Resource1));
                    System.Threading.Thread.MemoryBarrier();
                    _resMgr = temp;
                }
                return _resMgr;
            }
        }
        
        public static System.Drawing.Bitmap CopyToolStripMenuItem_Image {
            get {
                return ((System.Drawing.Bitmap)(ResourceManager.GetObject("CopyToolStripMenuItem.Image")));
            }
        }
        
        public static System.Drawing.Bitmap CutToolStripMenuItem_Image {
            get {
                return ((System.Drawing.Bitmap)(ResourceManager.GetObject("CutToolStripMenuItem.Image")));
            }
        }
        
        public static System.Drawing.Bitmap NewDeflateToolStripMenuItem_Image {
            get {
                return ((System.Drawing.Bitmap)(ResourceManager.GetObject("NewDeflateToolStripMenuItem.Image")));
            }
        }
        
        public static System.Drawing.Bitmap NewGzipToolStripMenuItem_Image {
            get {
                return ((System.Drawing.Bitmap)(ResourceManager.GetObject("NewGzipToolStripMenuItem.Image")));
            }
        }
        
        public static System.Drawing.Bitmap OpenToolStripMenuItem_Image {
            get {
                return ((System.Drawing.Bitmap)(ResourceManager.GetObject("OpenToolStripMenuItem.Image")));
            }
        }
        
        public static System.Drawing.Bitmap RedoToolStripMenuItem_Image {
            get {
                return ((System.Drawing.Bitmap)(ResourceManager.GetObject("RedoToolStripMenuItem.Image")));
            }
        }
        
        public static System.Drawing.Bitmap UndoToolStripMenuItem_Image {
            get {
                return ((System.Drawing.Bitmap)(ResourceManager.GetObject("UndoToolStripMenuItem.Image")));
            }
        }
    }
}
