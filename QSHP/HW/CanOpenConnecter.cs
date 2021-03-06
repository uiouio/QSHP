﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMO;
namespace QSHP.HW
{
    internal class CanOpenConnecter: IDisposable
    {
        private static CanOpenConnecter uniqueInstance = null;

        private static readonly object locker = new object();

        private bool connected = false;

        private int devIndex = 0;

        public CanOpenObj Pcan = null;

        public int DevIndex
        {
            get
            {
                return devIndex;
            }
            set
            {
                devIndex = value;
            }
        }

        public bool Connected
        {
            get
            {
                return connected;
            }
        }

        public CanOpenConnecter()
        {
            Pcan = new CanOpenObj();
        }

        public static CanOpenConnecter GetInstance()
        {
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new CanOpenConnecter();
                    }
                }
            }
            return uniqueInstance;
        }

        public bool ConnectDriver()
        {
            try
            {
                if (!connected)
                {
                    Pcan.BitRate = CML_BIT_RATES.BITRATE_500_Kbit_per_sec;
                    Pcan.PortName = string.Format("copley{0}", devIndex);
                    Pcan.DevIndex = devIndex;
                    Pcan.Initialize();
                }
                connected = true;
            }
            catch
            {
                connected = false;
            }
            return connected;
        }

        public bool DisConnectDriver()
        {
            try
            {
                if (connected)
                {
                    Pcan.Close();
                }
                connected = false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            if (connected)
            {
                DisConnectDriver();
            }
            Pcan.Dispose();
        }
    }
}
