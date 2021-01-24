using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using Gator;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;


namespace Gator25
    {
    class Plugin
        {
        public ArrayList FilePaths;
        public int Count;

        delegate string Name ( );
        delegate int A();
        delegate string Version ();
        
        private Gator2.Gator2Main gt;
        private ArrayList pInst;
        private ArrayList pTypes;

        public Plugin(Gator2.Gator2Main gtmain)
            {
            gt = gtmain;
            FilePaths = new ArrayList();
            pInst = new ArrayList();
            pTypes = new ArrayList();
            FilePaths.Clear();
            pInst.Clear();
            pTypes.Clear();
            Queue plgnfiles = new Queue();
            plgnfiles.Clear();
            GGDisk disk = new GGDisk();
            disk.GetPluginDlls(ref plgnfiles);
            Count = plgnfiles.Count;
            gtmain.Log("");


            for (int i = 0; i < Count; i++)
                {
                //FilePaths[i] = plgnfiles.Dequeue(); 
                FilePaths.Add(plgnfiles.Dequeue());
                }

            if (Count > 0)
                {
                //ipi = new IPlugin[Count];
                gtmain.pluginsToolStripMenuItem.Enabled = true;

                for (int i = 0; i < Count; i++)
                    {
                    Assembly assembly = Assembly.LoadFrom(FilePaths[i].ToString());
                    foreach (Type type in assembly.GetTypes())
                        {
                        // Pick up a class
                        if (type.IsClass == true)
                            {
                            // If it does not implement the IBase Interface, skip it
                            if (type.GetInterface("Gator.IPlugin") == null)
                                {
                                continue;
                                }
                            pTypes.Add(type);
                            // If however, it does implement the IBase Interface,
                            // create an instance of the object
                            object ibaseObject = Activator.CreateInstance(type);
                            pInst.Add(ibaseObject);

                            // Create the parameter list
                            //object[] arguments = new object[] { 10, 17.11 };
                            object result;
                            // Dynamically Invoke the Object

                            //result = type.InvokeMember("compute",
                            //                         BindingFlags.Default | BindingFlags.InvokeMethod,
                            //                         null,
                            //                         ibaseObject,
                            //                         arguments);
                            result = type.InvokeMember("Name",
                                                     BindingFlags.Default | BindingFlags.InvokeMethod,
                                                     null,
                                                     ibaseObject,
                                                     null);
                            //pNames.Add(result);
                            ToolStripMenuItem item = new ToolStripMenuItem();
                            item = new ToolStripMenuItem();
                            item.Name = i.ToString();
                            item.Tag = FilePaths[i];
                            item.Text = (string)result;
                            item.Click += new EventHandler(MenuItemClickHandler);
                            gtmain.pluginsToolStripMenuItem1.DropDownItems.Add(item);
                            }
                        //int hModule = LoadLibrary(FilePaths[i].ToString());
                        ////if (hModule == 0) break;
                        //IntPtr intPtr = GetProcAddress(hModule, "Wish");
                        //A a = (A)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(A));
                        //a();
                        //FreeLibrary(hModule);
                        //gtmain.Log ("Loaded plugin : ");
                        //Assembly ass = Assembly.Load(FilePaths[i].ToString());
                        //AppDomain.CurrentDomain.Load(ass.GetName());
                        //Type t = assembly.GetType("Wish");
                        //t.e
                        //Type ObjType = null;
                        //ass = 
                        //if (ass != null)
                        //    {
                        //    string args = "AdminPanel.AdminPanel";// FilePaths[i].ToString().Substring("AdminPanel.AdminPanel");
                        //    ObjType = ass.GetType(args);
                        //    if (ObjType != null)
                        //        {
                        //        ipi[i] = (IPlugin)Activator.CreateInstance(ObjType);
                        //        //ipi[i].Host = this;
                        //        }
                        //}

                        ////string name = GetPluginName ( FilePaths[i] );
                        //string name = ipi[i].GetName();

                        //ToolStripMenuItem item = new ToolStripMenuItem();
                        //item = new ToolStripMenuItem();
                        //item.Name = "dmenu" + i.ToString();
                        //item.Tag = FilePaths[i];
                        //item.Text = name;
                        //item.Click += new EventHandler(MenuItemClickHandler);
                        //gtmain.pluginsToolStripMenuItem.DropDownItems.Add(item);
                        //MessageBox.Show (name);
                        }
                    }
                }
            }

        //private string GetPluginName(object p)
        //    {
        //    string ret = LoadDllMethod(p.ToString(), "AdminPanel.AdminPanel", "GetName" );  
        //    //int hModule = LoadLibrary(p.ToString());
        //    //if (hModule == 0) return string.Empty; ;
        //    //IntPtr intPtr = GetProcAddress(hModule, "GetName");
        //    //Name name =(Name) Marshal.GetDelegateForFunctionPointer(intPtr, typeof(Name));
        //    //string ret = name();
        //    //FreeLibrary(hModule);
        //    return ret;

        //    }

        private void MenuItemClickHandler(object sender, EventArgs e)
            {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            int index = Convert.ToInt32(clickedItem.Name);
            Type type = (Type)pTypes[index];
            object result = type.InvokeMember("Display",
                                                     BindingFlags.Default | BindingFlags.InvokeMethod,
                                                     null,
                                                     pInst [ index],
                                                     null);
            }


        //public string LoadDllMethod(string path,  string title_class, string title_void, object[] parameters)
        //     {
        //     Assembly u = Assembly.LoadFile(path);
        //     Type t = u.GetType(title_class);
        //     if (t != null)
        //         {
        //         MethodInfo m = t.GetMethod(title_void);
        //         if (m != null)
        //             {
        //             if (parameters.Length >= 1)
        //                 {
        //                     object[] myparam = new object[1];
        //                     myparam[0] = parameters;
        //                     return (string)m.Invoke(null, myparam);
        //                 }
        //                 else
        //                    return (string)m.Invoke(null, null);
        //             }
        //         }
        //     Exception ex = new Exception("method/class not found");
        //     throw ex;
        //     }

        //string LoadDllMethod( string path, string title_class, string title_void )
        //    {
        //    Assembly u = Assembly.LoadFile(path);
            
        //    //if (t != null)
        //    //    {
        //        AdminPanel.AdminPanel apanel = (AdminPanel.AdminPanel)u.CreateInstance("AdminPanel.AdminPanel");
        //    Type t = apanel.GetType();
        //    MethodInfo mi = t.GetMethod("GetName");
        //    return (string)mi.Invoke(null, null);
        //        //MethodInfo m = t.GetMethod(title_void);
        //        //if (m != null)
        //        //    {
        //        //    return (string)m.Invoke(null, null);
        //        //    }
        //        //}
        //    //Exception ex = new Exception("method/class not found");
        //    //throw ex;
        //    }

        //[DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        //static extern int LoadLibrary(
        //    [MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

        //[DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
        //static extern IntPtr GetProcAddress(int hModule,
        //    [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        //[DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        //static extern bool FreeLibrary(int hModule);

        //[DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        //static extern int LoadLibrary(
        //    [MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

        //[DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
        //static extern IntPtr GetProcAddress(int hModule,
        //    [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        //[DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        //static extern bool FreeLibrary(int hModule);
        }
    
    }


//ToolStripMenuItem[] items = new ToolStripMenuItem[50];
//items[i] = new ToolStripMenuItem();
//items[i].Name = "dmenu" + i.ToString();
//items[i].Tag = FilePaths[i];
//items[i].Text = name;
//items[i].Click += new EventHandler(MenuItemClickHandler);
//gtmain.pluginsToolStripMenuItem.DropDownItems.AddRange(items);


                    
