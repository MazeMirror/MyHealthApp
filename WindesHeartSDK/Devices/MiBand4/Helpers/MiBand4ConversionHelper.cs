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
            byte[] secretKey = { 0x04, 0xa7, 0xd1, 0xe2, 0xee, 0x81, 0x60, 0xb0, 0x95, 0x56, 0xc2, 0xc8, 0xea, 0x60, 0x76, 0x2a };
            //El segundo MiBand
            //byte[] secretKey = { 0xa2, 0x0a, 0xfe, 0xbb, 0x8f, 0x23, 0x26, 0x8e, 0x76, 0xed, 0x6f, 0x70, 0x3d, 0x0c, 0xde, 0x69 };
            //El tercer MiBand
            //byte[] secretKey = { 0x4f, 0x83, 0xcc, 0x5e, 0x12, 0x2a, 0x8f, 0xdb, 0x9c, 0x3c, 0x51, 0x0e, 0x47, 0x98, 0xd3, 0x49 };
            //El cuarto MiBand
            //byte[] secretKey = { 0x19, 0x36, 0x48, 0xd3, 0x5d, 0x97, 0x35, 0x35, 0x92, 0xb7, 0x75, 0x76, 0xcf, 0xfb, 0xb5, 0xea };
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
