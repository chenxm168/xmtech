
namespace EQPIO.Common
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class DeviceMemory
    {
        private SortedList m_DeviceCodeList = new SortedList();
        private Hashtable m_MemoryList = new Hashtable();

        public event MemoryChangedEventHandler MemoryChanged;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void Add_Device(string device_name, byte device_code, DeviceType type, int points)
        {
            try
            {
                byte[] buffer;
                this.m_DeviceCodeList.Add(device_code, device_name);
                if (type == DeviceType.Bit)
                {
                    buffer = new byte[(points / 4) + (((points % 4) > 0) ? 4 : 0)];
                    this.m_MemoryList.Add(device_name, buffer);
                }
                else if (type == DeviceType.Word)
                {
                    buffer = new byte[points * 4];
                    this.m_MemoryList.Add(device_name, buffer);
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("DeviceMemory - Add_Device : " + exception.ToString());
            }
        }

        public void Dispose()
        {
            try
            {
                this.m_MemoryList = null;
                this.m_DeviceCodeList = null;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("DeviceMemory - Dispose : " + exception.ToString());
            }
        }

        public string GetDeviceName(byte device_code)
        {
            try
            {
                return this.m_DeviceCodeList[device_code].ToString();
            }
            catch (Exception exception)
            {
                Trace.WriteLine("GetDeviceName : " + exception.ToString());
            }
            return null;
        }

        public int MemoryLength(string aDeviceCode)
        {
            try
            {
                switch (aDeviceCode)
                {
                    case "B":
                        return ((bool[])this.m_MemoryList[aDeviceCode]).Length;

                    case "M":
                        return ((bool[])this.m_MemoryList[aDeviceCode]).Length;

                    case "W":
                        return ((ushort[])this.m_MemoryList[aDeviceCode]).Length;

                    case "X":
                        return ((bool[])this.m_MemoryList[aDeviceCode]).Length;

                    case "Y":
                        return ((bool[])this.m_MemoryList[aDeviceCode]).Length;

                    case "D":
                        return ((ushort[])this.m_MemoryList[aDeviceCode]).Length;

                    case "R":
                        return ((ushort[])this.m_MemoryList[aDeviceCode]).Length;

                    case "SB":
                        return ((bool[])this.m_MemoryList[aDeviceCode]).Length;

                    case "SW":
                        return ((ushort[])this.m_MemoryList[aDeviceCode]).Length;

                    case "L":
                        return ((bool[])this.m_MemoryList[aDeviceCode]).Length;

                    case "ZR":
                        return ((ushort[])this.m_MemoryList[aDeviceCode]).Length;
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("DeviceMemory - MomoryLength : " + exception.ToString());
            }
            return 0;
        }

        public string read_by_bit(string device_name, int addr, int points)
        {
            try
            {
                string str = "";
                object obj2 = this.m_MemoryList[device_name];
                if (obj2 is bool[])
                {
                    bool[] flagArray = (bool[])obj2;
                    if ((addr < 0) || (addr >= flagArray.Length))
                    {
                        return "";
                    }
                    if (flagArray.Length < (points + addr))
                    {
                        return "";
                    }
                    lock (this.m_MemoryList.SyncRoot)
                    {
                        for (int i = 0; i < points; i++)
                        {
                            str = str + (flagArray[addr + i] ? "1" : "0");
                        }
                    }
                }
                return str;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("DeviceMemory - read_by_bit : " + exception.ToString());
            }
            return null;
        }

        public bool[] read_by_bit_raw(byte device_code, int addr, int points)
        {
            try
            {
                string str = this.m_DeviceCodeList[device_code].ToString();
                return this.read_by_bit_raw(str, addr, points);
            }
            catch (Exception exception)
            {
                Trace.WriteLine("read_by_bit_raw : " + exception.ToString());
            }
            return null;
        }

        public bool[] read_by_bit_raw(string device_name, int addr, int points)
        {
            try
            {
                bool[] destinationArray = null;
                object obj2 = this.m_MemoryList[device_name];
                if (obj2 is bool[])
                {
                    destinationArray = new bool[points];
                    bool[] sourceArray = (bool[])obj2;
                    if ((addr < 0) || (addr >= sourceArray.Length))
                    {
                        return null;
                    }
                    if (sourceArray.Length < (points + addr))
                    {
                        return null;
                    }
                    lock (this.m_MemoryList.SyncRoot)
                    {
                        Array.Copy(sourceArray, addr, destinationArray, 0, points);
                    }
                }
                return destinationArray;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("read_by_bit_raw : " + exception.ToString());
                return null;
            }
        }

        public char[] read_by_word(string device_name, int addr, int points)
        {
            try
            {
                int num;
                byte[] bytes;
                object obj3;
                char[] chArray = new char[points * 2];
                object obj2 = this.m_MemoryList[device_name];
                if (obj2 is bool[])
                {
                    bool[] flagArray = (bool[])obj2;
                    if ((addr < 0) || (addr >= flagArray.Length))
                    {
                        return null;
                    }
                    if (flagArray.Length < ((points * 0x10) + addr))
                    {
                        return null;
                    }
                    lock ((obj3 = this.m_MemoryList.SyncRoot))
                    {
                        for (num = 0; num < points; num++)
                        {
                            ushort num2 = 0;
                            for (int i = 0; i < 0x10; i++)
                            {
                                if (flagArray[(i + addr) + (num * 0x10)])
                                {
                                    num2 = (ushort)(num2 + ((ushort)Math.Pow(2.0, (double)i)));
                                }
                            }
                            bytes = BitConverter.GetBytes(num2);
                            chArray[2 * num] = (char)bytes[0];
                            chArray[(2 * num) + 1] = (char)bytes[1];
                        }
                    }
                }
                else if (obj2 is ushort[])
                {
                    ushort[] numArray = (ushort[])obj2;
                    if ((addr < 0) || (addr >= numArray.Length))
                    {
                        return null;
                    }
                    if (numArray.Length < (points + addr))
                    {
                        return null;
                    }
                    lock ((obj3 = this.m_MemoryList.SyncRoot))
                    {
                        for (num = 0; num < points; num++)
                        {
                            bytes = BitConverter.GetBytes(numArray[addr + num]);
                            chArray[2 * num] = (char)bytes[0];
                            chArray[(2 * num) + 1] = (char)bytes[1];
                        }
                    }
                }
                return chArray;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("read_by_word : " + exception.ToString());
                return null;
            }
        }

        public ushort[] read_by_word_raw(byte device_code, int addr, int points)
        {
            try
            {
                string str = this.m_DeviceCodeList[device_code].ToString();
                return this.read_by_word_raw(str, addr, points);
            }
            catch (Exception exception)
            {
                Trace.WriteLine("read_by_word_raw : " + exception.ToString());
                return null;
            }
        }

        public ushort[] read_by_word_raw(string device_name, int addr, int points)
        {
            try
            {
                object obj3;
                ushort[] destinationArray = null;
                object obj2 = this.m_MemoryList[device_name];
                if (obj2 is bool[])
                {
                    bool[] flagArray = (bool[])obj2;
                    if ((addr < 0) || (addr >= flagArray.Length))
                    {
                        return null;
                    }
                    if (flagArray.Length < ((points * 0x10) + addr))
                    {
                        return null;
                    }
                    lock ((obj3 = this.m_MemoryList.SyncRoot))
                    {
                        destinationArray = new ushort[points];
                        for (int i = 0; i < points; i++)
                        {
                            for (int j = 0; j < 0x10; j++)
                            {
                                if (flagArray[(j + addr) + (i * 0x10)])
                                {
                                    destinationArray[i] = (ushort)(destinationArray[i] + ((ushort)Math.Pow(2.0, (double)j)));
                                }
                            }
                        }
                    }
                }
                else if (obj2 is ushort[])
                {
                    ushort[] sourceArray = (ushort[])obj2;
                    if ((addr < 0) || (addr >= sourceArray.Length))
                    {
                        return null;
                    }
                    if (sourceArray.Length < (points + addr))
                    {
                        return null;
                    }
                    lock ((obj3 = this.m_MemoryList.SyncRoot))
                    {
                        destinationArray = new ushort[points];
                        Array.Copy(sourceArray, addr, destinationArray, 0, points);
                    }
                }
                return destinationArray;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("read_by_word_raw : " + exception.ToString());
                return null;
            }
        }

        public string read_by_word_rev(string device_name, int addr, int points)
        {
            try
            {
                int num;
                byte[] bytes;
                object obj3;
                string str = "";
                object obj2 = this.m_MemoryList[device_name];
                if (obj2 is bool[])
                {
                    bool[] flagArray = (bool[])obj2;
                    if ((addr < 0) || (addr >= flagArray.Length))
                    {
                        return "";
                    }
                    if (flagArray.Length < ((points * 0x10) + addr))
                    {
                        return "";
                    }
                    lock ((obj3 = this.m_MemoryList.SyncRoot))
                    {
                        for (num = 0; num < points; num++)
                        {
                            ushort num2 = 0;
                            for (int i = 0; i < 0x10; i++)
                            {
                                if (flagArray[(i + addr) + (num * 0x10)])
                                {
                                    num2 = (ushort)(num2 + ((ushort)Math.Pow(2.0, (double)i)));
                                }
                            }
                            bytes = BitConverter.GetBytes(num2);
                            char ch = (char)bytes[0];
                            char ch2 = (char)bytes[1];
                            str = str + ch2.ToString() + ch.ToString();
                        }
                    }
                }
                else if (obj2 is ushort[])
                {
                    ushort[] numArray = (ushort[])obj2;
                    if ((addr < 0) || (addr >= numArray.Length))
                    {
                        return "";
                    }
                    if (numArray.Length < (points + addr))
                    {
                        return "";
                    }
                    lock ((obj3 = this.m_MemoryList.SyncRoot))
                    {
                        for (num = 0; num < points; num++)
                        {
                            bytes = BitConverter.GetBytes(numArray[addr + num]);
                            str = str + ((char)bytes[1]).ToString() + ((char)bytes[0]).ToString();
                        }
                    }
                }
                return str;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("read_by_word_rev : " + exception.ToString());
                return null;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void Remove_Device(string device_name)
        {
            try
            {
                this.m_MemoryList.Remove(device_name);
                int index = this.m_DeviceCodeList.IndexOfValue(device_name);
                if (index >= 0)
                {
                    this.m_DeviceCodeList.RemoveAt(index);
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("DeviceMemory - Remove_Device : " + exception.ToString());
            }
        }

        public void Reset()
        {
            try
            {
                foreach (Array array in this.m_MemoryList.Values)
                {
                    Array.Clear(array, 0, array.Length);
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("DeviceMemory - Reset : " + exception.ToString());
            }
        }

        public bool write_by_bit(string device_name, int addr, int points, string data)
        {
            try
            {
                object obj2 = this.m_MemoryList[device_name];
                if (obj2 is bool[])
                {
                    bool[] flagArray = (bool[])obj2;
                    if ((addr < 0) || (addr >= flagArray.Length))
                    {
                        return false;
                    }
                    if (flagArray.Length < (points + addr))
                    {
                        return false;
                    }
                    lock (this.m_MemoryList.SyncRoot)
                    {
                        for (int i = 0; (i < points) && (i < data.Length); i++)
                        {
                            if (data[i] == '0')
                            {
                                flagArray[addr + i] = false;
                            }
                            else if (data[i] == '1')
                            {
                                flagArray[addr + i] = true;
                            }
                            if (this.MemoryChanged != null)
                            {
                                this.MemoryChanged.BeginInvoke(device_name, addr, points, flagArray[addr + i], null, null);
                            }
                        }
                    }
                }
                else if (obj2 is ushort[])
                {
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("write_by_bit : " + exception.ToString());
                return false;
            }
        }

        public bool write_by_bit_raw(byte device_code, int addr, int points, bool[] rawdata)
        {
            try
            {
                string str = this.m_DeviceCodeList[device_code].ToString();
                return this.write_by_bit_raw(str, addr, points, rawdata);
            }
            catch (Exception exception)
            {
                Trace.WriteLine("write_by_bit_raw : " + exception.ToString());
            }
            return false;
        }

        public bool write_by_bit_raw(string device_name, int addr, int points, bool[] rawdata)
        {
            try
            {
                object obj2 = this.m_MemoryList[device_name];
                if (obj2 is bool[])
                {
                    bool[] destinationArray = (bool[])obj2;
                    if ((addr < 0) || (addr >= destinationArray.Length))
                    {
                        return false;
                    }
                    if (destinationArray.Length < (points + addr))
                    {
                        return false;
                    }
                    lock (this.m_MemoryList.SyncRoot)
                    {
                        Array.Copy(rawdata, 0, destinationArray, addr, points);
                        if (this.MemoryChanged != null)
                        {
                            for (int i = 0; i < points; i++)
                            {
                                this.MemoryChanged.BeginInvoke(device_name, addr, points, destinationArray[addr + i], null, null);
                            }
                        }
                    }
                }
                else if (obj2 is ushort[])
                {
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("write_by_bit_raw : " + exception.ToString());
                return false;
            }
        }

        public bool write_by_word(string device_name, int addr, int points, string data)
        {
            try
            {
                int num;
                int num2;
                object obj3;
                object obj2 = this.m_MemoryList[device_name];
                if (obj2 is bool[])
                {
                    bool[] flagArray = (bool[])obj2;
                    if ((addr < 0) || (addr >= flagArray.Length))
                    {
                        return false;
                    }
                    if (flagArray.Length < ((points * 0x10) + addr))
                    {
                        return false;
                    }
                    lock ((obj3 = this.m_MemoryList.SyncRoot))
                    {
                        data = data.PadRight(points * 2, ' ');
                        for (num = 0; num < data.Length; num++)
                        {
                            for (num2 = 0; num2 < 8; num2++)
                            {
                                if (flagArray.Length <= ((addr + (num * 8)) + num2))
                                {
                                    return false;
                                }
                                if ((data[num] & ((char)((ushort)Math.Pow(2.0, (double)num2)))) == ((ushort)Math.Pow(2.0, (double)num2)))
                                {
                                    flagArray[(addr + (num * 8)) + num2] = true;
                                }
                                else
                                {
                                    flagArray[(addr + (num * 8)) + num2] = false;
                                }
                            }
                        }
                    }
                }
                else if (obj2 is ushort[])
                {
                    ushort[] numArray = (ushort[])obj2;
                    if ((addr < 0) || (addr >= numArray.Length))
                    {
                        return false;
                    }
                    if (numArray.Length < (points + addr))
                    {
                        return false;
                    }
                    lock ((obj3 = this.m_MemoryList.SyncRoot))
                    {
                        data = data.PadRight(points * 2, ' ');
                        num = 0;
                        for (num2 = 0; num < points; num2 += 2)
                        {
                            ushort num3 = BitConverter.ToUInt16(new byte[] { (byte)data[num2], (byte)data[num2 + 1] }, 0);
                            numArray[addr + num] = num3;
                            num++;
                        }
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("write_by_word : " + exception.ToString());
            }
            return false;
        }

        public bool write_by_word(string device_name, int addr, int points, ushort rawdata)
        {
            try
            {
                int num;
                object obj3;
                object obj2 = this.m_MemoryList[device_name];
                if (obj2 is bool[])
                {
                    bool[] flagArray = (bool[])obj2;
                    if ((addr < 0) || (addr >= flagArray.Length))
                    {
                        return false;
                    }
                    if (flagArray.Length < ((points * 0x10) + addr))
                    {
                        return false;
                    }
                    lock ((obj3 = this.m_MemoryList.SyncRoot))
                    {
                        for (num = 0; num < points; num++)
                        {
                            for (int i = 0; i < 0x10; i++)
                            {
                                if (((ushort)(rawdata & ((ushort)Math.Pow(2.0, (double)i)))) == ((ushort)Math.Pow(2.0, (double)i)))
                                {
                                    flagArray[(addr + (num * 0x10)) + i] = true;
                                }
                                else
                                {
                                    flagArray[(addr + (num * 0x10)) + i] = false;
                                }
                            }
                        }
                    }
                }
                else if (obj2 is ushort[])
                {
                    ushort[] numArray = (ushort[])obj2;
                    if ((addr < 0) || (addr >= numArray.Length))
                    {
                        return false;
                    }
                    if (numArray.Length < (points + addr))
                    {
                        return false;
                    }
                    lock ((obj3 = this.m_MemoryList.SyncRoot))
                    {
                        for (num = 0; num < points; num++)
                        {
                            numArray[addr + num] = rawdata;
                        }
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("write_by_word : " + exception.ToString());
            }
            return false;
        }

        public bool write_by_word_raw(byte device_code, int addr, int points, ushort[] rawdata)
        {
            try
            {
                string str = this.m_DeviceCodeList[device_code].ToString();
                return this.write_by_word_raw(str, addr, points, rawdata);
            }
            catch (Exception exception)
            {
                Trace.WriteLine("write_by_word_raw : " + exception.ToString());
            }
            return false;
        }

        public bool write_by_word_raw(string device_name, int addr, int points, ushort[] rawdata)
        {
            try
            {
                object obj3;
                object obj2 = this.m_MemoryList[device_name];
                if (obj2 is bool[])
                {
                    bool[] flagArray = (bool[])obj2;
                    if ((addr < 0) || (addr >= flagArray.Length))
                    {
                        return false;
                    }
                    if (flagArray.Length < ((points * 0x10) + addr))
                    {
                        return false;
                    }
                    lock ((obj3 = this.m_MemoryList.SyncRoot))
                    {
                        for (int i = 0; i < points; i++)
                        {
                            for (int j = 0; j < 0x10; j++)
                            {
                                if (((ushort)(rawdata[i] & ((ushort)Math.Pow(2.0, (double)j)))) == ((ushort)Math.Pow(2.0, (double)j)))
                                {
                                    flagArray[(addr + (i * 0x10)) + j] = true;
                                }
                                else
                                {
                                    flagArray[(addr + (i * 0x10)) + j] = false;
                                }
                            }
                        }
                    }
                }
                else if (obj2 is ushort[])
                {
                    ushort[] destinationArray = (ushort[])obj2;
                    if ((addr < 0) || (addr >= destinationArray.Length))
                    {
                        return false;
                    }
                    if (destinationArray.Length < (points + addr))
                    {
                        return false;
                    }
                    lock ((obj3 = this.m_MemoryList.SyncRoot))
                    {
                        Array.Copy(rawdata, 0, destinationArray, addr, points);
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("write_by_word_raw : " + exception.ToString());
            }
            return false;
        }

        public bool write_by_word_rev(string device_name, int addr, int points, string data)
        {
            try
            {
                object obj2 = this.m_MemoryList[device_name];
                if (obj2 is bool[])
                {
                    return false;
                }
                if (obj2 is ushort[])
                {
                    ushort[] numArray = (ushort[])obj2;
                    if ((addr < 0) || (addr >= numArray.Length))
                    {
                        return false;
                    }
                    if (numArray.Length < (points + addr))
                    {
                        return false;
                    }
                    lock (this.m_MemoryList.SyncRoot)
                    {
                        data = data.PadRight(points * 2, ' ');
                        int num = 0;
                        for (int i = 0; num < points; i += 2)
                        {
                            byte[] buffer = new byte[2];
                            buffer[1] = (byte)data[i];
                            buffer[0] = (byte)data[i + 1];
                            ushort num3 = BitConverter.ToUInt16(buffer, 0);
                            numArray[addr + num] = num3;
                            num++;
                        }
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("write_by_word_rev : " + exception.ToString());
            }
            return false;
        }

        public Hashtable MemoryList
        {
            get
            {
                return this.m_MemoryList;
            }
            set
            {
                this.m_MemoryList = value;
            }
        }

        public enum DeviceType
        {
            Bit,
            Word
        }

        public delegate void MemoryChangedEventHandler(string device_name, int addr, int points, bool data);
    }
}
