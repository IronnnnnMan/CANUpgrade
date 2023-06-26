using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;


namespace WindowsFormsApplication1
{
    /*------------兼容ZLG的数据类型---------------------------------*/

    //1.ZLGCAN系列接口卡信息的数据类型。
    public struct VCI_BOARD_INFO
    {
        public UInt16 hw_Version;
        public UInt16 fw_Version;
        public UInt16 dr_Version;
        public UInt16 in_Version;
        public UInt16 irq_Num;
        public byte can_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] str_Serial_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] str_hw_Type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved;
    }

    /////////////////////////////////////////////////////
    //2.定义CAN信息帧的数据类型。
    unsafe public struct VCI_CAN_OBJ  //使用不安全代码
    {
        public uint ID;
        public uint TimeStamp;        //时间标识
        public byte TimeFlag;         //是否使用时间标识
        public byte SendType;         //发送标志。保留，未用
        public byte RemoteFlag;       //是否是远程帧
        public byte ExternFlag;       //是否是扩展帧
        public byte DataLen;

        public fixed byte Data[8];

        public fixed byte Reserved[3];

    }

    //3.定义CAN控制器状态的数据类型。
    public struct VCI_CAN_STATUS
    {
        public byte ErrInterrupt;
        public byte regMode;
        public byte regStatus;
        public byte regALCapture;
        public byte regECCapture;
        public byte regEWLimit;
        public byte regRECounter;
        public byte regTECounter;
        public uint Reserved;
    }

    //4.定义错误信息的数据类型。
    public struct VCI_ERR_INFO
    {
        public uint ErrCode;
        public byte Passive_ErrData1;
        public byte Passive_ErrData2;
        public byte Passive_ErrData3;
        public byte ArLost_ErrData;
    }

    //5.定义初始化CAN的数据类型
    public struct VCI_INIT_CONFIG
    {
        public UInt32 AccCode;
        public UInt32 AccMask;
        public UInt32 Reserved;
        public byte Filter;   //1接收所有帧。2标准帧滤波，3是扩展帧滤波。
        public byte Timing0;
        public byte Timing1;
        public byte Mode;     //模式，0表示正常模式，1表示只听模式,2自测模式
    }

    /*------------其他数据结构描述---------------------------------*/
    //6.USB-CAN总线适配器板卡信息的数据类型1，该类型为VCI_FindUsbDevice函数的返回参数。
    public struct VCI_BOARD_INFO1
    {
        public UInt16 hw_Version;
        public UInt16 fw_Version;
        public UInt16 dr_Version;
        public UInt16 in_Version;
        public UInt16 irq_Num;
        public byte can_Num;
        public byte Reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] str_Serial_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] str_hw_Type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[][] str_Usb_Serial;
    }

    //7.定义常规参数类型
    public struct VCI_REF_NORMAL
    {
        public byte Mode;     //模式，0表示正常模式，1表示只听模式,2自测模式
        public byte Filter;   //1接收所有帧。2标准帧滤波，3是扩展帧滤波。
        public UInt32 AccCode;//接收滤波验收码
        public UInt32 AccMask;//接收滤波屏蔽码
        public byte kBaudRate;//波特率索引号，0-SelfDefine,1-5Kbps(未用),2-18依次为：10kbps,20kbps,40kbps,50kbps,80kbps,100kbps,125kbps,200kbps,250kbps,400kbps,500kbps,666kbps,800kbps,1000kbps,33.33kbps,66.66kbps,83.33kbps
        public byte Timing0;
        public byte Timing1;
        public byte CANRX_EN;//保留，未用
        public byte UARTBAUD;//保留，未用
    }

    //8.定义波特率设置参数类型
    public struct VCI_BAUD_TYPE
    {
        public UInt32 Baud;				//存储波特率实际值
        public byte SJW;				//同步跳转宽度，取值1-4
        public byte BRP;				//预分频值，取值1-64
        public byte SAM;				//采样点，取值0=采样一次，1=采样三次
        public byte PHSEG2_SEL;		    //相位缓冲段2选择位，取值0=由相位缓冲段1时间决定,1=可编程
        public byte PRSEG;				//传播时间段，取值1-8
        public byte PHSEG1;			    //相位缓冲段1，取值1-8
        public byte PHSEG2;			    //相位缓冲段2，取值1-8

    }

    //9.定义Reference参数类型
    public struct VCI_REF_STRUCT
    {
        public VCI_REF_NORMAL RefNormal;
        public byte Reserved;
        public VCI_BAUD_TYPE BaudType;
    }

    /*------------数据结构描述完成---------------------------------*/
    public partial class Form1 : Form
    {
        private const UInt32 gCanID = 0x1F1F1F1F;
        private Byte Timing0, Timing1;
        private const string LibName = "ControlCAN.dll";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DeviceInd"></param>
        /// <param name="Reserved"></param>
        /// <returns></returns>
        /*------------兼容ZLG的函数描述---------------------------------*/
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_OpenDevice(UInt32 DeviceType, UInt32 DeviceInd, UInt32 Reserved);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_CloseDevice(UInt32 DeviceType, UInt32 DeviceInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_InitCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_INIT_CONFIG pInitConfig);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ReadBoardInfo(UInt32 DeviceType, UInt32 DeviceInd, ref VCI_BOARD_INFO pInfo);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ReadErrInfo(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_ERR_INFO pErrInfo);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ReadCANStatus(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_STATUS pCANStatus);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_GetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_SetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_GetReceiveNum(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ClearBuffer(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_StartCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ResetCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_Transmit(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pSend, UInt32 Len);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_Transmit(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, IntPtr pSend, UInt32 Len);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pReceive, UInt32 Len, Int32 WaitTime);

        // [DllImport("controlcan.dll", CharSet = CharSet.Ansi)]
        //static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, IntPtr pReceive, UInt32 Len, Int32 WaitTime);

        /*------------其他函数描述---------------------------------*/
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_GetReference2(UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex, UInt32 Reserved, ref VCI_REF_STRUCT pRefStruct);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_SetReference2(UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex, UInt32 RefType, ref byte pData);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ResumeConfig(UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ConnectDevice(UInt32 DevType, UInt32 DevIndex);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_UsbDeviceReset(UInt32 DevType, UInt32 DevIndex, UInt32 Reserved);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_FindUsbDevice(ref VCI_BOARD_INFO1 pInfo);
        /*------------函数描述结束---------------------------------*/

        /*-----------全局变量声明-------------*/
        UInt16 LastestVerCheckedFlag = 1;
        UInt16 ChangeToOldVersion = 0;
        uint UpgradeChecksum = 0;
        uint CANIDChecksum = 0;


        public Form1()
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void InitializeProgressBar(System.Windows.Forms.ProgressBar progressBar, int minimum, int maximum)
        {
            progressBar.Value = minimum;
            progressBar.Minimum = minimum;
            progressBar.Maximum = maximum;
        }

        private void SetProgressBarValue(System.Windows.Forms.ProgressBar progressBar, int value)
        {
            progressBar.Value = value;
        }

        private void EnableControl(Control control)
        {
            control.Enabled = true;
        }

        private void DisableControl(Control control)
        {
            control.Enabled = false;
        }

        private void ShowMessage(string s)
        {
            MessageBox.Show(s);
        }
        private int ReadFile(string FilePath, out byte[] BinBuf)
        {
            int Len;
            int AlignLen;
            FileStream FS = new FileStream(FilePath, FileMode.Open);
            BinaryReader BR = new BinaryReader(FS);
            Len = (int)FS.Length;
            if (Len % 8 > 0)
            {
                AlignLen = (Len + 8) & (~0x07);
                AlignLen += 64;
            }
            else
            {
                AlignLen = Len;
                AlignLen += 64;
            }
            BinBuf = new byte[AlignLen];
            BR.Read(BinBuf, 0, Len);
            FS.Dispose();
            return AlignLen;
        }
        unsafe private void CanInit()
        {
            int Res;
            VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();

            config.AccCode = 0x00000000;
            config.AccMask = 0xFFFFFFFF;
            config.Timing0 = Timing0;
            config.Timing1 = Timing1;
            config.Filter = 0;
            config.Mode = 0;
            VCI_OpenDevice(4, 0, 0);
            VCI_InitCAN(4, 0, 0, ref config);
            Res = (int)VCI_StartCAN(4, 0, 0);
            if (Res <= 0)
                throw new Exception("CAN initial fail");
        }

        unsafe private void CanInitForUpdate()
        {
            int Res;

            VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();

            config.AccCode = 0x00000000;
            config.AccMask = 0xFFFFFFFF;
            config.Timing0 = 0x01;
            config.Timing1 = 0x1C;
            config.Filter = 0;
            config.Mode = 0;

            //VCI_ResetCAN(4, 0, 0);
            VCI_InitCAN(4, 0, 0, ref config);
            Res = (int)VCI_StartCAN(4, 0, 0);
            if (Res <= 0)
                throw new Exception("CAN initial fail");
        }

        unsafe private bool CanSendData(UInt32 CanID, byte[] data)
        {
            VCI_CAN_OBJ obj = new VCI_CAN_OBJ();
            int Res;

            obj.ID = CanID;
            obj.ExternFlag = 1;
            obj.TimeFlag = 0;
            obj.RemoteFlag = 0;
            obj.SendType = 0;
            for (int i = 0; i < 8; i++)
            {
                obj.Data[i] = data[i];
            }
            obj.DataLen = 8;
            Res = (int)VCI_Transmit(4, 0, 0, ref obj, 1);
            if (Res < 1)
            {
                return false;
            }
            return true;
        }
        // 因为C#是运行在 .NETFRAMEWORK中的，类似JAVA的虚拟机，内存都是由虚拟机托管，所以转成BYTE需要点功夫
        // struct -> byte[]  
        private static byte[] StructToBytes(object structObj, int size)
        {
            //StructDemo sd;
            //int num = 2;
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);      /// malloc
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            //从内存空间拷贝到byte 数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);             // free
            return bytes;
        }
        //将Byte转换为结构体类型
        private static object ByteToStruct(byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }
            //分配结构体内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间转换为目标结构体
            object obj = Marshal.PtrToStructure(structPtr, type);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }
        unsafe private bool CanSendData(UInt32 CanID, byte[] data, int Len)
        {

            int objSize = sizeof(VCI_CAN_OBJ);
            int Offset;
            int BlockCount, BlockSize;
            int Res;
            byte[] tmpBuf;
            VCI_CAN_OBJ obj = new VCI_CAN_OBJ();
            IntPtr pobj;

            BlockSize = Len / 8;
            if (Len % 8 > 0)
            {
                BlockSize += 1;
            }
            pobj = Marshal.AllocHGlobal(BlockSize * objSize);
            Offset = 0;
            BlockCount = 0;
            while (Offset < Len)
            {
                obj.ID = CanID;
                obj.ExternFlag = 1;
                obj.TimeFlag = 0;
                obj.RemoteFlag = 0;
                obj.SendType = 0;
                obj.DataLen = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (i + Offset < Len)
                    {
                        obj.Data[i] = data[Offset + i];
                        obj.DataLen++;
                    }
                    else
                    {
                        obj.Data[i] = 0x00;
                    }
                }

                tmpBuf = StructToBytes(obj, objSize);
                for (int i = 0; i < objSize; i++)
                {
                    Marshal.WriteByte(pobj, BlockCount * objSize + i, tmpBuf[i]);
                }

                Offset += 8;
                BlockCount++;
            }

            Res = (int)VCI_Transmit(4, 0, 0, pobj, (uint)BlockSize);
            Marshal.FreeHGlobal(pobj);
            return Res < BlockSize ? false : true;
            //return true;
        }
        unsafe private bool SendPacket(UInt32 CanID, byte[] Pkt, int TimeOut)
        {
            int Idx = 0;
            byte[] tmpBuf = new byte[8];
            while (Idx < 32)
            {
                MemoryCopy(ref tmpBuf, 0, ref Pkt, Idx, 8);
                if (!CanSendData(CanID, tmpBuf))
                {
                    if (TimeOut == 0)
                    {
                        return false;
                    }
                    TimeOut--;
                }
                else
                {
                    Idx += 8;
                }
            }
            return true;
        }
        unsafe private bool CanRecvData(ref UInt32 CanID, ref byte[] Dat)
        {
            VCI_CAN_OBJ obj = new VCI_CAN_OBJ();
            int Res;

            Res = (int)VCI_Receive(4, 0, 0, ref obj, 1, 10);
            if (Res <= 0)
            {
                return false;
            }
            for (int i = 0; i < 8; i++)
            {
                if (i < obj.DataLen)
                    Dat[i] = obj.Data[i];
                else
                    Dat[i] = 0x00;
            }
            CanID = obj.ID;
            return true;
        }
        private void CanClose()
        {
            VCI_CloseDevice(4, 0);
        }
        private void MemoryCopy(ref byte[] dstBuf, int dstIdx, ref byte[] srcBuf, int srcIdx, int cpSize)
        {
            int i;
            for (i = 0; i < cpSize; i++)
            {
                dstBuf[i + dstIdx] = srcBuf[i + srcIdx];
            }
        }

        private void SendUpgradeMark(UInt32 CanID)
        {
            byte[] Pkt = new byte[8];

            Pkt[0] = 0x00;
            Pkt[1] = 0xAA;
            Pkt[2] = 0x00;
            Pkt[3] = 0xAA;
            Pkt[4] = 0x00;
            Pkt[5] = 0xAA;
            Pkt[6] = 0x00;
            Pkt[7] = 0xAA;
            if (!CanSendData(CanID, Pkt))
            {
                throw new Exception("Error : 10001");
            }
        }

        private void SendCanIdConfigMark(UInt32 CanID)
        {
            byte[] Pkt = new byte[8];

            Pkt[0] = 0x00;
            Pkt[1] = 0xCC;
            Pkt[2] = 0x00;
            Pkt[3] = 0xCC;
            Pkt[4] = 0x00;
            Pkt[5] = 0xCC;
            Pkt[6] = 0x00;
            Pkt[7] = 0xCC;
            if (!CanSendData(CanID, Pkt))
            {
                throw new Exception("Error : 10001");
            }
        }

        private void WaitSYN(int HandshakingType, int TimeOut)
        {
            byte[] tmpBuf = new byte[8];
            uint CanID = 0;

            do
            {
                if (!CanRecvData(ref CanID, ref tmpBuf))
                {
                    if (TimeOut == 0)
                    {
                        if ((HandshakingType == 1) || (HandshakingType == 3) || (HandshakingType == 4))
                        {
                            throw new Exception("Error : 10002");
                        }
                        else if (HandshakingType == 2)
                        {
                            throw new Exception("WARNNING : Please try to upgrade with an old version of the host computer and software. Note: The old version of the project cannot prompt whether the upgrading is successful, it needs to be confirmed through testing after upgrading!");
                        }
                    }
                    TimeOut--;
                    Thread.Sleep(10);
                }
                else
                {
                    if (HandshakingType == 1) // 第一次握手
                    {
                        if (tmpBuf[0] == 0xAA && tmpBuf[1] == 0x00
                            && tmpBuf[2] == 0xAA && tmpBuf[3] == 0x00
                            && tmpBuf[4] == 0xAA && tmpBuf[5] == 0x00
                            && tmpBuf[6] == 0xAA && tmpBuf[7] == 0x00)
                        {
                            break;
                        }
                    }
                    else if (HandshakingType == 2) // 擦除完FLASH扇区后握手
                    {
                        if (tmpBuf[0] == 0xBB && tmpBuf[1] == 0x00
                            && tmpBuf[2] == 0xBB && tmpBuf[3] == 0x00
                            && tmpBuf[4] == 0xBB && tmpBuf[5] == 0x00
                            && tmpBuf[6] == 0xBB && tmpBuf[7] == 0x00)
                        {
                            break;
                        }
                    }
                    else if (HandshakingType == 3) // 发送CANID配置握手信号
                    {
                        if (tmpBuf[0] == 0xCC && tmpBuf[1] == 0x00
                            && tmpBuf[2] == 0xCC && tmpBuf[3] == 0x00
                            && tmpBuf[4] == 0xCC && tmpBuf[5] == 0x00
                            && tmpBuf[6] == 0xCC && tmpBuf[7] == 0x00)
                        {
                            break;
                        }
                    }
                    else if (HandshakingType == 4) // 烧写CANID配置完成后握手
                    {
                        if (tmpBuf[0] == 0xDD && tmpBuf[1] == 0x00
                            && tmpBuf[2] == 0xDD && tmpBuf[3] == 0x00
                            && tmpBuf[4] == 0xDD && tmpBuf[5] == 0x00
                            && tmpBuf[6] == 0xDD && tmpBuf[7] == 0x00)
                        {
                            break;
                        }
                    }
                }
            } while (true);
        }

        private void WaitChecksum(short ChecksumType, uint ChecksumFromPC, short TimeOut)
        {
            byte[] tmpBuf = new byte[8];
            uint CanID = 0;
            uint ChecksumFromLower = 0;

            do
            {
                if (!CanRecvData(ref CanID, ref tmpBuf))
                {
                    if (TimeOut == 0)
                    {
                        if (ChecksumType == 1)
                        {
                            throw new Exception("Error : 20001");
                        }
                        else if (ChecksumType == 2)
                        {
                            throw new Exception("Error : 20002");
                        }
                    }
                    TimeOut--;
                    Thread.Sleep(10);
                }
                else
                {
                    if (ChecksumType == 1) // 
                    {

                    }
                    else if (ChecksumType == 2) // 
                    {
                        ChecksumFromLower = ((uint)tmpBuf[4] << 24) | ((uint)tmpBuf[5] << 16) | ((uint)tmpBuf[6] << 8) | tmpBuf[7];
                        if (ChecksumFromLower == ChecksumFromPC)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("Error : 20004");
                        }

                    }
                }
            } while (true);
        }

        private void CanDataClear()
        {
            VCI_ClearBuffer(4, 0, 0);
        }
        private UInt16 BinGetWord(byte[] Buf, int Offset)
        {
            uint a, b;

            a = Buf[Offset];
            b = Buf[Offset + 1];
            return (UInt16)(a | (b << 8));
        }
        private UInt32 BinGetLong(byte[] Buf, int Offset)
        {
            uint a, b;

            a = BinGetWord(Buf, Offset);
            a <<= 16;
            b = BinGetWord(Buf, Offset + 8);
            return a | b;
        }
        private bool CheckBinaryFile(byte[] Buf)
        {
            if (Buf[0] != 0xAA)
                return false;
            if (Buf[1] != 0x08)
                return false;
            return true;
        }
        private void UpdateProcess(object FilePath)
        {
            const int targetBufSize = 0x80;
            string BinPath = (string)FilePath;
            int Len, Offset = 0;
            byte[] BinBuffer;
            byte[] TmpBuf;
            bool Res = false;
            bool initFlag = false;
            UInt32 CanID = gCanID;
            UInt32 DestAddr;
            int SendLen;
            UInt16 BlockSize;

            DisableControl(UpgradeButton);

            TmpBuf = new byte[targetBufSize * 8];
            try
            {
                CanInit();
                Len = ReadFile(BinPath, out BinBuffer);                
                InitializeProgressBar(UpgradeProgressBar, 0, Len);
                initFlag = true;

                if (!CheckBinaryFile(BinBuffer))
                {
                    throw new Exception("Error : 10004");
                }

                SendUpgradeMark(CanID);
                CanInitForUpdate();
                WaitSYN(1, 200);
                if (LastestVerCheckedFlag == 1)
                {
                    WaitSYN(2, 3000);
                    Thread.Sleep(100);
                }
                else if (LastestVerCheckedFlag == 0)
                {
                    // 取消校验和检测
                    // .............
                    Thread.Sleep(16000);
                }
                CanDataClear();

                Offset = (1 + 8 + 2) * 8;
                MemoryCopy(ref TmpBuf, 0, ref BinBuffer, 0, Offset);
                if (!CanSendData(CanID, TmpBuf, Offset))
                {
                    throw new Exception("Error : 10001");
                }

                SetProgressBarValue(UpgradeProgressBar, Offset);

                while (Offset < Len)
                {
                    BlockSize = BinGetWord(BinBuffer, Offset);
                    //Offset += 8;
                    DestAddr = BinGetLong(BinBuffer, Offset + 8);
                    //Offset += 16;
                    SendLen = 16 + 8;
                    MemoryCopy(ref TmpBuf, 0, ref BinBuffer, Offset, SendLen);
                    if (!CanSendData(CanID, TmpBuf, SendLen))
                    {
                        throw new Exception("Error : 10001");
                    }
                    Offset += SendLen;

                    while (BlockSize > 0)
                    {
                        if (BlockSize > targetBufSize)
                        {
                            SendLen = targetBufSize * 8;
                            MemoryCopy(ref TmpBuf, 0, ref BinBuffer, Offset, SendLen);
                            Offset += SendLen;
                            BlockSize -= targetBufSize;
                        }
                        else
                        {
                            SendLen = BlockSize * 8;
                            MemoryCopy(ref TmpBuf, 0, ref BinBuffer, Offset, SendLen);
                            Offset += SendLen;
                            BlockSize -= BlockSize;
                        }
                        if (!CanSendData(CanID, TmpBuf, SendLen))
                        {
                            throw new Exception("Error : 10001");
                        }
                        SetProgressBarValue(UpgradeProgressBar, Offset);
                        Thread.Sleep(30);
                    }
                }
                Res = true;
            }
            catch (Exception Err)
            {
                ShowMessage(Err.Message);
            }

            if (initFlag)
            {
                CanClose();
            }
            if (Res)
            {
                ShowMessage("OK");
            }
            EnableControl(UpgradeButton);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void BaudrateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (BaudrateComboBox.SelectedIndex)
            {
                case 0: Timing0 = 0x00; Timing1 = 0x14; break;
                case 1: Timing0 = 0x00; Timing1 = 0x16; break;
                case 2: Timing0 = 0x00; Timing1 = 0x1c; break;
                case 3: Timing0 = 0x01; Timing1 = 0x1c; break;
                case 4: Timing0 = 0x03; Timing1 = 0x1c; break;
                case 5: Timing0 = 0x04; Timing1 = 0x1c; break;
                case 6: Timing0 = 0x09; Timing1 = 0x1c; break;
                case 7: Timing0 = 0x18; Timing1 = 0x1c; break;
                case 8: Timing0 = 0x31; Timing1 = 0x1c; break;
                case 9: Timing0 = 0xbf; Timing1 = 0xff; break;
            }
        }

        private void CANIDConfigButton_Click(object sender, EventArgs e)
        {
            bool Res = false;
            bool initFlag = false;

            DisableControl(CANIDConfigButton);

            // 初始化CAN分析仪
            CanInit();
            initFlag = true;

            // 每次配置ID之前checksum清零
            CANIDChecksum = 0;

            // 只有新版本有此功能
            if (LastestVerCheckedFlag == 1)
            {
                UInt32 CanID = gCanID;
                byte[] CANIDBuff = new byte[8];
                // 获取几个CANID textBox的内容
                System.Windows.Forms.TextBox[] textBoxes = new System.Windows.Forms.TextBox[] { Specific2FanText, Global2FanText, FromFanText };
                uint[] numbers = new uint[3];
                bool isValidInput = true; // 标记是否输入有效

                // 检查输入字符是否有效
                for (int i = 0; i < textBoxes.Length; i++)
                {
                    string inputText = textBoxes[i].Text;

                    // 检查输入是否为有效的十六进制数
                    if (uint.TryParse(inputText, System.Globalization.NumberStyles.HexNumber, null, out uint number))
                    {
                        // 检查文本框是否为空
                        if (string.IsNullOrEmpty(textBoxes[i].Text))
                        {
                            // 文本框为空，标记输入无效
                            isValidInput = false;
                            // 清空文本框内容或执行其他操作
                            textBoxes[i].Text = string.Empty;
                        }

                        // 限制输入范围
                        uint minValue = 0x00000000; // 最小值
                        uint maxValue = 0x1FFFFFFF; // 最大值

                        if (number >= minValue && number <= maxValue)
                        {
                            numbers[i] = number;
                        }
                        else
                        {
                            // 超出范围，标记输入无效
                            isValidInput = false;
                            // 清空文本框内容或执行其他操作
                            textBoxes[i].Text = string.Empty;
                        }
                    }
                    else
                    {
                        // 输入不是有效的十六进制数，标记输入无效
                        isValidInput = false;
                        // 清空文本框内容或执行其他操作
                        textBoxes[i].Text = string.Empty;
                    }
                }

                if (!isValidInput)
                {
                    // 提示用户输入合法范围的数字
                    MessageBox.Show($"Please enter a valid hexadecimal number and ensure it is within the range of 0x00000000 to 0x1FFFFFFF.");
                    CanClose();
                    EnableControl(CANIDConfigButton);
                    return;
                }
                else
                {
                    SendCanIdConfigMark(CanID);
                    CanInitForUpdate();
                    WaitSYN(3, 1000);
                    //WaitSYN(3, 1000);
                    Thread.Sleep(2000);

                    // 数字合法
                    int totalNumbers = numbers.Length;
                    int progress = 0;

                    for (int numbersIndex = 0; numbersIndex < totalNumbers; numbersIndex++)
                    {
                        for (int CANIDBufTmpIndex = 0; CANIDBufTmpIndex < 2; CANIDBufTmpIndex++)
                        {
                            if (CANIDBufTmpIndex == 0)
                            {
                                int startIndex = 0;

                                CANIDBuff[startIndex] = (byte)((numbers[numbersIndex] >> 8) & 0xFF);
                                CANIDBuff[startIndex + 1] = (byte)(numbers[numbersIndex] & 0xFF);

                                for (int i = startIndex + 2; i < startIndex + 8; i++)
                                {
                                    CANIDBuff[i] = 0x00;
                                }

                                CANIDChecksum += ((uint)CANIDBuff[0] << 8) | CANIDBuff[1];

                                if (!CanSendData(CanID, CANIDBuff))
                                {
                                    // 发生错误，中断进度条并显示错误信息
                                    MessageBox.Show("Error : 10001");
                                    return;
                                }
                                Thread.Sleep(20);
                            }
                            else if (CANIDBufTmpIndex == 1)
                            {
                                int startIndex = 0;

                                CANIDBuff[startIndex] = (byte)((numbers[numbersIndex] >> 24) & 0xFF);
                                CANIDBuff[startIndex + 1] = (byte)((numbers[numbersIndex] >> 16) & 0xFF);

                                for (int i = startIndex + 2; i < startIndex + 8; i++)
                                {
                                    CANIDBuff[i] = 0x00;
                                }

                                CANIDChecksum += ((uint)CANIDBuff[0] << 8) | CANIDBuff[1];

                                if (!CanSendData(CanID, CANIDBuff))
                                {
                                    // 发生错误，中断进度条并显示错误信息
                                    MessageBox.Show("Error : 10001");
                                    return;
                                }
                                Thread.Sleep(20);
                            }
                        }
                        // 更新进度条
                        progress = (numbersIndex + 1) * 100 / (totalNumbers + 2);
                        ConfigProgressBar.Value = progress;
                    }

                    // 等待烧写成功的握手信号
                    WaitSYN(4, 1000);
                    ConfigProgressBar.Value = 80;

                    // 等待校验和匹配
                    WaitChecksum(2, CANIDChecksum, 1000);
                    ConfigProgressBar.Value = 100;

                    Res = true;
                }
            }
            else
            {
                MessageBox.Show("This feature is only supported in the latest version.");
            }

            if (initFlag)
            {
                CanClose();
            }
            if (Res)
            {
                ShowMessage("OK");
            }
            EnableControl(CANIDConfigButton);

        }


        private void ConfigProgressBar_Click(object sender, EventArgs e)
        {

        }

        private void UpgradeButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ParameterizedThreadStart(UpdateProcess));
            t.Start(UgFileTextBox.Text);
            Thread.Sleep(10);
        }

        private void UgFileTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpgradeProgressBar_Click(object sender, EventArgs e)
        {

        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.CheckPathExists = true;
            dlg.Filter = "二进制文件|*.bin";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    UgFileTextBox.Text = dlg.FileName;
                }
            }
        }

        private void OldVersionButton_CheckedChanged(object sender, EventArgs e)
        {
            if (OldVersionButton.Checked)
            {
                LastestVerCheckedFlag = 0;
            }
        }

        private void Specific2FanText_TextChanged(object sender, EventArgs e)
        {

        }

        private void LastestVersionButton_CheckedChanged(object sender, EventArgs e)
        {
            if (LastestVersionButton.Checked)
            {
                LastestVerCheckedFlag = 1;
            }
        }
    }
}
