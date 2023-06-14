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

        public Form1()
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.CheckPathExists = true;
            dlg.Filter = "二进制文件|*.bin";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    textBox1.Text = dlg.FileName;
                }
            }
        }
        private void ProcessBarInit(int Len)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = Len;
        }
        private void ProcessBarSet(int Val)
        {
            progressBar1.Value = Val;
        }
        private void EnableControl()
        {
            button2.Enabled = true;
        }
        private void DisableControl()
        {
            button2.Enabled = false;
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

        private void Send_UpdateMark(UInt32 CanID)
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
        private void WaitSYN(int HandshakingType, int TimeOut)
        {
            byte[] tmpBuf = new byte[8];
            UInt32 CanID = 0;

            do
            {
                if (!CanRecvData(ref CanID, ref tmpBuf))
                {
                    if (TimeOut == 0)
                    {
                        if (HandshakingType == 1)
                        {
                            throw new Exception("Error : 10002");
                        }
                        else if (HandshakingType == 2)
                        {
                            throw new Exception("Error : 10003");
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

            DisableControl();
            TmpBuf = new byte[targetBufSize * 8];
            try
            {
                CanInit();
                Len = ReadFile(BinPath, out BinBuffer);
                ProcessBarInit(Len);
                initFlag = true;

                if (!CheckBinaryFile(BinBuffer))
                {
                    throw new Exception("Error : 10004");
                }

                Send_UpdateMark(CanID);
                WaitSYN(1, 100);
                CanInitForUpdate();
                WaitSYN(2, 10000);
                Thread.Sleep(10);
                CanDataClear();

                Offset = (1 + 8 + 2) * 8;
                MemoryCopy(ref TmpBuf, 0, ref BinBuffer, 0, Offset);
                if (!CanSendData(CanID, TmpBuf, Offset))
                {
                    throw new Exception("Error : 10001");
                }
                ProcessBarSet(Offset);

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
                        ProcessBarSet(Offset);
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
            EnableControl();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ParameterizedThreadStart(UpdateProcess));
            t.Start(textBox1.Text);
            Thread.Sleep(10);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
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
    }
}
