using System;
using System.Security.Cryptography;
using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Devices.MiBand4Device.Helpers
{
    public static class MiBand4ConversionHelper
    {

        public static byte[] CreateKey(byte[] value)
        {
            byte[] bytes = { 0x03, 0x00 };
            //El primer MiBand
            //byte[] secretKey = { 0x04, 0xa7, 0xd1, 0xe2, 0xee, 0x81, 0x60, 0xb0, 0x95, 0x56, 0xc2, 0xc8, 0xea, 0x60, 0x76, 0x2a }; 
            //El segundo MiBand
            byte[] secretKey = { 0xe4, 0xd1, 0xd9, 0xf3, 0x6f, 0x8c, 0xbd, 0x41, 0x4e, 0xfe, 0x3c, 0xf1, 0x67, 0x25, 0x43, 0x20 }; 
            value = ConversionHelper.CopyOfRange(value, 3, 19);
            byte[] buffer = EncryptBuff(secretKey, value);
            byte[] endBytes = new byte[18];
            Buffer.BlockCopy(bytes, 0, endBytes, 0, 2);
            Buffer.BlockCopy(buffer, 0, endBytes, 2, 16);
            return endBytes;
        }

        public static byte[] EncryptBuff(byte[] sessionKey, byte[] buffer)
        {
            AesManaged myAes = new AesManaged();

            myAes.Mode = CipherMode.ECB;
            myAes.Key = sessionKey;
            myAes.Padding = PaddingMode.None;

            ICryptoTransform encryptor = myAes.CreateEncryptor();
            return encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
        }


    }
}
