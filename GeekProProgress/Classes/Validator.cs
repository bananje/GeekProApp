using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeekProProgress.Classes
{
    public class Validator
    {
        public static bool ValidationSymbols(string text)
        {
            bool valid = true;

            foreach (char pass in text)
            {
                string str = pass.ToString();

                if (Regex.IsMatch(str, @"[а-я А-Я a-z A-Z 0-9]"))
                    valid = true;
                else
                    valid = false;
                break;
            }
            return valid;
        }
        public static bool ValidationPassword(string FIO,string login,string password)
        {
            if (FIO == "" || login == "" || password == "")
            {
                Manager.ErrorMessage("Не все обязательные поля заполнены");
                return false;
            }  
            if(password.Length < 7 || login.Length < 5)
            {
                Manager.ErrorMessage("Неприемлемое количество символов в поле пароля или логина");
                return false;
            }

            bool validation = ValidationSymbols(password);
            if(validation == false)
            {
                Manager.ErrorMessage("В поле пароля введены недопустимые знаки. Допустимые знаки: а-я А-Я a-z A-Z 0-9");
                return false;
            }
            return true;
        }        
    }
}
