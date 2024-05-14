using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Implementation
{
    public class OTPRepository
    {
        public OTP CreateOTP()
        {
            string numbers = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random objrandom = new Random();
            string passwordString = "";
            string OTPname = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                int temp = objrandom.Next(0, numbers.Length);
                passwordString = numbers.ToCharArray()[temp].ToString();
                OTPname += passwordString;
            }

            string encryptedOTP = AesRepository.EncryptString(OTPname);
            return new OTP { OTPName = encryptedOTP };
        }

        public OTP getOTP()
        {
            string numbers = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random objrandom = new Random();
            string passwordString = "";
            string OTPname = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                int temp = objrandom.Next(0, numbers.Length);
                passwordString = numbers.ToCharArray()[temp].ToString();
                OTPname += passwordString;
            }
            return new OTP { OTPName = OTPname };
        }
    }

}
