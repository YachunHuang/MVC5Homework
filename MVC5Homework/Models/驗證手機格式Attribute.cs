using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MVC5Homework.Models
{
    internal class 驗證手機格式Attribute : DataTypeAttribute
    {
        public 驗證手機格式Attribute() : base(DataType.Text)
        {

        }
        public override bool IsValid(object data)
        {
            string str = (string)data;
            Regex regex = new Regex("\\d{4}-\\d{6}");

            return regex.IsMatch(str);
        }
    }
}