using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Management;
using Microsoft.Win32;

namespace Gator
    {
    class FingerPrint
        {
        public string mid;
        public FingerPrint()
            {
            mid = string.Empty;
            }
        
        private string GetWindowsProductID()
            {
            string id = string.Empty;
            try
                {
                RegistryKey reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion", false);
                id = Convert.ToString(reg.GetValue("ProductId"));
                reg.Close();
                }
            catch (Exception ex)
                {
                ;
                }
            return id;

            }

        private string GetMACaddress()
            {

            ManagementClass mc = new ManagementClass();
            ManagementObjectCollection moc = null;
            string MACAddress = String.Empty;

            try
                {
                mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                    {
                    if (MACAddress == String.Empty)
                        { // only return MAC Address from first card
                        if ((bool)mo["IPEnabled"] == true)
                            MACAddress = mo["MacAddress"].ToString();
                        }
                    }
                }
            catch (Exception ex)
                {
                //MACAddress = "DEADCAFEBEEFDEAD";
                MACAddress = string.Empty;
                }
            finally
                {
                moc.Dispose();
                mc.Dispose();
                }
            return MACAddress;
            }

        private string GetCpuId()
            {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass();
            ManagementObjectCollection moc = null;
            try
                {
                mc = new ManagementClass("win32_processor");
                moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                    {
                    if (cpuInfo == "")
                        {
                        //Get only the first CPU's ID
                        cpuInfo = mo.Properties["processorID"].Value.ToString();
                        break;
                        }
                    }
                }
            catch (Exception ex)
                {
                //cpuInfo = "DEADBEEFCAFEDEAD";
                cpuInfo = string.Empty;
                }
            finally
                {
                mc.Dispose();
                moc.Dispose();
                }

            return cpuInfo;
            }

        public string GetComuputerID()
            {
            string compid = string.Empty;
            string mac = GetMACaddress();
            string cpu = GetCpuId();
            string winid = string.Empty;

            if ((mac == string.Empty) && (cpu == string.Empty))
                {
                winid = GetWindowsProductID();
                if (winid == string.Empty)
                    winid = "BEEFCAFEDEADDEADBEEFCAFE";
                compid = winid;                
                }
            else
                compid = mac + cpu;
            string id_md5 = string.Empty;
            id_md5 = CreateMD5Hash(compid);
            if (id_md5.Length > 32)
                id_md5 = id_md5.Substring(0, 32);

            return id_md5;

            }

        public string CreateMD5Hash(string input)
            {
            // Use input string to calculate MD5 hash
            StringBuilder sb = new StringBuilder();
            try
                {
                System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string

                for (int i = 0; i < hashBytes.Length; i++)
                    {
                    sb.Append(hashBytes[i].ToString("X2"));
                    // To force the hex string to lower-case letters instead of
                    // upper-case, use he following line instead:
                    // sb.Append(hashBytes[i].ToString("x2")); 
                    }
                }
            catch (Exception ex)
                {
                ;
                }
            return sb.ToString();
            }
        }
    }

//managementobject dsk;
//    string volumeSerial = string.Empty;

//    try
//        {
//        dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
//        dsk.Get();
//        volumeSerial = dsk["VolumeSerialNumber"].ToString();
//        }
//    catch (Exception ex)
//        {
//        volumeSerial = "DEADCAFEBEEFDEAD";
//        }

//    return volumeSerial;
//    }
